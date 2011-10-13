using System;

namespace NetWebScript.Script
{
    [Imported, IgnoreNamespace]
    public sealed class Window
    {
        private Window()
        {
        }

        public static void ClearInterval(int intervalID)
        {
            throw new PlatformNotSupportedException();
        }

        public static void ClearTimeout(int timeoutID)
        {
            throw new PlatformNotSupportedException();
        }

        public static int SetInterval(Delegate code, int milliseconds)
        {
            throw new PlatformNotSupportedException();
        }

        public static int SetTimeout(Delegate code, int milliseconds)
        {
            throw new PlatformNotSupportedException();
        }

        public static void Alert(string p)
        {
            throw new PlatformNotSupportedException();
        }

        internal static object Eval(string data)
        {
            throw new NotImplementedException();
        }
    }
}

