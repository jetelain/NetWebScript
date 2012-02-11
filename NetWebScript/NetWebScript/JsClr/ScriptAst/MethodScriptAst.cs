using System.Collections.Generic;
using System.Linq;
using NetWebScript.JsClr.AstBuilder;

namespace NetWebScript.JsClr.ScriptAst
{
    public sealed class MethodScriptAst
    {
        public MethodScriptAst(MethodAst netAst)
        {
            Arguments = netAst.Arguments.Select(a => new ScriptArgument() { Index = a.Position, Name = a.Name }).ToList();
            Variables = netAst.Variables.Select(a => new ScriptVariable() { Index = a.LocalIndex, Name = a.Name, IsCompilerGenerated = a.IsCompilerGenerated }).ToList();
            IsDebuggerHidden = netAst.IsDebuggerHidden;
        }

        public MethodScriptAst()
        {
            IsDebuggerHidden = true;
            Arguments = new List<ScriptArgument>();
            Variables = new List<ScriptVariable>();
            Statements = new List<ScriptStatement>();
        }

        public List<ScriptStatement> Statements { get; set; }

        public List<ScriptArgument> Arguments { get; private set; }

        public List<ScriptVariable> Variables { get; private set; }

        internal bool IsDebuggerHidden { get; private set; }
    }
}
