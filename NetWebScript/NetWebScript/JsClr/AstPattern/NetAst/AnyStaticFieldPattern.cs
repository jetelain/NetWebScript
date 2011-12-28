using System;

namespace NetWebScript.JsClr.AstPattern.NetAst
{
    class AnyStaticFieldPattern : NetPatternBase
    {
        internal string Name { get; set; }

        internal Type Type { get; set; }

        public override PatternMatch Visit(Ast.FieldReferenceExpression fieldReferenceExpression)
        {
            if (fieldReferenceExpression.Target == null)
            {
                if (Type == null || fieldReferenceExpression.Field.FieldType == Type)
                {
                    return new PatternMatch(Name, fieldReferenceExpression.Field);
                }
            }
            return null;
        }
    }
}
