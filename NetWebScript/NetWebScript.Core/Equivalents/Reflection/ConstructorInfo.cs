using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.ComponentModel;

namespace NetWebScript.Equivalents.Reflection
{
    [ScriptEquivalent(typeof(System.Reflection.ConstructorInfo))]
    [ScriptAvailable]
    [TypeConverter(typeof(ConstructorInfoConverter))]
    public class ConstructorInfo : MethodBase
    {
        private readonly Type type;
        private readonly JSFunction method;

        internal ConstructorInfo(Type type, JSFunction method)
        {
            this.type = type;
            this.method = method;
        }

        [ScriptBody(Inline = "new t()")]
        private static object CreateInstance(Type t)
        {
            throw new System.NotImplementedException();
        }

        public object Invoke(object[] parameters)
        {
            return method.Apply(CreateInstance(type), parameters);
        }

        public override object Invoke(object target, object[] parameters)
        {
            return method.Apply(target, parameters);
        }
    }

    internal class ConstructorInfoConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(System.Reflection.ConstructorInfo).IsAssignableFrom(sourceType))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var ctor = value as System.Reflection.ConstructorInfo;
            if (ctor != null)
            {
                return new ConstructorInfo(ctor.DeclaringType, new JSFunction(ctor));
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
