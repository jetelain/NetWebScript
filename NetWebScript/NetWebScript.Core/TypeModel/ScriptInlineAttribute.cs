using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Field|AttributeTargets.Constructor, AllowMultiple=false, Inherited=false)]
    public sealed class ScriptBodyAttribute : Attribute
    {

        public ScriptBodyAttribute()
        {
        }

        public string Inline
        {
            get;
            set;
        }

        public string Body
        {
            get;
            set;
        }
    }
}
