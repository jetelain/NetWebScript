using System;

namespace NetWebScript.Remoting
{
    /// <summary>
    /// Attribute to specify that a method (or all methods of a class) must be executed
    /// throw AJAX remoting when used in a script available type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class)]
    public sealed class ServerSideAttribute : Attribute
    {
    }
}
