using System;

namespace NetWebScript
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class AnonymousObjectAttribute : Attribute
    {
        public CaseConvention Convention { get; private set; }

        public AnonymousObjectAttribute(CaseConvention convention)
        {
            this.Convention = convention;
        }

        public AnonymousObjectAttribute()
            : this(CaseConvention.JavaScriptConvention)
        {
        }
    }
}
