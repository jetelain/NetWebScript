using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.ScriptAst
{
    public abstract class ScriptAssignableExpression : ScriptExpression
    {
        internal ScriptAssignableExpression(int? ilOffset)
            : base(ilOffset)
        {

        }
    }
}
