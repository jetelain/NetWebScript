using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using System.Runtime.InteropServices;
using NetWebScript.Debug.Engine.Script;
using System.Diagnostics;

namespace NetWebScript.Debug.Engine.Port
{
    [Guid(NWSPortSupplier.ClsId)]
    [ComVisible(true)]
    public class NWSPortSupplier : IDebugPortSupplier3, IDebugPortSupplierDescription2
    {
        public const string ClsId = "7B589914-6569-4aeb-BDED-EAFEE56E1667";
        public const string Id = "9D17E7CB-E7DB-4eb8-BF2C-DA2A454703AC";
        public const string Name = "NetWebScript";

        public static readonly Guid PortSupplierId = new Guid("{" + Id + "}");

        private readonly ScriptPort localPort;
        private readonly List<ScriptPort> ports = new List<ScriptPort>();

        public NWSPortSupplier()
        {
            localPort = new ScriptPort(this, "localhost", new Guid("{5C155ECD-C175-42d1-893F-749892E5C12F}"));
            ports.Add(localPort);
        }

        public int AddPort(IDebugPortRequest2 pRequest, out IDebugPort2 ppPort)
        {
            ppPort = localPort;
            return Constants.S_OK;
        }

        public int CanAddPort()
        {
            return Constants.S_OK;
        }

        public int CanPersistPorts()
        {
            return Constants.S_OK;
        }

        public int EnumPersistedPorts(BSTR_ARRAY PortNames, out IEnumDebugPorts2 ppEnum)
        {
            ppEnum = new DebugPorts(ports.ToArray());
            return Constants.S_OK;
        }

        public int EnumPorts(out IEnumDebugPorts2 ppEnum)
        {
            ppEnum = new DebugPorts(ports.ToArray());
            return Constants.S_OK;
        }

        public int GetPort(ref Guid guidPort, out IDebugPort2 ppPort)
        {
            Guid id = guidPort;
            ppPort = ports.FirstOrDefault(p => p.Id == id);
            if (ppPort != null)
            {
                return Constants.S_OK;
            }
            return Constants.E_PORTSUPPLIER_NO_PORT;
        }

        public int GetPortSupplierId(out Guid pguidPortSupplier)
        {
            pguidPortSupplier = PortSupplierId;
            return Constants.S_OK;
        }

        public int GetPortSupplierName(out string pbstrName)
        {
            pbstrName = Name;
            return Constants.S_OK;
        }

        public int RemovePort(IDebugPort2 pPort)
        {
            Trace.TraceInformation("IDebugPortSupplier3.RemovePort");
            return Constants.E_NOTIMPL;
        }

        public int GetDescription(enum_PORT_SUPPLIER_DESCRIPTION_FLAGS[] pdwFlags, out string pbstrText)
        {
            pbstrText = "Permet de s'attacher à un programme NetWebScript s'executant sur le serveur local.";
            return Constants.S_OK;
        }

    }
}
