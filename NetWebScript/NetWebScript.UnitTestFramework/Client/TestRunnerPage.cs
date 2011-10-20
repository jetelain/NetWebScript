using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.UnitTestFramework.Client
{

    [ScriptAvailable]
    public class TestRunnerPage
    {
        private TestClassInfo[] tests;


        public TestRunnerPage(TestClassInfo[] tests)
        {
            this.tests = tests;
        }

        public void Run()
        {
            var results = new TestRunner().Execute(tests);

            var body = JQuery.Select("body");
            var table = JQuery.Select("<table></table>");
            body.Append(table);

            for (int i = 0; i < results.Length; ++i)
            {
                var classResult = results[i];

                var classRow = JQuery.Select("<tr></tr>");
                classRow.Append(JQuery.Select("<td></td>").Attr("colspan", "3").Text(classResult.name));
                table.Append(classRow);

                for (int j = 0; j < classResult.methods.Length; ++j)
                {
                    var methodResult = classResult.methods[j];
                    var methodRow = JQuery.Select("<tr></tr>");

                    methodRow.Append(JQuery.Select("<td></td>").Text(methodResult.name));

                    if (methodResult.isSuccess)
                    {
                        methodRow.Append(JQuery.Select("<td style=\"color:green;\">OK</td>"));
                        methodRow.Append(JQuery.Select("<td>" + methodResult.duration + " msec</td>"));
                    }
                    else
                    {
                        methodRow.Append(JQuery.Select("<td style=\"color:red;\">KO</td>"));
                        methodRow.Append(JQuery.Select("<td></td>").Text(methodResult.message));
                    }
                    table.Append(methodRow);
                }
            }

        }
    }
}
