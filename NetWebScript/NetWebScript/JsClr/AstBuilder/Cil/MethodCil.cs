using System;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using NetWebScript.JsClr.AstBuilder.PdbInfo;

namespace NetWebScript.JsClr.AstBuilder.Cil
{
    /// <summary>
    /// SDILReader
    /// http://www.codeproject.com/KB/cs/sdilreader.aspx
    /// </summary>
    public sealed class MethodCil
    {
        private readonly static Dictionary<int, object> Cache = new Dictionary<int, object>();

        private readonly static OpCode[] multiByteOpCodes;
        private readonly static OpCode[] singleByteOpCodes;

        static MethodCil()
        {
            singleByteOpCodes = new OpCode[0x100];
            multiByteOpCodes = new OpCode[0x100];
            foreach (FieldInfo info1 in typeof(OpCodes).GetFields())
            {
                if (info1.FieldType == typeof(OpCode))
                {
                    OpCode code1 = (OpCode)info1.GetValue(null);
                    ushort num2 = (ushort)code1.Value;
                    if (num2 < 0x100)
                    {
                        singleByteOpCodes[(int)num2] = code1;
                    }
                    else
                    {
                        if ((num2 & 0xff00) != 0xfe00)
                        {
                            throw new Exception("Invalid OpCode.");
                        }
                        multiByteOpCodes[num2 & 0xff] = code1;
                    }
                }
            }
        }

        private readonly List<Instruction> instructions = new List<Instruction>();
        private readonly MethodBase mi = null;

        #region il read methods
        private ushort ReadUInt16(byte[] il, ref int position)
        {
            return (ushort)((il[position++] | (il[position++] << 8)));
        }
        private int ReadInt32(byte[] il, ref int position)
        {
            return (((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18));
        }
        private ulong ReadInt64(byte[] il, ref int position)
        {
            return (ulong)(((il[position++] | (il[position++] << 8)) | (il[position++] << 0x10)) | (il[position++] << 0x18) | (il[position++] << 0x20) | (il[position++] << 0x28) | (il[position++] << 0x30) | (il[position++] << 0x38));
        }
        private double ReadDouble(byte[] il, ref int position)
        {
            Double v = BitConverter.ToDouble(il, position);
            position += 8;
            return v;
        }
        private sbyte ReadSByte(byte[] il, ref int position)
        {
            return (sbyte)il[position++];
        }
        private byte ReadByte(byte[] il, ref int position)
        {
            return (byte)il[position++];
        }
        private Single ReadSingle(byte[] il, ref int position)
        {
            Single v = BitConverter.ToSingle(il, position);
            position += 4;
            return v;
        }
        #endregion

        /// <summary>
        /// Constructs the array of Instructions according to the IL byte code.
        /// </summary>
        /// <param name="module"></param>
        private void ConstructInstructions(MethodBase mi, MethodBody body)
        {
            Type[] genericTypeArguments = null;
            if (mi.DeclaringType.IsGenericType)
            {
                genericTypeArguments = mi.DeclaringType.GetGenericArguments();
            }
            Type[] genericMethodArguments = null;
            if (mi.IsGenericMethod)
            {
                genericMethodArguments = mi.GetGenericArguments();
            }

            byte[] il = body.GetILAsByteArray();
            Module module = mi.Module;

            int position = 0;
            Instruction previous = null;
            while (position < il.Length)
            {
                Instruction instruction = new Instruction();
                instruction.Previous = previous;
                if (previous != null)
                {
                    previous.Next = instruction;
                }
                previous = instruction;

                // get the operation code of the current instruction
                OpCode code = OpCodes.Nop;
                ushort value = il[position++];
                if (value != 0xfe)
                {
                    code = singleByteOpCodes[(int)value];
                    instruction.Offset = position - 1;
                }
                else
                {
                    value = il[position++];
                    code = multiByteOpCodes[(int)value];
                    value = (ushort)(value | 0xfe00);
                    instruction.Offset = position - 2;
                }
                instruction.OpCode = code;

                int metadataToken = 0;
                // get the operand of the current operation
                switch (code.OperandType)
                {
                    case OperandType.InlineBrTarget:
                        metadataToken = ReadInt32(il, ref position);
                        metadataToken += position;
                        instruction.Operand = metadataToken;
                        break;
                    case OperandType.InlineField:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveField(metadataToken, genericTypeArguments, genericMethodArguments);
                        break;
                    case OperandType.InlineMethod:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveMember(metadataToken, genericTypeArguments, genericMethodArguments);
                        break;
                    case OperandType.InlineSig:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveSignature(metadataToken);
                        break;
                    case OperandType.InlineTok:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveMember(metadataToken, genericTypeArguments, genericMethodArguments);
                        break;
                    case OperandType.InlineType:
                        metadataToken = ReadInt32(il, ref position);
                        instruction.Operand = module.ResolveType(metadataToken, genericTypeArguments, genericMethodArguments);
                        break;
                    case OperandType.InlineI:
                        {
                            instruction.Operand = ReadInt32(il, ref position);
                            break;
                        }
                    case OperandType.InlineI8:
                        {
                            instruction.Operand = ReadInt64(il, ref position);
                            break;
                        }
                    case OperandType.InlineNone:
                        {
                            instruction.Operand = null;
                            break;
                        }
                    case OperandType.InlineR:
                        {
                            instruction.Operand = ReadDouble(il, ref position);
                            break;
                        }
                    case OperandType.InlineString:
                        {
                            metadataToken = ReadInt32(il, ref position);
                            instruction.Operand = module.ResolveString(metadataToken);
                            break;
                        }
                    case OperandType.InlineSwitch:
                        {
                            int count = ReadInt32(il, ref position);
                            int[] casesAddresses = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                casesAddresses[i] = ReadInt32(il, ref position);
                            }
                            int[] cases = new int[count];
                            for (int i = 0; i < count; i++)
                            {
                                cases[i] = position + casesAddresses[i];
                            }
                            instruction.Operand = cases;
                            break;
                        }
                    case OperandType.InlineVar:
                        {
                            instruction.Operand = (int)ReadUInt16(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineBrTarget:
                        {
                            instruction.Operand = ReadSByte(il, ref position) + position;
                            break;
                        }
                    case OperandType.ShortInlineI:
                        {
                            instruction.Operand = (int)ReadSByte(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineR:
                        {
                            instruction.Operand = ReadSingle(il, ref position);
                            break;
                        }
                    case OperandType.ShortInlineVar:
                        {
                            instruction.Operand = (int)ReadByte(il, ref position);
                            break;
                        }
                    default:
                        {
                            throw new AstBuilderException(instruction.Offset, string.Format("Unknown operand type: '{0}'.", code.OperandType.ToString()));
                        }
                }
                instructions.Add(instruction);
            }
        }

        private readonly List<ExceptionHandlingClause> exceptionHandlers;
        private readonly List<LocalVariable> variables = new List<LocalVariable>();
        private readonly List<ParameterInfo> arguments;
        private readonly PdbMethod pdb;
        internal MethodCil(MethodBase mi)
        {
            this.mi = mi;
            pdb = PdbCatalog.GetPdbMethod(mi);

            MethodBody body = mi.GetMethodBody();
            try
            {
                ConstructInstructions(mi, body);
            }
            catch (AstBuilderException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new AstBuilderException(string.Format("Error fetching IL of '{0}' declared by '{1}' : {2}.", mi.ToString(), mi.DeclaringType.FullName, e.Message), e);
            }
            Clean();

            exceptionHandlers = new List<ExceptionHandlingClause>(body.ExceptionHandlingClauses);
            arguments = new List<ParameterInfo>(mi.GetParameters());

            foreach (LocalVariableInfo info in body.LocalVariables)
            {
                if (info.LocalIndex != variables.Count)
                {
                    throw new InvalidOperationException();
                }
                variables.Add(new LocalVariable(pdb, info));
            }

            if (pdb != null)
            {
                foreach (PdbSequencePoint point in pdb.SequencePointList)
                {
                    Instruction instr = GetInstructionAtOffset(point.Offset);
                    instr.Point = point;
                }
            }

        }

        public List<Instruction> Instructions
        {
            get { return instructions; }
        }

        internal List<LocalVariable> Variables
        {
            get { return variables; }
        }

        public List<ParameterInfo> Arguments
        {
            get { return arguments; }
        }

        public List<ExceptionHandlingClause> ExceptionHandlers
        {
            get { return exceptionHandlers; }
        }

        public MethodBase Method
        {
            get { return mi; }
        }

        public Instruction GetInstructionAtOffset(int offset)
        {
            foreach (Instruction instr in instructions)
            {
                if (instr.Offset == offset)
                {
                    return instr;
                }
            }
            throw new ArgumentOutOfRangeException("offset");
        }

        public bool IsVoidMethod()
        {
            MethodInfo method = mi as MethodInfo;
            if (method != null)
            {
                return method.ReturnType == typeof(void);
            }
            return true;
        }

        /// <summary>
        /// Supprime les sauts inutiles
        /// </summary>
        private void Clean()
        {
            foreach (Instruction a in instructions)
            {
                if (a.OpCode == OpCodes.Br || a.OpCode == OpCodes.Br_S)
                {
                    if (a.Next != null && a.Next.Offset == a.OperandOffset)
                    {
                        a.OpCode = OpCodes.Nop;
                        a.Operand = null;
                    }
                }
            }
        }

    }



}
