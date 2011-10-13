using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class PreserveCaseAttribute : Attribute
    {
    }
}
