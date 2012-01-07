using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Diagnostics;
using NetWebScript.Metadata;
using System.Xml;

namespace NetWebScript.Debug.Server
{
    public enum JSThreadState
    {
        Running,
        Breaked,
        Stopped
    }
    
    internal class JSThread : IJSThread
    {
        private readonly JSProgram prog;
        private readonly Queue<String> messages = new Queue<String>();
        private readonly HashSet<String> ackedBreakPoints = new HashSet<String>();

        private bool waiting = false;
        private DateTime lastActivity;
        private volatile JSThreadState state = JSThreadState.Running;

        private JSModuleDebugPoint currentPoint;
        private JSStack currentStack;

        private readonly List<IJSThreadCallback> callbacks = new List<IJSThreadCallback>();

        private readonly int id;
        private readonly string uid;

        public JSThread (int id, JSProgram prog)
        {
            this.prog = prog;
            this.id = id;
            this.uid = prog.Id + "/" + id;
        }

        public int Id
        {
            get { return id; }
        }

        public string UId
        {
            get { return uid; }
        }

        internal void NotifyNewBreakPoint(String uid)
        {
            SendMessage("addbp:"+uid);	
        }

        internal void NotifyRemoveBreakPoint(String uid)
        {
            SendMessage("rmbp:" + uid);
        }

        internal void NotifyDetach()
        {
            Stopped();
            SendMessage("detach");
        }
        
        public void StepOver ( )
        {
            SendMessage("step:eq");
        }

        public void StepOut()
        {
            SendMessage("step:up");
        }

        public void StepInto()
        {
            SendMessage("step:dw");
        }
        
        public void Continue()
        {
            SendMessage("continue");
        }

        public JSThreadState State
        {
            get { return state; }
        }

        public String CurrentPoint
        {
            get { return currentPoint == null ? null : currentPoint.UId; }
        }
        
        public event EventHandler BreakPointReady;
    
        private void Stopped ( )
        {
            lock ( this )
            {
                state = JSThreadState.Stopped;	
            }
            lock (callbacks)
            {
                foreach (IJSThreadCallback callback in callbacks.ToArray())
                {
                    callback.OnStopped();
                }
            }
        }
        
        private void ReachedPoint ( String uid, String stackXml )
        {
            if (!Reached(uid, stackXml))
            {
                return;
            }
            lock (callbacks)
            {
                foreach (IJSThreadCallback callback in callbacks)
                {
                    callback.OnBreakpoint(currentPoint, currentStack);
                }
            }
        }

        private void StepedPoint(String uid, String stackXml)
        {
            if (!Reached(uid, stackXml))
            {
                return;
            }
            lock (callbacks)
            {
                foreach (IJSThreadCallback callback in callbacks)
                {
                    callback.OnStepDone(currentPoint, currentStack);
                }
            }
        }

        private bool Reached(string uid, string stackXml)
        {
            lock (this)
            {
                state = JSThreadState.Breaked;
                currentPoint = GetPointById(uid);
                if (currentPoint == null)
                {
                    Continue();
                    return false;
                }
                else
                {
                    currentStack = new JSStack(this, currentPoint, stackXml);
                }
            }
            return true;
        }

        private void ContinueAck()
        {
            lock ( this )
            {
                state = JSThreadState.Running;
                currentPoint = null;
                currentStack = null;
            }
            lock (callbacks)
            {
                foreach (IJSThreadCallback callback in callbacks)
                {
                    callback.OnContinueDone();
                }
            }
        }
        
        private void BreakPointAck(String[] uids)
        {
            lock (this)
            {
                ackedBreakPoints.UnionWith(uids);
            }
        }

        private ManualResetEvent waitHandle = new ManualResetEvent(true);
        
        private void SendMessage ( String message )
        {
            lock ( this )
            {
                messages.Enqueue(message);
                if (waiting)
                {
                    waitHandle.Set();	
                }
            }
        }

        internal String GetBlockingMessage()
        {
            lock ( this )
            {
                if ( messages.Count > 0 )
                {
                    return messages.Dequeue();
                }
                waiting = true;
            }
            waitHandle.WaitOne(2000);
            lock(this)
            {
                waiting = false;
                if ( messages.Count > 0 )
                {
                    return messages.Dequeue();
                }
            }
            return "wait";
        }
        
        internal String GetNonBlockingMessage()
        {
            lock ( this )
            {
                if ( messages.Count > 0 )
                {
                    return messages.Dequeue();
                }
            }
            return "nop";
        }

        internal String InitMessage()
        {
            return UId+":" + String.Join(",", prog.BreakPoints.ToArray());	
        }

        internal String Query ( String cmd, String data, String postData, bool blocking )
        {
            lastActivity = DateTime.Now;
            
            // Commandes spéciales : ne renvoie pas de message (requetes "asynchrones")
            if ( cmd == "status" )
            {
                lock ( this )
                {
                    return messages.Count == 0 ? "false" : "true";	
                }
            }
            
            if ( cmd == "stop" )
            {
                Stopped();
                return String.Empty;	
            }
            
            // Commandes classiques (requetes synchrone)
            if (cmd == "continueACK" )
            {
                ContinueAck();
            }
            else if ( cmd == "bpACK" )
            {
                BreakPointAck(data.Split(','));
            }
            else if ( cmd == "reached" )
            {
                ReachedPoint(data, postData);
            }
            else if (cmd == "steped")
            {
                StepedPoint(data, postData);
            }
            else if (cmd == "result")
            {
                ResultReceived(data, postData);
            }

            if ( blocking )
            {
                return GetBlockingMessage();
            }
            else
            {			
                return GetNonBlockingMessage();
            }
        }

        private class ResultWaiter
        {
            public string Expression;
            public ManualResetEvent WaitHandle = new ManualResetEvent(false);
            public JSData Data;
        }

        private readonly List<ResultWaiter> resultWaiters = new List<ResultWaiter>();

        private void ResultReceived(string expression, string postData)
        {
            ResultWaiter waiter;
            lock (resultWaiters)
            {
                waiter = resultWaiters.FirstOrDefault(w => w.Expression == expression);
                if (waiter != null)
                {
                    resultWaiters.Remove(waiter);
                }
            }

            if (waiter != null)
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(postData);
                XmlElement node = document.SelectSingleNode("/Dump/P") as XmlElement;
                if (node != null)
                {
                    waiter.Data = new JSData(this, node, expression, "result");
                }
                else
                {
                    waiter.Data = null;
                }
                waiter.WaitHandle.Set();
            }
        }

        public void RegisterCallback(IJSThreadCallback callback)
        {
            lock (callbacks)
            {
                callbacks.Add(callback);
            }
        }

        public void UnRegisterCallback(IJSThreadCallback callback)
        {
            lock (callbacks)
            {
                callbacks.Remove(callback);
            }
        }


        internal JSModuleDebugPoint GetPointById(string pointId)
        {
            return prog.GetPointById(pointId);
        }

        internal MethodBaseMetadata GetMethodById(string methodId)
        {
            return prog.GetMethodById(methodId);
        }

        internal TypeMetadata GetTypeById(string typeId)
        {
            return prog.GetTypeById(typeId);
        }

        public JSData Expand(JSData data)
        {
            if (string.IsNullOrEmpty(data.Path))
            {
                return null;
            }
            string expression = "return " + data.Path + ";";

            ResultWaiter waiter = new ResultWaiter() { Expression = expression };
            lock (resultWaiters)
            {
                resultWaiters.Add(waiter);
            }
            SendMessage("retreive:" + expression);
            waiter.WaitHandle.WaitOne(5000);
            lock (resultWaiters)
            {
                resultWaiters.Remove(waiter);
            }
            return waiter.Data;
        }
    }
}

