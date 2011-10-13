using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
namespace NetWebScript.Test.Server
{
    public enum TestRunnerState
	{
		Starting,
		Running,
		Stopped
	}
	
	public class TestRunner
	{
        private readonly Queue<String> sendqueue = new Queue<String>();
        

        private bool waiting = false;
        private DateTime lastActivity;
        private volatile TestRunnerState state = TestRunnerState.Starting;

        private Semaphore resultEvent = new Semaphore(0,1);
        private volatile string lastResult;

        public TestRunner(String id)
		{
            this.Id = id;
		}

        public String Id { get; private set; }

		public String RunTest ( String method, String script )
		{
            SendMessage("run:" + method + ":" + script);
            if (resultEvent.WaitOne(30000))
            {
                Console.WriteLine(lastResult);
                return lastResult;
            }
            return String.Empty;
		}
		
		public void ContinueUntil ( String uid )
		{
			SendMessage("continueto:"+uid);
		}
		
		public void Continue()
		{
			SendMessage("continue");
		}

        public TestRunnerState State
        {
            get { return state; }
        }
		

		private void Ready (  )
		{
			lock ( this )
			{
                state = TestRunnerState.Running;	
			}
        }
		
		private void Stopped (  )
		{
			lock ( this )
			{
                state = TestRunnerState.Stopped;	
			}
		}

		private Semaphore receivesemaphore = new Semaphore(1,1);
		
		private void SendMessage ( String message )
		{
			lock ( this )
			{
				sendqueue.Enqueue(message);
				if ( waiting )
				{
					receivesemaphore.Release();	
				}
			}
		}

        private String GetBlockingMessage()
		{
			lock ( this )
			{
				if ( sendqueue.Count > 0 )
				{
					return sendqueue.Dequeue();
				}
				waiting = true;
			}
            receivesemaphore.WaitOne(2000);
			lock(this)
			{
				waiting = false;
				if ( sendqueue.Count > 0 )
				{
					return sendqueue.Dequeue();
				}
			}
			return "wait";
		}
		
		private String GetNonBlockingMessage()
		{
			lock ( this )
			{
				if ( sendqueue.Count > 0 )
				{
					return sendqueue.Dequeue();
				}
			}
			return "nop";
		}
		
		internal String Query ( String cmd, String data, bool blocking )
		{
			lastActivity = DateTime.Now;
			
			// Commandes spéciales : ne renvoie pas de message (requetes "asynchrones")
			if ( cmd == "status" )
			{
				lock ( this )
				{
					return sendqueue.Count == 0 ? "false" : "true";	
				}
			}
			
			if ( cmd == "stop" )
			{
				Stopped();
				return String.Empty;	
			}

            if (cmd == "result")
            {
                lock (this)
                {
                    lastResult = data;
                }
                resultEvent.Release(1);
            }

			// Commandes classiques (requetes synchrone)
			if ( cmd == "ready" )
			{
				Ready();
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
		
	}
}

