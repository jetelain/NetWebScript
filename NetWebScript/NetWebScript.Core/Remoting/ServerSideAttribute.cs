using System;

namespace NetWebScript.Remoting
{
    /// <summary>
    /// Specifies that a type methods should be executed throw AJAX remoting when used in a script available type.
    /// Target class must inherits from <see cref="MarshalByRefObject"/>.
    /// </summary>
    /// <remarks>
    /// A class with this attribute is a trust boundary. You must not trust callers, ensures that all provided data is valid and that user has the appropriate rights.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ServerSideAttribute : Attribute
    {
    }
}
