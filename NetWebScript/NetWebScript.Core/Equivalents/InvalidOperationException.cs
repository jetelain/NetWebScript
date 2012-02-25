
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.InvalidOperationException))]
    internal class InvalidOperationException : Exception
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
