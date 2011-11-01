using System;
using System.Collections.Generic;
using System.Reflection;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.AstBuilder.AstFilter;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.Script;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Runtime;

namespace NetWebScript.JsClr.Compiler
{
    /// <summary>
    /// Transform some CLR features into simple methods calls.
    /// </summary>
    internal class RuntimeAstFilter : AstFilterBase
    {
        private static readonly MethodInfo CreateDelegate = new Func<object, JSFunction, Delegate>(RuntimeHelper.CreateDelegate).Method;
        private static readonly MethodInfo As = new Func<object, JSFunction, object>(RuntimeHelper.As).Method;
        private static readonly MethodInfo Cast = new Func<object, JSFunction, object>(RuntimeHelper.Cast).Method;
        private static readonly MethodInfo GetTypeFromHandle = new Func<RuntimeTypeHandle, Type>(Type.GetTypeFromHandle).Method;
        private static readonly MethodInfo WrapException = new Func<object, Equivalents.Exception>(Equivalents.Exception.Convert).Method;
       
        private readonly ScriptSystem system;
        private readonly List<InternalMessage> errors;
        private MethodAst current;

        internal RuntimeAstFilter(ScriptSystem system, List<InternalMessage> errors)
        {
            this.system = system;
            this.errors = errors;
        }

        public override Statement Visit(ArrayCreationExpression arrayCreationExpression)
        {
            if (arrayCreationExpression.Initialize == null)
            {
                LiteralExpression value;
                if (arrayCreationExpression.ItemType.IsValueType)
                {
                    value = new LiteralExpression(0);
                }
                else
                {
                    value = new LiteralExpression(null);
                }
                return new MethodInvocationExpression(arrayCreationExpression.IlOffset, false, new Func<int, object, JSArray<object>>(RuntimeHelper.CreateArray).Method, null, new List<Expression>() { arrayCreationExpression .Size, value }).Accept(this);
            }
            return base.Visit(arrayCreationExpression);
        }

        public override Statement Visit(CastExpression castExpression)
        {
            var type = system.GetScriptType(castExpression.Type);
            if (type == null)
            {
                AddError(castExpression, string.Format("Type '{0}' is not script available. You cannot cast to that type.", castExpression.Type.FullName));
            }
            else if (!type.HaveCastInformation)
            {
                // Allow unverified cast
                AddWarning(castExpression, string.Format("Cast to type '{0}' cannot be verified at runtime. It will never fail, even if object type mismatch.", castExpression.Type.FullName));
            }
            else
            {
                return new MethodInvocationExpression(castExpression.IlOffset, false, Cast, null, new List<Expression>() {castExpression.Value, new LiteralExpression(castExpression.Type)}).Accept(this);
            }
            return castExpression.Value.Accept(this);
        }

        public override Statement Visit(SafeCastExpression safeCastExpression)
        {
            var type = system.GetScriptType(safeCastExpression.Type);
            if (type == null)
            {
                AddError(safeCastExpression, string.Format("Type '{0}' is not script available. You can not cast to that type.", safeCastExpression.Type.FullName));
            }
            else if (!type.HaveCastInformation)
            {
                // Safe cast cannot rely on an unverifiable cast
                AddError(safeCastExpression, string.Format("Cast to type '{0}' cannot be verified at runtime. Operation will always fail.", safeCastExpression.Type.FullName));
            }
            // SafeCast is implemented by RuntimeHelper.As
            return new MethodInvocationExpression(safeCastExpression.IlOffset, false, As, null, new List<Expression>() { safeCastExpression.Value, new LiteralExpression(safeCastExpression.Type) }).Accept(this);
        }

        public override Statement Visit(FieldReferenceExpression fieldReferenceExpression)
        {
            var field = system.GetScriptField(fieldReferenceExpression.Field);
            if (field == null)
            {
                AddError(fieldReferenceExpression, string.Format("Field '{0}' of '{1}' is not script available.", fieldReferenceExpression.Field.ToString(), fieldReferenceExpression.Field.DeclaringType.FullName));
            }
            else if (field.Field.DeclaringType == typeof(Variable))
            {
                return fieldReferenceExpression;
            }
            return base.Visit(fieldReferenceExpression);
        }

        public override Statement Visit(LiteralExpression literalExpression)
        {
            if (literalExpression.Value != null)
            {
                var type = system.GetScriptType(literalExpression.GetExpressionType());
                if (type == null || type.Serializer == null)
                {
                    AddError(literalExpression, string.Format("Type '{0}' is not script available or cannot be serialized in script. You can not use literal value of that type.", literalExpression.GetExpressionType().FullName));
                }
                Type literalType = literalExpression.Value as Type;
                if (literalType != null)
                {
                    type = system.GetScriptType(literalType);
                    if (type == null)
                    {
                        AddError(literalExpression, string.Format("Type '{0}' is not script available. You can not make references to that type.", literalType.FullName));
                    }
                }
            }
            return base.Visit(literalExpression);
        }

        public override Statement Visit(MethodInvocationExpression methodInvocationExpression)
        {
            //if (methodInvocationExpression.Method == WrapException && methodInvocationExpression.Arguments[0] is CurrentExceptionExpression)
            //{
            //    // XXX: Make the WrapException in a specific filter to avoid recursion ???
            //    return methodInvocationExpression;
            //}

            if (methodInvocationExpression.Method == GetTypeFromHandle)
            {
                // Type and RuntimeTypeHandle are "the same" in script
                // So the method GetTypeFromHandle is the identity function
                return methodInvocationExpression.Arguments[0].Accept(this);
            }

            var method = system.GetScriptMethodBase(methodInvocationExpression.Method);
            if (method == null)
            {
                AddError(methodInvocationExpression, string.Format("Method '{0}' of '{1}' is not script available", methodInvocationExpression.Method.ToString(), methodInvocationExpression.Method.DeclaringType.FullName));
            }
            return base.Visit(methodInvocationExpression);
        }

        public override Statement Visit(ObjectCreationExpression objectCreationExpression)
        {
            if (typeof(Delegate).IsAssignableFrom(objectCreationExpression.Constructor.DeclaringType))
            {
                // Delegates constructor is implemented by RuntimeHelper.CreateDelegate
                // Test if delegate is created with a literal expression
                if (IsLiteralNull(objectCreationExpression.Arguments[0]))
                {
                    return objectCreationExpression.Arguments[1].Accept(this);
                }
                // Otherwise invoke the RuntimeHelper
                return new MethodInvocationExpression(objectCreationExpression.IlOffset, false, CreateDelegate, null, objectCreationExpression.Arguments).Accept(this);
            }
            var ctor = system.GetScriptConstructor(objectCreationExpression.Constructor);
            if (ctor == null)
            {
                AddError(objectCreationExpression, string.Format("Constructor '{0}' of '{1}' is not script available", objectCreationExpression.Constructor.ToString(), objectCreationExpression.Constructor.DeclaringType.FullName));
            }
            return base.Visit(objectCreationExpression);
        }

        public override Statement Visit(BoxExpression boxExpression)
        {
            // Box is delegated to type
            var type = system.GetScriptType(boxExpression.Type);
            if (type == null || type.Boxing == null)
            {
                AddError(boxExpression, string.Format("Type '{0}' is not script available. You can not call any method on it, or either cast it to object.", boxExpression.Type.FullName));
            }
            return type.Boxing.BoxValue(type, boxExpression).Accept(this);
        }

        public override Statement Visit(UnboxExpression unboxExpression)
        {
            // Unbox is delegated to type
            var type = system.GetScriptType(unboxExpression.Type);
            if (type == null || type.Boxing == null)
            {
                AddError(unboxExpression, string.Format("Type '{0}' is not script available. You can not call any method on it, or either cast it to object.", unboxExpression.Type.FullName));
            }
            return type.Boxing.UnboxValue(type, unboxExpression).Accept(this);
        }

        public override void Visit(MethodAst method)
        {
            current = method;
            base.Visit(method);
            foreach (var variable in method.Variables)
            {
                if (variable.AllowRef)
                {
                    method.Statements.Insert(0, new AssignExpression(null, new VariableReferenceExpression(null, variable), (Expression)new ObjectCreationExpression(null, typeof(Variable).GetConstructor(Type.EmptyTypes), new List<Expression>()).Accept(this)));
                }
            }
        }

        public override Statement Visit(CurrentExceptionExpression currentExceptionExpression)
        {
            return new MethodInvocationExpression(currentExceptionExpression.IlOffset, false, WrapException, null, new List<Expression>() { currentExceptionExpression });
        }

        public override Statement Visit(MakeByRefFieldExpression refExpression)
        {
            var field = system.GetScriptField(refExpression.Field);
            if (field != null)
            {
                // FIXME: should delegate work to Invoker
                if (field.SlodId == null || field.Invoker != StandardInvoker.Instance)
                {
                    AddError(refExpression, string.Format("Could not make a reference to field '{0}'", field.Field.Name));
                }
                else
                {
                    Expression obj = null;
                    if (field.Field.IsStatic)
                    {
                        obj = new LiteralExpression(field.Field.DeclaringType);
                    }
                    else
                    {
                        obj = refExpression.Target;
                    }
                    Expression prop = new LiteralExpression(field.SlodId);
                    return new ObjectCreationExpression(refExpression.IlOffset, typeof(FieldRef).GetConstructor(new[] { typeof(JSObject), typeof(string) }), new List<Expression>() { obj, prop }).Accept(this);
                }
            }
            else
            {
                AddError(refExpression, string.Format("Field '{0}' of '{1}' is not script available.", refExpression.Field.ToString(), refExpression.Field.DeclaringType.FullName));
            }
            return base.Visit(refExpression);
        }

        public override Statement Visit(ByRefSetExpression byRefSetExpression)
        {
            return new MethodInvocationExpression(
                byRefSetExpression.IlOffset, 
                true,
                typeof(IRef).GetMethod("Set"), 
                byRefSetExpression.Target, 
                new List<Expression>() { byRefSetExpression.Value }).Accept(this);
        }

        public override Statement Visit(ByRefGetExpression byRefGetExpression)
        {
            return new MethodInvocationExpression(
                byRefGetExpression.IlOffset,
                true,
                typeof(IRef).GetMethod("Get"),
                byRefGetExpression.Target,
                new List<Expression>()).Accept(this);
        }

        public override Statement Visit(VariableReferenceExpression variableReferenceExpression)
        {
            var variable = variableReferenceExpression.Variable;
            if (variable.AllowRef)
            {
                return new FieldReferenceExpression(variableReferenceExpression.IlOffset, variableReferenceExpression, typeof(Variable).GetField("localValue")).Accept(this);
            }
            return base.Visit(variableReferenceExpression);
        }

        private static bool IsLiteralNull(Expression expr)
        {
            LiteralExpression targetliteral = expr as LiteralExpression;
            return targetliteral != null && targetliteral.Value == null;
        }

        private void AddError(Expression statement, string message)
        {
            errors.Add(new InternalMessage() { Severity = MessageSeverity.Error, Method = current.Method, Message = message, IlOffset = statement.IlOffset });
        }

        private void AddWarning(Expression statement, string message)
        {
            errors.Add(new InternalMessage() { Severity = MessageSeverity.Warning, Method = current.Method, Message = message, IlOffset = statement.IlOffset });
        }

    }
}
