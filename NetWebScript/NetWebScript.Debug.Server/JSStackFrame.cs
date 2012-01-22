using System;
using System.Linq;
using NetWebScript.Metadata;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public sealed class JSStackFrame
    {
        private readonly JSModuleDebugPoint point;
        private readonly string name;
        private readonly string displayName;
        private readonly JSData locals;
        private readonly JSData instance;
        private readonly JSData statics;

        internal JSStackFrame(JSThread thread, JSModuleDebugPoint point, string name, MethodBaseMetadata metadata, JSData locals)
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
            if (locals != null && locals.Children != null)
            {
                instance = locals.Children.FirstOrDefault(p => p.IsSpecial && p.DisplayName == JSData.SpecialNameInstance);
                statics = locals.Children.FirstOrDefault(p => p.IsSpecial && p.DisplayName == JSData.SpecialNameStaticMembers);
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

        public JSData Instance
        {
            get { return instance; }
        }

        public JSData Static
        {
            get { return statics; }
        }

        public JSDebugPoint Point
        {
            get { return point.Point; }
        }

        public JSModuleDebugPoint ModulePoint
        {
            get { return point; }
        }
    }
}
