
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.NotImplementedException))]
    internal class NotImplementedException : Exception
    {
        public NotImplementedException()
           : base("NotImplemented")
        {

        }
    }
}
