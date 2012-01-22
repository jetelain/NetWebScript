using System;
using System.Diagnostics;
using NetWebScript.Runtime;
using NetWebScript.Script;
using NetWebScript.Script.HTML;
using NetWebScript.Script.Xml;

namespace NetWebScript.Diagnostics
{
    [ScriptAvailable]
	public static class Debugger
	{
		private static string serverUrl = "http://localhost:9090/";
		private static string threadId = null;
		private static Script.JSObject breakPoints = new Script.JSObject();
		private static Script.JSArray<StackFrame> callStack = new Script.JSArray<StackFrame>();
		private static int stepDeep = 0;
        private static int timer;
        private static bool detached = false;
        private static bool active;

		[DebuggerHidden]
		private static void Break(string cmd, string id)
		{
			var c = new Channel("wait");
			c.SendPost(cmd, id, DumpStack());
			while (true)
			{
				var msg = c.Get();
				if (msg == null)
				{
					Detach();
					return;
				}
				if (msg.cmd == "continue")
				{
					stepDeep = 0;
					break;
				}
				else if (msg.cmd == "step")
				{
					var mode = msg.data;
					if (mode == "up")
					{
						stepDeep = callStack.Length - 1;
					}
					else if (mode == "dw")
					{
						stepDeep = callStack.Length + 1;
					}
					else
					{
						stepDeep = callStack.Length;
					}
					break;
				}
				else if (msg.cmd == "detach")
				{
					Detach();
					return;
				}
				else
				{
					Process(c, msg.cmd, msg.data);

				}
			}
			c.ModeNoWait();
			c.Send("continueACK", null);
			ProcessAll(c);
		}

		/// <summary>
		/// Starts connection with debug server
		/// </summary>
		[DebuggerHidden]
		public static void Start()
		{
            active = true;
			//FIXME: $.support.cors = true;
            JQuery.Support["cors"] = true;
			var success = true;
			var xhr = JQuery.Ajax(new JQueryAjaxSettings()
			{
				Type = "POST",
				Url = serverUrl + "?cmd=start",
				Data = Identity(),
				DataType = "text",
				Timeout = 500,
				Async = false,
				Cache = false,
                Error = (a, b, c) => { success = false;}
			});
			if (success)
			{
				var result = xhr.ResponseText;
				var i = result.IndexOf(':');
				threadId = result.Substring(0, i);

				var c = new Channel(null);
				Process(c, "listbp", result.Substring(i + 1));
				ProcessAll(c);

				timer = Window.SetInterval(new Action(QueryStatus), 5000);

				JQuery.Query(Window.Instance).Unload(delegate (JQueryEvent e) {
                    NotifyStop();
                    return true;
				});
			}
			else
			{
                Window.Alert("Unable to attach to debugger. Ensures that debugger is started.\nUrl='" + serverUrl + "'\nStatus='" + xhr.Status + "'\nResponseText='" + xhr.ResponseText + "'");
			}
		}


        [DebuggerHidden]
        private static void NotifyStop()
        {
            if (detached)
            {
                return;
            }
            Window.ClearInterval(timer);
            var c = new Debugger.Channel("nowait");
            c.Send("stop", "");
            detached = true;
        }

		[DebuggerHidden]
		private static void Detach()
		{
            if (!detached)
            {
                Window.ClearInterval(timer);
                stepDeep = 0;
                breakPoints = new Script.JSObject();
                detached = true;
            }
		}

		[DebuggerHidden]
		private static void QueryStatus()
		{
			JQuery.Ajax(new JQueryAjaxSettings()
			{
				Url = serverUrl,
				Data = new ToServerMessage { t = threadId, cmd = "status" },
				DataType = "text",
				Async = true,
				Cache = false,
				Success = new Action<string>(Status)
			});
		}

		[DebuggerHidden]
		private static void Status(string hasp)
		{
			if (hasp == "true" && !detached)
			{
				var c = new Channel(null);
				c.Send("ping", null);
				ProcessAll(c);
			}
		}


		[DebuggerHidden]
		private static void ProcessAll(Channel c)
		{
			Message msg = null;
			while ((msg = c.Get()) != null)
			{
				Process(c, msg.cmd, msg.data);
			}
		}

		[DebuggerHidden]
		private static void Process(Channel c, string cmd, string data)
		{
			if (cmd == "addbp")
			{
				breakPoints[data] = "full";
				c.Send("bpACK", data);
			}
			else if (cmd == "rmbp")
			{
				breakPoints.Delete(data);
				c.Send("rmbpACK", data);
			}
			else if (cmd == "listbp")
			{
				var ids = ((JSString)data).Split(",");
				for (var i = 0; i < ids.Length; ++i)
				{
					breakPoints[ids[i]] = "full";
				}
				c.Send("bpACK", data);
			}
			else if (cmd == "detach")
			{
				Detach();
			}
			else if (cmd == "retreive")
			{
				object result;
				try
				{
                    result = new JSFunction("$s", data).Call(null, callStack);
				}
				catch (Exception e)
				{
					result = e.Message;
				}
				c.SendPost("result", data, DumpResult(result));
			}
		}

        [ScriptAvailable]
        private class Channel
		{
			private Script.JSArray<Message> q;
			private string mode;

			[DebuggerHidden]
			public Channel(string m)
			{
				this.q = new Script.JSArray<Message>();
				this.mode = m ?? "nowait";
			}

			[DebuggerHidden]
			public void ModeNoWait()
			{
				mode = "nowait";
			}

			[DebuggerHidden]
			public bool Send(string cmd, string data)
			{
				bool success = true;
				var xhr = JQuery.Ajax(new JQueryAjaxSettings()
				{
					Url = serverUrl,
					Data = new ToServerMessage() { t = threadId, cmd = cmd, data = data ?? "", mode = mode },
					DataType = "text",
					Timeout = 2500,
					Async = false,
					Cache = false,
					Error = (a, b, c) => { success = false; }
				});
				if (success) Push(xhr.ResponseText);
				return success;
			}
			[DebuggerHidden]
			public bool SendPost(string cmd, string data, string postData)
			{
				bool success = true;
				var xhr = JQuery.Ajax(new JQueryAjaxSettings()
				{
					Type = "POST",
					Url = serverUrl + "?" + JQuery.Param(new ToServerMessage() { t = threadId, cmd = cmd, data = data ?? "", mode = mode }),
					Data = postData,
					DataType = "text",
					Timeout = 2500,
					Async = false,
					Cache = false,
					Error = (a, b, c) => { success = false; }
				});
				if (success) Push(xhr.ResponseText);
				return success;
			}
			[DebuggerHidden]
			public void Push(string result)
			{
				if (result != null && result != "nop" && result != "wait")
				{
					var i = result.IndexOf(':');
					if (i == -1)
					{
						q.Push(new Message() { cmd = result, data = null });
					}
					else
					{
						q.Push(new Message() { cmd = result.Substring(0, i), data = result.Substring(i + 1) });
					}
				}
			}
			[DebuggerHidden]
			public Message Get()
			{
				if (q.Length > 0)
				{
					return q.Shift();
				}
				if (mode == "wait")
				{
					while (q.Length == 0 && Send("ping", null)) ;
					if (q.Length > 0)
					{
						return q.Shift();
					}
				}
				return null;
			}
		}

		[AnonymousObject]
		private class Message
		{
			public string cmd;
			public string data;
		}

		[AnonymousObject]
		private class ToServerMessage
		{
			public string cmd;
			public string data;
			public string t;
			public string mode;
		}

		[AnonymousObject]
		private class StackFrame
		{
			public int Id;
			public string Name;
			public object Context;
			public string Point;
		}



		/// <summary>
		/// Called when entering a method
		/// </summary>
		/// <param name="name">Method identifier</param>
		/// <param name="context">Method local variables (including $this)</param>
		[DebuggerHidden]
		public static void E(string name, object context)
		{
            if (active)
            {
                callStack.Push(new StackFrame() { Id = callStack.Length, Name = name, Context = context });
            }
		}

		/// <summary>
		/// Called when leaving a method
		/// </summary>
		/// <param name="v">Value to return as-is</param>
		/// <returns>Value of <paramref name="v"/></returns>
		[DebuggerHidden]
        public static void L()
		{
            if (active)
            {
                callStack.Pop();
            }
		}

		[DebuggerHidden]
		private static string DumpStack()
		{
			var doc = XmlToolkit.CreateDocument("StackFrame");
			for (var i = 0; i < callStack.Length; ++i)
			{
				var frame = callStack[i];
				var element = doc.CreateElement("Frame");
				element.SetAttribute("Id", Unsafe.NumberToString(frame.Id));
				element.SetAttribute("Name", frame.Name);
				element.SetAttribute("Point", frame.Point);
				if (i == callStack.Length - 1)
				{
                    element.AppendChild(DumpAny(frame.Context, doc, -1)); // 3 levels dump
				}
				doc.DocumentElement.AppendChild(element);
			}
			return XmlToolkit.ToXml(doc);
		}

		[DebuggerHidden]
		private static string DumpResult(object obj)
		{
			var doc = XmlToolkit.CreateDocument("Dump");
			doc.DocumentElement.AppendChild(DumpAny(obj, doc, 0)); // 2 levels dump
			return XmlToolkit.ToXml(doc);
		}

		[DebuggerHidden]
		private static string ToStringSafe(object obj)
		{
			try
			{
				return obj.ToString();
			}
			catch
			{
				return "(exception catched)";
			}
		}

        [DebuggerHidden]
        private static void DumpObject(IXmlElement node, object obj, IXmlDocument doc, int depth)
        {
            var typename = Unsafe.GetScriptTypeName(obj);
            if (typename == "function")
            {
                var type = JSObject.Get(obj, "$n");
                if (type != JSObject.Undefined)
                {
                    typename = ToStringSafe(type);
                }
            }
            node.SetAttribute("Type", typename);

            if (depth == 2)
            {
                node.SetAttribute("Retreive", "true");
                return;
            }
            
            foreach (var key in Unsafe.GetAll(obj))
            {
                var value = JSObject.Get(obj, key);

                if ( JQuery.IsFunction(value) && JSObject.Get(value,"$n") == JSObject.Undefined)
                {
                    continue;
                }

                if (key == "$this")
                {
                    IXmlElement dump = DumpAny(value, doc, 0);
                    dump.SetAttribute("Name", key);
                    node.AppendChild(dump);
                }
                else
                {
                    IXmlElement vnode = DumpAny(value, doc, depth + 1);
                    vnode.SetAttribute("Name", key);
                    node.AppendChild(vnode);
                }
            }
        }

        [DebuggerHidden]
        private static void DumpArray(IXmlElement node, object obj, IXmlDocument doc, int depth)
        {
            node.SetAttribute("Type", "array");

            if (depth == 2)
            {
                node.SetAttribute("Retreive", "true");
                return;
            }

            JSArray<object> array = (JSArray<object>)obj;

            if (array.Length > 50)
            {
                node.SetAttribute("Partial", "50");
            }

            var vnode = doc.CreateElement("P");
            vnode.SetAttribute("Value", ToStringSafe(array.Length));
            vnode.SetAttribute("Type", "number");
            vnode.SetAttribute("Name", "Length");
            node.AppendChild(vnode);

            int index = 0;
            while (index < 50 && index < array.Length)
            {
                vnode = DumpAny(array[index], doc, depth + 1);
                vnode.SetAttribute("Name", "[" + index + "]");
                node.AppendChild(vnode);
                index++;
            }


        }

		[DebuggerHidden]
		private static IXmlElement DumpAny(object obj, IXmlDocument doc, int depth)
		{
            if (obj == null || obj == JSObject.Undefined)
            {
                var xnode = doc.CreateElement("P");
                xnode.SetAttribute("Value", obj == null ? "(null)" : "(undefined)");
                xnode.SetAttribute("Type", "unknown");
                return xnode;
            }

			var node = doc.CreateElement("P");
			node.SetAttribute("Value", ToStringSafe(obj));
            try
            {
                if (JQuery.IsArray(obj))
                {
                    DumpArray(node, obj, doc, depth);
                }
                else
                {
                    var type = JSObject.TypeOf(obj);
                    if (type == "object" || type == "function")
                    {
                        DumpObject(node, obj, doc, depth);
                    }
                }
            }
            catch
            {
                var vnode = doc.CreateElement("P");
                vnode.SetAttribute("Value", "(exception catched)");
                vnode.SetAttribute("Type", "unknown");
                vnode.SetAttribute("Name", "$dumpFailed");
                node.AppendChild(vnode);
            }

			return node;
		}

		[DebuggerHidden]
		private static string Identity()
		{
			var doc = XmlToolkit.CreateDocument("Identity");
			doc.DocumentElement.SetAttribute("Url", Window.Instance.Location.Href);
			foreach(var mod in Modules.List)
            {
			    var element = doc.CreateElement("Module");
			    element.SetAttribute("Name", mod.Name);
			    element.SetAttribute("Version", mod.Version);
			    element.SetAttribute("Filename", mod.Filename);
                element.SetAttribute("Timestamp", mod.Timestamp);
			    doc.DocumentElement.AppendChild(element);
			}
			return XmlToolkit.ToXml(doc);
		}

		/// <summary>
		/// Called by each potential break point
		/// </summary>
		/// <param name="id">Point of code identifier</param>
		/// <returns>always true</returns>
		[DebuggerHidden]
		public static bool P(string id)
		{
            if (active)
            {
                if (callStack.Length > 0)
                {
                    callStack[callStack.Length - 1].Point = id;
                }
                if (stepDeep > 0 && stepDeep >= callStack.Length)
                {
                    active = false;
                    Break("steped", id);
                    active = true;
                }
                else if (breakPoints[id] != Script.JSObject.Undefined)
                {
                    active = false;
                    Break("reached", id);
                    active = true;
                }
            }
			return true;
		}
	}
}
