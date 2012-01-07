using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NetWebScript.Debug.Server
{
    [Serializable]
    public sealed class JSStack
    {
        private readonly List<JSStackFrame> frames = new List<JSStackFrame>();

        internal JSStack(JSThread thread, JSModuleDebugPoint currentPoint, string stackXmlData)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(stackXmlData);
            foreach (XmlElement element in document.DocumentElement.SelectNodes("Frame"))
            {
                string name = element.GetAttribute("Name");
                string pointId = element.GetAttribute("Point");

                var metadata = thread.GetMethodById(name);
                JSModuleDebugPoint point = null;
                JSData locals = null;
                XmlElement p = (XmlElement)element.SelectSingleNode("P");
                if (p != null)
                {
                    point = currentPoint;
                    locals = new JSData(thread, metadata, p);
                }
                else
                {
                    point = thread.GetPointById(pointId);
                }
                frames.Add(new JSStackFrame(thread, point, name, metadata, locals));
            }
            if (frames.Count == 0)
            {
                frames.Add(new JSStackFrame(thread, currentPoint, "unknown()", null, null));
            }
            else
            {
                frames.Reverse();
            }
        }

        public IList<JSStackFrame> Frames
        {
            get { return frames; }
        }
    }
}
