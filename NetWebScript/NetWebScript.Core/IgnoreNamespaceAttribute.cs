using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript
{
    /// <summary>
    /// On a class with <see cref="ImportedAttribute"/>, ignore C# namespace in script version
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class IgnoreNamespaceAttribute : Attribute
    {
    }
}
