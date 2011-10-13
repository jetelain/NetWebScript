using System;

namespace NetWebScript
{
    /// <summary>
    /// Imported type (available in plain JavaScript)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class ImportedAttribute : Attribute
    {
        public CaseConvention Convention { get; set; }

        public string Name { get; set; }

        public ImportedAttribute(CaseConvention convention)
        {
            this.Convention = convention;
        }

        public ImportedAttribute()
            : this(CaseConvention.JavaScriptConvention)
        {

        }

    }
}
