using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Standard;

namespace NetWebScript.JsClr.TypeSystem
{
    interface IScriptTypeExtender
    {
        IEnumerable<ScriptMethod> Extensions { get; }
    }
}
