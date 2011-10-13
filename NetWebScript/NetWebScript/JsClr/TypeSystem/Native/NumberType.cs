using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using System.Globalization;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    class NumberType : NativeType, IValueSerializer, ITypeBoxing
    {
        public NumberType(Type type)
            : base("Number", type)
        {

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
                return this;
            }
        }


        public JsToken LiteralValue(IScriptType type, object value, IRootInvoker converter)
        {
            return JsToken.Name(value.ToString());
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
