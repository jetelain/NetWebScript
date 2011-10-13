using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.JsClr.AstBuilder.PdbInfo
{
    static class PdbCatalog
    {
        private static Dictionary<Assembly, PdbAssembly> assemblies = new Dictionary<Assembly, PdbAssembly>();

        public static PdbAssembly GetPdbAssembly(Assembly assembly)
        {
            PdbAssembly pdb;
            if (!assemblies.TryGetValue(assembly, out pdb))
            {
                pdb = new PdbAssembly(assembly);
                assemblies.Add(assembly,pdb); 
            }
            return pdb;
        }

        public static PdbMethod GetPdbMethod(MethodBase method)
        {
            if (method.DeclaringType != null)
            {
                return GetPdbAssembly(method.DeclaringType.Assembly).GetPdbMethod(method);
            }
            return null;
        }


    }
}
