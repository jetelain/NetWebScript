using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NetWebScript.JsClr.Runtime
{
    public static class CoreRuntime
    {
        private const string CoreScript = "NetWebScript.JsClr.Runtime.Core.js";
        private const string DebugClientScript = "NetWebScript.JsClr.Runtime.Debug.Client.js";
        private const string JQueryScript = "NetWebScript.JsClr.Runtime.jquery-1.6.4.min.js";

        public static string JQueryFilename { get { return "jquery-1.6.4.min.js"; } }

        public static void WriteJQuery(TextWriter writer)
        {
            var assembly = typeof(CoreRuntime).Assembly;

            using (var stream = assembly.GetManifestResourceStream(JQueryScript))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    writer.Write(reader.ReadToEnd());
                }
            }
        }

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
