using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Helped;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class ObjectType : ScriptTypeHelped
    {
        public ObjectType(ScriptSystem system) : base(system, typeof(object), typeof(NetWebScript.Equivalents.ObjectHelper))
        {
            var safeGHC = system.GetScriptMethod(new Func<object, int>(NetWebScript.Equivalents.ObjectHelper.SafeGetHashCode).Method);
            var safeEq = system.GetScriptMethod(new Func<object, object, bool>(NetWebScript.Equivalents.ObjectHelper.SafeEquals).Method);

            constructors.Add(new ObjectConstructor(this));
            methods.Add(new NativeMethod(this, type.GetMethod("ToString", Type.EmptyTypes), "toString"));
            methods.Add(new NativeMethodHelper(this, type.GetMethod("GetHashCode", Type.EmptyTypes), "$ghc", safeGHC));
            methods.Add(new NativeMethodHelper(this, type.GetMethod("Equals", new Type[] { typeof(object) }), "$eq", safeEq));
            
        }

        public override string TypeId
        {
            get
            {
                return "Object";
            }
        }

    }
}
