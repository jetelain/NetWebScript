using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public sealed class JSStackFrame
    {
        private readonly JSDebugPoint point;
        private readonly string name;
        private readonly string displayName;
        private readonly JSData locals;

        internal JSStackFrame(JSThread thread, JSDebugPoint point, string name, MethodBaseMetadata metadata, JSData locals)
        {
            this.point = point;
            this.name = name;
            this.locals = locals;
            if (metadata == null)
            {
                displayName = name;
            }
            else
            {
                displayName = metadata.CRef;
            }
        }

        public string DisplayName
        {
            get { return displayName; }
        }

        public JSData Locals
        {
            get { return locals; }
        }

        public JSDebugPoint Point
        {
            get { return point; }
        }
    }
}
