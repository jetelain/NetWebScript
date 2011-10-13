using System;

namespace NetWebScript
{
    /// <summary>
    /// Forces a class from an other assembly to be considered as marked with <see cref="ScriptAvailableAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=true)]
    public sealed class ForceScriptAvailableAttribute : Attribute
    {
        private readonly Type type;

        public ForceScriptAvailableAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}
