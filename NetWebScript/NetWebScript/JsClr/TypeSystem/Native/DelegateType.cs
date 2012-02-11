using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class DelegateType : NativeType
    {
        public DelegateType(ScriptSystem system, Type type)
            : base(system, "Function", type)
        {
            var invokeMethod = type.GetMethod("Invoke");
            //var ctor = type.GetConstructor(new Type[]{typeof(object), typeof(IntPtr)});
            //methods.Add(new DelegateConstructor(system, this, ctor));
            methods.Add(new DelegateMethodInvoke(this, invokeMethod));
        }
    }
}
