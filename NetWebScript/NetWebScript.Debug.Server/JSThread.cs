using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
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

        private String currentPoint;

        private readonly List<IJSThreadCallback> callbacks = new List<IJSThreadCallback>();

        private readonly int id;
        private readonly String uid;

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

        public String UId
        {
            get { return uid; }
        }

        public void NotifyNewBreakPoint ( String uid )
        {
            SendMessage("addbp:"+uid);	
        }

        public void NotifyRemoveBreakPoint(String uid)
        {
            SendMessage("rmbp:" + uid);
        }

        public void NotifyDetach()
        {
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
            get { return currentPoint; }
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
                foreach (IJSThreadCallback callback in callbacks)
                {
                    callback.OnStopped();
                }
            }
        }
        
        private void ReachedPoint ( String uid, String stackXml )
        {
            lock ( this )
            {
                state = JSThreadState.Breaked;
                currentPoint = uid;
            }
            lock (callbacks)
            {
                foreach (IJSThreadCallback callback in callbacks)
                {
                    callback.OnBreakpoint(uid, stackXml);
                }
            }
        }

        private void StepedPoint(String uid, String stackXml)
        {
            lock (this)
            {
                state = JSThreadState.Breaked;
                currentPoint = uid;
            }
            lock (callbacks)
            {
                foreach (IJSThreadCallback callback in callbacks)
                {
                    callback.OnStepDone(uid, stackXml);
                }
            }
        }

        private void ContinueAck()
        {
            lock ( this )
            {
                state = JSThreadState.Running;
                currentPoint = null;
            }
            //if (StateChanged != null)
            //{
            //    StateChanged(this, EventArgs.Empty);
            //}
        }
        
        private void BreakPointAck(String[] uids)
        {
            lock (this)
            {
                ackedBreakPoints.UnionWith(uids);
            }
        }
        
        private Semaphore semaphore = new Semaphore(1,1);
        
        private void SendMessage ( String message )
        {
            lock ( this )
            {
                messages.Enqueue(message);
                if ( waiting )
                {
                    semaphore.Release();	
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
            semaphore.WaitOne(2000);
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

            if ( blocking )
            {
                return GetBlockingMessage();
            }
            else
            {			
                return GetNonBlockingMessage();
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

    }
}

