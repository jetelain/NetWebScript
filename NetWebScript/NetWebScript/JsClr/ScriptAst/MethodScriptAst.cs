using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class MethodScriptAst
    {
        private readonly MethodAst netAst;

        public MethodScriptAst(MethodAst netAst)
        {
            this.netAst = netAst;
        }

        public List<ScriptStatement> Statements { get; set; }

        public bool IsVoidMethod()
        {
            return netAst.IsVoidMethod();
        }

        public List<ParameterInfo> Arguments
        {
            get { return netAst.Arguments; }
        }

        public List<LocalVariable> Variables
        {
            get { return netAst.Variables; }
        }

        internal bool IsDebuggerHidden
        {
            get { return netAst.IsDebuggerHidden; }
        }

        public MethodBase Method 
        { 
            get { return netAst.Method; } 
        }
    }
}
