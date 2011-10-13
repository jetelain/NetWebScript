using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWebScript.JsClr.JsBuilder.JsSyntax;
using NetWebScript.JsClr.JsBuilder.Pattern;

namespace NetWebScript.Test
{
    [TestClass]
    public class JsSyntaxTest
    {
        [TestMethod]
        public void Syntax_SimpleToken()
        {
            JsToken token = JsToken.Name("a").Op("+", JsToken.Name("b"));
            Assert.AreEqual("a+b", token.ToString());

            JsToken token3 = token.Op("*", JsToken.Name("c"));
            Assert.AreEqual("(a+b)*c", token3.ToString());

            JsToken token2 = JsToken.Name("c").Op("*", token);
            Assert.AreEqual("c*(a+b)", token2.ToString());

            JsToken token4 = JsToken.Name("c").Op("-", token);
            Assert.AreEqual("c-(a+b)", token4.ToString());

            Assert.AreEqual("(a+b).toString()", token.Call("toString", new List<JsToken>()).Text);

            Assert.AreEqual("a+b*c", JsToken.Name("a").Op("+", JsToken.Name("b").Op("*", JsToken.Name("c"))).Text);

            Assert.AreEqual("b*c+a", JsToken.Name("b").Op("*", JsToken.Name("c")).Op("+", JsToken.Name("a")).Text);
       
            Assert.AreEqual("a+b-c", JsToken.Name("a").Op("+", JsToken.Name("b")).Op("-", JsToken.Name("c")).ToString());
        }

        [TestMethod]
        public void Syntax_WriterMethodCall()
        {
            JsTokenWriter writer = new JsTokenWriter();
            writer.WriteLeft(JsPrecedence.Member, JsToken.Name("target"));
            writer.Write('.');
            writer.Write("SomeMethod");
            writer.WriteOpenArgs();
            writer.WriteArg(JsToken.Name("arg0"));
            writer.WriteArg(JsToken.Name("arg1"));
            writer.WriteArg(JsToken.Name("arg2"));
            writer.WriteCloseArgs();
            Assert.AreEqual("target.SomeMethod(arg0,arg1,arg2)", writer.ToString());
        }

        [TestMethod]
        public void Syntax_InlineFragment()
        {
            var dict = new Dictionary<string,JsToken>(){{"a",JsToken.Name("xxAxx")},{"b",JsToken.Name("xxBxx")},{"c",JsToken.Name("xxCxx")}};

            var result = new InlineFragment("a.length").Execute(dict);
            Assert.AreEqual("xxAxx.length", result.Text);
            Assert.AreEqual(JsPrecedence.Member, result.Precedence);

            result = new InlineFragment("a.property=b").Execute(dict);
            Assert.AreEqual("xxAxx.property=xxBxx", result.Text);
            Assert.AreEqual(JsPrecedence.Assignement, result.Precedence);

            result = new InlineFragment("a==b").Execute(dict);
            Assert.AreEqual("xxAxx==xxBxx", result.Text);
            Assert.AreEqual(JsPrecedence.EqualityInequality, result.Precedence);

            result = new InlineFragment("a.method(b)").Execute(dict);
            Assert.AreEqual("xxAxx.method(xxBxx)", result.Text);
            Assert.AreEqual(JsPrecedence.FunctionCall, result.Precedence);

            result = new InlineFragment("a.method(b,c)").Execute(dict);
            Assert.AreEqual("xxAxx.method(xxBxx,xxCxx)", result.Text);
            Assert.AreEqual(JsPrecedence.FunctionCall, result.Precedence);

            result = new InlineFragment("new a()").Execute(dict);
            Assert.AreEqual("new xxAxx()", result.Text);
            Assert.AreEqual(JsPrecedence.New, result.Precedence);

            result = new InlineFragment("a+b").Execute(dict);
            Assert.AreEqual("xxAxx+xxBxx", result.Text);
            Assert.AreEqual(JsPrecedence.AddSubtract, result.Precedence);

            result = new InlineFragment("a+b+c").Execute(dict);
            Assert.AreEqual("xxAxx+xxBxx+xxCxx", result.Text);
            Assert.AreEqual(JsPrecedence.AddSubtract, result.Precedence);

            result = new InlineFragment("typeof a==='object'").Execute(dict);
            Assert.AreEqual("typeof xxAxx===\"object\"", result.Text);
            Assert.AreEqual(JsPrecedence.EqualityInequality, result.Precedence);

            result = new InlineFragment("delete a[b]").Execute(dict);
            Assert.AreEqual("delete xxAxx[xxBxx]", result.Text);
            Assert.AreEqual(JsPrecedence.Statement, result.Precedence);

            result = new InlineFragment("a instanceof b").Execute(dict);
            Assert.AreEqual("xxAxx instanceof xxBxx", result.Text);
            Assert.AreEqual(JsPrecedence.Relational, result.Precedence);

            result = new InlineFragment("a.property||null").Execute(dict);
            Assert.AreEqual("xxAxx.property||null", result.Text);
            Assert.AreEqual(JsPrecedence.LogicalOr, result.Precedence);

            result = new InlineFragment("a.method('')").Execute(dict);
            Assert.AreEqual("xxAxx.method(\"\")", result.Text);
            Assert.AreEqual(JsPrecedence.FunctionCall, result.Precedence);

            result = new InlineFragment("type.method(a)").Execute(dict);
            Assert.AreEqual("type.method(xxAxx)", result.Text);
            Assert.AreEqual(JsPrecedence.FunctionCall, result.Precedence);

            result = new InlineFragment("a[b]").Execute(dict);
            Assert.AreEqual("xxAxx[xxBxx]", result.Text);
            Assert.AreEqual(JsPrecedence.Member, result.Precedence);

            result = new InlineFragment("a[b]=c").Execute(dict);
            Assert.AreEqual("xxAxx[xxBxx]=xxCxx", result.Text);
            Assert.AreEqual(JsPrecedence.Assignement, result.Precedence);

            result = new InlineFragment("''").Execute(dict);
            Assert.AreEqual("\"\"", result.Text);
            Assert.AreEqual(JsPrecedence.Name, result.Precedence);

            result = new InlineFragment("constant").Execute(dict);
            Assert.AreEqual("constant", result.Text);
            Assert.AreEqual(JsPrecedence.Name, result.Precedence);
        }
    }
}
