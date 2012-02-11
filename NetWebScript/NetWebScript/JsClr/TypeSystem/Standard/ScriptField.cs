using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptField : MappedField, IScriptFieldDeclaration
    {
        private readonly string slotId;
        private readonly ScriptLiteralExpression initialValue;
        private readonly bool isGlobals;

        internal ScriptField(ScriptSystem system, ScriptType owner, FieldInfo field, string exportedSlot)
            : base(owner, field)
        {
            this.isGlobals = owner.IsGlobals;
            if (exportedSlot != null)
            {
                this.slotId = exportedSlot;
            }
            else
            {
                this.slotId = system.CreateSplotId();
            }
            CreateMetadata();


            //if (field.Field.IsLiteral)
            //{
            //    var value = field.Field.GetRawConstantValue();
            //    var strValue = value as string;
            //    if (strValue != null)
            //    {
            //        writer.WriteLine("={0}; // RAW", JsToken.LiteralString(strValue));
            //    }
            //    else if (value != null)
            //    {
            //        writer.WriteLine("=null; // RAW");
            //    }
            //    else
            //    {
            //        writer.WriteLine("={0}; // RAW", value);
            //    }
            //}
            if (ScriptSystem.IsNumberType(field.FieldType) || field.FieldType.IsEnum)
            {
                initialValue = ScriptLiteralExpression.IntegerLiteral(0);
            }
            else
            {
                initialValue = ScriptLiteralExpression.NullValue;
            }
        }


        private void CreateMetadata()
        {
            var meta = new FieldMetadata();
            meta.Name = SlodId;
            meta.CRef = CRefToolkit.GetCRef(Field);
            meta.CompilerGenerated = Attribute.IsDefined(Field, typeof(CompilerGeneratedAttribute));
            owner.Metadata.Fields.Add(meta);
        }

        public override string SlodId
        {
            get { return slotId; }
        }

        public override IFieldInvoker Invoker
        {
            get
            {
                if (isGlobals)
                {
                    return GlobalsInvoker.Instance;
                }
                return StandardInvoker.Instance;
            } 
        }

        public ScriptLiteralExpression InitialValue
        {
            get 
            {
                return initialValue;
            }
        }

        public string PrettyName
        {
            get { return field.ToString(); }
        }
    }
}
