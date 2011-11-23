using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace MappingGenerator
{
    /// <summary>
    /// Quick and dirty API mapping generator for JQuery.
    /// 
    /// Important: result need to be (a little) manually reworked 
    /// </summary>
    class Program
    {
        private class Class
        {
            internal List<Method> methods = new List<Method>();
            internal List<Property> properties = new List<Property>();
            internal string name;
            internal bool isMap;
        }

        private class Method
        {
            internal string name;
            internal bool isStatic;
            internal string csReturnType;
            internal string description;
            internal List<Argument> arguments = new List<Argument>();
        }
        private class Property
        {
            internal string name;
            internal bool isStatic;
            internal string cstype;
            internal string description;
        }
        private class Argument
        {
            internal string name;
            internal string description;
            internal string cstype;
        }

        private class IntermediateArgument
        {
            internal string name;
            internal string description;
            internal string jstype;
            internal bool isOptional;
        }

        private static List<Class> classes = new List<Class>();

        static void Main(string[] args)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"http://api.jquery.com/api/");

            foreach (XmlElement entry in doc.SelectNodes("/api/entries/entry"))
            {
                var type = entry.GetAttribute("type");
                switch(type)
                {
                    case "method":
                        HandleMethod(entry);
                        break;
                    case "property":
                        HandleProperty(entry);
                        break;
                }
            }


            using (var writer = new StreamWriter(new FileStream("jQuery.cs", FileMode.Create, FileAccess.Write)))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using NetWebScript.Script.HTML;");
                writer.WriteLine("using NetWebScript.Script.Xml;");
                writer.WriteLine();
                writer.WriteLine("namespace NetWebScript.Script");
                writer.WriteLine("{");
                foreach (var @class in classes)
                {
                    WriteClass(writer, @class);
                }
                writer.WriteLine("}");
            }
        }

        private static string SafeName(string name)
        {
            name = name.Replace("-", "").Replace(" ", "");
            var i = name.IndexOf('(');
            if (i != -1)
            {
                name = name.Substring(0, i).Trim();
            }
            return name;
        }


        private static void WriteClass(StreamWriter writer, Class @class)
        {
            @class.properties.Sort((a, b) => a.name.CompareTo(b.name));
            @class.methods.Sort((a, b) => a.name.CompareTo(b.name));

            var className = CSCase(@class.name);

            if (className == "Event")
            {
                className = "JQueryEvent";
            }

            if (className == "JQuery")
            {
                writer.WriteLine("    [Imported(Name=\"$\")]");
            }
            else if (@class.isMap)
            {
                writer.WriteLine("    [AnonymousObject]");
            }
            else
            {
                writer.WriteLine("    [Imported]");
            }
            
            writer.WriteLine("    public sealed class {0}", className);
            writer.WriteLine("    {");
            if (!@class.isMap)
            {
                writer.WriteLine("        private {0}() {{ }}", className);
            }
            foreach (var property in @class.properties)
            {
                //if (IsDelegateType(property.type))
                //{
                //    property.type = DelegateType(property.name);
                //}
                property.name = SafeName(property.name);
                writer.WriteLine("        /// <summary>");
                writer.WriteLine("        /// {0}", property.description.Replace("\r", "").Replace("\n", ""));
                writer.WriteLine("        /// </summary>");
                if (!@class.isMap)
                {
                    writer.WriteLine("        [IntrinsicProperty]");
                }
                writer.Write    ("        public ");
                if (property.isStatic)
                {
                    writer.Write("static ");
                }
                writer.Write(property.cstype);
                writer.Write(" ");
                writer.Write(CSCase(property.name));
                if (!@class.isMap)
                {
                    writer.WriteLine(" { get; set; }");
                }
                else
                {
                    writer.WriteLine(";");
                }
                writer.WriteLine();
            }

            foreach (var method in @class.methods)
            {
                writer.WriteLine("        /// <summary>");
                writer.WriteLine("        /// {0}", method.description.Replace("\r", "").Replace("\n", ""));
                writer.WriteLine("        /// </summary>");
                foreach (var arg in method.arguments)
                {
                    arg.name = SafeName(arg.name);
                    writer.WriteLine("        /// <param name=\"{0}\">{1}</param>", arg.name, arg.description.Replace("\r", "").Replace("\n", ""));
                }
                if (CSCase(method.name) == "JQuery")
                {
                    writer.WriteLine("        [ScriptAlias(\"$\")]");
                    writer.Write(    "        public static JQuery Query");
                }
                else
                {
                    writer.Write("        public ");
                    if (method.isStatic)
                    {
                        writer.Write("static ");
                    }
                    writer.Write(method.csReturnType);
                    writer.Write(" ");
                    writer.Write(CSCase(method.name));
                }
                writer.Write("(");
                bool first = true;
                foreach (var arg in method.arguments)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Write(", ");
                    }
                    writer.Write(arg.cstype);
                    writer.Write(" ");
                    writer.Write(CSSafe(arg.name));
                }
                writer.WriteLine(")");
                writer.WriteLine("        {");
                writer.WriteLine("            throw new PlatformNotSupportedException();");
                writer.WriteLine("        }");
                writer.WriteLine();
            }
            writer.WriteLine("    }");
        }

        private static string CSCase(string p)
        {
            return p.Substring(0, 1).ToUpperInvariant() + p.Substring(1);
        }

        private static string CSSafe(string p)
        {
            if (p == "namespace" || p == "object" || p == "event" || p == "false" || p == "switch")
            {
                return "@" + p;
            }
            return p;
        }

        private static string CSNameOf(string p)
        {
            if (p == "undefined")
            {
                return "void";
            }
            if (p == "Boolean"||p=="boolean")
            {
                return "bool";
            }
            if (p == "String")
            {
                return "string";
            }
            if (p == "Number")
            {
                return "double";
            }
            if (p == "Integer")
            {
                return "int";
            }
            if (p == "Function")
            {
                return "Delegate";
            }
            if (p == "Selector" || p == "selector")
            {
                return "string";
            }
            if (p == "Document" || p == "document")
            {
                return "IHTMLDocument";
            }
            if (p == "Element" || p == "element" || p == "Elements" || p == "elements")
            {
                return "IHTMLElement";
            }

            if (p == "" || p == "Anything" || p == "Any" || p == "Map" || p == "Object" || (p.Contains(',') && !p.Contains('<')))
            {
                return "object";
            }
            if (p == "jQuery object")
            {
                return "JQuery";
            }
            if (p == "Event" || p == "event")
            {
                return "JQueryEvent";
            }
            if (p == "Callback" || p == "callback")
            {
                return "Action";
            }
            if (p == "HTML")
            {
                return "string";
            }
            if (p == "jqXHR")
            {
                return "XMLHttpRequest";
            }
            if (p == "XMLDocument")
            {
                return "IXmlDocument";
            }
            return CSCase(p);
        }

        private static Class GetClass(string name)
        {
            var @class = classes.FirstOrDefault(c => c.name == name);
            if (@class == null)
            {
                @class = new Class() { name = name };
                classes.Add(@class);
            }
            return @class; 
        }

        private static void HandleProperty(XmlElement entry)
        {
            bool isStatic = false;
            var returnType = entry.GetAttribute("return").Trim();
            var name = entry.GetAttribute("name").Trim();
            var desc = entry.SelectSingleNode("desc").InnerText.Trim();
            Class ownerClass;
            int i = name.IndexOf('.');
            if (i != -1)
            {
                var className = name.Substring(0, i);
                name = name.Substring(i + 1);
                if (className == "jQuery")
                {
                    isStatic = true;
                }
                ownerClass = GetClass(className);
            }
            else
            {
                ownerClass = GetClass("jQuery");
            }
            if (name.Contains('.'))
            {
                return;
            }

            string cstype;
            if (IsDelegateType(returnType))
            {
                cstype = DelegateType(name, out name);
            }
            else
            {
                cstype = CSNameOf(returnType);
            }
            ownerClass.properties.Add(new Property() { description = desc, isStatic = isStatic, name = name, cstype = cstype });
        }

        private static void HandleMethod(XmlElement entry)
        {
            bool isStatic = false;
            var returnType = entry.GetAttribute("return").Trim();
            var name = entry.GetAttribute("name").Trim();
            var desc = entry.SelectSingleNode("desc").InnerText.Trim();
            Class ownerClass;
            int i = name.IndexOf('.');
            if (i != -1)
            {
                var className = name.Substring(0, i);
                name = name.Substring(i + 1);
                if (className == "jQuery")
                {
                    isStatic = true;
                }
                ownerClass = GetClass(className);
            }
            else
            {
                ownerClass = GetClass("jQuery");
            }
            
            foreach (XmlElement signature in entry.SelectNodes("signature"))
            {

                var method = new Method();
                method.description = desc;
                method.name = name;
                method.csReturnType = CSNameOf(returnType);
                method.isStatic = isStatic;

                var args = new List<IntermediateArgument>();

                foreach (XmlElement argument in signature.SelectNodes("argument"))
                {
                    var arg = new IntermediateArgument()
                    {
                        name = argument.GetAttribute("name").Trim(),
                        jstype = argument.GetAttribute("type").Trim(),
                        description = argument.SelectSingleNode("desc").InnerText.Trim(),
                        isOptional = argument.GetAttribute("optional") == "true"
                    };
                    if (args.Any(a => a.name == arg.name))
                    {
                        continue;
                    }
                    args.Add(arg);
                    if (arg.jstype == "map" || arg.jstype == "Map")
                    {
                        var options = argument.SelectNodes("option");
                        if (options.Count > 0)
                        {
                            var specificname = CSCase(ownerClass.name) + CSCase(method.name) + CSCase(arg.name);
                            arg.jstype = specificname;
                            GenerateSpecific(specificname, options);
                        }
                    }
                }
                AddMethod(ownerClass, method, args);
                
            }

        }

        private static void AddMethod(Class ownerClass, Method method, List<IntermediateArgument> args)
        {
            var optional = args.FirstOrDefault(a => a.isOptional);
            if (optional != null)
            {
                optional.isOptional = false;
                var overload = new Method() { description = method.description, name = method.name, csReturnType = method.csReturnType, isStatic = method.isStatic };
                var overloadArgs = new List<IntermediateArgument>(args);
                overloadArgs.Remove(optional);
                AddMethod(ownerClass, overload, overloadArgs);
            }

            var multiple = args.FirstOrDefault(a => a.jstype.Contains(','));
            if (multiple != null)
            {
                var idx = args.IndexOf(multiple);
                foreach (var jstype in multiple.jstype.Split(','))
                {
                    var overload = new Method() { description = method.description, name = method.name, csReturnType = method.csReturnType, isStatic = method.isStatic };
                    var overloadArgs = new List<IntermediateArgument>(args);
                    overloadArgs[idx] = new IntermediateArgument() { name = multiple.name, jstype = jstype.Trim(), description = multiple.description };
                    AddMethod(ownerClass, overload, overloadArgs);
                }
                return;
            }


            foreach (var intermediate in args)
            {
                Argument arg = new Argument() { description = intermediate.description };
                if (IsDelegateType(intermediate.jstype))
                {
                    arg.cstype = DelegateType(intermediate.name, out arg.name);
                }
                else
                {
                    arg.name = intermediate.name;
                    arg.cstype = CSNameOf(intermediate.jstype);
                }
                method.arguments.Add(arg);
            }

            AddMethod(ownerClass, method);
        }

        private static void GenerateSpecific(string specificname, XmlNodeList options)
        {
            var @class = new Class() { name = specificname, isMap = true };

            foreach (XmlElement element in options)
            {
                var property = new Property()
                {
                    description = element.SelectSingleNode("desc").InnerText.Trim(),
                    isStatic = false
                };

                string name = element.GetAttribute("name");
                string jstype = element.GetAttribute("type");
                if (IsDelegateType(jstype))
                {
                    property.cstype = DelegateType(name, out property.name);
                }
                else
                {
                    property.name = name;
                    property.cstype = CSNameOf(jstype);
                }
                @class.properties.Add(property);

            }

            classes.Add(@class);
        }


        private static void AddMethod(Class ownerClass, Method original)
        {
            if (!ownerClass.methods.Any(a => Equals(a, original)))
            {
                ownerClass.methods.Add(original);
            }
        }

        private static bool Equals(Method a, Method b)
        {
            return a.name == b.name && a.csReturnType == b.csReturnType && a.arguments.Count == b.arguments.Count && a.arguments.Zip(b.arguments, (aa, ab) => aa.cstype == ab.cstype).All(r => r);
        }

        private static bool IsDelegateType(string typename)
        {
            return string.Equals("Function", typename, StringComparison.OrdinalIgnoreCase);
        }

        private static string DelegateType(string name, out string cleanname)
        {
            if (name == "handler(eventObject)")
            {
                cleanname = "handler";
                return "Func<JQueryEvent,bool>";
            }

            int i = name.IndexOf('(');
            if (i != -1)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("Func<");
                foreach (var arg in name.Substring(i + 1, name.LastIndexOf(')') - i - 1).Split(','))
                {

                    var cleanarg = arg.Trim();
                    if (!string.IsNullOrEmpty(cleanarg))
                    {
                        var supposedType = "object";
                        if (cleanarg == "index")
                        {
                            supposedType = "int";
                        }
                        else if ( cleanarg == "element" )
                        {
                            supposedType = "IHTMLElement";
                        }
                        else if (cleanarg == "eventObject")
                        {
                            supposedType = "JQueryEvent";
                        }
                        else if (cleanarg == "jqXHR" || cleanarg == "XMLHttpRequest")
                        {
                            supposedType = "XMLHttpRequest";
                        }
                        else if (cleanarg.StartsWith("text") ||cleanarg.Contains("Text"))
                        {
                            supposedType = "string";
                        }
                        builder.Append(supposedType);
                        builder.Append(',');
                    }
                    
                }
                builder.Append("object>");
                cleanname = name.Substring(0, i);
                return builder.ToString();
            }
            cleanname = name;
            return "Delegate";
        }
    }
}
