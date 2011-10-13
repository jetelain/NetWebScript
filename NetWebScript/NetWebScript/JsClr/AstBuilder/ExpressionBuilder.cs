using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.Ast;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.PdbInfo;
using System.Reflection.Emit;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.AstBuilder
{
    internal interface IExpressionBuilderClient
    {
        void Exec(Expression expr);

        void Branch(Expression condition, Instruction instruction);

        void Return(Expression expr);

        void Throw(Expression expr);
    }

    internal sealed class ExpressionBuilder : InstructionVisitor
    {
        private class BuilderStackFrame { }
        private class ExpressionFrame : BuilderStackFrame { 
            public Expression Expression {get;set;}
        }
        private class DupFrame : BuilderStackFrame {
            public Expression Expression {get; set;}
            public DupFrame ParentFrame {get; set;}
        }

        private readonly Stack<BuilderStackFrame> stack = new Stack<BuilderStackFrame>();
        private readonly MethodCil body;
        private readonly MethodAst built;
        private readonly Stack<IExpressionBuilderClient> clients = new Stack<IExpressionBuilderClient>();
        private IExpressionBuilderClient client;

        private bool enforceInline = false;

        private LocalVariable mustPushVariableRef;
        private AssignExpression associatedAssign;

        private PdbSequencePoint pendingPoint;

        private readonly HashSet<LocalVariable> registers = new HashSet<LocalVariable>();

        internal ExpressionBuilder(MethodAst built, MethodCil body, IExpressionBuilderClient client)
        {
            this.built = built;
            this.body = body;
            this.client = client;
        }

        private static readonly PropertyInfo ArrayLengthProperty = typeof(Array).GetProperty("Length");

        internal int StackHeight
        {
            get { return stack.Count; }
        }

        public void SetEnforceInline()
        {
            enforceInline = true;
        }

        public void UnsetEnforceInline()
        {
            enforceInline = false;
        }

        private BuilderStackFrame PopFrame()
        {
            return stack.Pop();
        }

        internal Expression Pop()
        {
            var frame = PopFrame();
            DupFrame dup = frame as DupFrame;
            if (dup != null)
            {
                if (dup.Expression == null)
                {
                    throw new InvalidOperationException();
                }
                if (dup.ParentFrame != null)
                {
                    var parent = dup.ParentFrame;
                    var expr = dup.Expression;
                    var register = AllocateRegister(expr.GetExpressionType());
                    SetParentExpression(parent, register, expr);
                    return new VariableReferenceExpression(null, register);
                }
                return dup.Expression;
            }
            return ((ExpressionFrame)frame).Expression;
        }

        private void SetParentExpression(DupFrame parent, LocalVariable register, Expression expr )
        {
            if (parent.ParentFrame != null)
            {
                // Register should be used by whole hierachy
                parent.Expression = new VariableReferenceExpression(null, register);
                SetParentExpression(parent.ParentFrame, register, expr);
                parent.ParentFrame = null;
            }
            else
            {
                parent.Expression = new AssignExpression(null, new VariableReferenceExpression(null, register), expr); ;
            }
        }

        private LocalVariable AllocateRegister(Type type)
        {
            var register = built.AllocateVariable(type);
            registers.Add(register);
            return register;
        }


        internal void Push(Expression expr)
        {
            if (mustPushVariableRef != null)
            {
                VariableReferenceExpression var = expr as VariableReferenceExpression;
                if (var == null || var.Variable != mustPushVariableRef)
                {
                    throw new AstBuilderException(expr.IlOffset, "Unable to enforce inline.");
                }
                stack.Push(new ExpressionFrame() { Expression = associatedAssign });
                associatedAssign = null;
                mustPushVariableRef = null;
                return;
            }

            stack.Push(new ExpressionFrame() { Expression = expr });
        }

        /// <summary>
        /// Ensure execution of <paramref name="expr"/> and set it's result into a variable.
        /// </summary>
        /// <param name="expr"></param>
        /// <returns>An expression giving result of <paramref name="expr"/>.</returns>
        private Expression AllocateRegisterFor(Expression expr)
        {
            if (!IsRegister(expr))
            {
                var assign = expr as AssignExpression;
                if (assign != null && IsRegister(assign.Target))
                {
                    // If expression is assignement of a register, avoid creating a new register
                    // do assignement, then send a new reference to register
                    Exec(assign);
                    return assign.Target.Clone();
                }
                // Expression is not it self a register, or a pure constant within the method (argument, this etc.)
                var register = AllocateRegister(expr.GetExpressionType());
                ExecAssign(null, new VariableReferenceExpression(null, register), expr);
                return new VariableReferenceExpression(null, register);
            }
            // Expression is a register reference, or have constant value within the method
            // simply clone expression in that case
            return expr.Clone();
        }

        private void ExecAssign(int offset, AssignableExpression target, BuilderStackFrame value)
        {
            DupFrame dup = value as DupFrame;
            Expression valueExpr = null;
            if (dup != null)
            {
                if (dup.ParentFrame != null)
                {
                    dup.ParentFrame.Expression = new AssignExpression(offset, target, dup.Expression);
                    return;
                }
                valueExpr = dup.Expression;
            }
            else
            {
                valueExpr = ((ExpressionFrame)value).Expression;
            }
            ExecAssign(offset, target, valueExpr);
        }

        private void ExecAssign(int? offset, AssignableExpression target, Expression valueExpr)
        {
            Exec(new AssignExpression(offset, target, valueExpr));
        }

        private void Exec(Expression expr)
        {
            if (stack.Count > 0)
            {
                // Execute previous expression, and save its value into a register
                var temp = AllocateRegisterFor(Pop());

                // Execute asked expression
                Exec(expr);
                
                // Push-back value to restore stack in the correct state
                Push(temp);
            }
            else
            {
                // If stack is empty, can ask execution of expression
                ExecNoStack(expr);
            }
        }

        private void ExecNoStack(Expression expr)
        {
            // Stack MUST be empty before calling this method
            Contract.Requires(stack.Count == 0);

            if (enforceInline)
            {
                // Caller asked us to produce a single expression into "Branch", call to client.Exec is forbidden
                // Try to execute expression without having to call client.Exec 
                AssignExpression assign = expr as AssignExpression;
                if (assign != null)
                {
                    VariableReferenceExpression var = assign.Target as VariableReferenceExpression;
                    if (var != null)
                    {
                        mustPushVariableRef = var.Variable;
                        associatedAssign = assign;
                        return;
                    }
                }
                throw new AstBuilderException(expr.IlOffset, "Unable to enforce inline.");
            }

            if (pendingPoint != null)
            {
                client.Exec(new DebugPointExpression(pendingPoint));
                pendingPoint = null;
            }
            client.Exec(expr);
        }


        /// <summary>
        /// Determines if expression is a variable reference to a "register".
        /// A "register" is a variable generated by builder and assigned only once.
        /// Such variables have constant value.
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        private bool IsRegister(Expression top)
        {
            var varref = top as VariableReferenceExpression;
            if (varref != null)
            {
                return registers.Contains(varref.Variable);
            }
            return top.IsConstInMethod();
        }

        private void Branch(Expression condition, Instruction instruction)
        {
            if (pendingPoint != null)
            {
                condition = new DebugPointExpression(pendingPoint, condition);
                pendingPoint = null;
            }
            client.Branch(condition, instruction);
        }

        internal void PushClient(IExpressionBuilderClient newclient)
        {
            clients.Push(client);
            client = newclient;
        }

        internal void PopClient(IExpressionBuilderClient oldclient)
        {
            if (oldclient != client)
            {
                throw new InvalidOperationException();
            }
            client = clients.Pop();
        }


        #region Debugger

        public override void OnNop(Instruction instruction)
        {

        }

        public override void OnBreak(Instruction instruction)
        {
        }

        #endregion

        #region Binary operators

        public override void OnCeq(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.ValueEquality, Pop(), Pop()));
        }

        public override void OnCgt(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.GreaterThan, Pop(), Pop()));
        }

        public override void OnCgt_Un(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.GreaterThan, Pop(), Pop()));
        }

        public override void OnClt(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.LessThan, Pop(), Pop()));
        }

        public override void OnClt_Un(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.LessThan, Pop(), Pop()));
        }

        public override void OnAdd(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Add, Pop(), Pop()));
        }

        public override void OnSub(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Subtract, Pop(), Pop()));
        }

        public override void OnMul(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Multiply, Pop(), Pop()));
        }

        public override void OnDiv(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Divide, Pop(), Pop()));
        }

        public override void OnDiv_Un(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Divide, Pop(), Pop()));
        }

        public override void OnAnd(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.BitwiseAnd, Pop(), Pop()));
        }

        public override void OnOr(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.BitwiseOr, Pop(), Pop()));
        }

        public override void OnXor(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.BitwiseXor, Pop(), Pop()));
        }

        public override void OnShl(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.LeftShift, Pop(), Pop()));
        }

        public override void OnShr(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.RightShift, Pop(), Pop()));
        }

        public override void OnShr_Un(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.RightShift, Pop(), Pop()));
        }

        public override void OnRem(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Modulo, Pop(), Pop()));
        }

        public override void OnRem_Un(Instruction instruction)
        {
            Push(new BinaryExpression(instruction.Offset, BinaryOperator.Modulo, Pop(), Pop()));
        }
        #endregion

        #region Unary operators

        public override void OnNot(Instruction instruction)
        {
            Push(new UnaryExpression(instruction.Offset, UnaryOperator.BitwiseNot, Pop()));
        }

        public override void OnNeg(Instruction instruction)
        {
            Push(new UnaryExpression(instruction.Offset, UnaryOperator.Negate, Pop()));
        }

        #endregion

        #region Literals

        public override void OnLdstr(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, instruction.Operand));
        }

        public override void OnLdc_R4(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, Convert.ToSingle(instruction.Operand)));
        }

        public override void OnLdc_R8(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, Convert.ToDouble(instruction.Operand)));
        }

        public override void OnLdc_I8(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, Convert.ToInt64(instruction.Operand)));
        }

        public override void OnLdc_I4(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, Convert.ToInt32(instruction.Operand)));
        }

        public override void OnLdc_I4_M1(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, -1));
        }

        public override void OnLdc_I4_0(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 0));
        }

        public override void OnLdc_I4_1(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 1));
        }

        public override void OnLdc_I4_2(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 2));
        }

        public override void OnLdc_I4_3(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 3));
        }

        public override void OnLdc_I4_4(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 4));
        }

        public override void OnLdc_I4_5(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 5));
        }

        public override void OnLdc_I4_6(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 6));
        }

        public override void OnLdc_I4_7(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 7));
        }

        public override void OnLdc_I4_8(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, 8));
        }

        public override void OnLdnull(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, null));
        }

        public override void OnLdtoken(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, instruction.Operand));
        }

        #endregion

        #region Arguments

        public override void OnLdarg(Instruction instruction)
        {
            Push(ArgumentReference(instruction, instruction.OperandArgumentIndex));
        }

        public override void OnLdarg_0(Instruction instruction)
        {
            Push(ArgumentReference(instruction, 0));
        }

        public override void OnLdarg_1(Instruction instruction)
        {
            Push(ArgumentReference(instruction, 1));
        }

        public override void OnLdarg_2(Instruction instruction)
        {
            Push(ArgumentReference(instruction, 2));
        }

        public override void OnLdarg_3(Instruction instruction)
        {
            Push(ArgumentReference(instruction, 3));
        }

        private Expression ArgumentReference(Instruction instruction, int index)
        {
            if (!body.Method.IsStatic)
            {
                if (index == 0)
                {
                    return new ThisReferenceExpression(instruction.Offset, body.Method.DeclaringType);
                }
                index -= 1; // the Parameters collection dos not contain the implict this argument
            }
            return new ArgumentReferenceExpression(instruction.Offset, body.Arguments[index]);
        }
        #endregion

        #region Variables

        public override void OnLdloc(Instruction instruction)
        {
            Push(VariableReference(instruction, instruction.OperandVariableIndex));
        }

        public override void OnLdloc_0(Instruction instruction)
        {
            Push(VariableReference(instruction, 0));
        }

        public override void OnLdloc_1(Instruction instruction)
        {
            Push(VariableReference(instruction, 1));
        }

        public override void OnLdloc_2(Instruction instruction)
        {
            Push(VariableReference(instruction, 2));
        }

        public override void OnLdloc_3(Instruction instruction)
        {
            Push(VariableReference(instruction, 3));
        }

        public override void OnLdloca(Instruction instruction)
        {
            LocalVariable loc = body.Variables[instruction.OperandVariableIndex];
            if (loc.LocalType.IsValueType)
            {
                Push(VariableReference(instruction, instruction.OperandVariableIndex));
            }
            else
            {
                base.OnLdloca(instruction);
            }
        }

        public override void OnStloc(Instruction instruction)
        {
            ExecAssign(instruction.Offset, VariableReference(instruction, instruction.OperandVariableIndex), PopFrame());
        }

        public override void OnStloc_0(Instruction instruction)
        {
            ExecAssign(instruction.Offset, VariableReference(instruction, 0), PopFrame());
        }

        public override void OnStloc_1(Instruction instruction)
        {
            ExecAssign(instruction.Offset, VariableReference(instruction, 1), PopFrame());
        }

        public override void OnStloc_2(Instruction instruction)
        {
            ExecAssign(instruction.Offset, VariableReference(instruction, 2), PopFrame());
        }

        public override void OnStloc_3(Instruction instruction)
        {
            ExecAssign(instruction.Offset, VariableReference(instruction, 3), PopFrame());
        }

        VariableReferenceExpression VariableReference(Instruction instruction, int index)
        {
            return new VariableReferenceExpression(instruction.Offset, body.Variables[index]);
        }

        #endregion

        #region Method call

        public override void OnCallvirt(Instruction instruction)
        {
            OnCall(true, instruction);
        }

        public override void OnCall(Instruction instruction)
        {
            OnCall(false, instruction);
        }

        private bool CallInitializeArray(Instruction instruction)
        {
            // FIXME: find a more elegant solution
            var prev = stack.Pop();
            var tokenFrame = prev as ExpressionFrame;
            var dupFrame = stack.Peek() as DupFrame;
            if (tokenFrame != null && dupFrame != null )
            {
                var array = dupFrame.Expression as ArrayCreationExpression;
                var token = tokenFrame.Expression as LiteralExpression;
                if (array != null && token != null)
                {
                    stack.Pop(); // Pop the dup frame
                    dupFrame.ParentFrame.Expression = array;
                    RuntimeHelpersToolkit.InitializeArray(instruction.Offset, array, token);
                    return true;
                }
            }
            stack.Push(prev); // Restore stack in previous state
            return false;
        }

        private void OnCall(bool virt, Instruction instruction)
        {
            MethodBase method = instruction.OperandMethodBase;
            if (method.Name == "InitializeArray" && method.DeclaringType == typeof(System.Runtime.CompilerServices.RuntimeHelpers))
            {
                if (CallInitializeArray(instruction))
                {
                    return;
                }
            }
            List<Expression> arguments = new List<Expression>();
            for (int i = 0; i < method.GetParameters().Length; ++i)
            {
                arguments.Insert(0, Pop());
            }
            Expression target = null;
            if (!method.IsStatic)
            {
                target = Pop();
            }
            Expression call = new MethodInvocationExpression(instruction.Offset, virt, method, target, arguments);

            MethodInfo mi = method as MethodInfo;

            if (mi == null || mi.ReturnType == typeof(void))
            {
                Exec(call);
            }
            else
            {
                Push(call);
            }
        }

        #endregion

        #region Fields

        public override void OnLdfld(Instruction instruction)
        {
            Push(new FieldReferenceExpression(instruction.Offset, Pop(), instruction.OperandField));
        }

        public override void OnLdsfld(Instruction instruction)
        {
            Push(new FieldReferenceExpression(instruction.Offset, null, instruction.OperandField));
        }

        public override void OnStfld(Instruction instruction)
        {
            BuilderStackFrame  value = PopFrame();
            Expression target = Pop();
            ExecAssign(instruction.Offset, new FieldReferenceExpression(instruction.Offset, target, instruction.OperandField), value);
        }

        public override void OnStsfld(Instruction instruction)
        {
            ExecAssign(instruction.Offset, new FieldReferenceExpression(instruction.Offset, null, instruction.OperandField), PopFrame());
        }

        public override void OnLdlen(Instruction instruction)
        {
            Push(new MethodInvocationExpression(instruction.Offset, false, ArrayLengthProperty.GetGetMethod(), Pop(), new List<Expression>()));
        }

        #endregion

        #region Array indexers

        public override void OnLdelem_Any(Instruction instruction)
        {
            Expression index = Pop();
            Expression array = Pop();
            Push(new ArrayIndexerExpression(instruction.Offset, array, index));
        }

        public override void OnLdelem_I(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_I1(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_I2(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_I4(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_I8(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_R4(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_R8(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_Ref(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_U1(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_U2(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnLdelem_U4(Instruction instruction)
        {
            OnLdelem_Any(instruction);
        }

        public override void OnStelem_Any(Instruction instruction)
        {
            BuilderStackFrame value = PopFrame();
            Expression index = Pop();
            Expression array = Pop();
            ExecAssign(instruction.Offset, new ArrayIndexerExpression(instruction.Offset, array, index), value);
        }

        public override void OnStelem_I(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_I1(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_I2(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_I4(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_I8(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_R4(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_R8(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        public override void OnStelem_Ref(Instruction instruction)
        {
            OnStelem_Any(instruction);
        }

        #endregion

        #region Branches

        public override void OnBeq(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.ValueEquality, Pop(), Pop()), instruction);
        }

        public override void OnBge(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.GreaterThanOrEqual, Pop(), Pop()), instruction);
        }

        public override void OnBge_Un(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.GreaterThanOrEqual, Pop(), Pop()), instruction);
        }

        public override void OnBgt(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.GreaterThan, Pop(), Pop()), instruction);
        }

        public override void OnBgt_Un(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.GreaterThan, Pop(), Pop()), instruction);
        }

        public override void OnBle(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.LessThanOrEqual, Pop(), Pop()), instruction);
        }

        public override void OnBle_Un(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.LessThanOrEqual, Pop(), Pop()), instruction);
        }

        public override void OnBlt(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.LessThan, Pop(), Pop()), instruction);
        }

        public override void OnBlt_Un(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.LessThan, Pop(), Pop()), instruction);
        }

        public override void OnBne_Un(Instruction instruction)
        {
            Branch(new BinaryExpression(instruction.Offset, BinaryOperator.ValueInequality, Pop(), Pop()), instruction);
        }

        public override void OnBrfalse(Instruction instruction)
        {
            Branch(Pop().Negate(), instruction);
        }

        public override void OnBrtrue(Instruction instruction)
        {
            Branch(Pop(), instruction);
        }

        public override void OnBr(Instruction instruction)
        {
            Branch(null, instruction);
        }

        public override void ONWSitch(Instruction instruction)
        {
            Branch(Pop(), instruction);
        }

        public override void OnLeave(Instruction instruction)
        {
            Branch(null, instruction);
        }

        #endregion

        #region Object creation

        public override void OnNewarr(Instruction instruction)
        {
            Push(new ArrayCreationExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }

        public override void OnNewobj(Instruction instruction)
        {
            ConstructorInfo method = (ConstructorInfo)instruction.OperandMethodBase;
            List<Expression> arguments = new List<Expression>();
            for (int i = 0; i < method.GetParameters().Length; ++i)
            {
                arguments.Insert(0, Pop());
            }
            Push(new ObjectCreationExpression(instruction.Offset, method, arguments));
        }

        #endregion

        #region Conversions

        public override void OnConv_I(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(int), Pop()));
        }

        public override void OnConv_I1(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(byte), Pop()));
        }

        public override void OnConv_I2(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(short), Pop()));
        }

        public override void OnConv_I4(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(int), Pop()));
        }

        public override void OnConv_I8(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(long), Pop()));
        }

        public override void OnConv_R_Un(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(float), Pop()));
        }

        public override void OnConv_R4(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(float), Pop()));
        }

        public override void OnConv_R8(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(double), Pop()));
        }

        public override void OnConv_U(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(uint), Pop()));
        }

        public override void OnConv_U1(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(sbyte), Pop()));
        }

        public override void OnConv_U2(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(ushort), Pop()));
        }

        public override void OnConv_U4(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(uint), Pop()));
        }

        public override void OnConv_U8(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, typeof(ulong), Pop()));
        }

        public override void OnCastclass(Instruction instruction)
        {
            Push(new CastExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }

        public override void OnBox(Instruction instruction)
        {
            if (!instruction.OperandSystemType.IsValueType)
            {
                // In generic method, a box operation can be asked on an object
                // wich has no meaning. We simply ignore the instruction in that case
                // TODO: check in ECMA if this the expected behavior
                return;
            }
            Push(new BoxExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }

        public override void OnUnbox(Instruction instruction)
        {
            if (!instruction.OperandSystemType.IsValueType)
            {
                // In generic method, a box operation can be asked on an object
                // wich has no meaning. We simply ignore the instruction in that case
                // TODO: check in ECMA if this the expected behavior
                return;
            }
            Push(new UnboxExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }
        
        #endregion

        public override void OnLdftn(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, instruction.OperandMethod));
        }

        public override void OnPop(Instruction instruction)
        {
            Exec(Pop());
        }

        public override void OnThrow(Instruction instruction)
        {
            Expression expr = Pop();
            if (pendingPoint != null)
            {
                expr = new DebugPointExpression(pendingPoint, expr);
                pendingPoint = null;
            }
            client.Throw(expr);
        }

        public override void OnRet(Instruction instruction)
        {
            if (body.IsVoidMethod())
            {
                if (pendingPoint != null)
                {
                    client.Exec(new DebugPointExpression(pendingPoint));
                    pendingPoint = null;
                }
                client.Return(null);
            }
            else
            {
                Expression expr = Pop();
                if (pendingPoint != null)
                {
                    expr = new DebugPointExpression(pendingPoint, expr);
                    pendingPoint = null;
                }
                client.Return(expr);
            }
        }

        public override void OnDup(Instruction instruction)
        {
            var frame = PopFrame();
            DupFrame dup = frame as DupFrame;
            if (dup != null)
            {
                var primary = dup;
                var secondary = new DupFrame() { Expression = dup.Expression, ParentFrame = primary };
                dup.Expression = null;
                stack.Push(primary);
                stack.Push(secondary);
            }
            else 
            {
                Expression expr = ((ExpressionFrame)frame).Expression;
                if (!IsRegister(expr))
                {
                    var primary = new DupFrame();
                    var secondary = new DupFrame() { Expression = expr, ParentFrame = primary };
                    stack.Push(primary);
                    stack.Push(secondary);
                }
                else
                {
                    Push(expr);
                    Push(expr.Clone());
                }
            }
        }

        public override void OnVolatile(Instruction instruction)
        {
            
        }

        public override void OnConstrained(Instruction instruction)
        {
            // XXX: faut-il faire quelquechose ?
        }

        public override void OnEndfinally(Instruction instruction)
        {

        }

        public override void Visit(Instruction instruction)
        {
            if (instruction.Point != null && !built.IsDebuggerHidden)
            {
                if (pendingPoint == null)
                {
                    pendingPoint = instruction.Point;
                }
                else if (stack.Count == 0)
                {
                    client.Exec(new DebugPointExpression(pendingPoint));
                    pendingPoint = instruction.Point;
                }
            }
            base.Visit(instruction);
        }

        public override void OnIsinst(Instruction instruction)
        {
            Push(new SafeCastExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }



    }
}
