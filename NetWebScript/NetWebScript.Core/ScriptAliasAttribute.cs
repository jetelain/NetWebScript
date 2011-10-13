using System;

namespace NetWebScript
{
    /// <summary>
    /// In an imported class (<see cref="ImportedAttribute"/>), can be applied on a static method to
    /// replace method name in a global scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ScriptAliasAttribute : Attribute
    {
        public String Alias { get; private set; }

        public ScriptAliasAttribute(string alias)
        {
            this.Alias = alias;
        }
    }
}
