using System;

namespace NetWebScript
{
    /// <summary>
    /// Method that does not exists on imported object, but that system should add to object prototype.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ImportedExtensionAttribute : Attribute
    {
        public ImportedExtensionAttribute()
        {
        }
    }
}
