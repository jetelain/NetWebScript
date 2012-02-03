using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;
using System.Runtime.CompilerServices;
using NetWebScript.JsClr.ScriptWriter.Declaration;
using NetWebScript.JsClr.ScriptAst;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    class ScriptField : IScriptField, IScriptFieldDeclaration
    {
        private readonly FieldInfo field;
        private readonly string slotId;
        private readonly ScriptType owner;
        private readonly ScriptLiteralExpression initialValue;

        internal ScriptField(ScriptSystem system, ScriptType owner, FieldInfo field, string exportedSlot)
        {
            this.owner = owner;
            this.field = field;
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
                initialValue = new ScriptLiteralExpression(null, 0, system.GetScriptType(typeof(int)));
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

        public string SlodId
        {
            get { return slotId; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public FieldInfo Field
        {
            get { return field; }
        }

        public IFieldInvoker Invoker
        {
            get
            {
                if (owner.IsGlobals)
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

        public bool IsStatic
        {
            get { return field.IsStatic; }
        }
    }
}
