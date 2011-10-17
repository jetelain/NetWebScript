using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.Reflection;

namespace NetWebScript.UnitTestFramework.Client
{
    [AnonymousObject]
    public class TestClassInfo
    {
        public string name;
        public ConstructorInfo ctor;
        public TestMethodInfo[] methods;
    }

    [AnonymousObject]
    public class TestMethodInfo
    {
        public string name;
        public MethodInfo method;
    }

    [AnonymousObject]
    public class TestClassResult
    {
        public string name;
        public TestMethodResult[] methods;
    }

    [AnonymousObject]
    public class TestMethodResult
    {
        public string name;
        public string message;
        public bool isSuccess;
        public double duration;
    }

    [ScriptAvailable]
    public class TestRunner
    {
        public TestClassResult Execute(TestClassInfo test)
        {
            TestClassResult result = new TestClassResult();
            result.name = test.name;
            object target = test.ctor.Invoke(new object[0]);
            result.methods = new TestMethodResult[test.methods.Length];
            for(int i =0; i<test.methods.Length;++i)
            {
                result.methods[i] = Execute(target, test.methods[i]);
            }
            return result;
        }

        private TestMethodResult Execute(object target, TestMethodInfo test)
        {
            TestMethodResult result = new TestMethodResult();
            result.name = test.name;
            result.isSuccess = true;
            result.message = null;
            Date start = new Date();
            try
            {
                test.method.Invoke(target, new object[0]);
            }
            catch (Exception e)
            {
                result.isSuccess = false;
                result.message = e.Message;
            }
            result.duration = new Date() - start;
            return result;
        }
    }
}
