using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using NetWebScript.Page;

namespace NetWebScript.UnitTestFramework.Client
{

    [ScriptAvailable]
    public class TestRunnerPage : IScriptPage
    {
        private TestClassInfo[] tests;


        public TestRunnerPage(TestClassInfo[] tests)
        {
            this.tests = tests;
        }

        public void OnLoad()
        {
            var start = new Date();
            var results = new TestRunner().Execute(tests);
            var duration = new Date() - start;

            int ok = 0, ko = 0;
            
            var body = JQuery.Query("body");
            var table = JQuery.Query("<table></table>");
            body.Append(table);

            foreach (var classResult in results)
            {
                var classRow = JQuery.Query("<tr></tr>");
                classRow.Append(JQuery.Query("<td></td>").Attr("colspan", "3").Css("font-weight", "bold").Css("background", "#c0c0c0").Text(classResult.name));
                table.Append(classRow);

                foreach (var methodResult in classResult.methods)
                {
                    var methodRow = JQuery.Query("<tr></tr>");

                    methodRow.Append(JQuery.Query("<td></td>").Text(methodResult.name));

                    if (methodResult.isSuccess)
                    {
                        methodRow.Append(JQuery.Query("<td style=\"color:green;\">OK</td>"));
                        methodRow.Append(JQuery.Query("<td>" + methodResult.duration + " msec</td>"));
                        ok++;
                    }
                    else
                    {
                        methodRow.Append(JQuery.Query("<td style=\"color:red;\">KO</td>"));
                        methodRow.Append(JQuery.Query("<td></td>").Text(methodResult.message));
                        ko++;
                    }
                    table.Append(methodRow);
                }
            }
            var title = JQuery.Query("title");
            if (ko == 0)
            {
                JQuery.Query("<h1></h1>").Css("color", "green").Text("Success - OK : " + ok + "/" + ok).InsertBefore(table);
                title.Text("Success - " + title.Text()); ;
            }
            else
            {
                JQuery.Query("<h1></h1>").Css("color", "red").Text("Failed - OK : " + ok + "/" + (ok + ko)).InsertBefore(table);
                title.Text("Failed - " + title.Text()); ;
            }
            JQuery.Query("<p></p>").Text("Duration : " +duration+" msec").InsertBefore(table);
        }


    }
}
