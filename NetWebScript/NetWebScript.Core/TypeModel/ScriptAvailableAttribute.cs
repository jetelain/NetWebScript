using System;

namespace NetWebScript
{
    /// <summary>
    /// Mark the type or the member to be available in JavaScript
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Assembly | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public sealed class ScriptAvailableAttribute : Attribute
    {
    }
}
