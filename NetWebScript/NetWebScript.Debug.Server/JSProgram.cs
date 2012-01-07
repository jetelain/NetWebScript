using System;
using System.Linq;
using System.Collections.Generic;
using NetWebScript.Metadata;
using System.Diagnostics.Contracts;
using System.Diagnostics;
namespace NetWebScript.Debug.Server
{
    internal sealed class JSProgram : IJSProgram
    {
        private readonly Dictionary<String,JSThread> threadsDict = new Dictionary<String, JSThread>();
        private readonly List<JSModuleDebugPoint> breakPointsUids = new List<JSModuleDebugPoint>();
        private readonly List<JSDebugPoint> breakPoints = new List<JSDebugPoint>();

        private readonly int id;
        private readonly WebServer server;
        
        private int nextThreadId = 1;

        private readonly List<IJSProgramCallback> callbacks = new List<IJSProgramCallback>();

        //private readonly List<ModuleInfo> moduleInfos = new List<ModuleInfo>();
        private readonly List<JSModule> modules = new List<JSModule>();
        private readonly ProgramIdentity identity;
        private readonly Uri uri;

        private readonly object locker = new object();

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

        public string Name
        {
            get
            {
                lock (locker)
                {
                    if (modules.Count == 0)
                    {
                        return uri.ToString();
                    }
                    return string.Join(" ,", modules.Select(m => m.ModuleInfo.Name));
                }
            }
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

        internal IEnumerable<String> BreakPoints
        {
            get { return breakPointsUids.Select(p => p.UId); }	
        }

        private void AddBreakPoint(JSModuleDebugPoint point)
        {
            if (!breakPointsUids.Any(p => p.UId == point.UId))
            {
                breakPointsUids.Add(point);
                foreach (JSThread thread in threadsDict.Values)
                {
                    thread.NotifyNewBreakPoint(point.UId);
                }
            }
        }

        private void RemoveBreakPoint(JSModuleDebugPoint point)
        {
            if (breakPointsUids.Remove(point))
            {
                foreach (JSThread thread in threadsDict.Values)
                {
                    thread.NotifyRemoveBreakPoint(point.UId);
                }
            }
        }

        public void ClearAndDetachAll()
        {
            lock (locker)
            {
                breakPoints.Clear();
                breakPointsUids.Clear();
                foreach (JSThread thread in threadsDict.Values)
                {
                    thread.NotifyDetach();
                }
            }
        }

        internal JSThread CreateThread()
        {
            JSThread thread;
            lock (locker)
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

        public ICollection<IJSThread> Threads
        {
            get 
            {
                lock (locker)
                {
                    return threadsDict.Values.Cast<IJSThread>().ToList();
                }
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

        private void ComputeNewBreakpointsUids()
        {
            Contract.Requires(threadsDict.Count == 0);
            breakPointsUids.Clear();
            foreach (var module in modules)
            {
                foreach (var point in breakPoints)
                {
                    var resolved = module.ResolvePoint(point);
                    foreach (var modulePoint in resolved)
                    {
                        AddBreakPoint(modulePoint);
                    }
                }
            }
        }

        private void DetachAllThreads()
        {
            Contract.Ensures(threadsDict.Count == 0);
            foreach (JSThread thread in threadsDict.Values)
            {
                thread.NotifyDetach();
            }
            threadsDict.Clear();
        }

        private void ModuleUpdate(JSModule existing, ModuleInfo newModule)
        {
            lock (locker)
            {
                // Detach all existing threads
                DetachAllThreads();

                // Update module metadata
                existing.UpdateMetadata(newModule);

                // Computes new breakpoints
                ComputeNewBreakpointsUids();
            }

            lock (callbacks)
            {
                foreach (IJSProgramCallback callback in callbacks)
                {
                    callback.OnModuleUpdate(existing.ModuleInfo);
                }
            }
        }


        internal void MergeModules(List<ModuleInfo> newModules)
        {
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



        internal JSModuleDebugPoint GetPointById(string pointId)
        {
            lock (locker)
            {
                return modules.Select(m => m.GetPointById(pointId)).FirstOrDefault(p => p != null);
            }
        }

        internal MethodBaseMetadata GetMethodById(string methodId)
        {
            lock (locker)
            {
                return modules.Select(m => m.GetMethodById(methodId)).FirstOrDefault(p => p != null);
            }
        }

        internal TypeMetadata GetTypeById(string typeId)
        {
            lock (locker)
            {
                return modules.Select(m => m.GetTypeById(typeId)).FirstOrDefault(p => p != null);
            }
        }

        public List<JSDebugPoint> FindPoints(string fileName, int startCol, int startRow)
        {
            HashSet<JSDebugPoint> points;
            lock (locker)
            {
                points = new HashSet<JSDebugPoint>(modules.SelectMany(m => m.FindPoints(fileName, startCol, startRow)));
            }
            return points.ToList();
        }

        public List<JSDebugPoint> FindPoints(string fileName, int startRow)
        {
            HashSet<JSDebugPoint> points;
            lock (locker)
            {
                points = new HashSet<JSDebugPoint>(modules.SelectMany(m => m.FindPoints(fileName, startRow)));
            }
            return points.ToList();
        }

        public List<string> ListSourceFiles()
        {
            HashSet<string> files;
            lock (locker)
            {
                files = new HashSet<string>(modules.SelectMany(m => m.ListSourceFiles()), StringComparer.OrdinalIgnoreCase);
            }
            return files.ToList();
        }

        public List<JSDebugPoint> ActivePoints
        {
            get
            {
                lock (locker)
                {
                    return breakPoints.ToList();
                }
            }
        }


        public void RemoveBreakPoint(JSDebugPoint point)
        {
            lock (locker)
            {
                if (breakPoints.Remove(point))
                {
                    var resolved = breakPointsUids.Where(p => p.Point.Equals(point)).ToList();
                    foreach (var modulePoint in resolved)
                    {
                        RemoveBreakPoint(modulePoint);
                    }
                }
            }
        }

        public void AddBreakPoint(JSDebugPoint point)
        {
            lock (locker)
            {
                if (!breakPoints.Contains(point))
                {
                    breakPoints.Add(point);
                    foreach (var module in modules)
                    {
                        var resolved = module.ResolvePoint(point);
                        foreach (var modulePoint in resolved)
                        {
                            AddBreakPoint(modulePoint);
                        }
                    }
                }
            }
        }

    }
}

