using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class ObjectType : NativeType
    {
        public ObjectType() : base("Object", typeof(object))
        {
            methods.Add(new ObjectConstructor(this));
            methods.Add(new NativeMethod(this, type.GetMethod("ToString", Type.EmptyTypes), "toString"));
            methods.Add(new NativeMethod(this, type.GetMethod("GetHashCode", Type.EmptyTypes), "$ghc"));
            methods.Add(new NativeMethod(this, type.GetMethod("Equals", new Type[]{typeof(object)}), "$eq"));
            
        }

    }
}
