using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;
using NetWebScript.JsClr.AstBuilder.Flow;

namespace NetWebScript.Test.AstBuilder.FlowTree
{
    [TestClass]
    public class SimpleTree
    {

        [TestMethod]
        public void SimpleTree_If_Else()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            var label2 = tester.Il.DefineLabel();
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Br, label2);
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("C");
            tester.Il.MarkLabel(label2);
            tester.Il.EmitWriteLine("D");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            var ifBlock = result[0] as Condition;
            var endBlock = result[1] as SingleBlock;
            Assert.IsNotNull(ifBlock);
            Assert.IsNotNull(endBlock);
            Assert.AreEqual(1, ifBlock.Jump.Count);
            Assert.AreEqual(1, ifBlock.NoJump.Count);
            Assert.AreEqual(0, ifBlock.StackAfter);

            var jump = ifBlock.Jump[0] as SingleBlock;
            var nojump = ifBlock.NoJump[0] as SingleBlock;
            Assert.IsNotNull(jump);
            Assert.IsNotNull(nojump);
            
            Assert.AreEqual("A", ifBlock.Block.First.Operand);
            Assert.AreEqual("B", nojump.Block.First.Operand);
            Assert.AreEqual("C", jump.Block.First.Operand);
            Assert.AreEqual("D", endBlock.Block.First.Operand);

            Assert.AreEqual(@"If ( Block 1 )
Then {
	Block 3
}
Else {
	Block 2
}", ifBlock.ToString());
            Assert.AreEqual("Block 4", endBlock.ToString());
        
        }

        [TestMethod]
        public void SimpleTree_If_NoJump()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Br, label1);
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            var ifBlock = result[0] as Condition;
            var endBlock = result[1] as SingleBlock;
            Assert.IsNotNull(ifBlock);
            Assert.IsNotNull(endBlock);
            Assert.IsNull(ifBlock.Jump);
            Assert.AreEqual(1, ifBlock.NoJump.Count);
            Assert.AreEqual(0, ifBlock.StackAfter);

            var nojump = ifBlock.NoJump[0] as SingleBlock;
            Assert.IsNotNull(nojump);

            Assert.AreEqual("A", ifBlock.Block.First.Operand);
            Assert.AreEqual("B", nojump.Block.First.Operand);
            Assert.AreEqual("C", endBlock.Block.First.Operand);

            Assert.AreEqual(@"If ( Block 1 )
Else {
	Block 2
}", ifBlock.ToString());
            Assert.AreEqual("Block 3", endBlock.ToString());
        }

        [TestMethod]
        public void SimpleTree_If_Return()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ret);
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(1, result.Count);
            var ifBlock = result[0] as Condition;
            Assert.IsNotNull(ifBlock);
            Assert.AreEqual(1, ifBlock.Jump.Count);
            Assert.AreEqual(1, ifBlock.NoJump.Count);
            Assert.AreEqual(0, ifBlock.StackAfter);

            var jump = ifBlock.Jump[0] as SingleBlock;
            var nojump = ifBlock.NoJump[0] as SingleBlock;
            Assert.IsNotNull(jump);
            Assert.IsNotNull(nojump);

            Assert.AreEqual("A", ifBlock.Block.First.Operand);
            Assert.AreEqual("B", nojump.Block.First.Operand);
            Assert.AreEqual("C", jump.Block.First.Operand);

            Assert.AreEqual(@"If ( Block 1 )
Then {
	Block 3
}
Else {
	Block 2
}", ifBlock.ToString());

        }

        [TestMethod]
        public void SimpleTree_If_Jump()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            var label2 = tester.Il.DefineLabel();
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label2);
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ret);
            tester.Il.MarkLabel(label2);
            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Br, label1);
            
            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            var ifBlock = result[0] as Condition;
            var endBlock = result[1] as SingleBlock;
            Assert.IsNotNull(ifBlock);
            Assert.IsNotNull(endBlock);
            Assert.IsNull(ifBlock.NoJump);
            Assert.AreEqual(1, ifBlock.Jump.Count);
            Assert.AreEqual(0, ifBlock.StackAfter);

            var jump = ifBlock.Jump[0] as SingleBlock;
            Assert.IsNotNull(jump);

            Assert.AreEqual("A", ifBlock.Block.First.Operand);
            Assert.AreEqual("C", jump.Block.First.Operand);
            Assert.AreEqual("B", endBlock.Block.First.Operand);

            Assert.AreEqual(@"If ( Block 1 )
Then {
	Block 3
}", ifBlock.ToString());
            Assert.AreEqual("Block 2", endBlock.ToString());
        }


        [TestMethod]
        public void SimpleTree_LoopPreTested_NoJump()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            var label2 = tester.Il.DefineLabel();
            tester.Il.MarkLabel(label2);
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Br, label2);
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            var loop = result[0] as PreLoop;
            var endBlock = result[1] as SingleBlock;
            Assert.IsNotNull(loop);
            Assert.IsNotNull(endBlock);
            Assert.AreEqual(1, loop.Body.Count);
            Assert.AreEqual(LoopBody.NoJump, loop.Jump);

            var body = loop.Body[0] as SingleBlock;
            Assert.IsNotNull(body);

            Assert.AreEqual("A", loop.Block.First.Operand);
            Assert.AreEqual("B", body.Block.First.Operand);
            Assert.AreEqual("C", endBlock.Block.First.Operand);

            Assert.AreEqual(@"While NoJump ( Block 1 )
{
	Block 2
}", loop.ToString());
            Assert.AreEqual("Block 3", endBlock.ToString());
        }

        [TestMethod]
        public void SimpleTree_LoopPreTested_Jump()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            var label2 = tester.Il.DefineLabel();
            tester.Il.MarkLabel(label2);
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ret);
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Br, label2);
            
            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            var loop = result[0] as PreLoop;
            var endBlock = result[1] as SingleBlock;
            Assert.IsNotNull(loop);
            Assert.IsNotNull(endBlock);
            Assert.AreEqual(1, loop.Body.Count);
            Assert.AreEqual(LoopBody.Jump, loop.Jump);

            var body = loop.Body[0] as SingleBlock;
            Assert.IsNotNull(body);

            Assert.AreEqual("A", loop.Block.First.Operand);
            Assert.AreEqual("C", body.Block.First.Operand);
            Assert.AreEqual("B", endBlock.Block.First.Operand);

            Assert.AreEqual(@"While Jump ( Block 1 )
{
	Block 3
}", loop.ToString());
            Assert.AreEqual("Block 2", endBlock.ToString());
        }


        [TestMethod]
        public void SimpleTree_LoopPostTested_Jump()
        {
            var tester = new FTTester();
            var label2 = tester.Il.DefineLabel();
            tester.Il.MarkLabel(label2);
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, label2);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            var loop = result[0] as PostLoop;
            var endBlock = result[1] as SingleBlock;
            Assert.IsNotNull(loop);
            Assert.IsNotNull(endBlock);
            Assert.AreEqual(1, loop.Body.Count);
            Assert.AreEqual(LoopBody.Jump, loop.Jump);

            var body = loop.Body[0] as SingleBlock;
            Assert.IsNotNull(body);

            Assert.AreEqual("A", body.Block.First.Operand);
            Assert.AreEqual("B", endBlock.Block.First.Operand);

            Assert.AreEqual(@"Do
{
	Block 1
} While Jump", loop.ToString());
            Assert.AreEqual("Block 2", endBlock.ToString());
        }



        [TestMethod]
        public void SimpleTree_LoopPostTested_NoJump()
        {
            var tester = new FTTester();
            var label1 = tester.Il.DefineLabel();
            var label2 = tester.Il.DefineLabel();
            var label3 = tester.Il.DefineLabel();

            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Br, label2);
            
            tester.Il.MarkLabel(label1);
            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ldc_I4_1);
            tester.Il.Emit(OpCodes.Brtrue, label3);
            
            tester.Il.MarkLabel(label2);
            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Br, label1);

            tester.Il.MarkLabel(label3);
            tester.Il.EmitWriteLine("D");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(3, result.Count);

            var start = result[0] as SingleBlock;
            var loop = result[1] as PostLoop;
            var end = result[2] as SingleBlock;

            Assert.IsNotNull(start);
            Assert.IsNotNull(loop);
            Assert.IsNotNull(end);

            Assert.AreEqual(2, loop.Body.Count);
            Assert.AreEqual(LoopBody.NoJump, loop.Jump);

            var body1 = loop.Body[0] as SingleBlock;
            var body2 = loop.Body[1] as SingleBlock;

            Assert.AreEqual("A", start.Block.First.Operand);
            Assert.AreEqual("B", body2.Block.First.Operand);
            Assert.AreEqual("C", body1.Block.First.Operand);
            Assert.AreEqual("D", end.Block.First.Operand);

            Assert.AreEqual("Block 1", start.ToString());
            Assert.AreEqual(@"Do
{
	Block 3
	Block 2
} While NoJump", loop.ToString());
            Assert.AreEqual("Block 4", end.ToString());
        }
    }
}
