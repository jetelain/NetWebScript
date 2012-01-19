using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Flavor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Web.Application;

namespace NetWebScript.Debug.Launcher
{
    class NwsFlavorProject : FlavoredProjectBase, IVsProjectFlavorCfgProvider
    {
        private readonly NwsFlavorPackage package;
        private IVsProjectFlavorCfgProvider innerCfgProvider;
        private IVsWebApplicationProject2 webApplication;

        public NwsFlavorProject(NwsFlavorPackage package)
        {
            this.package = package;
        }

        protected override void SetInnerProject(IntPtr innerIUnknown)
        {
            object inner = null;
            inner = Marshal.GetObjectForIUnknown(innerIUnknown);
            if (base.serviceProvider == null)
            {
                base.serviceProvider = this.package;
            }
            innerCfgProvider = inner as IVsProjectFlavorCfgProvider;
            webApplication = inner as IVsWebApplicationProject2;
            base.SetInnerProject(innerIUnknown);
        }

        internal string Url
        {
            get
            {
                if (webApplication != null)
                {
                    string value;
                    webApplication.GetBrowseUrl(out value);
                    return value;
                }
                return null;
            }
        }

        protected override void InitializeForOuter(string fileName, string location, string name, uint flags, ref Guid guidProject, out bool cancel)
        {
            base.InitializeForOuter(fileName, location, name, flags, ref guidProject, out cancel);
        }

        public int CreateProjectFlavorCfg(IVsCfg pBaseProjectCfg, out IVsProjectFlavorCfg ppFlavorCfg)
        {
            if (innerCfgProvider != null)
            {
                innerCfgProvider.CreateProjectFlavorCfg(pBaseProjectCfg, out ppFlavorCfg);
                pBaseProjectCfg = (IVsCfg)ppFlavorCfg;
            }
            ppFlavorCfg = new NwsProjCfg(this, pBaseProjectCfg);
            return VSConstants.S_OK;
        }
    }
}
