using System.IO;
using System.Text;

namespace NetWebScript.JsClr.Runtime
{
    public static class CoreRuntime
    {
        private const string JQueryScript = "NetWebScript.JsClr.Runtime.jquery-1.6.4.min.js";

        public static string JQueryFilename { get { return "jquery-1.6.4.min.js"; } }

        public static void WriteJQuery(Stream output)
        {
            var assembly = typeof(CoreRuntime).Assembly;

            using (var stream = assembly.GetManifestResourceStream(JQueryScript))
            {
                stream.CopyTo(output);
            }
        }
    }
}
