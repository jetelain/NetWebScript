using System;

namespace NetWebScript.JsClr.ScriptAst
{
	public enum ScriptBinaryOperator {
		Add,
		Subtract,
		Multiply,
		Divide,
		ValueEquality,
		ValueInequality,
		LogicalOr,
		LogicalAnd,
		LessThan,
		LessThanOrEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LeftShift,
		RightShift,
		BitwiseOr,
		BitwiseAnd,
		BitwiseXor,
		Modulo,
	}
}
