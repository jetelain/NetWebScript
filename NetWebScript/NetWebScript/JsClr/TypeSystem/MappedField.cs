using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem
{
    public abstract class MappedField : IScriptField
    {
        protected readonly FieldInfo field;
        protected readonly IScriptType owner;

        protected MappedField(IScriptType owner, FieldInfo field)
        {
            this.owner = owner;
            this.field = field;
        }

        public FieldInfo Field
        {
            get { return field; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public abstract string SlodId
        {
            get;
        }

        public bool IsStatic
        {
            get { return field.IsStatic; }
        }

        ScriptAst.IInvocableType ScriptAst.IInvocableField.DeclaringType
        {
            get { return owner; }
        }

        public virtual Invoker.IFieldInvoker Invoker
        {
            get { return StandardInvoker.Instance; }
        }

        public string DisplayName
        {
            get
            {
                return field.Name;
            }
        }
    }
}
