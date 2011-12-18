﻿
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.InvalidOperationException))]
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException()
            : base("InvalidOperation")
        {

        }

        public InvalidOperationException(string message)
            : base(message)
        {

        }
    }
}
