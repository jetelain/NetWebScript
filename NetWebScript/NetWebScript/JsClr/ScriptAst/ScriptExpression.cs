using System;
using System.Collections.Generic;
using NetWebScript.JsClr.AstBuilder.Cil;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.ScriptAst
{

    public abstract class ScriptExpression : ScriptStatement
    {
        public abstract Type GetExpressionType();

        public int? IlOffset { get; protected set; }

        internal ScriptExpression(int? ilOffset)
        {
            this.IlOffset = ilOffset;
        }

        public virtual int? FirstIlOffset
        {
            get { return IlOffset; }
        }

		public virtual ScriptExpression Negate()
		{
			return new ScriptUnaryExpression(null, ScriptUnaryOperator.LogicalNot, this);
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

        public virtual ScriptExpression Clone()
        {
            throw new NotImplementedException();
        }
    }

}
