using System;
using System.Collections.Generic;
using NetWebScript.JsClr.AstBuilder.Cil;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.Ast
{

    public abstract class Expression : Statement
    {
        public abstract Type GetExpressionType();

        public int? IlOffset { get; protected set; }

        internal Expression(int? ilOffset)
        {
            this.IlOffset = ilOffset;
        }

        public virtual int? FirstIlOffset
        {
            get { return IlOffset; }
        }

		public virtual Expression Negate()
		{
			return new UnaryExpression(null, UnaryOperator.LogicalNot, this);
		}

        //public virtual bool IsAssignementOrSame(Expression b)
        //{
        //    return this == b;
        //}

        /// <summary>
        /// Value is constant within method, and have no side effet
        /// </summary>
        /// <returns></returns>
        public virtual bool IsConstInMethod()
        {
            return false;
        }

        /// <summary>
        /// Value have a side effect
        /// </summary>
        /// <returns></returns>
        public virtual bool HasSideEffect()
        {
            return true;
        }

        public virtual Expression Clone()
        {
            throw new NotImplementedException();
        }

        public virtual Expression GetRefValue()
        {
            Contract.Requires(IlOffset != null);
            Contract.Requires(GetExpressionType() != null && GetExpressionType().IsByRef);
            return new ByRefGetExpression(IlOffset.Value, this);
        }
    }

}
