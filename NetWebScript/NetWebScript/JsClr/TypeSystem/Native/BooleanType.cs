//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NetWebScript.JsClr.TypeSystem.Invoker;
//using NetWebScript.JsClr.JsBuilder.JsSyntax;

//namespace NetWebScript.JsClr.TypeSystem.Native
//{
//    class BooleanType : NativeType, ITypeBoxing, IValueSerializer
//    {
//        public BooleanType()
//            : base("Boolean", typeof(bool))
//        {

//        }

//        public override ITypeBoxing Boxing
//        {
//            get
//            {
//                return this;
//            }
//        }

//        public override IValueSerializer Serializer
//        {
//            get
//            {
//                return this;
//            }
//        }

//        public JsToken LiteralValue(IScriptType type, object value, IRootInvoker converter)
//        {
//            if ((bool)value)
//            {
//                return JsToken.Name("true");
//            }
//            return JsToken.Name("false");
//        }

//        public Ast.Expression BoxValue(IScriptType type, Ast.BoxExpression boxExpression)
//        {
//            // FIXME: should really box into an object
//            return boxExpression.Value;
//        }

//        public Ast.Expression UnboxValue(IScriptType type, Ast.UnboxExpression boxExpression)
//        {
//            return boxExpression.Value;
//        }


//    }
//}
