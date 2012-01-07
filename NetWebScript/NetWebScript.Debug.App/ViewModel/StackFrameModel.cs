using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.App.ViewModel
{
    public class StackFrameModel
    {
        private readonly JSStackFrame frame;
        private readonly DataModel locals;

        public StackFrameModel(ThreadModel thread, JSStackFrame frame)
        {
            this.frame = frame;
            if (frame.Locals != null)
            {
                locals = new DataModel(thread, frame.Locals);
            }
            
        }

        public string DisplayName
        {
            get { return frame.DisplayName; }
        }

        public DataModel Locals
        {
            get { return locals; }
        }

        public JSDebugPoint Point
        {
            get { return frame.Point; }
        }

        public JSModuleDebugPoint ModulePoint
        {
            get { return frame.ModulePoint; }
        }
    }
}
