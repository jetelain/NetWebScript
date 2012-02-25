using System;

namespace NetWebScript
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ScriptEquivalentAttribute : Attribute
    {
        private readonly Type type;

        public ScriptEquivalentAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}
