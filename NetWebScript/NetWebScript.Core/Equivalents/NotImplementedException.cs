
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.NotImplementedException))]
    public class NotImplementedException : Exception
    {
        public NotImplementedException()
           : base("NotImplemented")
        {

        }
    }
}
