using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptField
    {
        string SlodId { get; }

        FieldInfo Field { get; }

        IScriptType Owner { get; }

        IFieldInvoker Invoker { get; }
    }
}
