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

    internal sealed class ExpressionBuilder : ExpressionBuilderBase
    {

        private class ExpressionFrame : BuilderStackFrame { 
            public Expression Expression {get;set;}
        }
        private class DupFrame : BuilderStackFrame {
            public Expression Expression {get; set;}
            public DupFrame ParentFrame {get; set;}
        }

        private readonly Stack<BuilderStackFrame> stack = new Stack<BuilderStackFrame>();
        
        private readonly MethodAst built;
        private readonly Stack<IExpressionBuilderClient> clients = new Stack<IExpressionBuilderClient>();
        private IExpressionBuilderClient client;

        private bool enforceInline = false;

        private LocalVariable mustPushVariableRef;
        private AssignExpression associatedAssign;

        private PdbSequencePoint pendingPoint;

        private readonly HashSet<LocalVariable> registers = new HashSet<LocalVariable>();

        internal ExpressionBuilder(MethodAst built, MethodCil body, IExpressionBuilderClient client) : base(body)
        {
            this.built = built;
            this.client = client;
        }

        internal MethodAst MethodBuilt
        {
            get { return built; } 
        }

        internal void Reset()
        {
            registers.Clear();
            pendingPoint = null;
            mustPushVariableRef = null;
            associatedAssign = null;
            enforceInline = false;
        }

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

        protected override BuilderStackFrame PopToAssign()
        {
            return stack.Pop();
        }

        internal override Expression Pop()
        {
            var frame = PopToAssign();
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


        internal override void Push(Expression expr)
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

        protected override void Assign(int offset, AssignableExpression target, BuilderStackFrame value)
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

        protected override void Exec(Expression expr)
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

        protected override void Branch(Expression condition, Instruction instruction)
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

        #region InitializeArray

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

        protected override void OnCall(bool virt, Instruction instruction)
        {
            MethodBase method = instruction.OperandMethodBase;
            if (method.Name == "InitializeArray" && method.DeclaringType == typeof(System.Runtime.CompilerServices.RuntimeHelpers))
            {
                if (CallInitializeArray(instruction))
                {
                    return;
                }
            }
            base.OnCall(virt, instruction);
        }

        #endregion

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
            var frame = PopToAssign();
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

        internal List<LocalVariable> FullStackToVariables()
        {
            // TODO: Consider a register based version
            List<Expression> expressions = new List<Expression>(stack.Count);
            while (stack.Count > 0)
            {
                expressions.Insert(0, Pop());
            }
            List<LocalVariable> registers = new List<LocalVariable>();
            foreach (var expr in expressions)
            {
                var register = built.AllocateVariable(expr.GetExpressionType());
                ExecAssign(null, new VariableReferenceExpression(null, register), expr);
                registers.Add(register);
            }
            return registers;
        }
    }
}
