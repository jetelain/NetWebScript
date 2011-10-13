using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstBuilder.FlowGraph
{
    [System.Diagnostics.DebuggerDisplay("Block {index+1}")]
    public class InstructionBlock : IEnumerable<Instruction>, IComparable<InstructionBlock>
    {

        static readonly InstructionBlock[] NoSuccessors = new InstructionBlock[0];

        int index;
        Instruction first;
        Instruction last;
        InstructionBlock[] successors = NoSuccessors;

        public int Index
        {
            get { return index; }
            internal set { index = value; }
        }

        public Instruction First
        {
            get { return first; }
            //internal set { first = value; }
        }

        public Instruction Last
        {
            get { return last; }
            internal set { last = value; }

        }

        public InstructionBlock[] Successors
        {
            get { return successors; }
            internal set { successors = value; }
        }

        internal static readonly InstructionBlock EndOfFunction = new InstructionBlock();

        private InstructionBlock()
        {
        }

        internal InstructionBlock(Instruction first)
        {
            if (first == null)
                throw new ArgumentNullException("first");

            this.first = first;
        }

        public int CompareTo(InstructionBlock block)
        {
            return first.Offset - block.First.Offset;
        }

        public IEnumerator<Instruction> GetEnumerator()
        {
            var instruction = first;
            while (true)
            {
                yield return instruction;

                if (instruction == last)
                    yield break;

                instruction = instruction.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //private static void RangeTo(InstructionBlock current, InstructionBlock target, List<InstructionBlock> range)
        //{
        //    if (current == target || range.Contains(current))
        //    {
        //        return;
        //    }
        //    range.Add(current);
        //    foreach (InstructionBlock sucessor in current.Successors)
        //    {
        //        RangeTo(sucessor, target, range);
        //    }
        //}

        //public List<InstructionBlock> RangeTo(InstructionBlock target)
        //{
        //    List<InstructionBlock> list = new List<InstructionBlock>();
        //    RangeTo(this, target, list);
        //    return list;
        //}

        public int StackBefore { get { return first.StackBefore; } }

        public int StackAfter { get { return last.StackAfter; } }

    }
}
