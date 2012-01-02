using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using NetWebScript.Debug.Server;
using System.Windows.Threading;
using System.Threading;

namespace NetWebScript.Debug.App.ViewModel
{
    public class ProgramModel : IJSProgramCallback, IDisposable
    {
        private readonly IJSProgram program;

        private readonly DispatcherObservableCollection<ThreadModel> threads;
        private readonly SourceFilesModel sources;
        private readonly ObservableCollection<JSDebugPoint> activePoints;
        private readonly Dispatcher dispatcher;

        public ObservableCollection<ThreadModel> Threads
        {
            get { return threads; }
        }

        public ProgramModel(Dispatcher dispatcher, IJSProgram program)
        {
            this.dispatcher = dispatcher;
            this.program = program;
            this.threads = new DispatcherObservableCollection<ThreadModel>(dispatcher);

            Name = program.Uri.ToString();
            program.RegisterCallback(this);
            foreach (var thread in program.Threads)
            {
                threads.Add(new ThreadModel(this, thread));
            }
            sources = new SourceFilesModel(dispatcher, program.ListSourceFiles());
            activePoints = new ObservableCollection<JSDebugPoint>(program.ListActivePoints());
        }

        public void Dispose()
        {
            program.UnRegisterCallback(this);
        }

        public void OnNewThread(IJSThread thread)
        {
            threads.AddAsync(new ThreadModel(this, thread));
        }

        public void OnNewModule(JSModuleInfo module)
        {
            sources.SetFilesAsync(program.ListSourceFiles());
        }

        public void OnModuleUpdate(JSModuleInfo module)
        {
            OnNewModule(module);
        }

        internal void Stopped(ThreadModel threadModel)
        {
            threads.RemoveAsync(threadModel);
            threadModel.Dispose();
        }

        public string Name
        {
            get;
            private set;
        }

        public ObservableCollection<SourceFolderModel> SourceFiles
        {
            get { return sources.Roots; }
        }

        public ObservableCollection<JSDebugPoint> ActivePoints
        {
            get { return activePoints; }
        }

        internal void TryAddBreakPoint(string filename, int lineNumber)
        {
            var list = program.FindPoints(filename, lineNumber);
            if (list.Count > 0)
            {
                if (list.Count > 1)
                {
                    list.Sort((a, b) => a.StartCol.CompareTo(b.StartCol));
                }
                ToggleBreakPoint(list.First());
            }
        }

        private void ToggleBreakPoint(JSDebugPoint point)
        {
            var existing = activePoints.FirstOrDefault(p => p.ModuleId == point.ModuleId && p.UId == point.UId);
            if (existing != null)
            {
                activePoints.Remove(point);
                program.RemoveBreakPoint(point.UId);
            }
            else
            {
                activePoints.Add(point);
                program.AddBreakPoint(point.UId);
            }
            if (BreakPointsChanged != null)
            {
                BreakPointsChanged(this, EventArgs.Empty);
            }
        }

        public JSDebugPoint CurrentPoint { get; private set; }

        public ThreadModel CurrentBreakedThread { get; private set; }

        public JSStack CurrentBreakedThreadStack { get; private set; }

        public event EventHandler BreakPointsChanged;

        public event EventHandler CurrentPointChanged;

        private class PendingThread
        {
            public ThreadModel Thread { get; set; }
            public JSDebugPoint Point { get; set; }
            public JSStack Stack { get; set; }
        }

        private readonly Queue<PendingThread> queue = new Queue<PendingThread>();

        internal void SetCurrentPointAsync(ThreadModel thread, JSDebugPoint point, JSStack stack)
        {
            dispatcher.Invoke(new Action(() => SetCurrentPoint(thread, point, stack)));
        }

        private void SetCurrentPoint(ThreadModel thread, JSDebugPoint point, JSStack stack)
        {
            if (CurrentBreakedThread != null && CurrentBreakedThread != thread)
            {
                queue.Enqueue(new PendingThread() { Thread = thread, Point = point, Stack = stack });
                return;
            }
            CurrentPoint = point;
            CurrentBreakedThread = thread;
            CurrentBreakedThreadStack = stack;
            if (CurrentPointChanged != null)
            {
                CurrentPointChanged(this, EventArgs.Empty);
            }
        }

        private void ClearCurrentPoint(bool canSwitchThread)
        {
            if (queue.Count > 0 && canSwitchThread)
            {
                CurrentBreakedThread = null;
                var next = queue.Dequeue();
                SetCurrentPoint(next.Thread, next.Point, next.Stack);
                return;
            }
            CurrentPoint = null;
            CurrentBreakedThread = null;
            CurrentBreakedThreadStack = null;
            if (CurrentPointChanged != null)
            {
                CurrentPointChanged(this, EventArgs.Empty);
            }
        }

        internal void Continue()
        {
            if (CurrentBreakedThread != null)
            {
                CurrentBreakedThread.Continue();
                ClearCurrentPoint(true);
            }
        }

        internal void StepInto()
        {
            if (CurrentBreakedThread != null)
            {
                CurrentBreakedThread.StepInto();
                ClearCurrentPoint(false);
            }
        }

        internal void StepOut()
        {
            if (CurrentBreakedThread != null)
            {
                CurrentBreakedThread.StepOut();
                ClearCurrentPoint(false);
            }
        }

        internal void StepOver()
        {
            if (CurrentBreakedThread != null)
            {
                CurrentBreakedThread.StepOver();
                ClearCurrentPoint(false);
            }
        }
    }
}
