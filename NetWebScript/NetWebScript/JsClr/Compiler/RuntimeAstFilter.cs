using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.Ast;
using NetWebScript.JsClr.AstBuilder;
using NetWebScript.JsClr.ScriptAst;
using NetWebScript.JsClr.TypeSystem;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Runtime;
using NetWebScript.Script;

namespace NetWebScript.JsClr.Compiler
{
    /// <summary>
    /// Transform CLR oriented AST to JavaScript oriented AST. <br />
    /// Convert all CLR features into simple methods calls using all "NetWebScript.Runtime" classes.
    /// </summary>
    internal class RuntimeAstFilter : IStatementVisitor<ScriptStatement>
    {
        private static readonly MethodInfo CreateDelegate = new Func<object, JSFunction, Delegate>(RuntimeHelper.CreateDelegate).Method;
        private static readonly MethodInfo AsInterface = new Func<object, JSFunction, object>(RuntimeHelper.AsInterface).Method;
        private static readonly MethodInfo AsClass = new Func<object, JSFunction, object>(RuntimeHelper.AsClass).Method;
        private static readonly MethodInfo CastInterface = new Func<object, JSFunction, object>(RuntimeHelper.CastInterface).Method;
        private static readonly MethodInfo CastClass = new Func<object, JSFunction, object>(RuntimeHelper.CastClass).Method;
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

        private ScriptExpression Error(Expression expr)
        {
            return new ScriptLiteralExpression(expr.IlOffset, null, null);
        }

        public ScriptStatement Visit(ArrayCreationExpression arrayCreationExpression)
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
            var scriptCreation = new ScriptArrayCreationExpression(arrayCreationExpression.IlOffset, arrayCreationExpression.ItemType, Visit(arrayCreationExpression.Size));
            scriptCreation.Initialize = Visit(arrayCreationExpression.Initialize);
            return scriptCreation;
        }

        public ScriptStatement Visit(CastExpression castExpression)
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
            else if (type.Type.IsInterface)
            {
                return new MethodInvocationExpression(castExpression.IlOffset, false, CastInterface, null, new List<Expression>() { castExpression.Value, new LiteralExpression(castExpression.Type) }).Accept(this);
            }
            else
            {
                return new MethodInvocationExpression(castExpression.IlOffset, false, CastClass, null, new List<Expression>() {castExpression.Value, new LiteralExpression(castExpression.Type)}).Accept(this);
            }
            return castExpression.Value.Accept(this);
        }

        public ScriptStatement Visit(SafeCastExpression safeCastExpression)
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
            if (type.Type.IsInterface)
            {
                return new MethodInvocationExpression(safeCastExpression.IlOffset, false, AsInterface, null, new List<Expression>() { safeCastExpression.Value, new LiteralExpression(safeCastExpression.Type) }).Accept(this);
            }
            return new MethodInvocationExpression(safeCastExpression.IlOffset, false, AsClass, null, new List<Expression>() { safeCastExpression.Value, new LiteralExpression(safeCastExpression.Type) }).Accept(this);
        }

        public ScriptStatement Visit(FieldReferenceExpression fieldReferenceExpression)
        {
            var field = system.GetScriptField(fieldReferenceExpression.Field);
            if (field == null)
            {
                AddError(fieldReferenceExpression, string.Format("Field '{0}' of '{1}' is not script available.", fieldReferenceExpression.Field.ToString(), fieldReferenceExpression.Field.DeclaringType.FullName));
            }
            else
            {
                return new ScriptFieldReferenceExpression(
                    fieldReferenceExpression.IlOffset,
                    Visit(fieldReferenceExpression.Target),
                    field);
            }
            Visit(fieldReferenceExpression.Target);
            return Error(fieldReferenceExpression);
        }

        private ScriptExpression Visit(Expression expression)
        {
            if (expression != null)
            {
                return (ScriptExpression)expression.Accept(this);
            }
            return null;
        }
        private ScriptAssignableExpression Visit(AssignableExpression expression)
        {
            if (expression != null)
            {
                return (ScriptAssignableExpression)expression.Accept(this);
            }
            return null;
        }

        public ScriptStatement Visit(LiteralExpression literalExpression)
        {
            IScriptType type = null;
            if (literalExpression.Value != null)
            {
                type = system.GetScriptType(literalExpression.GetExpressionType());
                if (type == null || type.Serializer == null)
                {
                    AddError(literalExpression, string.Format("Type '{0}' is not script available or cannot be serialized in script. You can not use literal value of that type.", literalExpression.GetExpressionType().FullName));
                }
                Type literalType = literalExpression.Value as Type;
                if (literalType != null)
                {
                    var targettype = system.GetScriptType(literalType);
                    if (targettype == null)
                    {
                        AddError(literalExpression, string.Format("Type '{0}' is not script available. You can not make references to that type.", literalType.FullName));
                    }
                }
            }
            return new ScriptLiteralExpression(literalExpression.IlOffset, literalExpression.Value, type);
        }

        public ScriptStatement Visit(MethodInvocationExpression methodInvocationExpression)
        {
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
            else
            {
                return new ScriptMethodInvocationExpression(methodInvocationExpression.IlOffset, methodInvocationExpression.Virtual, method, Visit(methodInvocationExpression.Target), Visit(methodInvocationExpression.Arguments));
            }
            Visit(methodInvocationExpression.Target);
            Visit(methodInvocationExpression.Arguments);
            return Error(methodInvocationExpression);
        }



        public ScriptStatement Visit(ObjectCreationExpression objectCreationExpression)
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
            else
            {
                return new ScriptObjectCreationExpression(objectCreationExpression.IlOffset, ctor, Visit(objectCreationExpression.Arguments));
            }
            Visit(objectCreationExpression.Arguments);
            return Error(objectCreationExpression);
        }

        public ScriptStatement Visit(BoxExpression boxExpression)
        {
            // Box is delegated to type
            var type = system.GetScriptType(boxExpression.Type);
            if (type == null || type.Boxing == null)
            {
                AddError(boxExpression, string.Format("Type '{0}' is not script available. You can not call any method on it, or either cast it to object.", boxExpression.Type.FullName));
            }
            return type.Boxing.BoxValue(type, boxExpression).Accept(this);
        }

        public ScriptStatement Visit(UnboxExpression unboxExpression)
        {
            // Unbox is delegated to type
            var type = system.GetScriptType(unboxExpression.Type);
            if (type == null || type.Boxing == null)
            {
                AddError(unboxExpression, string.Format("Type '{0}' is not script available. You can not call any method on it, or either cast it to object.", unboxExpression.Type.FullName));
            }
            return type.Boxing.UnboxValue(type, unboxExpression).Accept(this);
        }

        public MethodScriptAst Visit(MethodAst method)
        {
            MethodScriptAst scriptMethod = new MethodScriptAst(method);
            current = method;
            scriptMethod.Statements = Visit(method.Statements);
            foreach (var variable in method.Variables)
            {
                if (variable.AllowRef)
                {
                    scriptMethod.Statements.Insert(0, new ScriptAssignExpression(null,
                        new ScriptVariableReferenceExpression(null, variable),
                        (ScriptExpression)new ScriptObjectCreationExpression(null, 
                            system.GetScriptConstructor(typeof(Variable).GetConstructor(Type.EmptyTypes)),
                            new List<ScriptExpression>())));
                }
            }
            return scriptMethod;
        }

        private List<ScriptStatement> Visit(List<Statement> list)
        {
            if (list == null)
            {
                return null;
            }
            List<ScriptStatement> scriptList = new List<ScriptStatement>();
            foreach (var statement in list)
            {
                scriptList.Add(statement.Accept(this));
            }
            return scriptList;
        }

        private List<ScriptExpression> Visit(List<Expression> list)
        {
            if (list == null)
            {
                return null;
            }
            List<ScriptExpression> scriptList = new List<ScriptExpression>();
            foreach (var statement in list)
            {
                scriptList.Add((ScriptExpression)statement.Accept(this));
            }
            return scriptList;
        }
        public ScriptStatement Visit(CurrentExceptionExpression currentExceptionExpression)
        {
            return new ScriptMethodInvocationExpression(
                currentExceptionExpression.IlOffset, false, 
                system.GetScriptMethodBase(WrapException), null,
                new List<ScriptExpression>() { new ScriptCurrentExceptionExpression(currentExceptionExpression.GetExpressionType()) });
        }

        public ScriptStatement Visit(MakeByRefFieldExpression refExpression)
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
            Visit(refExpression.Target);
            return Error(refExpression);
        }

        public ScriptStatement Visit(ByRefSetExpression byRefSetExpression)
        {
            return new MethodInvocationExpression(
                byRefSetExpression.IlOffset, 
                true,
                typeof(IRef).GetMethod("Set"), 
                byRefSetExpression.Target, 
                new List<Expression>() { byRefSetExpression.Value }).Accept(this);
        }

        public ScriptStatement Visit(ByRefGetExpression byRefGetExpression)
        {
            return new MethodInvocationExpression(
                byRefGetExpression.IlOffset,
                true,
                typeof(IRef).GetMethod("Get"),
                byRefGetExpression.Target,
                new List<Expression>()).Accept(this);
        }

        public ScriptStatement Visit(VariableReferenceExpression variableReferenceExpression)
        {
            var variable = variableReferenceExpression.Variable;
            if (variable.AllowRef)
            {
                return new ScriptFieldReferenceExpression(
                    variableReferenceExpression.IlOffset, 
                    new ScriptVariableReferenceExpression(variableReferenceExpression.IlOffset, variableReferenceExpression.Variable), 
                    system.GetScriptField(typeof(Variable).GetField("localValue")));
            }
            return new ScriptVariableReferenceExpression(variableReferenceExpression.IlOffset, variableReferenceExpression.Variable);
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




        public ScriptStatement Visit(ArgumentReferenceExpression node)
        {
            return new ScriptArgumentReferenceExpression(node.IlOffset, node.Argument);
        }

        public ScriptStatement Visit(ArrayIndexerExpression arrayIndexerExpression)
        {
            return new ScriptArrayIndexerExpression(arrayIndexerExpression.IlOffset,
                target: Visit(arrayIndexerExpression.Target),
                index: Visit(arrayIndexerExpression.Index));
        }

        public ScriptStatement Visit(AssignExpression assignExpression)
        {
            return new ScriptAssignExpression(assignExpression.IlOffset, 
                target: Visit(assignExpression.Target), 
                value: Visit(assignExpression.Value));
        }

        public ScriptStatement Visit(BinaryExpression binaryExpression)
        {
            return new ScriptBinaryExpression(binaryExpression.IlOffset,
                @operator: (ScriptBinaryOperator)binaryExpression.Operator, 
                right: Visit(binaryExpression.Right), 
                left: Visit(binaryExpression.Left));
        }

        public ScriptStatement Visit(BreakStatement breakStatement)
        {
            return new ScriptBreakStatement();
        }

        public ScriptStatement Visit(ContinueStatement continueStatement)
        {
            return new ScriptContinueStatement();
        }

        public ScriptStatement Visit(IfStatement ifStatement)
        {
            return new ScriptIfStatement( 
                condition: Visit(ifStatement.Condition),
                @then: Visit(ifStatement.Then),
                @else: Visit(ifStatement.Else)
                );
        }

        public ScriptStatement Visit(ReturnStatement returnStatement)
        {
            return new ScriptReturnStatement(Visit(returnStatement.Value));
        }

        public ScriptStatement Visit(SwitchStatement switchStatement)
        {
            return new ScriptSwitchStatement(
                Visit(switchStatement.Value), 
                switchStatement.Cases.Select(c => new ScriptCase(c.Value, Visit(c.Statements))).ToList());
        }

        public ScriptStatement Visit(ThisReferenceExpression thisReferenceExpression)
        {
            return new ScriptThisReferenceExpression(thisReferenceExpression.IlOffset, thisReferenceExpression.GetExpressionType());
        }

        public ScriptStatement Visit(UnaryExpression unaryExpression)
        {
            return new ScriptUnaryExpression(unaryExpression.IlOffset,
                @operator: (ScriptUnaryOperator) unaryExpression.Operator, 
                operand: Visit(unaryExpression.Operand));
        }

        public ScriptStatement Visit(WhileStatement whileStatement)
        {
            return new ScriptWhileStatement(
                Visit(whileStatement.Condition),
                Visit(whileStatement.Body));
        }

        public ScriptStatement Visit(ConditionExpression conditionExpression)
        {
            return new ScriptConditionExpression(conditionExpression.IlOffset,
                cond: Visit(conditionExpression.Condition),
                then: Visit(conditionExpression.Then),
                @else: Visit(conditionExpression.Else));
        }

        public ScriptStatement Visit(TryCatchStatement tryCatchStatement)
        {
            return new ScriptTryCatchStatement(
                body: Visit(tryCatchStatement.Body), 
                @finally: Visit(tryCatchStatement.Finally),
                @catch: Visit(tryCatchStatement.CatchList)
                );
        }

        private List<ScriptStatement> Visit(List<Catch> list)
        {
            if (list != null)
            {
                List<ScriptStatement> statements = new List<ScriptStatement>();
                // XXX: list is assumed to be to more specific to less specific exception type
                // XXX: exception that are not rised by NWS are assumed to be of type "Exception" (no Wrap on current exception before tests)
                ScriptIfStatement previousIf = null;
                foreach (var @catch in list)
                {
                    if (@catch.Type != null && @catch.Type != typeof(object))
                    {
                        // $exception as Type
                        if (@catch.Type.IsInterface)
                        {
                            throw new NotImplementedException();
                        }
                        var currentAsRequest = new ScriptMethodInvocationExpression(null, false, system.GetScriptMethod(AsClass), null, new List<ScriptExpression>() { new ScriptCurrentExceptionExpression(typeof(object)), (ScriptExpression)Visit(new LiteralExpression(@catch.Type)) });

                        // ($exception as Type) != null
                        var condition = (new ScriptBinaryExpression(null, ScriptBinaryOperator.ValueInequality, currentAsRequest, new ScriptLiteralExpression(null, null, null)));

                        // if ( ($exception as Type) != null ) Body();
                        var @if = new ScriptIfStatement(condition, Visit(@catch), null);

                        if (previousIf != null)
                        {
                            previousIf.Else = new List<ScriptStatement>() { @if };
                        }
                        else
                        {
                            statements.Add(@if);
                        }
                        previousIf = @if;
                    }
                    else
                    {
                        if (previousIf != null)
                        {
                            previousIf.Else = Visit(@catch);
                        }
                        else
                        {
                            statements.AddRange(Visit(@catch));
                        }
                        previousIf = null;
                    }
                }

                if (previousIf != null)
                {
                    previousIf.Else = new List<ScriptStatement>() { new ScriptThrowStatement(new ScriptCurrentExceptionExpression(typeof(object)) )};
                }

                return statements;
            }
            return null;
        }

        private List<ScriptStatement> Visit(Catch @catch)
        {
            var list = Visit(@catch.Body);
            if (@catch.IsFault)
            {
                list.Add(new ScriptThrowStatement(new ScriptCurrentExceptionExpression(typeof(object))));
            }
            return list;
        }

        public ScriptStatement Visit(DebugPointExpression point)
        {
            return new ScriptDebugPointExpression(point.Point, Visit(point.Value));
        }

        public ScriptStatement Visit(ThrowStatement throwStatement)
        {
            return new ScriptThrowStatement(Visit(throwStatement.Value));
        }

        public ScriptStatement Visit(DoWhileStatement doWhileStatement)
        {
            return new ScriptDoWhileStatement(
                Visit(doWhileStatement.Condition), 
                Visit(doWhileStatement.Body));
        }

        public ScriptStatement Visit(MakeByRefVariableExpression makeByRefVariableExpression)
        {
            return new ScriptVariableReferenceExpression(makeByRefVariableExpression.IlOffset, makeByRefVariableExpression.Variable);
        }

        public ScriptStatement Visit(MakeByRefArgumentExpression makeByRefArgumentExpression)
        {
            throw new NotImplementedException();
        }
    }
}
