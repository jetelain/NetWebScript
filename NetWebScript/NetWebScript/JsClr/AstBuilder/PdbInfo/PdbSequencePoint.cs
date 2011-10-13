using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NetWebScript.JsClr.AstBuilder.PdbInfo
{
    class PdbSequencePoint
    {
        public int Offset { get; set; }
        public int StartCol { get; set; }
        public int StartRow { get; set; }
        public int EndCol { get; set; }
        public int EndRow { get; set; }
        public String Filename { get; set; }

        public override string ToString()
        {
            if (StartRow == EndRow)
            {
                return String.Format("{0:0000} {1}@{2},{3}-{4}", Offset, Path.GetFileName(Filename), StartRow, StartCol, EndCol);
            }
            return String.Format("{0:0000} {1}@{2},{3}-{4},{5}",Offset,Path.GetFileName(Filename),StartRow,StartCol,EndRow,EndCol);
        }
    }
}
