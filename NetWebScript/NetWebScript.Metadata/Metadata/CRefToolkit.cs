﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.Metadata
{
    /// <summary>
    /// Utilitaires pour la manipulation des CRef C#. 
    /// (ECMA-334 3rd Edition, Annex  E.3.1.)
    /// </summary>
    /// <seealso href="http://manpages.ubuntu.com/manpages/lucid/man5/mdoc.5.html">Documentation MONO (CREF FORMAT)</seealso>
    public static class CRefToolkit
    {
        public static String GetCRef(Type type)
        {
            StringBuilder signature = new StringBuilder();
            signature.Append("T:");
            signature.AppendType(type);
            return signature.ToString();
        }

        private static void AppendType(this StringBuilder builder, Type type)
        {
            if (type.IsByRef)
            {
                builder.AppendType(type.GetElementType());
                builder.Append("@");
            }
            else if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                if (!String.IsNullOrEmpty(type.Namespace))
                {
                    builder.Append(type.Namespace);
                    builder.Append('.');
                }
                int i = type.Name.IndexOf('`');
                if (i != -1)
                {
                    builder.Append(type.Name.Substring(0, i));
                    builder.Append('{');
                    bool first = true;
                    foreach (Type param in type.GetGenericArguments())
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            builder.Append(',');
                        }
                        AppendType(builder, param);
                    }
                    builder.Append('}');
                }
            }
            else if (type.IsGenericParameter)
            {
                if (type.DeclaringMethod != null)
                {
                    builder.Append("``");
                    builder.Append(Array.IndexOf(type.DeclaringMethod.GetGenericArguments(), type));
                }
                else
                {
                    builder.Append('`');
                    builder.Append(Array.IndexOf(type.DeclaringType.GetGenericArguments(), type));
                }
            }
            else if (type.FullName != null)
            {
                builder.Append(type.FullName.Replace('+', '.'));
            }
            else
            {
                builder.Append(type.Name);
            }

        }

        private static void AppendParameters(this StringBuilder signature, ParameterInfo[] parameters)
        {
            if (parameters.Length > 0)
            {
                signature.Append("(");
                for (int i = 0; i < parameters.Length; ++i)
                {
                    if (i > 0)
                    {
                        signature.Append(",");
                    }
                    AppendType(signature, parameters[i].ParameterType);
                }
                signature.Append(")");
            }
        }

        public static String GetCRef(ConstructorInfo method)
        {
            StringBuilder signature = new StringBuilder();
            signature.Append("M:");
            signature.AppendType(method.DeclaringType);
            signature.Append(".#ctor");
            signature.AppendParameters(method.GetParameters());
            return signature.ToString();
        }

        public static String GetCRef(MethodInfo method)
        {
            StringBuilder signature = new StringBuilder();
            signature.Append("M:");
            signature.AppendType(method.DeclaringType);
            signature.Append(".");
            signature.Append(method.Name);
            if (method.IsGenericMethodDefinition)
            {
                signature.Append("``");
                signature.Append(method.GetGenericArguments().Length);
            }
            signature.AppendParameters(method.GetParameters());
            return signature.ToString();
        }

        public static string GetCRef(PropertyInfo property)
        {
            StringBuilder signature = new StringBuilder();
            signature.Append("P:");
            signature.AppendType(property.DeclaringType);
            signature.Append(".");
            signature.Append(property.Name);
            signature.AppendParameters(property.GetIndexParameters());
            return signature.ToString();
        }

        public static string GetCRef(FieldInfo property)
        {
            StringBuilder signature = new StringBuilder();
            signature.Append("F:");
            signature.AppendType(property.DeclaringType);
            signature.Append(".");
            signature.Append(property.Name);
            return signature.ToString();
        }

        public static string GetCRef(EventInfo ev)
        {
            StringBuilder signature = new StringBuilder();
            signature.Append("E:");
            signature.AppendType(ev.DeclaringType);
            signature.Append(".");
            signature.Append(ev.Name);
            return signature.ToString();
        }

        public static string GetCRef(MemberInfo member)
        {
            Type type = member as Type;
            if (type != null)
            {
                return GetCRef(type);
            }

            FieldInfo field = member as FieldInfo;
            if (field != null)
            {
                return GetCRef(field);
            }

            PropertyInfo property = member as PropertyInfo;
            if (property != null)
            {
                return GetCRef(property);
            }

            MethodInfo method = member as MethodInfo;
            if (method != null)
            {
                return GetCRef(method);
            }

            ConstructorInfo ctor = member as ConstructorInfo;
            if (ctor != null)
            {
                return GetCRef(ctor);
            }

            EventInfo ev = member as EventInfo;
            if (ev != null)
            {
                return GetCRef(ev);
            }

            throw new InvalidOperationException();
        }

        private static Type[] ResolveCRefTypeArray(String name)
        {
            List<String> tokens = new List<string>();
            StringBuilder token = new StringBuilder();
            int nested = 0;
            for (int i = 0; i < name.Length; ++i)
            {
                char c = name[i];
                if (c == '{')
                {
                    nested++;
                    token.Append(c);
                }
                else if (c == '}')
                {
                    nested--;
                    token.Append(c);
                }
                else if (c == ',' && nested == 0)
                {
                    tokens.Add(token.ToString());
                    token.Length = 0;
                }
                else
                {
                    token.Append(c);
                }
            }
            tokens.Add(token.ToString());
            Type[] array = new Type[tokens.Count];
            for (int i = 0; i < tokens.Count; ++i)
            {
                array[i] = ResolveCRefType(tokens[i]);
                if (array[i] == null)
                {
                    return null;
                }
            }
            return array;
        }

        private static Type ResolveCRefType(String name)
        {
            if (name.EndsWith("@"))
            {
                Type t = ResolveCRefType(name.Substring(0, name.Length - 1));
                if (t != null)
                {
                    return t.MakeByRefType();
                }
                return null;
            }

            int idx = name.IndexOf('{');
            if (idx != -1)
            {
                String generic = name.Substring(0, idx);
                Type[] array = ResolveCRefTypeArray(name.Substring(idx + 1, name.Length - idx - 2));
                if (array == null)
                {
                    return null;
                }
                Type t = ResolveCRefType(generic + "`" + array.Length);
                if (t == null)
                {
                    return null;
                }
                return t.MakeGenericType(array);
            }

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(name, false);
                if (type != null)
                {
                    return type;
                }
            }

            idx = name.LastIndexOf('.');
            if (idx != -1)
            {
                Type type = ResolveCRefType(name.Substring(0, idx));
                if (type != null)
                {
                    return type.GetNestedType(name.Substring(idx + 1), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                }
            }
            return null;
        }

        public static string GetDisplayName(string cref)
        {
            if (cref == null)
            {
                return null;
            }

            if (cref.StartsWith("T:")) // Type
            {
                return cref.Substring(2);
            }

            int p = cref.IndexOf('(');
            int d = p == -1 ? cref.LastIndexOf('.') : cref.LastIndexOf('.', p);

            if (d != -1)
            {
                String typeName = cref.Substring(2, d - 2);
                String memberName;

                Type type = ResolveCRefType(typeName);
                if (type == null)
                {
                    return null;
                }

                Type[] args;
                if (p == -1)
                {
                    memberName = cref.Substring(d + 1);
                    args = new Type[0];
                }
                else
                {
                    memberName = cref.Substring(d + 1, p - d - 1);
                    String argsNames = cref.Substring(p + 1, cref.Length - p - 2);
                    args = ResolveCRefTypeArray(argsNames);
                }
                if (cref.StartsWith("F:")) // Field
                {
                    return memberName;
                }
                if (cref.StartsWith("E:")) // Event
                {
                    return memberName;
                }
                if (cref.StartsWith("P:")) // Property
                {
                    return memberName;
                }
                if (cref.StartsWith("M:")) // Property
                {
                    if (memberName == "#ctor")
                    {
                        return memberName;
                    }
                    else
                    {
                        return memberName;
                    }
                }
            }
            return null;


        }

        public static MemberInfo Resolve(String cref)
        {
            if (cref == null)
            {
                return null;
            }

            if (cref.StartsWith("T:")) // Type
            {
                return ResolveCRefType(cref.Substring(2));
            }

            int p = cref.IndexOf('(');
            int d = p == -1 ? cref.LastIndexOf('.') : cref.LastIndexOf('.', p);

            if (d != -1)
            {
                String typeName = cref.Substring(2, d-2);
                String memberName;

                Type type = ResolveCRefType(typeName);
                if (type == null)
                {
                    return null;
                }

                Type[] args;
                if (p == -1)
                {
                    memberName = cref.Substring(d + 1);
                    args = new Type[0];
                }
                else
                {
                    memberName = cref.Substring(d + 1, p - d-1);
                    String argsNames = cref.Substring(p + 1, cref.Length - p - 2);
                    args = ResolveCRefTypeArray(argsNames);
                }
                if (cref.StartsWith("F:")) // Field
                {
                    return type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                }
                if (cref.StartsWith("E:")) // Event
                {
                    return type.GetEvent(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                }
                if (cref.StartsWith("P:")) // Property
                {
                    return type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, null, null, args, null);
                }
                if (cref.StartsWith("M:")) // Property
                {
                    if (memberName == "#ctor")
                    {
                        return type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, null, args, null);
                    }
                    else
                    {
                        return type.GetMethod(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, null, args, null);
                    }
                }
            }
            return null;
        }
    }
}
