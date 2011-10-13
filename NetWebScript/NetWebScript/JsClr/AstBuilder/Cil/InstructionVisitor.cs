using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;

namespace NetWebScript.JsClr.AstBuilder.Cil
{
    /// <summary>
    /// <see cref="Instruction"/> visitor 
    /// </summary>
    public abstract class InstructionVisitor
    {
        public abstract void OnNop(Instruction instruction);

        public abstract void OnBreak(Instruction instruction);

        public abstract void OnLdarg_0(Instruction instruction);

        public abstract void OnLdarg_1(Instruction instruction);

        public abstract void OnLdarg_2(Instruction instruction);

        public abstract void OnLdarg_3(Instruction instruction);

        public abstract void OnLdloc_0(Instruction instruction);

        public abstract void OnLdloc_1(Instruction instruction);

        public abstract void OnLdloc_2(Instruction instruction);

        public abstract void OnLdloc_3(Instruction instruction);

        public abstract void OnStloc_0(Instruction instruction);

        public abstract void OnStloc_1(Instruction instruction);

        public abstract void OnStloc_2(Instruction instruction);

        public abstract void OnStloc_3(Instruction instruction);

        public abstract void OnLdarg(Instruction instruction);

        public virtual void OnLdarga(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStarg(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnLdloc(Instruction instruction);

        public virtual void OnLdloca(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnStloc(Instruction instruction);

        public abstract void OnLdnull(Instruction instruction);

        public abstract void OnLdc_I4_M1(Instruction instruction);

        public abstract void OnLdc_I4_0(Instruction instruction);

        public abstract void OnLdc_I4_1(Instruction instruction);

        public abstract void OnLdc_I4_2(Instruction instruction);

        public abstract void OnLdc_I4_3(Instruction instruction);

        public abstract void OnLdc_I4_4(Instruction instruction);

        public abstract void OnLdc_I4_5(Instruction instruction);

        public abstract void OnLdc_I4_6(Instruction instruction);

        public abstract void OnLdc_I4_7(Instruction instruction);

        public abstract void OnLdc_I4_8(Instruction instruction);

        public abstract void OnLdc_I4(Instruction instruction);

        public abstract void OnLdc_I8(Instruction instruction);

        public abstract void OnLdc_R4(Instruction instruction);

        public abstract void OnLdc_R8(Instruction instruction);

        public abstract void OnDup(Instruction instruction);

        public abstract void OnPop(Instruction instruction);

        public virtual void OnJmp(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnCall(Instruction instruction);

        public virtual void OnCalli(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnRet(Instruction instruction);

        public abstract void OnBr(Instruction instruction);

        public abstract void OnBrfalse(Instruction instruction);

        public abstract void OnBrtrue(Instruction instruction);

        public abstract void OnBeq(Instruction instruction);

        public abstract void OnBge(Instruction instruction);

        public abstract void OnBgt(Instruction instruction);

        public abstract void OnBle(Instruction instruction);

        public abstract void OnBlt(Instruction instruction);

        public abstract void OnBne_Un(Instruction instruction);

        public abstract void OnBge_Un(Instruction instruction);

        public abstract void OnBgt_Un(Instruction instruction);

        public abstract void OnBle_Un(Instruction instruction);

        public abstract void OnBlt_Un(Instruction instruction);

        public abstract void ONWSitch(Instruction instruction);

        public virtual void OnLdind_I1(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_U1(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_I2(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_U2(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_I4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_U4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_I8(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_I(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_R4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_R8(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdind_Ref(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_Ref(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_I1(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_I2(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_I4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_I8(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_R4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStind_R8(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnAdd(Instruction instruction);

        public abstract void OnSub(Instruction instruction);

        public abstract void OnMul(Instruction instruction);

        public abstract void OnDiv(Instruction instruction);

        public abstract void OnDiv_Un(Instruction instruction);

        public abstract void OnRem(Instruction instruction);

        public abstract void OnRem_Un(Instruction instruction);

        public abstract void OnAnd(Instruction instruction);

        public abstract void OnOr(Instruction instruction);

        public abstract void OnXor(Instruction instruction);

        public abstract void OnShl(Instruction instruction);

        public abstract void OnShr(Instruction instruction);

        public abstract void OnShr_Un(Instruction instruction);

        public abstract void OnNeg(Instruction instruction);

        public abstract void OnNot(Instruction instruction);

        public abstract void OnConv_I1(Instruction instruction);

        public abstract void OnConv_I2(Instruction instruction);

        public abstract void OnConv_I4(Instruction instruction);

        public abstract void OnConv_I8(Instruction instruction);

        public abstract void OnConv_R4(Instruction instruction);

        public abstract void OnConv_R8(Instruction instruction);

        public abstract void OnConv_U4(Instruction instruction);

        public abstract void OnConv_U8(Instruction instruction);

        public abstract void OnCallvirt(Instruction instruction);

        public virtual void OnCpobj(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdobj(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnLdstr(Instruction instruction);

        public abstract void OnNewobj(Instruction instruction);

        public abstract void OnCastclass(Instruction instruction);

        public abstract void OnIsinst(Instruction instruction);

        public abstract void OnConv_R_Un(Instruction instruction);

        public virtual void OnUnbox(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnThrow(Instruction instruction);

        public abstract void OnLdfld(Instruction instruction);

        public virtual void OnLdflda(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnStfld(Instruction instruction);

        public virtual void OnLdsfld(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLdsflda(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStsfld(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnStobj(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I1_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I2_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I4_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I8_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U1_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U2_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U4_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U8_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnBox(Instruction instruction);

        public abstract void OnNewarr(Instruction instruction);

        public abstract void OnLdlen(Instruction instruction);

        public virtual void OnLdelema(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnLdelem_I1(Instruction instruction);

        public abstract void OnLdelem_U1(Instruction instruction);

        public abstract void OnLdelem_I2(Instruction instruction);

        public abstract void OnLdelem_U2(Instruction instruction);

        public abstract void OnLdelem_I4(Instruction instruction);

        public abstract void OnLdelem_U4(Instruction instruction);

        public abstract void OnLdelem_I8(Instruction instruction);

        public abstract void OnLdelem_I(Instruction instruction);

        public abstract void OnLdelem_R4(Instruction instruction);

        public abstract void OnLdelem_R8(Instruction instruction);

        public abstract void OnLdelem_Ref(Instruction instruction);

        public abstract void OnStelem_I(Instruction instruction);

        public abstract void OnStelem_I1(Instruction instruction);

        public abstract void OnStelem_I2(Instruction instruction);

        public abstract void OnStelem_I4(Instruction instruction);

        public abstract void OnStelem_I8(Instruction instruction);

        public abstract void OnStelem_R4(Instruction instruction);

        public abstract void OnStelem_R8(Instruction instruction);

        public abstract void OnStelem_Ref(Instruction instruction);

        public abstract void OnLdelem_Any(Instruction instruction);

        public abstract void OnStelem_Any(Instruction instruction);

        public virtual void OnUnbox_Any(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I1(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U1(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I2(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U2(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U4(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_I8(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U8(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnRefanyval(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnCkfinite(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnMkrefany(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnLdtoken(Instruction instruction);

        public abstract void OnConv_U2(Instruction instruction);

        public abstract void OnConv_U1(Instruction instruction);

        public abstract void OnConv_I(Instruction instruction);
        

        public virtual void OnConv_Ovf_I(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnConv_Ovf_U(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnAdd_Ovf(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnAdd_Ovf_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnMul_Ovf(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnMul_Ovf_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnSub_Ovf(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnSub_Ovf_Un(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnEndfinally(Instruction instruction);

        public abstract void OnLeave(Instruction instruction);

        public virtual void OnStind_I(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnConv_U(Instruction instruction);

        public virtual void OnArglist(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnCeq(Instruction instruction);

        public abstract void OnCgt(Instruction instruction);

        public abstract void OnCgt_Un(Instruction instruction);

        public abstract void OnClt(Instruction instruction);

        public abstract void OnClt_Un(Instruction instruction);

        public abstract void OnLdftn(Instruction instruction);

        public virtual void OnLdvirtftn(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnLocalloc(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnEndfilter(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnUnaligned(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnVolatile(Instruction instruction);

        public virtual void OnTail(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnInitobj(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnCpblk(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnInitblk(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnRethrow(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnSizeof(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public virtual void OnRefanytype(Instruction instruction)
        {
            Unsupported(instruction);
        }

        public abstract void OnConstrained(Instruction instruction);

        private void Unsupported(Instruction instruction)
        {
            throw new AstBuilderException(instruction.Offset, string.Format("IL operation '{0}' is not supported (Instruction: '{1}').", instruction.OpCode.Name, instruction.ToString()));
        }

        public virtual void Visit(Instruction instruction)
        {
            if (instruction.OpCode == OpCodes.Nop)
            {
                OnNop(instruction);
            }
            else if (instruction.OpCode == OpCodes.Break)
            {
                OnBreak(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldarg_0)
            {
                OnLdarg_0(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldarg_1)
            {
                OnLdarg_1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldarg_2)
            {
                OnLdarg_2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldarg_3)
            {
                OnLdarg_3(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldloc_0)
            {
                OnLdloc_0(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldloc_1)
            {
                OnLdloc_1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldloc_2)
            {
                OnLdloc_2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldloc_3)
            {
                OnLdloc_3(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stloc_0)
            {
                OnStloc_0(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stloc_1)
            {
                OnStloc_1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stloc_2)
            {
                OnStloc_2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stloc_3)
            {
                OnStloc_3(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldarg || instruction.OpCode == OpCodes.Ldarg_S)
            {
                OnLdarg(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldarga || instruction.OpCode == OpCodes.Ldarga_S)
            {
                OnLdarga(instruction);
            }
            else if (instruction.OpCode == OpCodes.Starg || instruction.OpCode == OpCodes.Starg_S)
            {
                OnStarg(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldloc || instruction.OpCode == OpCodes.Ldloc_S)
            {
                OnLdloc(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldloca || instruction.OpCode == OpCodes.Ldloca_S)
            {
                OnLdloca(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stloc || instruction.OpCode == OpCodes.Stloc_S)
            {
                OnStloc(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldnull)
            {
                OnLdnull(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_M1)
            {
                OnLdc_I4_M1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_0)
            {
                OnLdc_I4_0(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_1)
            {
                OnLdc_I4_1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_2)
            {
                OnLdc_I4_2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_3)
            {
                OnLdc_I4_3(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_4)
            {
                OnLdc_I4_4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_5)
            {
                OnLdc_I4_5(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_6)
            {
                OnLdc_I4_6(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_7)
            {
                OnLdc_I4_7(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4_8)
            {
                OnLdc_I4_8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I4 || instruction.OpCode == OpCodes.Ldc_I4_S)
            {
                OnLdc_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_I8)
            {
                OnLdc_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_R4)
            {
                OnLdc_R4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldc_R8)
            {
                OnLdc_R8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Dup)
            {
                OnDup(instruction);
            }
            else if (instruction.OpCode == OpCodes.Pop)
            {
                OnPop(instruction);
            }
            else if (instruction.OpCode == OpCodes.Jmp)
            {
                OnJmp(instruction);
            }
            else if (instruction.OpCode == OpCodes.Call)
            {
                OnCall(instruction);
            }
            else if (instruction.OpCode == OpCodes.Calli)
            {
                OnCalli(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ret)
            {
                OnRet(instruction);
            }
            else if (instruction.OpCode == OpCodes.Br || instruction.OpCode == OpCodes.Br_S)
            {
                OnBr(instruction);
            }
            else if (instruction.OpCode == OpCodes.Brfalse || instruction.OpCode == OpCodes.Brfalse_S)
            {
                OnBrfalse(instruction);
            }
            else if (instruction.OpCode == OpCodes.Brtrue || instruction.OpCode == OpCodes.Brtrue_S)
            {
                OnBrtrue(instruction);
            }
            else if (instruction.OpCode == OpCodes.Beq || instruction.OpCode == OpCodes.Beq_S)
            {
                OnBeq(instruction);
            }
            else if (instruction.OpCode == OpCodes.Bge || instruction.OpCode == OpCodes.Bge_S)
            {
                OnBge(instruction);
            }
            else if (instruction.OpCode == OpCodes.Bgt || instruction.OpCode == OpCodes.Bgt_S)
            {
                OnBgt(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ble || instruction.OpCode == OpCodes.Ble_S)
            {
                OnBle(instruction);
            }
            else if (instruction.OpCode == OpCodes.Blt || instruction.OpCode == OpCodes.Blt_S)
            {
                OnBlt(instruction);
            }
            else if (instruction.OpCode == OpCodes.Bne_Un || instruction.OpCode == OpCodes.Bne_Un_S)
            {
                OnBne_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Bge_Un || instruction.OpCode == OpCodes.Bge_Un_S)
            {
                OnBge_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Bgt_Un || instruction.OpCode == OpCodes.Bgt_Un_S)
            {
                OnBgt_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ble_Un || instruction.OpCode == OpCodes.Ble_Un_S)
            {
                OnBle_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Blt_Un || instruction.OpCode == OpCodes.Blt_Un_S)
            {
                OnBlt_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Switch)
            {
                ONWSitch(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_I1)
            {
                OnLdind_I1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_U1)
            {
                OnLdind_U1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_I2)
            {
                OnLdind_I2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_U2)
            {
                OnLdind_U2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_I4)
            {
                OnLdind_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_U4)
            {
                OnLdind_U4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_I8)
            {
                OnLdind_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_I)
            {
                OnLdind_I(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_R4)
            {
                OnLdind_R4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_R8)
            {
                OnLdind_R8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldind_Ref)
            {
                OnLdind_Ref(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_Ref)
            {
                OnStind_Ref(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_I1)
            {
                OnStind_I1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_I2)
            {
                OnStind_I2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_I4)
            {
                OnStind_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_I8)
            {
                OnStind_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_R4)
            {
                OnStind_R4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_R8)
            {
                OnStind_R8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Add)
            {
                OnAdd(instruction);
            }
            else if (instruction.OpCode == OpCodes.Sub)
            {
                OnSub(instruction);
            }
            else if (instruction.OpCode == OpCodes.Mul)
            {
                OnMul(instruction);
            }
            else if (instruction.OpCode == OpCodes.Div)
            {
                OnDiv(instruction);
            }
            else if (instruction.OpCode == OpCodes.Div_Un)
            {
                OnDiv_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Rem)
            {
                OnRem(instruction);
            }
            else if (instruction.OpCode == OpCodes.Rem_Un)
            {
                OnRem_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.And)
            {
                OnAnd(instruction);
            }
            else if (instruction.OpCode == OpCodes.Or)
            {
                OnOr(instruction);
            }
            else if (instruction.OpCode == OpCodes.Xor)
            {
                OnXor(instruction);
            }
            else if (instruction.OpCode == OpCodes.Shl)
            {
                OnShl(instruction);
            }
            else if (instruction.OpCode == OpCodes.Shr)
            {
                OnShr(instruction);
            }
            else if (instruction.OpCode == OpCodes.Shr_Un)
            {
                OnShr_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Neg)
            {
                OnNeg(instruction);
            }
            else if (instruction.OpCode == OpCodes.Not)
            {
                OnNot(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_I1)
            {
                OnConv_I1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_I2)
            {
                OnConv_I2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_I4)
            {
                OnConv_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_I8)
            {
                OnConv_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_R4)
            {
                OnConv_R4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_R8)
            {
                OnConv_R8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_U4)
            {
                OnConv_U4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_U8)
            {
                OnConv_U8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Callvirt)
            {
                OnCallvirt(instruction);
            }
            else if (instruction.OpCode == OpCodes.Cpobj)
            {
                OnCpobj(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldobj)
            {
                OnLdobj(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldstr)
            {
                OnLdstr(instruction);
            }
            else if (instruction.OpCode == OpCodes.Newobj)
            {
                OnNewobj(instruction);
            }
            else if (instruction.OpCode == OpCodes.Castclass)
            {
                OnCastclass(instruction);
            }
            else if (instruction.OpCode == OpCodes.Isinst)
            {
                OnIsinst(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_R_Un)
            {
                OnConv_R_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Unbox)
            {
                OnUnbox(instruction);
            }
            else if (instruction.OpCode == OpCodes.Throw)
            {
                OnThrow(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldfld)
            {
                OnLdfld(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldflda)
            {
                OnLdflda(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stfld)
            {
                OnStfld(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldsfld)
            {
                OnLdsfld(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldsflda)
            {
                OnLdsflda(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stsfld)
            {
                OnStsfld(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stobj)
            {
                OnStobj(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I1_Un)
            {
                OnConv_Ovf_I1_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I2_Un)
            {
                OnConv_Ovf_I2_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I4_Un)
            {
                OnConv_Ovf_I4_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I8_Un)
            {
                OnConv_Ovf_I8_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U1_Un)
            {
                OnConv_Ovf_U1_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U2_Un)
            {
                OnConv_Ovf_U2_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U4_Un)
            {
                OnConv_Ovf_U4_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U8_Un)
            {
                OnConv_Ovf_U8_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I_Un)
            {
                OnConv_Ovf_I_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U_Un)
            {
                OnConv_Ovf_U_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Box)
            {
                OnBox(instruction);
            }
            else if (instruction.OpCode == OpCodes.Newarr)
            {
                OnNewarr(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldlen)
            {
                OnLdlen(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelema)
            {
                OnLdelema(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_I1)
            {
                OnLdelem_I1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_U1)
            {
                OnLdelem_U1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_I2)
            {
                OnLdelem_I2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_U2)
            {
                OnLdelem_U2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_I4)
            {
                OnLdelem_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_U4)
            {
                OnLdelem_U4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_I8)
            {
                OnLdelem_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_I)
            {
                OnLdelem_I(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_R4)
            {
                OnLdelem_R4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_R8)
            {
                OnLdelem_R8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem_Ref)
            {
                OnLdelem_Ref(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_I)
            {
                OnStelem_I(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_I1)
            {
                OnStelem_I1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_I2)
            {
                OnStelem_I2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_I4)
            {
                OnStelem_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_I8)
            {
                OnStelem_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_R4)
            {
                OnStelem_R4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_R8)
            {
                OnStelem_R8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem_Ref)
            {
                OnStelem_Ref(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldelem)
            {
                OnLdelem_Any(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stelem)
            {
                OnStelem_Any(instruction);
            }
            else if (instruction.OpCode == OpCodes.Unbox_Any)
            {
                OnUnbox_Any(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I1)
            {
                OnConv_Ovf_I1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U1)
            {
                OnConv_Ovf_U1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I2)
            {
                OnConv_Ovf_I2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U2)
            {
                OnConv_Ovf_U2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I4)
            {
                OnConv_Ovf_I4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U4)
            {
                OnConv_Ovf_U4(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I8)
            {
                OnConv_Ovf_I8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U8)
            {
                OnConv_Ovf_U8(instruction);
            }
            else if (instruction.OpCode == OpCodes.Refanyval)
            {
                OnRefanyval(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ckfinite)
            {
                OnCkfinite(instruction);
            }
            else if (instruction.OpCode == OpCodes.Mkrefany)
            {
                OnMkrefany(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldtoken)
            {
                OnLdtoken(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_U2)
            {
                OnConv_U2(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_U1)
            {
                OnConv_U1(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_I)
            {
                OnConv_I(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_I)
            {
                OnConv_Ovf_I(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_Ovf_U)
            {
                OnConv_Ovf_U(instruction);
            }
            else if (instruction.OpCode == OpCodes.Add_Ovf)
            {
                OnAdd_Ovf(instruction);
            }
            else if (instruction.OpCode == OpCodes.Add_Ovf_Un)
            {
                OnAdd_Ovf_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Mul_Ovf)
            {
                OnMul_Ovf(instruction);
            }
            else if (instruction.OpCode == OpCodes.Mul_Ovf_Un)
            {
                OnMul_Ovf_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Sub_Ovf)
            {
                OnSub_Ovf(instruction);
            }
            else if (instruction.OpCode == OpCodes.Sub_Ovf_Un)
            {
                OnSub_Ovf_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Endfinally)
            {
                OnEndfinally(instruction);
            }
            else if (instruction.OpCode == OpCodes.Leave || instruction.OpCode == OpCodes.Leave_S)
            {
                OnLeave(instruction);
            }
            else if (instruction.OpCode == OpCodes.Stind_I)
            {
                OnStind_I(instruction);
            }
            else if (instruction.OpCode == OpCodes.Conv_U)
            {
                OnConv_U(instruction);
            }
            else if (instruction.OpCode == OpCodes.Arglist)
            {
                OnArglist(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ceq)
            {
                OnCeq(instruction);
            }
            else if (instruction.OpCode == OpCodes.Cgt)
            {
                OnCgt(instruction);
            }
            else if (instruction.OpCode == OpCodes.Cgt_Un)
            {
                OnCgt_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Clt)
            {
                OnClt(instruction);
            }
            else if (instruction.OpCode == OpCodes.Clt_Un)
            {
                OnClt_Un(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldftn)
            {
                OnLdftn(instruction);
            }
            else if (instruction.OpCode == OpCodes.Ldvirtftn)
            {
                OnLdvirtftn(instruction);
            }
            else if (instruction.OpCode == OpCodes.Localloc)
            {
                OnLocalloc(instruction);
            }
            else if (instruction.OpCode == OpCodes.Endfilter)
            {
                OnEndfilter(instruction);
            }
            else if (instruction.OpCode == OpCodes.Unaligned)
            {
                OnUnaligned(instruction);
            }
            else if (instruction.OpCode == OpCodes.Volatile)
            {
                OnVolatile(instruction);
            }
            else if (instruction.OpCode == OpCodes.Tailcall)
            {
                OnTail(instruction);
            }
            else if (instruction.OpCode == OpCodes.Initobj)
            {
                OnInitobj(instruction);
            }
            else if (instruction.OpCode == OpCodes.Cpblk)
            {
                OnCpblk(instruction);
            }
            else if (instruction.OpCode == OpCodes.Initblk)
            {
                OnInitblk(instruction);
            }
            else if (instruction.OpCode == OpCodes.Rethrow)
            {
                OnRethrow(instruction);
            }
            else if (instruction.OpCode == OpCodes.Sizeof)
            {
                OnSizeof(instruction);
            }
            else if (instruction.OpCode == OpCodes.Refanytype)
            {
                OnRefanytype(instruction);
            }
            else if (instruction.OpCode == OpCodes.Constrained)
            {
                OnConstrained(instruction);
            }
            else
            {
                Unsupported(instruction);
            }
        }

        

    }
}
