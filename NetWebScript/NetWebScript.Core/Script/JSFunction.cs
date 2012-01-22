using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.Script
{
    /// <summary>
    /// Represents a JavaScript Function. <br />
    /// In NetWebScript type system, it can be either a <see cref="Type"/>, <see cref="Delegate"/> or <see cref="MethodInfo"/>.
    /// </summary>
    [Imported(Name="Function", IgnoreNamespace=true)]
    public sealed class JSFunction
    {
        private readonly Type type;
        private readonly MethodBase method;
        private readonly Delegate deleg;

        public JSFunction(string arg0, string body)
        {
            throw new PlatformNotSupportedException();
        }

        internal JSFunction(Type type)
        {
            this.type = type;
        }

        internal JSFunction(MethodBase method)
        {
            this.method = method;
        }

        internal JSFunction(Delegate deleg)
        {
            this.deleg = deleg;
        }

        public object Apply(object target)
        {
            return Apply(target, new object[0]);
        }

        public object Apply(object target, params object[] arguments)
        {
            if ( deleg != null )
            {
                return deleg.DynamicInvoke(arguments);
            }
            if (method != null)
            {
                return method.Invoke(target, arguments);
            }
            throw new PlatformNotSupportedException();
        }

        public object Call(object target)
        {
            return Apply(target);
        }

        public object Call(object target, object arg0)
        {
            return Apply(target, arg0);
        }

        public object Call(object target, object arg0, object arg1)
        {
            return Apply(target, arg0, arg1);
        }

        public object Call(object target, object arg0, object arg1, object arg3)
        {
            return Apply(target, arg0, arg1, arg3);
        }

        internal MethodBase Method
        {
            get { return method; }
        }

        [IntrinsicProperty]
        public object Prototype { get; set; }
    }
}
