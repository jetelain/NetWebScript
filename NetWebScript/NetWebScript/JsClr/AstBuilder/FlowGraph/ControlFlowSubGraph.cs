using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstBuilder.FlowGraph
{
    public class ControlFlowSubGraph
    {
        private readonly InstructionBlock loopStart;
        private readonly InstructionBlock loopEnd;

        private readonly InstructionBlock first;
        private readonly InstructionBlock last;
        private readonly List<ProtectedRegionData> regions;

        internal ControlFlowSubGraph(ControlFlowGraph graph)
        {
            this.first = graph.StartBlock;
            this.regions = graph.ExceptionData;
        }

        private ControlFlowSubGraph(InstructionBlock first, InstructionBlock last, InstructionBlock loopStart, InstructionBlock loopEnd, List<ProtectedRegionData> regions)
        {
            this.first = first;
            this.last = last;
            this.regions = regions;
            this.loopEnd = loopEnd;
            this.loopStart = loopStart;
        }

        /// <summary>
        /// Début (inclus)
        /// </summary>
        public InstructionBlock First
        {
            get { return first; }
        }

        /// <summary>
        /// Fin (non inclus)
        /// </summary>
        public InstructionBlock End
        {
            get { return last; }
        }

        public InstructionBlock LoopEnd
        {
            get { return loopEnd; }
        }

        public InstructionBlock LoopStart
        {
            get { return loopStart; }
        }

        internal ControlFlowSubGraph SubGraph(InstructionBlock first, InstructionBlock last)
        {
            return new ControlFlowSubGraph(first, last, loopStart, loopEnd, regions);
        }

        internal ControlFlowSubGraph SubGraph(InstructionBlock first, InstructionBlock last, InstructionBlock subLoopStart, InstructionBlock subLoopEnd)
        {
            return new ControlFlowSubGraph(first, last, subLoopStart, subLoopEnd, regions);
        }

        internal ControlFlowSubGraph SubGraph(InstructionBlock first, InstructionBlock last, ProtectedRegionData inregion)
        {
            List<ProtectedRegionData> subregions = new List<ProtectedRegionData>(regions);
            subregions.Remove(inregion);
            return new ControlFlowSubGraph(first, last, loopStart, loopEnd, subregions);
        }

        private List<InstructionBlock> Sucessors(InstructionBlock block)
        {
            HashSet<InstructionBlock> sucessors = new HashSet<InstructionBlock>();
            Sucessors(sucessors, block);
            return sucessors.ToList();
        }

        internal IEnumerable<ProtectedRegionData> ExceptionData
        {
            get { return regions; }
        }

        private void Sucessors(HashSet<InstructionBlock> sucessors, InstructionBlock block)
        {
            if (!sucessors.Contains(block))
            {
                sucessors.Add(block);
                if (block.Successors.Length == 0)
                {
                    sucessors.Add(InstructionBlock.EndOfFunction);
                }
                else
                {
                    foreach (InstructionBlock sucessor in block.Successors)
                    {
                        Sucessors(sucessors, sucessor);
                    }
                }
            }
        }

        /// <summary>
        /// Détermine le sucesseur commun
        /// </summary>
        /// <param name="block"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool HasCommonSucessor(InstructionBlock a, InstructionBlock b, out InstructionBlock common)
        {
            List<InstructionBlock> aSucessors = Sucessors(a);
            List<InstructionBlock> bSucessors = Sucessors(b);

            List<InstructionBlock> intersect = aSucessors.Intersect(bSucessors).ToList();
            
            common = null;
            if (intersect.Count  > 0 )
            {
                foreach (InstructionBlock candidate in intersect)
                {
                    if (candidate == InstructionBlock.EndOfFunction)
                    {
                        continue;
                    }
                    if (DoesAllPathsGoTo(a, candidate) && DoesAllPathsGoTo(b, candidate))
                    {
                        common = candidate;
                        return true;
                    }
                }
                if (intersect.Contains(InstructionBlock.EndOfFunction))
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasCommonSucessor(InstructionBlock[] blocks, out InstructionBlock common)
        {
            InstructionBlock a = blocks[0];
            InstructionBlock b = null;
            for (int i = 1; i < blocks.Length; ++i)
            {
                if (!HasCommonSucessor(a, blocks[i], out b))
                {
                    common = null;
                    return false;
                }
                a = b ?? InstructionBlock.EndOfFunction;
            }
            if (a == InstructionBlock.EndOfFunction)
            {
                common = null;
            }
            else
            {
                common = a;
            }
            return true;
        }

        /// <summary>
        /// Determines if all paths starting from <paramref name="block"/> goes to <paramref name="target"/>.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool DoesAllPathsGoTo(InstructionBlock block, InstructionBlock target)
        {
            return DoesAllPathsGoTo(new HashSet<InstructionBlock>(), block, target);
        }

        /// <summary>
        /// Determines if all paths starting from <paramref name="block"/> goes to one of <paramref name="targets"/> or reach end of the method (return).
        /// </summary>
        /// <param name="block"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public bool DoesAllPathsGoToOrReturn(InstructionBlock block, InstructionBlock[] targets)
        {
            var visited = new HashSet<InstructionBlock>();
            foreach (var suc in block.Successors)
            {
                if (!DoesAllPathsGoToOrReturn(visited, block, targets))
                {
                    return false;
                }
            }
            return true;
        }

        private bool DoesAllPathsGoToOrReturn(HashSet<InstructionBlock> done, InstructionBlock block, InstructionBlock[] targets)
        {
            if (targets.Contains(block))
            {
                return true;
            }
            done.Add(block);

            if (block.Successors.Length == 0)
            {
                return true;
            }

            int count = 0;
            foreach (InstructionBlock sucessor in block.Successors)
            {
                if (!IsLimit(sucessor))
                {
                    count++;
                    if (!done.Contains(sucessor))
                    {
                        if (!DoesAllPathsGoToOrReturn(done, sucessor, targets))
                        {
                            return false;
                        }
                    }
                }
            }
            return count > 0;
        }

        private bool DoesAllPathsGoTo(HashSet<InstructionBlock> done, InstructionBlock block, InstructionBlock target)
        {
            if (target == block)
            {
                return true;
            }
            done.Add(block);

            if (block.Successors.Length == 0)
            {
                return false;
            }

            int count = 0;
            foreach (InstructionBlock sucessor in block.Successors)
            {
                if (!IsLimit(sucessor))
                {
                    count++;
                    if (!done.Contains(sucessor))
                    {
                        if (!DoesAllPathsGoTo(done, sucessor, target))
                        {
                            return false;
                        }
                    }
                }
            }
            return count > 0;
        }

        /// <summary>
        /// Existe il un chemin entre "block" et "target" ?
        /// </summary>
        /// <param name="block"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool HasPathTo(InstructionBlock block, InstructionBlock target)
        {
            var visited = new HashSet<InstructionBlock>();
            foreach (InstructionBlock sucessor in block.Successors)
            {
                if (HasPathTo(visited, sucessor, target))
                {
                    return true;
                }
            }
            return false;
        }

        private bool HasPathTo(HashSet<InstructionBlock> visited, InstructionBlock block, InstructionBlock target)
        {
            if (IsLimit(block) || visited.Contains(block))
            {
                return false;
            }
            if (block == target)
            {
                return true;
            }
            visited.Add(block);
            foreach (InstructionBlock sucessor in block.Successors)
            {
                if (HasPathTo(visited, sucessor, target))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEnd(InstructionBlock block)
        {
            return last == block;
        }

        public bool IsLimit(InstructionBlock block)
        {
            return last == block || block == loopEnd || block == loopStart;
        }

        internal List<InstructionBlock> FindAllPredececorsFrom(InstructionBlock block, InstructionBlock[] instructionBlock)
        {
            var predececors = new List<InstructionBlock>();
            var visited = new HashSet<InstructionBlock>();
            foreach (var current in instructionBlock)
            {
                FindAllPredececorsFrom(predececors, visited, block, current);
            }
            return predececors;
        }

        internal void FindAllPredececorsFrom(List<InstructionBlock> predececors, HashSet<InstructionBlock> visited, InstructionBlock block, InstructionBlock current)
        {
            if (visited.Contains(current))
            {
                return;
            }
            visited.Add(current);
            if (current.Successors.Contains(block))
            {
                predececors.Add(current);
            }
            foreach (var child in current.Successors)
            {
                if (child != block)
                {
                    FindAllPredececorsFrom(predececors, visited, block, child);
                }
            }
        }

        /// <summary>
        /// Find in <paramref name="instructionBlock"/> and its sucessors, firsts blocks that have no path to <paramref name="block"/>.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="instructionBlock"></param>
        /// <returns></returns>
        internal List<InstructionBlock> FindBlocksNotGoingTo(InstructionBlock block, InstructionBlock[] instructionBlock)
        {
            var nonlooping = new List<InstructionBlock>();
            var visited = new HashSet<InstructionBlock>();
            foreach (var current in instructionBlock)
            {
                FindBlocksNotGoingTo(nonlooping, visited, block, current);
            }
            return nonlooping;
        }

        private void FindBlocksNotGoingTo(List<InstructionBlock> nonlooping, HashSet<InstructionBlock> visited, InstructionBlock block, InstructionBlock current)
        {
            if (visited.Contains(current) || current == block || IsLimit(current))
            {
                return;
            }
            visited.Add(current);

            if (!DoesAllPathsGoToOrReturn(current, new [] { block }))
            {
                if (HasPathTo(current, block))
                {
                    foreach (var child in current.Successors)
                    {
                        FindBlocksNotGoingTo(nonlooping, visited, block, child);
                    }
                }
                else
                {
                    nonlooping.Add(current);
                }
            }
        }
    }
}
