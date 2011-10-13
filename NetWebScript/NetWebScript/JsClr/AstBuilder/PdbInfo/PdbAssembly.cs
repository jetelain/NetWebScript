using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics.SymbolStore;
using Microsoft.Samples.Debugging.CorSymbolStore;

namespace NetWebScript.JsClr.AstBuilder.PdbInfo
{
    class PdbAssembly
    {
        private readonly Assembly assembly;
        private readonly Dictionary<int, PdbMethod> methods = new Dictionary<int, PdbMethod>();

        internal PdbAssembly(Assembly assembly)
        {
            this.assembly = assembly;
            if (!assembly.IsDynamic)
            {
                LoadData(GetAssemblyDirectoryPath(assembly));
            }
        }

        private static string GetAssemblyDirectoryPath(Assembly assembly)
        {
            string assemblyFilePath = assembly.CodeBase;
            if (!String.IsNullOrEmpty(assemblyFilePath))
            {
                return assemblyFilePath.Replace("file:///", String.Empty);
            }
            else
            {
                return assembly.Location;
            }
        }

        private void LoadData(string filename)
        {
            ISymbolReader reader = SymbolAccess.GetReaderForFile(filename);
            foreach (Type t in assembly.GetTypes())
            {
                foreach (MethodInfo methodReflection in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                {
                    int token = methodReflection.MetadataToken;
                    ISymbolMethod methodSymbol = reader.GetMethod(new SymbolToken(token));
                    if (methodSymbol != null)
                    {
                        methods.Add(token, new PdbMethod(methodSymbol));
                    }
                }
            }
        }

        public PdbMethod GetPdbMethod(MethodBase method)
        {
            PdbMethod pdb;
            if (!methods.TryGetValue(method.MetadataToken, out pdb))
            {
                return null;
            }
            return pdb;
        }
    }
}
