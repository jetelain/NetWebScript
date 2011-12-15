using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder.AstFilter;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.Flow;
using NetWebScript.JsClr.AstBuilder.FlowGraph;
using System.Diagnostics;

namespace NetWebScript.JsClr.AstBuilder
{
    /// <summary>
    /// AST of a <see cref="MethodBase"/>.
    /// </summary>
    public sealed class MethodAst
    {
        private readonly MethodBase methodBase;
        private readonly List<LocalVariable> variables = new List<LocalVariable>();
        private readonly List<ParameterInfo> arguments = new List<ParameterInfo>();

        internal MethodAst(MethodCil body)
        {
            this.methodBase = body.Method;
            variables.AddRange(body.Variables);
            arguments.AddRange(body.Arguments);
            IsDebuggerHidden = Attribute.IsDefined(methodBase, typeof(DebuggerHiddenAttribute));
        }

        public MethodAst(MethodBase info)
        {
            this.methodBase = info;
            arguments.AddRange(info.GetParameters());
        }

        public List<Statement> Statements { get; set; }

        internal MethodBase Info { get { return methodBase; } }

        public bool IsVoidMethod()
        {
            MethodInfo method = methodBase as MethodInfo;
            if (method != null)
            {
                return method.ReturnType == typeof(void);
            }
            return true;
        }
        
        public List<ParameterInfo> Arguments
        {
            get
            {
                return arguments;
            }
        }

        public List<LocalVariable> Variables
        {
            get
            {
                return variables;
            }
        }

        /// <summary>
        /// Create a new local variable.
        /// </summary>
        /// <param name="type">Type of variable to create.</param>
        /// <returns>A new <see cref="LocalVariable"/>.</returns>
        internal LocalVariable AllocateVariable(Type type)
        {
            LocalVariable v = new LocalVariable(string.Format("t{0}", variables.Count), type);
            variables.Add(v);
            return v;
        }

        /// <summary>
        /// Generate the AST of the given <see cref="MethodBase"/> from it's IL code.
        /// </summary>
        /// <param name="method">Source method (or constructor)</param>
        /// <returns>Corresponding AST</returns>
        public static MethodAst GetMethodAst(MethodBase method)
        {
            // 1 - Get IL
            var il = new MethodCil(method);

            // 2 - Create control flow graph
            var graph = ControlFlowGraph.Create(il);

            List<Sequence> primitives;
            try
            {
                // 3 - Transform graph using control flow primitives
                primitives = FlowTransform.Transform(graph);
            }
            catch
            {
                // Trace debug informations
                Trace.TraceInformation(graph.ToString());
                throw;
            }

            MethodAst ast;
            try
            {
                // 4 - Create statement froms IL and control flow primitives
                ast = StatementBuilder.Transform(il, primitives);
            }
            catch
            {
                // Trace debug informations
                Trace.TraceInformation(graph.ToString());
                Trace.TraceInformation(string.Join("\r\n", primitives));
                throw;
            }

            // 5 - Apply filters to get more semantics from graph and reduce compilers "tricks"
            AstFilters.Filter(ast);

            return ast;
        }

        internal bool IsDebuggerHidden
        {
            get;
            set;
        }

        public MethodBase Method { get { return methodBase; } }
    }
}
