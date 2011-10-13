using System;
using System.Linq;
using System.Collections.Generic;
namespace NetWebScript.Debug.Server
{
    internal sealed class JSProgram : IJSProgram
	{
		private readonly Dictionary<String,JSThread> threadsDict = new Dictionary<String, JSThread>();
		private readonly List<String> breakPoints = new List<String>();
        private readonly int id;
        private readonly WebServer server;
        
        private int nextThreadId = 1;

        private readonly List<IJSProgramCallback> callbacks = new List<IJSProgramCallback>();

        private readonly List<ModuleInfo> modules = new List<ModuleInfo>();
        private readonly ProgramIdentity identity;
        private readonly Uri uri;

		public JSProgram (WebServer server, int id, Uri uri, IEnumerable<ModuleInfo> initialModules)
		{
            this.server = server;
            this.id = id;
            this.uri = uri;
            if (initialModules != null)
            {
                modules.AddRange(initialModules);
            }
            identity = new ProgramIdentity(uri);
		}

        public int Id
        {
            get { return id; }
        }

        public Uri Uri
        {
            get { return uri; }
        }

        public IList<ModuleInfo> Modules
        {
            get { return modules; }
        }

        internal bool IsPartOfProgram(Uri uri)
        {
            return identity.IsPartOfProgram(uri);
        }

		public List<String> BreakPoints
		{
			get { return breakPoints; }	
		}

		public void AddBreakPoint ( String uid )
		{
			lock (this)
			{
				if ( !breakPoints.Contains(uid) )
				{
					breakPoints.Add(uid);
					foreach ( JSThread thread in threadsDict.Values )
					{
						thread.NotifyNewBreakPoint(uid);
					}
				}
			}
		}

        public void RemoveBreakPoint(String uid)
        {
            lock (this)
            {
                if (breakPoints.Contains(uid))
                {
                    breakPoints.Remove(uid);
                    foreach (JSThread thread in threadsDict.Values)
                    {
                        thread.NotifyRemoveBreakPoint(uid);
                    }
                }
            }
        }

        public void DetachAll()
        {
            lock (this)
            {
                breakPoints.Clear();
                foreach (JSThread thread in threadsDict.Values)
                {
                    thread.NotifyDetach();
                }
            }
        }

        internal JSThread CreateThread()
        {
            JSThread thread;
            lock (this)
            {
                thread = new JSThread(nextThreadId, this);
                nextThreadId++;

                threadsDict.Add(thread.UId, thread);
                lock (callbacks)
                {
                    foreach (IJSProgramCallback callback in callbacks)
                    {
                        callback.OnNewThread(thread);
                    }
                }
            }
            return thread;
        }

		/*public JSThread GetThread ( String t )
		{
			JSThread thread;
			if( !threadsDict.TryGetValue(t, out thread) )
			{
                lock (this)
                {
                    if (!threadsDict.TryGetValue(t, out thread))
                    {
                        thread = new JSThread(this);
                        threadsDict.Add(t, thread);
                        lock (callbacks)
                        {
                            foreach (IJSProgramCallback callback in callbacks)
                            {
                                callback.OnNewThread(thread);
                            }
                        }
                    }
                }
			}	
			return thread;
		}*/

        public ICollection<IJSThread> Threads
        {
            get 
            { 
                List<IJSThread> list = new List<IJSThread>();
                foreach (JSThread thread in threadsDict.Values)
                {
                    list.Add(thread);
                }
                return list;
            }
        }

        public void RegisterCallback(IJSProgramCallback callback)
        {
            lock (callbacks)
            {
                callbacks.Add(callback);
            }
        }

        public void UnRegisterCallback(IJSProgramCallback callback)
        {
            lock (callbacks)
            {
                callbacks.Remove(callback);
            }
        }

        private void AddModule(ModuleInfo info)
        {
            modules.Add(info);
            lock (callbacks)
            {
                foreach (IJSProgramCallback callback in callbacks)
                {
                    callback.OnNewModule(info);
                }
            }
        }


        internal void MergeModules(List<ModuleInfo> newModules)
        {
            foreach (ModuleInfo newModule in newModules)
            {
                if (modules.FirstOrDefault(m => m.Uri == newModule.Uri) == null)
                {
                    AddModule(newModule);
                }
            }
        }
    }
}

