using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Debug.Server;

namespace NetWebScript.Debug.App.ViewModel
{
    public class StackModel
    {
        private readonly List<StackFrameModel> frames = new List<StackFrameModel>();

        public StackModel(ThreadModel thread, JSStack stack)
        {
            frames.AddRange(stack.Frames.Select(f => new StackFrameModel(thread, f)));
        }

        public List<StackFrameModel> Frames
        {
            get { return frames; }
        }
    }
}
