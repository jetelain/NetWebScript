﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.PdbInfo;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptDebugPointExpression : ScriptExpression
    {
        internal ScriptDebugPointExpression(PdbSequencePoint point, ScriptExpression expression)
            : base(point.Offset)
        {
            this.Point = point;
            this.Value = expression;
        }

        public ScriptExpression Value { get; internal set; }

        internal PdbSequencePoint Point { get; private set; }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            if (Value != null)
            {
                return String.Format("/*{0}*/{1}", Point.ToString(), Value.ToString());
            }
            return String.Format("/*{0}*/",Point.ToString());
        }

        public override ScriptExpression Negate()
        {
            return new ScriptDebugPointExpression(Point, Value.Negate());
        }
    }
}
