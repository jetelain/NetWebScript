
using System.Diagnostics;
namespace NetWebScript.Equivalents
{
    [ScriptAvailable, ScriptEquivalent(typeof(System.Exception))]
    public class Exception : Script.Error
    {
        [DebuggerHidden]
        public Exception()
        {
        }

        [DebuggerHidden]
        public Exception(string message)
        {
            base.Message = message;
        }

        [DebuggerHidden]
        public new virtual string Message
        {
            get { return base.Message; }
        }

        [DebuggerHidden]
        public override string ToString()
        {
            return base.Message;
        }

        [ScriptBody(Body="function(o){return o.message||o.toString()}")]
        private static string GetMessage(object obj)
        {
            throw new System.PlatformNotSupportedException();
        }

        [DebuggerHidden]
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
