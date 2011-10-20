using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.ComponentModel;

namespace NetWebScript.Equivalents.Reflection
{
    [ScriptEquivalent(typeof(System.Reflection.MethodInfo))]
    [ScriptAvailable]
    [TypeConverter(typeof(MethodInfoConverter))]
    public class MethodInfo : MethodBase
    {
        private readonly JSFunction method;

        internal MethodInfo(JSFunction method)
        {
            this.method = method;
        }

        public override object Invoke(object target, object[] parameters)
        {
            return method.Apply(target, parameters);
        }
    }

    internal class MethodInfoConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (typeof(System.Reflection.MethodInfo).IsAssignableFrom(sourceType))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var method = value as System.Reflection.MethodInfo;
            if (method != null)
            {
                return new MethodInfo(new JSFunction(method));
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}
