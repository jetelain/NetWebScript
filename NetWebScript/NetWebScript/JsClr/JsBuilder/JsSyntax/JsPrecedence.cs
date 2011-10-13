using System;

namespace NetWebScript.JsClr.JsBuilder.JsSyntax
{
    /// <summary>
    /// Precedence of JavaScript operators (and operations)
    /// 
    /// https://developer.mozilla.org/en/JavaScript/Reference/Operators/Operator_Precedence
    /// </summary>
    [Flags]
    public enum JsPrecedence
    {
        LeftToRight = 0x1000,

        RightToLeft = 0x2000,

        AssocMask = 0x3000,

        PrecedenceMask = 0xfff,

        Name = 0,

        /// <summary>
        /// . []
        /// </summary>
        Member = 1 | LeftToRight,

        /// <summary>
        /// new
        /// </summary>
        New = 1 | RightToLeft,

        /// <summary>
        /// ()
        /// </summary>
        FunctionCall = 2 | LeftToRight,

        /// <summary>
        /// ++ -- (unary)
        /// </summary>
        PostPreIncrementDecrement = 3,

        /// <summary>
        /// ! - ~ (unary)
        /// </summary>
        NegateNot = 4 | RightToLeft,

        /// <summary>
        /// * / %
        /// </summary>
        MultiplyDivideModulo = 5 | LeftToRight,

        /// <summary>
        /// + -
        /// </summary>
        AddSubtract = 6 | LeftToRight,

        /// <summary>
        /// &gt;&gt; &lt;&lt;
        /// </summary>
        Shift = 7 | LeftToRight,

        /// <summary>
        /// &gt; &gt;= &lt; &lt;= instanceof
        /// </summary>
        Relational = 8 | LeftToRight,

        /// <summary>
        /// == != === !==
        /// </summary>
        EqualityInequality = 9 | LeftToRight,

        /// <summary>
        /// &amp;
        /// </summary>
        BitwiseAnd = 10 | LeftToRight,

        /// <summary>
        /// ^
        /// </summary>
        BitwiseXor = 11 | LeftToRight,

        /// <summary>
        /// |
        /// </summary>
        BitwiseOr = 12 | LeftToRight,

        /// <summary>
        /// &amp;&amp;
        /// </summary>
        LogicalAnd = 13 | LeftToRight,

        /// <summary>
        /// ||
        /// </summary>
        LogicalOr = 14 | LeftToRight,

        /// <summary>
        /// ?:
        /// </summary>
        Conditional = 15 | RightToLeft,

        /// <summary>
        /// = += -= |= &amp;= ^= *+ /= %=
        /// </summary>
        Assignement = 16 | RightToLeft,

        /// <summary>
        /// ,
        /// </summary>
        Comma = 17 | LeftToRight,

        
        /// <summary>
        /// others
        /// </summary>
        Other = 20,

        /// <summary>
        /// Statement
        /// </summary>
        Statement = 25,

        /// <summary>
        /// Statement (no need of ;)
        /// </summary>
        FullStatement = 30
    }
}
