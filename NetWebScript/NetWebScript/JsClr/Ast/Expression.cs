using System;
using System.Collections.Generic;
using NetWebScript.JsClr.AstBuilder.Cil;

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

        public virtual Expression Clone()
        {
            throw new NotImplementedException();
        }
    }

}
