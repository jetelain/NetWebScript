using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NetWebScript.JsClr.Runtime
{
    public static class CoreRuntime
    {
        private const string CoreScript = "NetWebScript.JsClr.Runtime.Core.xjs";
        private const string DebugClientScript = "NetWebScript.JsClr.Runtime.Debug.Client.xjs";

        public static void WriteRuntime(TextWriter writer, bool isDebuggable)
        {
            var assembly = typeof(CoreRuntime).Assembly;

            using ( var stream = assembly.GetManifestResourceStream(CoreScript) )
            {
                using ( var reader = new StreamReader(stream, Encoding.UTF8)) 
                {
                    writer.Write(reader.ReadToEnd());
                }
            }

            if (isDebuggable)
            {
                using (var stream = assembly.GetManifestResourceStream(DebugClientScript))
                {
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        writer.Write(reader.ReadToEnd());
                    }
                }
            }
        }

    }
}
