
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.NotSupportedException))]
    public class NotSupportedException : Exception
    {
        public NotSupportedException()
            : base("NotSupportedException")
        {

        }
    }
}
