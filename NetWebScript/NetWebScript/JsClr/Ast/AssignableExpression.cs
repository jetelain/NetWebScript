using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.Ast
{
    public abstract class AssignableExpression : Expression
    {
        internal AssignableExpression(int? ilOffset)
            : base(ilOffset)
        {

        }
    }
}
