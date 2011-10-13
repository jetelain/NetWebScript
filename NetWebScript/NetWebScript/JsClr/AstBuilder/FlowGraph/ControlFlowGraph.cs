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
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using NetWebScript.JsClr.AstBuilder.Cil;

namespace NetWebScript.JsClr.AstBuilder.FlowGraph
{

	internal class ControlFlowGraph {

		private readonly MethodCil body;
        private readonly InstructionBlock[] blocks;
        private readonly List<ProtectedRegionData> exception_data;
        private readonly HashSet<int> exception_objects_offsets;

        public MethodCil MethodBody
        {
			get { return body; }
		}

		public InstructionBlock [] Blocks {
			get { return blocks; }
		}

        public InstructionBlock StartBlock
        {
            get { return blocks[0]; }
        }

        internal ControlFlowGraph(
            MethodCil body,
			InstructionBlock [] blocks,
			List<ProtectedRegionData> exception_data,
			HashSet<int> exception_objects_offsets)
		{
			this.body = body;
			this.blocks = blocks;
			this.exception_data = exception_data;
			this.exception_objects_offsets = exception_objects_offsets;
		}

		public List<ProtectedRegionData> ExceptionData
		{
            get { return exception_data; }
		}

		public bool HasExceptionObject (int offset)
		{
			if (exception_objects_offsets == null)
				return false;

			return exception_objects_offsets.Contains (offset);
		}

        public static ControlFlowGraph Create(MethodCil il)
		{
			var builder = new ControlFlowGraphBuilder(il);
			return builder.ToGraph ();
		}

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Method: {0}.{1}", body.Method.DeclaringType.Name, body.Method.Name);
            builder.AppendLine();
            foreach (ProtectedRegionData region in ExceptionData)
            {
                builder.AppendFormat("region {0}-{1}:", region.TryOffset, region.TryOffset + region.TryLength);
                builder.AppendLine();
                builder.AppendFormat("\ttry: block {0}", region.Try.Index + 1);
                builder.AppendLine();
                builder.Append("\tleave: ");
                foreach (InstructionBlock leave in region.TryEnd)
                {
                    builder.AppendFormat("block {0}, ", leave.Index + 1);
                }
                builder.AppendLine();

                foreach (CatchHandlerData @catch in region.Catches)
                {
                    builder.AppendFormat("\tcatch {0}", @catch.Block.Index + 1);
                    builder.AppendLine();
                }

                if ( region.Finally != null )
                {
                    builder.AppendFormat("\tfinally {0}", region.Finally.Index + 1);
                    builder.AppendLine();
                }
            }

			foreach (InstructionBlock block in blocks) {
                builder.AppendFormat("block {0}:", block.Index + 1);
                builder.AppendLine();
                builder.AppendLine("\tbody:");
				foreach (Instruction instruction in block) {
                    builder.Append("\t\t");
                    builder.Append(instruction.ToString());
                    builder.AppendLine();
				}
				InstructionBlock [] successors = block.Successors;
				if (successors.Length > 0) {
                    builder.Append("\tsuccessors: ");
					foreach (InstructionBlock successor in successors) {
                        builder.AppendFormat("block {0}, ", successor.Index + 1);
					}
                    builder.AppendLine();
				}
                builder.AppendFormat("\tstack: before={0} after={1}", block.StackBefore,block.StackAfter);
                builder.AppendLine();
			}
            return builder.ToString();
        }
	}
}
