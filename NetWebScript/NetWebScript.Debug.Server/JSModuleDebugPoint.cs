using System;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public sealed class JSModuleDebugPoint
    {
        private readonly int moduleId;
        private readonly string id;
        private readonly JSDebugPoint point;

        internal JSModuleDebugPoint(JSModule jSModule, string id, string fileName, int startRow, int startCol, int endRow, int endCol)
        {
            this.moduleId = jSModule.Id;
            this.id = id;
            this.point = new JSDebugPoint(fileName, startRow, startCol, endRow, endCol);
        }

        public int ModuleId { get { return moduleId; } }

        public JSDebugPoint Point { get { return point; } }

        public string UId { get { return id; } }
    }
}
