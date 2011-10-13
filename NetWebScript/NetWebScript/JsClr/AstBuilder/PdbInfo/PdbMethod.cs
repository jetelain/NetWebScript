using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.SymbolStore;
using Microsoft.Samples.Debugging.CorSymbolStore;
using System.Diagnostics;

namespace NetWebScript.JsClr.AstBuilder.PdbInfo
{
    class PdbMethod
    {
        private List<PdbLocalVariable> variables = new List<PdbLocalVariable>();
        private List<PdbSequencePoint> sequences = new List<PdbSequencePoint>();

        internal PdbMethod(ISymbolMethod methodSymbol)
        {
            LoadSequencePoints(methodSymbol);
            LoadLocals(methodSymbol);
        }

        private void LoadLocals(ISymbolMethod method)
        {
            LoadLocals((ISymbolScope2)method.RootScope);
        }

        private void LoadLocals(ISymbolScope2 scope)
        {
            foreach (ISymbolVariable l in scope.GetLocals())
            {
                Debug.Assert(l.AddressKind == SymAddressKind.ILOffset);
                variables.Add(new PdbLocalVariable() { Name = l.Name, Index = l.AddressField1 });
            }
            foreach (ISymbolScope childScope in scope.GetChildren())
            {
                LoadLocals((ISymbolScope2)childScope);
            }
        }

        private void LoadSequencePoints(ISymbolMethod method)
        {
            int count = method.SequencePointCount;
            int[] offsets = new int[count];
            ISymbolDocument[] docs = new ISymbolDocument[count];
            int[] startColumn = new int[count];
            int[] endColumn = new int[count];
            int[] startRow = new int[count];
            int[] endRow = new int[count];
            method.GetSequencePoints(offsets, docs, startRow, startColumn, endRow, endColumn);
            for (int i = 0; i < count; i++)
            {
                if (startRow[i] != 0xfeefee)
                {
                    sequences.Add(new PdbSequencePoint() { Offset = offsets[i], StartRow = startRow[i], StartCol = startColumn[i], EndRow = endRow[i], EndCol = endColumn[i], Filename = docs[i].URL });
                }
            }
        }

        public PdbLocalVariable GetVariable(int index)
        {
            return variables.FirstOrDefault(v => v.Index == index);

        }

        public List<PdbSequencePoint> SequencePointList
        {
            get { return sequences; }
        }

    }
}
