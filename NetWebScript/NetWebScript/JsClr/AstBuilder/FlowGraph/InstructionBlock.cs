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

        private int index;
        private readonly Instruction first;
        private Instruction last;
        private InstructionBlock[] successors = NoSuccessors;

        public int Index
        {
            get { return index; }
            internal set { index = value; }
        }

        public Instruction First
        {
            get { return first; }
        }

        public Instruction Last
        {
            get { return last; }
            internal set { last = value; lowestStack = -1; }

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

        /// <summary>
        /// Stack size before first instruction of block
        /// </summary>
        public int StackBefore { get { return first.StackBefore; } }

        /// <summary>
        /// Stack size after last instruction of block
        /// </summary>
        public int StackAfter { get { return last.StackAfter; } }

        private int lowestStack = -1;

        /// <summary>
        /// Smallest stack before or after any instruction of block
        /// </summary>
        public int LowestStack 
        { 
            get 
            {
                if (lowestStack == -1)
                {
                    lowestStack = this.Min(i => Math.Min(i.StackAfter, i.StackBefore));
                }
                return lowestStack;
            } 
        }

    }
}
