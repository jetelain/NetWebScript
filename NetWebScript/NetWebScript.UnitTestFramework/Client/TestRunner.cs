using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.UnitTestFramework.Client
{
    class TestMethodInfo
    {
        internal string name;
        internal JSFunction function;
    }

    class TestMethodResult
    {
        internal string name;
        internal string message;
        internal bool isSuccess;
        internal double duration;
    }

    [ScriptAvailable]
    class TestRunner
    {

        public TestMethodResult Execute(object target, TestMethodInfo test)
        {
            TestMethodResult result = new TestMethodResult();
            result.name = test.name;
            result.isSuccess = true;
            result.message = null;
            Date start = new Date();
            try
            {
                test.function.Apply(target);
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
