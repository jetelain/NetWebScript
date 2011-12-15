#region license
//
//	(C) 2005 - 2007 db4objects Inc. http://www.db4o.com
//	(C) 2007 - 2008 Novell, Inc. http://www.novell.com
//	(C) 2007 - 2008 Jb Evain http://evain.net
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#endregion

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstBuilder.FlowGraph
{
    

    
    internal class ControlFlowGraphBuilder
    {
        private readonly MethodCil body;
        private readonly Dictionary<int, InstructionBlock> blocks = new Dictionary<int, InstructionBlock>();
        private readonly List<ProtectedRegionData> exception_data = new List<ProtectedRegionData>();
        private readonly HashSet<int> exception_objects_offsets = new HashSet<int>();

        internal ControlFlowGraphBuilder(MethodCil body)
        {
            this.body = body;

            DelimitBlocks();
            ConnectBlocks();
            ComputeInstructionData();
            ComputeExceptionHandlerData();
        }

        public ControlFlowGraph ToGraph()
        {
            return new ControlFlowGraph (body, ToArray (), exception_data, exception_objects_offsets);
        }

        void DelimitBlocks()
        {
            MarkBlockStarts(body.Instructions);

            MarkBlockStarts(body.ExceptionHandlers);

            MarkBlockEnds(body.Instructions);
        }

        void MarkBlockStarts(IList<ExceptionHandlingClause> handlers)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                var handler = handlers[i];
                MarkBlockStart(handler.TryOffset);
                MarkBlockStart(handler.HandlerOffset);

                if (handler.Flags == ExceptionHandlingClauseOptions.Filter)
                {
                    MarkExceptionObjectPosition(handler.FilterOffset);
                    MarkBlockStart(handler.FilterOffset);
                }
                else if (handler.Flags == ExceptionHandlingClauseOptions.Clause)
                    MarkExceptionObjectPosition(handler.HandlerOffset);
            }
        }

        void MarkExceptionObjectPosition(int offset)
        {
            exception_objects_offsets.Add(offset);
        }

        void MarkBlockStarts(IList<Instruction> instructions)
        {
            // the first instruction starts a block
            bool first = true;
            foreach (Instruction instruction in instructions)
            {

                if (first)
                {
                    MarkBlockStart(instruction.Offset);
                    first = false;
                }

                if (!IsBlockDelimiter(instruction))
                    continue;

                if (HasMultipleBranches(instruction))
                {
                    // each switch case first instruction starts a block
                    foreach (var target in instruction.OperandOffsetArray)
                        //if (target != null)
                            MarkBlockStart(target);
                }
                else if (instruction.OpCode.FlowControl != FlowControl.Return)
                {
                    // the target of a branch starts a block
                    var target = instruction.OperandOffset;
                    //if (target != null)
                        MarkBlockStart(target);
                }

                // the next instruction after a branch starts a block
                if (instruction.Next != null)
                    MarkBlockStart(instruction.Next.Offset);
            }
        }

        void MarkBlockEnds(IList<Instruction> instructions)
        {
            var blocks = ToArray();
            var current = blocks[0];

            for (int i = 1; i < blocks.Length; ++i)
            {
                var block = blocks[i];
                current.Last = block.First.Previous;
                current = block;
            }

            current.Last = instructions[instructions.Count - 1];
        }

        static bool IsBlockDelimiter(Instruction instruction)
        {
            switch (instruction.OpCode.FlowControl)
            {
                case FlowControl.Break:
                case FlowControl.Branch:
                case FlowControl.Return:
                case FlowControl.Cond_Branch:
                    return true;
            }
            return false;
        }

        void MarkBlockStart(int offset)
        {
            var block = GetBlock(offset);
            if (block != null)
                return;

            block = new InstructionBlock(body.GetInstructionAtOffset(offset));
            RegisterBlock(block);
        }

        void ComputeInstructionData()
        {
            var visited = new HashSet<InstructionBlock>();

            foreach (var block in this.blocks.Values)
            {
                if (!visited.Contains(block))
                {
                    ComputeInstructionData(visited, 0, block);
                }
            }
        }

        void ComputeInstructionData(HashSet<InstructionBlock> visited, int stackHeight, InstructionBlock block)
        {
            if (visited.Contains(block))
            {
                // Vérifie que la taille de la pile est cohérante avec les précedents calculs
                if (IsCatchStart(block.First))
                {
                    stackHeight++;
                }
                if (stackHeight != block.StackBefore)
                {
                    throw new AstBuilderException(block.First.Offset, String.Format("In {2} : Inconsistant stack height : Effective : {0}, Expected : {1}", stackHeight, block.StackBefore, body.Method));
                }
                return;
            }

            visited.Add(block);

            // Calcule la taille de la pile avant et après chaque instruction
            foreach (var instruction in block)
            {
                stackHeight = ComputeInstructionData(stackHeight, instruction);
            }

            // Visite les blocks suivants
            foreach (var successor in block.Successors)
            {
                ComputeInstructionData(visited, stackHeight, successor);
            }
        }

        bool IsCatchStart(Instruction instruction)
        {
            if (exception_objects_offsets == null)
                return false;

            return exception_objects_offsets.Contains(instruction.Offset);
        }

        int ComputeInstructionData(int stackHeight, Instruction instruction)
        {
            if (IsCatchStart(instruction))
                stackHeight++;

            instruction.StackBefore = stackHeight;

            var after = ComputeNewStackHeight(stackHeight, instruction);

            instruction.StackAfter = after;

            return after;
        }

        int ComputeNewStackHeight(int stackHeight, Instruction instruction)
        {
            return stackHeight + GetPushDelta(instruction) - GetPopDelta(stackHeight, instruction);
        }

        static int GetPushDelta(Instruction instruction)
        {
            OpCode code = instruction.OpCode;
            switch (code.StackBehaviourPush)
            {
                case StackBehaviour.Push0:
                    return 0;

                case StackBehaviour.Push1:
                case StackBehaviour.Pushi:
                case StackBehaviour.Pushi8:
                case StackBehaviour.Pushr4:
                case StackBehaviour.Pushr8:
                case StackBehaviour.Pushref:
                    return 1;

                case StackBehaviour.Push1_push1:
                    return 2;

                case StackBehaviour.Varpush:
                    if (code.FlowControl == FlowControl.Call)
                    {
                        var method = instruction.OperandMethodBase as MethodInfo;
                        //return IsVoid (method.ReturnType.ReturnType) ? 0 : 1;
                        return method == null || method.ReturnType == typeof(void) ? 0 : 1;
                    }

                    break;
            }
            throw new ArgumentException(instruction.ToString());
        }

        int GetPopDelta(int stackHeight, Instruction instruction)
        {
            OpCode code = instruction.OpCode;
            switch (code.StackBehaviourPop)
            {
                case StackBehaviour.Pop0:
                    return 0;
                case StackBehaviour.Popi:
                case StackBehaviour.Popref:
                case StackBehaviour.Pop1:
                    return 1;

                case StackBehaviour.Pop1_pop1:
                case StackBehaviour.Popi_pop1:
                case StackBehaviour.Popi_popi:
                case StackBehaviour.Popi_popi8:
                case StackBehaviour.Popi_popr4:
                case StackBehaviour.Popi_popr8:
                case StackBehaviour.Popref_pop1:
                case StackBehaviour.Popref_popi:
                    return 2;

                case StackBehaviour.Popi_popi_popi:
                case StackBehaviour.Popref_popi_popi:
                case StackBehaviour.Popref_popi_popi8:
                case StackBehaviour.Popref_popi_popr4:
                case StackBehaviour.Popref_popi_popr8:
                case StackBehaviour.Popref_popi_popref:
                    return 3;

                //case StackBehaviour.PopAll:
                //	return stackHeight;

                case StackBehaviour.Varpop:
                    if (code.FlowControl == FlowControl.Call)
                    {
                        MethodBase method = instruction.Operand as MethodBase;
                        if (method == null)
                        {
                            throw new AstBuilderException(instruction.Offset, instruction.OpCode + " " + instruction.Operand.GetType() + " " + instruction.Operand);
                        }
                        int count = method.GetParameters().Length;
                        if (!method.IsStatic && OpCodes.Newobj != code)
                            ++count;

                        return count;
                    }

                    if (code == OpCodes.Ret)
                        return body.IsVoidMethod() ? 0 : 1;

                    break;
            }
            throw new ArgumentException(instruction.ToString());
        }

        InstructionBlock[] ToArray()
        {
            var result = new InstructionBlock[blocks.Count];
            blocks.Values.CopyTo(result, 0);
            Array.Sort(result);
            ComputeIndexes(result);
            return result;
        }

        static void ComputeIndexes(InstructionBlock[] blocks)
        {
            for (int i = 0; i < blocks.Length; i++)
                blocks[i].Index = i;
        }

        void ConnectBlocks()
        {
            foreach (InstructionBlock block in blocks.Values)
                ConnectBlock(block);
        }

        void ConnectBlock(InstructionBlock block)
        {
            if (block.Last == null)
                throw new AstBuilderException(block.First.Offset, "Undelimited block at offset " + block.First.Offset);

            var instruction = block.Last;
            switch (instruction.OpCode.FlowControl)
            {
                case FlowControl.Branch:
                case FlowControl.Cond_Branch:
                    {
                        if (HasMultipleBranches(instruction))
                        {
                            var blocks = GetBranchTargetsBlocks(instruction);
                            if (instruction.Next != null)
                                blocks.Add(GetBlock(instruction.Next));

                            block.Successors = blocks.ToArray();
                            break;
                        }

                        var target = GetBranchTargetBlock(instruction);
                        if (instruction.OpCode.FlowControl == FlowControl.Cond_Branch && instruction.Next != null)
                            block.Successors = new[] { target, GetBlock(instruction.Next) };
                        else
                            block.Successors = new[] { target };
                        break;
                    }
                case FlowControl.Call:
                case FlowControl.Next:
                    if (null != instruction.Next)
                        block.Successors = new[] { GetBlock(instruction.Next) };

                    break;
                case FlowControl.Return:
                case FlowControl.Throw:
                    break;
                default:
                    throw new AstBuilderException(instruction.Offset,
                        string.Format("Unhandled instruction flow behavior {0}: {1}",
                                       instruction.OpCode.FlowControl,
                                       instruction.ToString()));
            }
        }

        static bool HasMultipleBranches(Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Switch;
        }

        List<InstructionBlock> GetBranchTargetsBlocks(Instruction instruction)
        {
            var targets = instruction.OperandOffsetArray;
            var blocks = new List<InstructionBlock>(targets.Length);
            for (int i = 0; i < targets.Length; i++)
            {
                blocks.Add(GetBlock(targets[i]));
            }
            return blocks;
        }

        InstructionBlock GetBranchTargetBlock(Instruction instruction)
        {
            return GetBlock(instruction.OperandOffset);
        }

        void RegisterBlock(InstructionBlock block)
        {
            blocks.Add(block.First.Offset, block);
        }

        InstructionBlock GetBlock(Instruction firstInstruction)
        {
            InstructionBlock block;
            blocks.TryGetValue(firstInstruction.Offset, out block);
            return block;
        }

        InstructionBlock GetBlock(int firstOffset)
        {
            InstructionBlock block;
            blocks.TryGetValue(firstOffset, out block);
            return block;
        }

        void ComputeExceptionHandlerData()
        {
            if (body.ExceptionHandlers.Count > 0)
            {
                foreach (ExceptionHandlingClause handler in body.ExceptionHandlers)
                {
                    ComputeExceptionHandlerData(exception_data, handler);
                }

                exception_data.Reverse();

                ConnectHandlerEnd(new List<ProtectedRegionData>(), blocks[0], new HashSet<InstructionBlock>());
            }
        }

        void ComputeExceptionHandlerData(List<ProtectedRegionData> regions, ExceptionHandlingClause handler)
        {
            ProtectedRegionData region = 
                regions.FirstOrDefault(
                    r => r.TryOffset == handler.TryOffset && 
                         r.TryLength == handler.TryLength);

            if (region == null)
            {
                var start = BlockAt(handler.TryOffset);
                region = new ProtectedRegionData(handler.TryOffset, handler.TryLength, start);
                regions.Add(region);
            }

            ComputeExceptionHandlerData(region, handler);

        }

        /*private ICollection<InstructionBlock> FindLeave(ExceptionHandlingClause handler, Instruction instr, int endOffset)
        {
            HashSet<InstructionBlock> leaves = new HashSet<InstructionBlock>();
            while (instr.Offset < endOffset) {
                if (instr.OpCode == OpCodes.Leave || instr.OpCode == OpCodes.Leave_S)
                {

                }
                instr = instr.Next;
            }
            return leaves;
        }*/

        void ConnectHandlerEnd(List<ProtectedRegionData> stack, InstructionBlock block, HashSet<InstructionBlock> seen)
        {
            if (seen.Contains(block))
            {
                return;
            }
            seen.Add(block);

            List<ProtectedRegionData> childStack = new List<ProtectedRegionData>(stack);

            foreach (ProtectedRegionData handler in exception_data)
            {
                if (handler.TryOffset == block.First.Offset)
                {
                    childStack.Add(handler);
                    foreach (CatchHandlerData @catch in handler.Catches)
                    {
                        ConnectHandlerEnd(childStack, @catch.Block, seen);
                    }
                }
            }

            if (block.Last.OpCode == OpCodes.Leave_S || block.Last.OpCode == OpCodes.Leave)
            {
                ProtectedRegionData region = childStack[childStack.Count-1];
                childStack.RemoveAt(childStack.Count - 1);
                region.TryEnd.UnionWith(block.Successors);
            }

            foreach (InstructionBlock next in block.Successors)
            {
                ConnectHandlerEnd(childStack, next, seen);
            }
        }



        void ComputeExceptionHandlerData(ProtectedRegionData data, ExceptionHandlingClause handler)
        {
            var start = BlockAt(handler.HandlerOffset);

            switch (handler.Flags)
            {
                case ExceptionHandlingClauseOptions.Clause:
                    data.Catches.Add(new CatchHandlerData(handler.CatchType, start, false));
                    break;
                case ExceptionHandlingClauseOptions.Finally:
                    data.Finally = start;
                    break;
                case ExceptionHandlingClauseOptions.Fault:
                    data.Catches.Add(new CatchHandlerData(null, start, true));
                    break;
                case ExceptionHandlingClauseOptions.Filter:
                    throw new NotImplementedException(handler.Flags.ToString());
            }
        }

        InstructionBlock BlockAt(int offset)
        {
            return blocks[offset];
        }
    }
}
