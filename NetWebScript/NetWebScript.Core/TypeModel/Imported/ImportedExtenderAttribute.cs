using System;

namespace NetWebScript
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class ImportedExtenderAttribute : Attribute
    {
        private readonly Type extendedType;

        public ImportedExtenderAttribute(Type extendedType)
        {
            this.extendedType = extendedType;
        }

        public Type ExtendedType
        {
            get { return extendedType; }
        }
    }
}
