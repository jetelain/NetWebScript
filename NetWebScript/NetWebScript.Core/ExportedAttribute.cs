using System;

namespace NetWebScript
{
    /// <summary>
    /// Make a class marked with <see cref="ScriptAvailableAttribute"/> visible to plain JavaScript.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class ExportedAttribute : Attribute
    {
        public CaseConvention Convention { get; set; }

        public string Name { get; set; }

        public ExportedAttribute()
        {
            Convention = CaseConvention.CSharpConvention;
        }

    }
}
