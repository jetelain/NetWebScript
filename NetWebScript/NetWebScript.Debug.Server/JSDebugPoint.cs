using System;

namespace NetWebScript.Debug.Server
{
    /// <summary>
    /// Segment of a source file where debugger can stop
    /// </summary>
    [Serializable]
    public sealed class JSDebugPoint : IEquatable<JSDebugPoint>
    {
        private readonly string fileName;
        private readonly int startRow;
        private readonly int startCol;
        private readonly int endRow;
        private readonly int endCol;

        internal JSDebugPoint(string fileName, int startRow, int startCol, int endRow, int endCol)
        {
            this.fileName = fileName;
            this.startRow = startRow;
            this.startCol = startCol;
            this.endRow = endRow;
            this.endCol = endCol;
        }

        /// <summary>
        /// Start columun (start at 1)
        /// </summary>
        public int StartCol { get { return startCol; } }

        /// <summary>
        /// Start line (start at 1)
        /// </summary>
        public int StartRow { get { return startRow; } }

        /// <summary>
        /// End column (start at 1)
        /// </summary>
        public int EndCol { get { return endCol; } }

        /// <summary>
        /// End line (start at 1)
        /// </summary>
        public int EndRow { get { return endRow; } }

        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get { return fileName; } }

        public bool Equals(JSDebugPoint other)
        {
            if (other == null)
            {
                return false;
            }
            return StartCol == other.StartCol &&
                StartRow == other.StartRow && 
                EndCol == other.EndCol &&
                EndRow == other.EndRow && 
                FileName == other.FileName;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as JSDebugPoint);
        }

        public override int GetHashCode()
        {
            return StartCol.GetHashCode() ^ 
                StartRow.GetHashCode() ^ 
                EndCol.GetHashCode() ^ 
                EndRow.GetHashCode() ^ 
                FileName.GetHashCode();
        }
    }
}
