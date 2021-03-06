﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class ScriptConditionExpression : ScriptExpression
    {

        public ScriptConditionExpression(int? ilOffset, ScriptExpression cond, ScriptExpression @then, ScriptExpression @else)
            : base(ilOffset)
        {
            this.Condition = cond;
            this.Then = @then;
            this.Else = @else;
        }


        public ScriptExpression Condition { get; internal set; }
        public ScriptExpression Then { get; internal set; }
        public ScriptExpression Else { get; internal set; }


        public override void Accept(IScriptStatementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override T Accept<T>(IScriptStatementVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override string ToString()
        {
            return String.Format("({0}?{1}:{2})", Condition.ToString(),Then.ToString(),Else.ToString());
        }
    }
}
