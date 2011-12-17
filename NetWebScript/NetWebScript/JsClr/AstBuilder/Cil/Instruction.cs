
using System;
using System.Reflection.Emit;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.PdbInfo;

namespace NetWebScript.JsClr.AstBuilder.Cil
{
	public sealed class Instruction
	{
        private static readonly OpCode[] neuralOpCodes = new[] { 
        OpCodes.Ret,
        OpCodes.Ldc_I4_0,
        OpCodes.Ldc_I4_1
        // FIXME: Be more exaustive
        };
        
        public bool CanRiseException
        {
            get { return Array.IndexOf<OpCode>(neuralOpCodes, OpCode) == -1; }
        }




        public Instruction Next { get; internal set; }

        public Instruction Previous { get; internal set; }

        public int StackBefore { get; internal set; }

        public int StackAfter { get; internal set; }

		public OpCode OpCode { get; internal set; }

        public object Operand { get; internal set; }

        //public byte[] OperandData { get; internal set; }

        public int Offset { get; internal set; }

        internal PdbSequencePoint Point { get; set; }

        public FieldInfo OperandField
        {
            get { return (FieldInfo)Operand; }
        }
		
        public MethodInfo OperandMethod
        {
            get { return (MethodInfo)Operand; }
        }
		public MethodBase OperandMethodBase
        {
            get { return (MethodBase)Operand; }
        }
		public Type OperandSystemType
        {
            get { return (Type)Operand; }
        }
		
		public ConstructorInfo OperandConstructor
        {
            get { return (ConstructorInfo)Operand; }
        }
		
		public int OperandOffset
        {
            get { return (int)Operand; }
        }
		
		public int[] OperandOffsetArray
        {
            get { return (int[])Operand; }
        }
		
		public int OperandVariableIndex
        {
            get { return (int)Operand; }
        }		
		
		public int OperandArgumentIndex 
		{
            get { return (int)Operand; }
		}
        
        public override string  ToString()
        {
            string result = "";
            result += String.Format("{0:0000}",Offset) + " : " + OpCode;
            if (Operand != null)
            {
                switch (OpCode.OperandType)
                {
                    case OperandType.InlineField:
                        System.Reflection.FieldInfo fOperand = OperandField;
                        result += " " + fOperand.FieldType.ToString() + " " +
                            fOperand.ReflectedType.ToString() +
                            "::" + fOperand.Name + "";
                        break;
                    case OperandType.InlineMethod:
                        try
                        {
                            System.Reflection.MethodInfo mOperand = OperandMethod;
                            result += " ";
                            if (!mOperand.IsStatic) result += "instance ";
                            result += mOperand.ReturnType.ToString() +
                                " " + mOperand.ReflectedType.ToString() +
                                "::" + mOperand.Name + "()";
                        }
                        catch
                        {
                            try
                            {
                                System.Reflection.ConstructorInfo mOperand = OperandConstructor;
                                result += " ";
                                if (!mOperand.IsStatic) result += "instance ";
                                result += "void " +
                                    mOperand.ReflectedType.ToString() +
                                    "::" + mOperand.Name + "()";
							
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case OperandType.ShortInlineBrTarget:
                    case OperandType.InlineBrTarget:
                        result += " " + String.Format("{0:0000}", (int)Operand);
                        break;
                    case OperandType.InlineType:
                        result += " " + Operand.ToString();
                        break;
                    case OperandType.InlineString:
                        if (Operand.ToString() == "\r\n") result += " \"\\r\\n\"";
                        else result += " \"" + Operand.ToString() + "\"";
                        break;
                    case OperandType.ShortInlineVar:
                        result += " " + Operand.ToString();
                        break;
                    case OperandType.InlineI:
                    case OperandType.InlineI8:
                    case OperandType.InlineR:
                    case OperandType.ShortInlineI:
                    case OperandType.ShortInlineR:
                        result += " " + Operand.ToString();
                        break;
                    case OperandType.InlineTok:
                        Type type = Operand as Type;
                        if (type != null)
                        {
                            result += " " + type.FullName;
                        }
                        else
                        {
                            FieldInfo field = Operand as FieldInfo;
                            if (field != null)
                            {
                                result += field.Name + " ("+field.FieldType.FullName+")";
                            }
                            else
                            {

                                result += Operand.ToString();
                            }
                        }
                        break;
                    default: result += "not supported"; break;
                }
            }
            if (Point != null)
            {
                return result + " << " + StackBefore + " >> " + Point;
            }
            return result;

        }
    }
}
