
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.NotSupportedException))]
    internal class NotSupportedException : Exception
    {
        public NotSupportedException()
            : base("NotSupportedException")
        {

        }
    }
}
