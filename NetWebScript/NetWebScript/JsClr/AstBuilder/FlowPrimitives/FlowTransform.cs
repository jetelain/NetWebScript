using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;
using NetWebScript.JsClr.AstBuilder.FlowGraph;

namespace NetWebScript.JsClr.AstBuilder.Flow
{
    /// <summary>
    /// Generate a sequence of control flow primitives (if, while, etc.) from a control flow graph.
    /// </summary>
    internal class FlowTransform
    {
        private readonly ControlFlowSubGraph graph;
        private readonly InstructionBlock loopStart;
        private readonly InstructionBlock loopEnd;
        
        public FlowTransform(ControlFlowGraph graph)
        {
            this.graph = new ControlFlowSubGraph(graph);
        }

        private FlowTransform(ControlFlowSubGraph graph, InstructionBlock loopStart, InstructionBlock loopEnd)
        {
            this.graph = graph;
            this.loopEnd = loopEnd;
            this.loopStart = loopStart;
        }

        public static List<Sequence> Transform(ControlFlowGraph graph)
        {
            return new FlowTransform(graph).Transform();
        }

        public List<Sequence> Transform()
        {
            List<Sequence> sequences = new List<Sequence>();
            ProcessNext(sequences, graph.First);
            return sequences;
        }

        private List<Sequence> PreLoopTransform(InstructionBlock start, InstructionBlock body, InstructionBlock end)
        {
            if (body == loopStart)
            {
                return new List<Sequence> { new Continue() };
            }
            if (body == loopEnd)
            {
                return new List<Sequence> { new Break() };
            }
            List<Sequence> result = new FlowTransform(graph.SubGraph(body, start), start, end).Transform();
            if (result.Count > 0 && result[result.Count - 1] is Continue)
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;
        }

        private List<Sequence> PostLoopTransform(InstructionBlock block, InstructionBlock condition, InstructionBlock end)
        {
            //if (block == loopStart)
            //{
            //    return new List<Sequence> { new Continue() };
            //}
            //if (block == loopEnd)
            //{
            //    return new List<Sequence> { new Break() };
            //}
            var result = new FlowTransform(graph.SubGraph(block, condition), condition, end).Transform();
            if (result.Count > 0 && result[result.Count - 1] is Continue)
            {
                result.RemoveAt(result.Count - 1);
            }
            return result;
        }
        

        private List<Sequence> CondTransform(InstructionBlock body, InstructionBlock end)
        {
            if (body == loopStart)
            {
                return new List<Sequence> { new Continue() };
            }
            if (body == loopEnd)
            {
                return new List<Sequence> { new Break() };
            }
            return new FlowTransform(graph.SubGraph(body, end), loopStart, loopEnd).Transform();
        }

        private List<Sequence> TryTransform(InstructionBlock body, InstructionBlock end, ProtectedRegionData newtry)
        {
            return new FlowTransform(graph.SubGraph(body, end, newtry), loopStart, loopEnd).Transform();
        }

        private List<Sequence> CatchTransform(InstructionBlock body, InstructionBlock end)
        {
            return new FlowTransform(graph.SubGraph(body, end), loopStart, loopEnd).Transform();
        }

        private List<Sequence> FinallyTransform(InstructionBlock body)
        {
            return new FlowTransform(graph.SubGraph(body, null), loopStart, loopEnd).Transform();
        }

        private void ProcessNext(List<Sequence> sequences, InstructionBlock block)
        {
            if (!graph.IsEnd(block))
            {
                if (block == loopStart)
                {
                    sequences.Add(new Continue());
                }
                else if (block == loopEnd)
                {
                    sequences.Add(new Break());
                }
                else
                {
                    Process(sequences, block);
                }
            }
        }

        private void Process(List<Sequence> sequences, InstructionBlock block)
        {
            int count = block.Successors.Length;

            ProtectedRegionData region = graph.ExceptionData.FirstOrDefault(r => r.Try == block);
            if (region != null)
            {
                if (count > 1)
                {
                    throw new NotImplementedException();
                }
                if (region.TryEnd.Count > 1)
                {
                    throw new NotImplementedException();
                }
                // TODO: vérifier que le try est bien contenu dans le sous-graph
                var end = region.TryEnd.Count == 0 ? null : region.TryEnd.First();

                TryCatch trycatch = new TryCatch();
                trycatch.Body = TryTransform(region.Try, end, region);
                if (region.Catches.Count > 0)
                {
                    trycatch.CatchList = new List<CatchFlow>();
                    foreach (CatchHandlerData @catch in region.Catches)
                    {
                        CatchFlow catchflow = new CatchFlow();
                        catchflow.Type = @catch.Type;
                        catchflow.Body = CatchTransform(@catch.Block, end);
                        trycatch.CatchList.Add(catchflow);
                    }
                }
                if (region.Finally != null)
                {
                    trycatch.Finally = FinallyTransform(region.Finally);
                }
                sequences.Add(trycatch);
                if (end != null)
                {
                    ProcessNext(sequences, end);
                }
                return;
            }

            if (graph.HasPathTo(block, block))
            {
                // There is a path from block back to itself, so we are in a loop
                // First, look for post-tested loop

                // Look for all predececors of block which can be reached from block
                var predececors = graph.FindAllPredececorsFrom(block, block.Successors);

                // In predececors, ensures that only one block have two sucessors (so one including block), all others
                // must have at most one sucessor (that is by definition block)
                var conditionCandidates = predececors.Where(p => p.Successors.Length == 2).ToList();
                if (conditionCandidates.Count == 1 && predececors.Where(p => p.Successors.Length > 1).Count() == 1)
                {
                    var condition = conditionCandidates[0];
                    // Find the block after condition that is not loop
                    var end = condition.Successors.First(b => b != block);

                    // continue goes to condition block
                    // break goes to end block

                    // Ensures that all paths from block goes to condition (continue), end (break), or returns (return).
                    if (graph.DoesAllPathsGoToOrReturn(block, new[] { condition, end }))
                    {
                        var doWhile = new DoWhile();
                        doWhile.Body = PostLoopTransform(block, condition, end);
                        doWhile.Body.Add(new SingleBlock(condition));
                        doWhile.Jump = condition.Successors[0] == end ? LoopBody.NoJump : LoopBody.Jump;
                        sequences.Add(doWhile);
                        ProcessNext(sequences, end);
                        return;
                    }
                }
                // Then look for pre-tested loop
                // Works only if block has two sucessor
                if (count == 2)
                {
                    var sucA = block.Successors[0]; // Jump sucessor
                    var sucB = block.Successors[1]; // NoJump sucessor

                    var hasPathFromA = graph.HasPathTo(sucA, block);
                    var hasPathFromB = graph.HasPathTo(sucB, block);

                    if (hasPathFromA && !hasPathFromB)
                    {
                        sequences.Add(new Loop(block) { Body = PreLoopTransform(block, sucA, sucB), Jump = LoopBody.Jump });
                        ProcessNext(sequences, sucB);
                        return;
                    }
                    if (!hasPathFromA && hasPathFromB)
                    {
                        sequences.Add(new Loop(block) { Body = PreLoopTransform(block, sucB, sucA), Jump = LoopBody.NoJump });
                        ProcessNext(sequences, sucA);
                        return;
                    }
                }
                // Loop cannot be transformed to either a pre-tested or a post-tested one
                throw new AstBuilderException(block.First.Offset, string.Format("Unsupported loop detected from block {0}", block.Index + 1));
            }

            if (count == 0)
            {
                sequences.Add(new SingleBlock(block));
            }
            else if (count == 1)
            {
                sequences.Add(new SingleBlock(block));
                ProcessNext(sequences, block.Successors[0]);
            }
            else if (count == 2)
            {
                InstructionBlock sucA = block.Successors[0]; // Jump sucessor
                InstructionBlock sucB = block.Successors[1]; // NoJump sucessor

                InstructionBlock common;
                if (!graph.HasCommonSucessor(sucA, sucB, out common))
                {
                    throw new AstBuilderException(block.First.Offset, string.Format("Unsupported condition detected from block {0}", block.Index + 1));
                }
                // C'est une simple condition
                if (common == sucA)
                {
                    sequences.Add(new Condition(block) { NoJump = CondTransform(sucB, common), StackAfter = common.StackBefore });
                    ProcessNext(sequences, common);
                }
                else if (common == sucB)
                {
                    sequences.Add(new Condition(block) { Jump = CondTransform(sucA, common), StackAfter = common.StackBefore });
                    ProcessNext(sequences, common);
                }
                else if (common != null)
                {
                    sequences.Add(new Condition(block) { NoJump = CondTransform(sucB, common), Jump = CondTransform(sucA, common), StackAfter = common.StackBefore });
                    ProcessNext(sequences, common);
                }
                else
                {
                    sequences.Add(new Condition(block) { NoJump = CondTransform(sucB, graph.End), Jump = CondTransform(sucA, graph.End), StackAfter = (graph.End == null ? 0 : graph.End.StackBefore) });
                }
                
            }
            else
            {
                InstructionBlock common;
                if (!graph.HasCommonSucessor(block.Successors, out common))
                {
                    throw new AstBuilderException(block.First.Offset, string.Format("Unsupported switch detected from block {0}", block.Index + 1));
                }
                Switch sw = new Switch(block);
                sequences.Add(sw);
                int index = 0;
                foreach (InstructionBlock succ in block.Successors)
                {
                    if (succ != common)
                    {
                        if (index == block.Last.OperandOffsetArray.Length)
                        {
                            sw.Cases.Add(-1, CondTransform(succ, common ?? graph.End));
                        }
                        else
                        {
                            sw.Cases.Add(index, CondTransform(succ, common ?? graph.End));
                        }
                    }
                    index++;
                }
                if (common != null)
                {
                    ProcessNext(sequences, common);
                }
            }
        }
    }
}
