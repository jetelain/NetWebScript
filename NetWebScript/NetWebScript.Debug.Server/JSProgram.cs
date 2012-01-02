using System;
using System.Linq;
using System.Collections.Generic;
using NetWebScript.Metadata;
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

        //private readonly List<ModuleInfo> moduleInfos = new List<ModuleInfo>();
        private readonly List<JSModule> modules = new List<JSModule>();
        private readonly ProgramIdentity identity;
        private readonly Uri uri;

        public JSProgram (WebServer server, int id, Uri uri, IEnumerable<ModuleInfo> initialModules)
        {
            this.server = server;
            this.id = id;
            this.uri = uri;
            if (initialModules != null)
            {
                foreach (var moduleInfo in initialModules)
                {
                    modules.Add(new JSModule(moduleInfo, modules.Count + 1));
                }
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

        public IList<JSModuleInfo> Modules
        {
            get { return modules.Select(m => m.ModuleInfo).ToList(); }
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
            var jsModule = new JSModule(info, modules.Count + 1);
            modules.Add(jsModule);
            lock (callbacks)
            {
                foreach (IJSProgramCallback callback in callbacks)
                {
                    callback.OnNewModule(jsModule.ModuleInfo);
                }
            }
        }

        private void ModuleUpdate(JSModule existing, ModuleInfo newModule)
        {
            existing.UpdateMetadata(newModule);
            
            lock (callbacks)
            {
                lock (callbacks)
                {
                    foreach (IJSProgramCallback callback in callbacks)
                    {
                        callback.OnModuleUpdate(existing.ModuleInfo);
                    }
                }
            }
        }


        internal void MergeModules(List<ModuleInfo> newModules)
        {
            // FIXME: Due to browser 'cache', multiple version of a same module may be connected to debugger at the same time.
            // Module should be attached to 'thread' and not to 'program' to avoid problems.

            foreach (ModuleInfo newModule in newModules)
            {
                var existing = modules.FirstOrDefault(m => m.ModuleUri == newModule.Uri);

                if (existing == null)
                {
                    AddModule(newModule);
                }
                else if (existing.ModuleInfo.Timestamp != newModule.Timestamp)
                {
                    ModuleUpdate(existing, newModule);
                }
            }
        }



        internal JSDebugPoint GetPointById(string pointId)
        {
            lock (modules)
            {
                return modules.Select(m => m.GetPointById(pointId)).FirstOrDefault(p => p != null);
            }
        }

        internal MethodBaseMetadata GetMethodById(string methodId)
        {
            lock (modules)
            {
                return modules.Select(m => m.GetMethodById(methodId)).FirstOrDefault(p => p != null);
            }
        }

        internal TypeMetadata GetTypeById(string typeId)
        {
            lock (modules)
            {
                return modules.Select(m => m.GetTypeById(typeId)).FirstOrDefault(p => p != null);
            }
        }

        public List<JSDebugPoint> FindPoints(string fileName, int startCol, int startRow)
        {
            return modules.SelectMany(m => m.FindPoints(fileName, startCol, startRow)).ToList();
        }

        public List<JSDebugPoint> FindPoints(string fileName, int startRow)
        {
            return modules.SelectMany(m => m.FindPoints(fileName, startRow)).ToList();
        }


        public List<string> ListSourceFiles()
        {
            HashSet<string> files;
            lock (modules)
            {
                files = new HashSet<string>(modules.SelectMany(m => m.ListSourceFiles()), StringComparer.OrdinalIgnoreCase);
            }
            return files.ToList();
        }

        public List<JSDebugPoint> ListActivePoints()
        {
            lock (this)
            {
                return breakPoints.Select(pointId => modules.Select(m => m.GetPointById(pointId)).FirstOrDefault(p => p != null)).Where(p => p != null).ToList();
            }
        }
    }
}

