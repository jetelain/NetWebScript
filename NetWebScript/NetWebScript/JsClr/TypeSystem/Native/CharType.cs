//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NetWebScript.JsClr.TypeSystem.Invoker;
//using NetWebScript.JsClr.JsBuilder.JsSyntax;
//using System.Globalization;
//using NetWebScript.Script;

//namespace NetWebScript.JsClr.TypeSystem.Native
//{
//    class CharType : NativeType, ITypeBoxing
//    {
//        public CharType()
//            : base(null, typeof(char))
//        {

//        }

//        public override ITypeBoxing Boxing
//        {
//            get
//            {
//                return this;
//            }
//        }

//        public Ast.Expression BoxValue(IScriptType type, Ast.BoxExpression boxExpression)
//        {
//            return new Ast.MethodInvocationExpression(boxExpression.IlOffset,
//                false,
//                new Func<int,string>(JSString.FromCharCode).Method,
//                null,
//                new List<Ast.Expression>(){boxExpression.Value});
//        }

//        public Ast.Expression UnboxValue(IScriptType type, Ast.UnboxExpression boxExpression)
//        {
//            return new Ast.MethodInvocationExpression(boxExpression.IlOffset,
//                false,
//                new Func<int, int>(((JSString)"\0").CharCodeAt).Method,
//                boxExpression.Value,
//                new List<Ast.Expression>(){new Ast.LiteralExpression(0)});
//        }
//    }
//}
