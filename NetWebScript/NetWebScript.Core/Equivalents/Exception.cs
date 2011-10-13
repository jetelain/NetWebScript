
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.Exception))]
    public class Exception : Script.Error
    {
        public Exception()
        {
        }

        public Exception(string message)
        {
            base.Message = message;
        }

        public new virtual string Message
        {
            get { return base.Message; }
        }
    }
}
