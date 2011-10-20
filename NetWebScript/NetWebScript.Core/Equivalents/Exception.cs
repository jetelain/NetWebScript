
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

        public override string ToString()
        {
            return base.Message;
        }

        [ScriptBody(Body="function(o){return o.message||o.toString()}")]
        public static string GetMessage(object obj)
        {
            throw new System.PlatformNotSupportedException();
        }

        public static Exception Convert(object obj)
        {
            var exception = obj as Exception;
            if (exception != null)
            {
                return exception;
            }
            return new Exception(GetMessage(obj));
        }

    }
}
