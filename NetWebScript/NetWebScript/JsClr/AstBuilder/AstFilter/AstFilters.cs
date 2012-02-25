
namespace NetWebScript.JsClr.AstBuilder.AstFilter
{
    public class AstFilters
    {
        public static void Filter(MethodAst method)
        {
            new LastReturn().Visit(method);
            new BinaryFix().Visit(method);
            new LiteralsTypeFix().Visit(method);
            new LogicOperations().Visit(method);
            new NullTest().Visit(method);
            new SwitchCaseString().Visit(method);
            new VariableRef().Visit(method);
            new ArrayInitialisation().Visit(method);
        }
    }
}
