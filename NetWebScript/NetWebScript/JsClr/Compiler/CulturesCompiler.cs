using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Globalization;
using System.IO;
using System.Globalization;
using NetWebScript.Equivalents.Globalization;
using NetWebScript.Remoting.Serialization;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.Compiler
{
    /// <summary>
    /// Generate culture-specific scripts
    /// </summary>
    class CulturesCompiler
    {
        private readonly List<CultureInfo> cultures = new List<CultureInfo>(); 

        internal void AddAssembly(System.Reflection.Assembly assembly)
        {
            var assemblyCultures = (CulturesAttribute)Attribute.GetCustomAttribute(assembly, typeof(CulturesAttribute));
            if (assemblyCultures != null)
            {
                foreach (var cultureName in assemblyCultures.Cultures)
                {
                    var culture = new CultureInfo(cultureName);
                    if (!cultures.Any(c => c.Name == culture.Name))
                    {
                        cultures.Add(culture);
                    }
                }
            }
        }

        internal IEnumerable<CultureInfo> Cultures 
        { 
            get
            {
                return cultures;
            }
        }

        internal void WriteFiles(string path, string name, ModuleMetadata metadata)
        {
            if (cultures.Count > 0)
            {
                var cache = new SerializerCache(metadata);
                foreach (var culture in cultures)
                {
                    var directoryPath = Path.Combine(path, culture.Name);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    WriteFile(Path.Combine(directoryPath, name + ".js"), culture, cache);
                }
            }
        }

        internal void WriteFile(string filepath, CultureInfo culture, SerializerCache cache)
        {
            using (var writer = new StreamWriter(new FileStream(filepath, FileMode.Create, FileAccess.Write)))
            {
                Write(writer, culture, cache);
            }
        }

        private void Write(TextWriter writer, CultureInfo culture, SerializerCache cache)
        {
            CultureInfoEquiv cultureEquiv = new CultureInfoEquiv(culture); 
            var serializer = new EvaluableSerializer(cache);
            writer.Write("Culture.Register(");
            serializer.Serialize(writer, cultureEquiv);
            writer.WriteLine(");");
        }
    }
}
