using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace NetWebScript.Test
{
    public static class Tester
    {
        private static ModuleBuilder Module()
        {
            var assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("Tester"), AssemblyBuilderAccess.RunAndSave);
            return assembly.DefineDynamicModule("test");
        }

        private static readonly ModuleBuilder module = Module();

        private static int num = 0;

        public static object Lock
        {
            get { return module; }
        }

        public static TypeBuilder CreateStaticTestType()
        {
            return CreateTestType(TypeAttributes.Abstract|TypeAttributes.Sealed, null);
        }

        public static TypeBuilder CreateTestType(TypeAttributes attributes, Type parent)
        {
            lock (module)
            {
                num++;
                if (parent == null)
                {
                    return module.DefineType(string.Format("test{0}", num), attributes);
                }
                return module.DefineType(string.Format("test{0}", num), attributes, parent);
            }
        }

    }
}
