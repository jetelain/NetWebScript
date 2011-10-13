using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;
using NetWebScript.Debug.Engine.Script;

namespace NetWebScript.Debug.Engine.Events
{
    class ModuleEventLoad : AsynchronousEvent, IDebugModuleLoadEvent2
    {
        public static readonly Guid IID = new Guid("{989DB083-0D7C-40D1-A9D9-921BF611A4B2}");

        private readonly ScriptModule module;
        private readonly bool loaded;

        internal ModuleEventLoad(ScriptModule module, bool loaded)
        {
            this.module = module;
            this.loaded = loaded;
        }

        #region IDebugModuleLoadEvent2 Members

        public int GetModule(out IDebugModule2 pModule, ref string pbstrDebugMessage, ref int pbLoad)
        {
            pModule = module;
            if (loaded)
            {
                pbstrDebugMessage = String.Format("Loaded '{0}'", module.ModuleFile);
                pbLoad = 1;
            }
            else
            {
                pbstrDebugMessage = String.Format("Unloaded '{0}'", module.ModuleFile);
                pbLoad = 0;
            }
            return Constants.S_OK;
        }

        #endregion
    }
}
