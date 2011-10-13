using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection.Emit;

namespace NetWebScript.Test.AstBuilder.FlowTree
{
    [TestClass]
    public class ConditionTest
    {
        [TestMethod]
        public void Condition_If_Nested_Duplicate()
        {
            var tester = new FTTester();

            var labelC = tester.Il.DefineLabel();
            var labelD = tester.Il.DefineLabel();
            var labelE = tester.Il.DefineLabel();
            var labelF = tester.Il.DefineLabel();

            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, labelE);

            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, labelF);

            tester.Il.EmitWriteLine("C");
            
            tester.Il.MarkLabel(labelD);
            tester.Il.EmitWriteLine("D");
            tester.Il.Emit(OpCodes.Ret);

            tester.Il.MarkLabel(labelE);
            tester.Il.EmitWriteLine("E");

            tester.Il.MarkLabel(labelF);
            tester.Il.EmitWriteLine("F");
            tester.Il.Emit(OpCodes.Br, labelD);

            var result = tester.GetSequence();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(@"If ( Block 1 )
Then {
	Block 5
	Block 6
}
Else {
	If ( Block 2 )
	Then {
		Block 6
	}
	Else {
		Block 3
	}
}", result[0].ToString());
            Assert.AreEqual(@"Block 4", result[1].ToString());
        }

        [TestMethod]
        public void Condition_If_Nested_Duplicate_CommonEnd()
        {
            var tester = new FTTester();

            var labelC = tester.Il.DefineLabel();
            var labelD = tester.Il.DefineLabel();
            var labelE = tester.Il.DefineLabel();
            
            tester.Il.EmitWriteLine("A");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, labelD);

            tester.Il.EmitWriteLine("B");
            tester.Il.Emit(OpCodes.Ldc_I4_0);
            tester.Il.Emit(OpCodes.Brtrue, labelE);

            tester.Il.EmitWriteLine("C");
            tester.Il.Emit(OpCodes.Ret);

            tester.Il.MarkLabel(labelD);
            tester.Il.EmitWriteLine("D");

            tester.Il.MarkLabel(labelE);
            tester.Il.EmitWriteLine("E");
            tester.Il.Emit(OpCodes.Ret);

            var result = tester.GetSequence();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(@"If ( Block 1 )
Then {
	Block 4
	Block 5
}
Else {
	If ( Block 2 )
	Then {
		Block 5
	}
	Else {
		Block 3
	}
}", result[0].ToString());
        }
    }
}
