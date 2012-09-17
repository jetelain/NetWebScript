using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.Script;
using System.Reflection;
using System.IO;
using System.Web.UI;

namespace NetWebScript.APIStatus
{

    class TypInfo
    {
        public Type Type { get; set; }
        public bool Available { get; set; }
        public List<MemberInfo<MethodInfo>> Methods { get; set; }

        public List<MemberInfo<ConstructorInfo>> Constructors { get; set; }

        public List<MemberInfo<FieldInfo>> Fields { get; set; }
        public bool IsPartial()
        {
            return Methods.Any(m => !m.Available) || Fields.Any(m => !m.Available) || Constructors.Any(m => !m.Available);
        }
    }
    
    class MemberInfo<T> where T : MemberInfo
    {
        public T Member { get; set; }
        public bool Available { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var typeInfos = new List<TypInfo>();
            var system = new ScriptSystem();
            system.ImportAssemblyMappings(typeof(TypeSystemHelper).Assembly);
            
            var sourceAssemblies = new []{typeof(String).Assembly, typeof(Enumerable).Assembly};
            var targetPath = "c:\\temp\\nws";

            foreach(var assembly in sourceAssemblies)
                foreach (var type in assembly.GetTypes())
            {
                if (type.IsPublic && !type.IsEnum )
                {
                    Type reftype = type;
                    if (type.IsGenericType)
                    {
                        reftype = GenericDefinitionSeal(type);
                    }
                    //Console.WriteLine(reftype.FullName);

                    var equiv = system.GetScriptType(reftype);
                    var info = new TypInfo() { Type = type };
                    if (equiv != null)
                    {
                        info.Available = true;
                        var methods = reftype.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.SetProperty);
                        info.Methods = methods.Select(m => new MemberInfo<MethodInfo>() { Member = m, Available = system.GetScriptMethod(m.IsGenericMethodDefinition ? GenericDefinitionSeal(m) : m) != null }).ToList();

                        var constructors = reftype.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance);
                        info.Constructors = constructors.Select(m => new MemberInfo<ConstructorInfo>() { Member = m, Available = system.GetScriptConstructor(m) != null }).ToList();

                        var fields = reftype.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                        info.Fields = fields.Select(m => new MemberInfo<FieldInfo>() { Member = m, Available = system.GetScriptField(m) != null }).ToList();
                    }
                    typeInfos.Add(info);
                }
            }
            typeInfos = typeInfos.OrderBy(i => i.Type.Namespace).ThenBy(i => i.Type.Name).ToList();

            Directory.CreateDirectory(targetPath);
            RenderPage(Path.Combine(targetPath,"index.html"),typeInfos);
            foreach (var info in typeInfos)
            {
                if (info.Available)
                {
                    RenderPage(Path.Combine(targetPath, Filename(info.Type)), info);
            
                }
            }

        }

        private static void RenderPage(string filePath, List<TypInfo> typeInfos)
        {
            using (var textWriter = new StreamWriter(File.Open(filePath, FileMode.Create, FileAccess.Write)))
            using (var writer = new HtmlTextWriter(textWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                writer.WriteEncodedText("NetWebScript .NET support");
                writer.RenderEndTag(); // Title
                writer.RenderEndTag(); // Head
                writer.RenderBeginTag(HtmlTextWriterTag.Body);
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.WriteEncodedText("Note: All enumerations are supported.");
                writer.RenderEndTag(); // P

                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.WriteEncodedText("Namespace");
                writer.RenderEndTag(); // Th
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.WriteEncodedText("Name");
                writer.RenderEndTag(); // Th
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.WriteEncodedText("Supported");
                writer.RenderEndTag(); // Th
                writer.RenderEndTag(); // Tr
                foreach (var info in typeInfos)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    writer.WriteEncodedText(info.Type.Namespace);
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    if (info.Available)
                    {
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, Filename(info.Type));
                        writer.RenderBeginTag(HtmlTextWriterTag.A);
                        writer.WriteEncodedText(info.Type.Name);
                        writer.RenderEndTag(); // Strong
                    }
                    else
                    {
                        writer.WriteEncodedText(info.Type.Name);
                    }
                    writer.RenderEndTag(); // Td
                    writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    if ( info.Available )
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Strong);
                        if (info.IsPartial())
                        {
                            writer.WriteEncodedText("Partial");
                        
                        }
                        else
                        {
                            writer.WriteEncodedText("Yes");
                        
                        }
                        writer.RenderEndTag(); // Strong
                    }
                    else
                    {
                        writer.WriteEncodedText("No");
                    }
                    writer.RenderEndTag(); // Td
                    writer.RenderEndTag(); // Tr
                }


                writer.RenderEndTag(); // Table
                writer.RenderEndTag(); // Body
                writer.RenderEndTag(); // Html
            }
        }

        private static void RenderPage(string filePath, TypInfo typeInfo)
        {
            using (var textWriter = new StreamWriter(File.Open(filePath, FileMode.Create, FileAccess.Write)))
            using (var writer = new HtmlTextWriter(textWriter))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Head);
                writer.RenderBeginTag(HtmlTextWriterTag.Title);
                writer.WriteEncodedText(typeInfo.Type.FullName+ " - NetWebScript Support");
                writer.RenderEndTag(); // Title
                writer.RenderEndTag(); // Head
                writer.RenderBeginTag(HtmlTextWriterTag.Body);
                writer.RenderBeginTag(HtmlTextWriterTag.Table);
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.WriteEncodedText("Name");
                writer.RenderEndTag(); // Th
                writer.RenderBeginTag(HtmlTextWriterTag.Th);
                writer.WriteEncodedText("Supported");
                writer.RenderEndTag(); // Th
                writer.RenderEndTag(); // Tr
                WriteMemberInfo(writer, typeInfo.Constructors);
                WriteMemberInfo(writer, typeInfo.Methods);
                WriteMemberInfo(writer, typeInfo.Fields);



                writer.RenderEndTag(); // Table
                writer.RenderEndTag(); // Body
                writer.RenderEndTag(); // Html
            }
        }

        private static void WriteMemberInfo<T>(HtmlTextWriter writer, List<MemberInfo<T>> list) where T : MemberInfo
        {

            foreach (var info in list)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                writer.WriteEncodedText(info.Member.ToString());
                writer.RenderEndTag(); // Td
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                if (info.Available)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Strong);
                    writer.WriteEncodedText("Yes");
                    writer.RenderEndTag(); // Strong
                }
                else
                {
                    writer.WriteEncodedText("No");
                }
                writer.RenderEndTag(); // Td
                writer.RenderEndTag(); // Tr
            }
        }

        private static Type[] ToGenericParameters(Type[] genericArguments)
        {
            return genericArguments.Select(p =>
            {
                var t = p.GetGenericParameterConstraints().FirstOrDefault() ?? typeof(object);
                if (t == typeof(ValueType))
                {
                    return typeof(int);
                }
                return t;
            }).ToArray();
        }

        private static Type GenericDefinitionSeal(Type type)
        {
            return type.MakeGenericType(ToGenericParameters(type.GetGenericArguments()));
        }
        private static MethodInfo GenericDefinitionSeal(MethodInfo method)
        {
            return method.MakeGenericMethod(ToGenericParameters(method.GetGenericArguments()));
        }

        public static string Filename(Type type)
        {
            return type.FullName.Replace('`', '_')+".html";
        }
    }
}
