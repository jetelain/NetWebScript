using System.Reflection;
using NetWebScript.Metadata;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Remoting
{
    class ServerSideMethod : MappedMethodBase, IScriptMethod
    {
        private readonly string slotId;
        private readonly MethodBaseMetadata metadata; 

        public ServerSideMethod(ScriptSystem system, ServerSideType owner, MethodInfo method)
            : base(owner, method)
        {
            slotId = system.CreateSplotId();
            metadata = MetadataHelper.CreateMethodMetadata(this);
        }

        public override string ImplId
        {
            get { return slotId; } // Explicit calls are always mapped to a virtual call in a serverside type
        }

        public string SlodId
        {
            get { return slotId; }
        }

        public bool InlineMethodCall
        {
            get { return false; }
        }

        public MethodScriptAst Ast
        {
            get { return null; }
        }
    }
}
