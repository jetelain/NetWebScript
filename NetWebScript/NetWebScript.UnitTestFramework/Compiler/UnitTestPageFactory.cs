using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Page;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.UnitTestFramework.Client;
using System.Reflection;
using NetWebScript.Remoting;

namespace NetWebScript.UnitTestFramework.Compiler
{
    public class UnitTestPageFactory : IScriptPageFactory
    {
        private readonly List<Type> tests = new List<Type>();

        public void Prepare(IEnumerable<System.Reflection.Assembly> targetedAssemblies, IScriptDependencies dependencies)
        {
            dependencies.AddType(typeof(TestRunner));
            dependencies.AddType(typeof(RemoteInvoker));
            dependencies.AddType(typeof(TestRunnerPage));

            foreach (var assembly in targetedAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (Attribute.IsDefined(type, typeof(TestClassAttribute))
                        && Attribute.IsDefined(type, typeof(ScriptAvailableAttribute)))
                    {
                        dependencies.AddType(type);
                        tests.Add(type);
                    }
                }
            }
        }

        public IScriptPage CreatePage()
        {
            var testsArray = tests.Select(t => new TestClassInfo()
            {
                ctor = t.GetConstructor(Type.EmptyTypes),
                name = t.Name,
                methods = t
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => m.GetParameters().Length == 0 && Attribute.IsDefined(m, typeof(TestMethodAttribute)))
                    .Select(m => new TestMethodInfo() { method = m, name = m.Name })
                    .ToArray()
            }).ToArray();
            return new TestRunnerPage(testsArray);
        }
    }
}
