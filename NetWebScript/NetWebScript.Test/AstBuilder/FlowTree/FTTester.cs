using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using NetWebScript.JsClr.AstBuilder.Flow;
using NetWebScript.JsClr.AstBuilder.FlowGraph;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.Test.AstBuilder.FlowTree
{
    class FTTester
    {
        private readonly ILGenerator il;
        private readonly TypeBuilder type;

        public FTTester()
        {
            lock (Tester.Lock)
            {
                type = Tester.CreateStaticTestType();
                var method = type.DefineMethod("test", MethodAttributes.Static | MethodAttributes.Public, typeof(void), Type.EmptyTypes);
                il = method.GetILGenerator();
            }
        }

        public ILGenerator Il
        {
            get { return il; }
        }

        public List<Sequence> GetSequence()
        {
            MethodInfo m;
            lock (Tester.Lock)
            {
                m = type.CreateType().GetMethod("test");
            }

            var il = new MethodCil(m);

            // 2 - Create control flow graph
            var graph = ControlFlowGraph.Create(il);

            // 3 - Transform graph using control flow primitives
            return FlowTransform.Transform(graph);
        }
    }
}
