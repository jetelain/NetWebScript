using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using System.Globalization;
using NetWebScript.Script;
using NetWebScript.JsClr.TypeSystem.Serializers;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class NumberType : NativeType, ITypeBoxing
    {
        public NumberType(ScriptSystem system, Type type)
            : base("Number", type)
        {
            system.GetScriptType(typeof(JSNumber));
        }

        public override ITypeBoxing Boxing
        {
            get
            {
                return this;
            }
        }

        public override IValueSerializer Serializer
        {
            get
            {
                return NumberSerializer.Instance;
            }
        }


        public Ast.Expression BoxValue(IScriptType type, Ast.BoxExpression boxExpression)
        {
            // FIXME: should really box into an object
            return boxExpression.Value;
        }

        public Ast.Expression UnboxValue(IScriptType type, Ast.UnboxExpression boxExpression)
        {
            // FIXME: should really box into an object
            return boxExpression.Value;
        }


    }
}
