using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.TypeSystem.Invoker;
using System.Reflection;
using NetWebScript.JsClr.Ast;

namespace NetWebScript.JsClr.TypeSystem.Helped
{
    class TypeBoxing : ITypeBoxing
    {
        private readonly MethodInfo unbox;
        private readonly ConstructorInfo box;

        public TypeBoxing(Type helper)
        {
            var unboxProperty = helper.GetProperty("Value");
            if (unboxProperty == null)
            {
                throw new Exception(string.Format("Type '{0}' is equivalent to a serializable value type, it must have a property named 'Value' for unboxing operations.", helper.FullName));
            }

            unbox = unboxProperty.GetGetMethod();
            box = helper.GetConstructor(new Type[] { unboxProperty.PropertyType });

            if (box == null)
            {
                throw new Exception(string.Format("Type '{0}' is equivalent to a serializable value type, it must have a constructor with a single parameter of type '{1}' for boxing operations.", helper.FullName, unboxProperty.PropertyType.FullName));
            }
        }

        public Ast.Expression BoxValue(IScriptType type, Ast.BoxExpression boxExpression)
        {
            return new ObjectCreationExpression(boxExpression.IlOffset, box, new List<Expression>() { 
                boxExpression.Value
            });
        }

        public Ast.Expression UnboxValue(IScriptType type, Ast.UnboxExpression boxExpression)
        {
            return new MethodInvocationExpression(boxExpression.IlOffset, false, unbox, boxExpression.Value, new List<Expression>());
        }
    }
}
