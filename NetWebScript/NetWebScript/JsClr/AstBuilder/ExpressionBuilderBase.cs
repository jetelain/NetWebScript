using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.Ast;
using System.Reflection;

namespace NetWebScript.JsClr.AstBuilder
{
    /// <summary>
    /// Base class of <see cref="ExpressionBuilder" /> : interprets simple instructions to high-end operations.
    /// </summary>
    internal abstract class ExpressionBuilderBase : InstructionVisitor
    {
        private static readonly PropertyInfo ArrayLengthProperty = typeof(Array).GetProperty("Length");

        protected readonly MethodCil body;

        protected ExpressionBuilderBase ( MethodCil body )
        {
            this.body = body;
        }

        protected class BuilderStackFrame { }

        /// <summary>
        /// Push a value on evaluation stack
        /// </summary>
        /// <param name="expr">Value to push</param>
        internal abstract void Push(Expression expr);

        /// <summary>
        /// Pop a value from evaluation stack to make an asignement with <see cref="Assign" />.
        /// </summary>
        /// <returns></returns>
        protected abstract BuilderStackFrame PopToAssign();

        /// <summary>
        /// Pop a value from evaluation stack
        /// </summary>
        /// <returns></returns>
        internal abstract Expression Pop();

        /// <summary>
        /// Execute an expression that has no result, or that should executed "right now"
        /// </summary>
        /// <param name="expr"></param>
        protected abstract void Exec(Expression expr);

        /// <summary>
        /// Perform an assignement
        /// </summary>
        /// <param name="offset">IL Offset of instruction</param>
        /// <param name="target">Target of assignement</param>
        /// <param name="value">Value to assign (<see cref="PopToAssign" /></param>
        protected abstract void Assign(int offset, AssignableExpression target, BuilderStackFrame value);

        /// <summary>
        /// Perform a branch instruction
        /// </summary>
        /// <param name="condition">Optionaly, expression that returns a boolean required to make branch operation (jump if true)</param>
        /// <param name="instruction">Instruction that ask branch operation</param>
        protected abstract void Branch(Expression condition, Instruction instruction);

        /// <summary>
        /// Determines if value type is ByRef
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        protected static bool IsByRefValue(Expression expr)
        {
            var type = expr.GetExpressionType();
            return type != null && type.IsByRef;
        }

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

        public override void OnLdarga(Instruction instruction)
        {
            int index = instruction.OperandArgumentIndex;
            if (!body.Method.IsStatic)
            {
                if (index == 0)
                {
                    Unsupported(instruction);
                }
                index--;
            }
            Push(new MakeByRefArgumentExpression(instruction.Offset, body.Arguments[index]));
        }

        private Expression ArgumentReference(Instruction instruction, int index)
        {
            if (!body.Method.IsStatic)
            {
                if (index == 0)
                {
                    return new ThisReferenceExpression(instruction.Offset, body.Method.DeclaringType);
                }
                index--; // the Parameters collection dos not contain the implict this argument
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
            var variable = body.Variables[instruction.OperandVariableIndex];
            Push(new MakeByRefVariableExpression(instruction.Offset, variable));
        }

        public override void OnStloc(Instruction instruction)
        {
            Assign(instruction.Offset, VariableReference(instruction, instruction.OperandVariableIndex), PopToAssign());
        }

        public override void OnStloc_0(Instruction instruction)
        {
            Assign(instruction.Offset, VariableReference(instruction, 0), PopToAssign());
        }

        public override void OnStloc_1(Instruction instruction)
        {
            Assign(instruction.Offset, VariableReference(instruction, 1), PopToAssign());
        }

        public override void OnStloc_2(Instruction instruction)
        {
            Assign(instruction.Offset, VariableReference(instruction, 2), PopToAssign());
        }

        public override void OnStloc_3(Instruction instruction)
        {
            Assign(instruction.Offset, VariableReference(instruction, 3), PopToAssign());
        }

        VariableReferenceExpression VariableReference(Instruction instruction, int index)
        {
            return new VariableReferenceExpression(instruction.Offset, body.Variables[index]);
        }

        #endregion

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

        public override void OnInitobj(Instruction instruction)
        {
            if (!instruction.OperandSystemType.IsValueType)
            {
                Expression expr = Pop();
                if (IsByRefValue(expr))
                {
                    Exec(new ByRefSetExpression(instruction.Offset, expr, new LiteralExpression(instruction.Offset, null)));
                    return;
                }
            }
            base.OnInitobj(instruction);
        }

        public override void OnLdtoken(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, instruction.Operand));
        }

        public override void OnLdftn(Instruction instruction)
        {
            Push(new LiteralExpression(instruction.Offset, instruction.OperandMethod));
        }

        #endregion

        #region Conversions

        private void NumberConversion(Instruction instruction, Type type)
        {
            Expression expr = Pop();
            if (IsByRefValue(expr))
            {
                // Forbids conversion of a reference to pointer
                Unsupported(instruction);
            }
            if (type == expr.GetExpressionType())
            {
                Push(expr);
            }
            else
            {
                Push(new CastExpression(instruction.Offset, type, expr));
            }
        }

        public override void OnConv_I(Instruction instruction)
        {
            NumberConversion(instruction, typeof(int));
        }

        public override void OnConv_I1(Instruction instruction)
        {
            NumberConversion(instruction, typeof(byte));
        }

        public override void OnConv_I2(Instruction instruction)
        {
            NumberConversion(instruction, typeof(short));
        }

        public override void OnConv_I4(Instruction instruction)
        {
            NumberConversion(instruction, typeof(int));
        }

        public override void OnConv_I8(Instruction instruction)
        {
            NumberConversion(instruction, typeof(long));
        }

        public override void OnConv_R_Un(Instruction instruction)
        {
            NumberConversion(instruction, typeof(float));
        }

        public override void OnConv_R4(Instruction instruction)
        {
            NumberConversion(instruction, typeof(float));
        }

        public override void OnConv_R8(Instruction instruction)
        {
            NumberConversion(instruction, typeof(double));
        }

        public override void OnConv_U(Instruction instruction)
        {
            NumberConversion(instruction, typeof(uint));
        }

        public override void OnConv_U1(Instruction instruction)
        {
            NumberConversion(instruction, typeof(sbyte));
        }

        public override void OnConv_U2(Instruction instruction)
        {
            NumberConversion(instruction, typeof(ushort));
        }

        public override void OnConv_U4(Instruction instruction)
        {
            NumberConversion(instruction, typeof(uint));
        }

        public override void OnConv_U8(Instruction instruction)
        {
            NumberConversion(instruction, typeof(ulong));
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
                // TODO: check in ECMA if this is the expected behavior
                return;
            }
            Push(new UnboxExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }

        #endregion

        #region Reference manipulation

        public override void OnStobj(Instruction instruction)
        {
            var value = Pop();
            var target = Pop();
            if (!IsByRefValue(target))
            {
                Unsupported(instruction);
            }
            Exec(new ByRefSetExpression(instruction.Offset, target, value));
        }

        public override void OnStind_Ref(Instruction instruction)
        {
            var value = Pop();
            var target = Pop();
            if (!IsByRefValue(target))
            {
                Unsupported(instruction);
            }
            Exec(new ByRefSetExpression(instruction.Offset, target, value));
        }

        public override void OnLdind_Ref(Instruction instruction)
        {
            var target = Pop();
            if (!IsByRefValue(target))
            {
                Unsupported(instruction);
            }
            Push(new ByRefGetExpression(instruction.Offset, target));
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

        #region Fields

        public override void OnLdfld(Instruction instruction)
        {
            Push(new FieldReferenceExpression(instruction.Offset, Pop(), instruction.OperandField));
        }

        public override void OnLdflda(Instruction instruction)
        {
            Push(new MakeByRefFieldExpression(instruction.Offset, Pop(), instruction.OperandField));
        }

        public override void OnLdsfld(Instruction instruction)
        {
            Push(new FieldReferenceExpression(instruction.Offset, null, instruction.OperandField));
        }

        public override void OnLdsflda(Instruction instruction)
        {
            Push(new MakeByRefFieldExpression(instruction.Offset, null, instruction.OperandField));
        }

        public override void OnStfld(Instruction instruction)
        {
            BuilderStackFrame value = PopToAssign();
            Expression target = Pop();
            Assign(instruction.Offset, new FieldReferenceExpression(instruction.Offset, target, instruction.OperandField), value);
        }

        public override void OnStsfld(Instruction instruction)
        {
            Assign(instruction.Offset, new FieldReferenceExpression(instruction.Offset, null, instruction.OperandField), PopToAssign());
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
            BuilderStackFrame value = PopToAssign();
            Expression index = Pop();
            Expression array = Pop();
            Assign(instruction.Offset, new ArrayIndexerExpression(instruction.Offset, array, index), value);
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

        #region Method call

        public override void OnCallvirt(Instruction instruction)
        {
            OnCall(true, instruction);
        }

        public override void OnCall(Instruction instruction)
        {
            OnCall(false, instruction);
        }

        protected virtual void OnCall(bool virt, Instruction instruction)
        {
            MethodBase method = instruction.OperandMethodBase;
            
            List<Expression> arguments = new List<Expression>();
            for (int i = 0; i < method.GetParameters().Length; ++i)
            {
                arguments.Insert(0, Pop());
            }
            Expression target = null;
            if (!method.IsStatic)
            {
                target = Pop();

                Type type = target.GetExpressionType();
                if (type != null && type.IsByRef)
                {
                    target = target.GetRefValue();
                }
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

        public override void OnPop(Instruction instruction)
        {
            // TODO: Should ignore "pop" of expressions that have no side effect
            Exec(Pop());
        }

        public override void OnVolatile(Instruction instruction)
        {

        }

        public override void OnConstrained(Instruction instruction)
        {
            // XXX: Should web do something ?
        }

        public override void OnEndfinally(Instruction instruction)
        {

        }

        public override void OnIsinst(Instruction instruction)
        {
            Push(new SafeCastExpression(instruction.Offset, instruction.OperandSystemType, Pop()));
        }
    }
}
