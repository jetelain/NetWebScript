using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public sealed class JSDebugPoint
    {
        private readonly int moduleId;
        private readonly string id;
        private readonly string fileName;
        private readonly int startRow;
        private readonly int startCol;
        private readonly int endRow;
        private readonly int endCol;

        internal JSDebugPoint(JSModule jSModule, string id, string fileName, int startRow, int startCol, int endRow, int endCol)
        {
            this.moduleId = jSModule.Id;
            this.id = id;
            this.fileName = fileName;
            this.startRow = startRow;
            this.startCol = startCol;
            this.endRow = endRow;
            this.endCol = endCol;
        }

        public int ModuleId { get { return moduleId; } }

        public int StartCol { get { return startCol; } }

        public int StartRow { get { return startRow; } }

        public int EndCol { get { return endCol; } }

        public int EndRow { get { return endRow; } }

        public string UId { get { return id; } }

        public string FileName { get { return fileName; } }
    }
}
