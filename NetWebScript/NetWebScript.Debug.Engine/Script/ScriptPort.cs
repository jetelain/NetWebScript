using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Debug.Server;
using NetWebScript.Debug.Engine.Port;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Diagnostics;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using NetWebScript.Debug.Engine.Events;

namespace NetWebScript.Debug.Engine.Script
{
    internal class ScriptPort : IDebugPort2, IDebugPortEx2, IDebugPortRequest2, IJSServerCallback, IDebugPortNotify2, IConnectionPointContainer, IConnectionPoint
    {
        private static readonly List<ScriptProgram> programs = new List<ScriptProgram>();

        private readonly NWSPortSupplier supplier;
        private readonly String name;
        private readonly Guid id;

        private readonly WebServer server;

        public ScriptPort(NWSPortSupplier supplier, string name, Guid id)
        {   
            this.supplier = supplier;
            this.name = name;
            this.id = id;

            server = new WebServer("http://localhost:9090/");
            server.RegisterCallback(this);
            server.Start();
        }

        public Guid Id
        {
            get { return id; }
        }

        public int EnumProcesses(out IEnumDebugProcesses2 ppEnum)
        {
            Trace.TraceInformation("IDebugPort2.EnumProcesses");
            ppEnum = new DebugProcesses(programs.ToArray());
            return Constants.S_OK;
        }

        public int GetPortId(out Guid pguidPort)
        {
            Trace.TraceInformation("IDebugPort2.GetPortId");
            pguidPort = id;
            return Constants.S_OK;
        }

        public int GetPortName(out string pbstrName)
        {
            pbstrName = name;
            return Constants.S_OK;
        }

        public int GetPortRequest(out IDebugPortRequest2 ppRequest)
        {
            ppRequest = null;
            return Constants.E_PORT_NO_REQUEST;
        }

        public int GetPortSupplier(out IDebugPortSupplier2 ppSupplier)
        {
            ppSupplier = supplier;
            return Constants.S_OK;
        }

        public int GetProcess(AD_PROCESS_ID ProcessId, out IDebugProcess2 ppProcess)
        {
            Trace.TraceInformation("IDebugPort2.GetProcess");
            if (ProcessId.ProcessIdType == (int)enum_AD_PROCESS_ID.AD_PROCESS_ID_SYSTEM)
            {
                ppProcess = programs.FirstOrDefault(p => p.ProcessNum == ProcessId.dwProcessId);
                if (ppProcess != null)
                {
                    return Constants.S_OK;
                }
            }
            ppProcess = null;
            return Constants.E_UNEXPECTED;
        }

        public void OnNewProgram(IJSProgram program)
        {
            programs.Add(new ScriptProgram(this, program));
        }

        internal ScriptProgram LaunchSuspended(Uri uri)
        {
            // OnNewProgram is going to be called.
            var program = server.GetOrCreateProgram(uri);
            
            return programs.First(p => p.JSProgram == program);
        }

        #region IDebugPortNotify2 Members

        public int AddProgramNode(IDebugProgramNode2 pProgramNode)
        {
            Trace.TraceInformation("IDebugPortNotify2.AddProgramNode");
            var node = (ProgramNode)pProgramNode;
            var program = programs.First(p => p.ProcessNum == node.ProcessNum);
            SendProgramCreateEvent(program);
            return Constants.S_OK;
        }

        public int RemoveProgramNode(IDebugProgramNode2 pProgramNode)
        {
            Trace.TraceInformation("IDebugPortNotify2.RemoveProgramNode");
            return Constants.E_NOTIMPL;
        }

        #endregion

        internal void SendProgramCreateEvent(ScriptProgram program)
        {
            foreach (IDebugPortEvents2 sink in sinks)
            {
                sink.Event(null, this, program, program, new ProgramCreateEvent(), ProgramCreateEvent.IID);
            }
        }

        //internal void SendProgramDestroyEvent(ScriptProgram program, uint code)
        //{
        //    foreach (IDebugPortEvents2 sink in sinks)
        //    {
        //        sink.Event(null, this, program, program, new ProgramDestroyEvent(program, code), ProgramDestroyEvent.IID);
        //    }
        //}

        //internal void SendProcessDestroyEvent(ScriptProgram program)
        //{
        //    foreach (IDebugPortEvents2 sink in sinks)
        //    {
        //        sink.Event(null, this, program, null, new ProcessDestroyEvent(), ProcessDestroyEvent.IID);
        //    }
        //}

        #region IConnectionPointContainer Members

        public void EnumConnectionPoints(out IEnumConnectionPoints ppEnum)
        {
            throw new NotImplementedException();
        }

        public void FindConnectionPoint(ref Guid riid, out IConnectionPoint ppCP)
        {
            if (riid == typeof(IDebugPortEvents2).GUID)
            {
                ppCP = this;
            }
            else
            {
                ppCP = null;
            }
        }

        #endregion

        #region IConnectionPoint Members

        private readonly EventSinkCollection sinks = new EventSinkCollection();

        public void Advise(object pUnkSink, out uint pdwCookie)
        {
            pdwCookie = sinks.Add(pUnkSink);
        }

        public void EnumConnections(out IEnumConnections ppEnum)
        {
            throw new NotImplementedException();
        }

        public void GetConnectionInterface(out Guid pIID)
        {
            pIID = typeof(IDebugPortEvents2).GUID;
        }

        public void GetConnectionPointContainer(out IConnectionPointContainer ppCPC)
        {
            ppCPC = this;
        }

        public void Unadvise(uint dwCookie)
        {
            sinks.RemoveAt(dwCookie);
        }

        #endregion

        #region IDebugPortEx2 Members

        public int CanTerminateProcess(IDebugProcess2 pPortProcess)
        {
            Trace.TraceInformation("IDebugPortEx2.CanTerminateProcess");
            // As our processes are "virtual", we cannont really terminate them.
            // So we always return False
            return Constants.S_FALSE; 
        }

        public int GetPortProcessId(out uint pdwProcessId)
        {
            pdwProcessId = uint.MaxValue;
            return Constants.S_OK;
        }

        public int GetProgram(IDebugProgramNode2 pProgramNode, out IDebugProgram2 ppProgram)
        {
            Trace.TraceInformation("IDebugPortEx2.GetProgram");
            var node = pProgramNode as ProgramNode;
            if (node != null)
            {
                var program = programs.FirstOrDefault(p => p.ProcessNum == node.ProcessNum);
                if (program != null)
                {
                    Trace.TraceInformation("IDebugPortEx2.GetProgram => OK");
                    ppProgram = program;
                    return Constants.S_OK;
                }
            }
            ppProgram = null;
            return Constants.E_INVALIDARG;
        }

        public int LaunchSuspended(string pszExe, string pszArgs, string pszDir, string bstrEnv, uint hStdInput, uint hStdOutput, uint hStdError, out IDebugProcess2 ppPortProcess)
        {
            Trace.TraceInformation("IDebugPortEx2.LaunchSuspended");
            throw new NotImplementedException();
        }

        public int ResumeProcess(IDebugProcess2 pPortProcess)
        {
            Trace.TraceInformation("IDebugPortEx2.ResumeProcess");
            throw new NotImplementedException();
        }

        public int TerminateProcess(IDebugProcess2 pPortProcess)
        {
            Trace.TraceInformation("IDebugPortEx2.TerminateProcess");
            throw new NotImplementedException();
        }

        #endregion
    }
}
