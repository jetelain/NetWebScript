using System;
using System.Diagnostics.Contracts;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.TypeSystem.Helped;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class ArrayType : NativeType
    {
        public ArrayType(ScriptSystem system, Type type)
            : base(system, "Array", type)
        {
        }
    }
}
