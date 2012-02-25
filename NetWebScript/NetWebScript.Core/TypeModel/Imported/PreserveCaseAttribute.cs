using System;

namespace NetWebScript
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class PreserveCaseAttribute : Attribute
    {
    }
}
