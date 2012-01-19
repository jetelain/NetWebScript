using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace NetWebScript.Debug.Launcher
{
    internal class NwsProjCfg : IVsProjectFlavorCfg, IVsDebuggableProjectCfg, IVsDebuggableProjectCfg2
    {
        private NwsFlavorProject _project;
        private IVsCfg _baseConfig;
        private IVsDebuggableProjectCfg2 _innerDebuggableProjCfg;
        private IVsQueryDebuggableProjectCfg innerQuery;

        public IVsDebuggableProjectCfg2 InnerDebuggableProjCfg
        {
            get { return _innerDebuggableProjCfg; }
        }

        internal NwsProjCfg(NwsFlavorProject project, IVsCfg baseConfig)
        {
            _project = project;
            _baseConfig = baseConfig;
            _innerDebuggableProjCfg = _baseConfig as IVsDebuggableProjectCfg2;
            innerQuery = _baseConfig as IVsQueryDebuggableProjectCfg;
        }

        public int Close()
        {
            return VSConstants.S_OK;
        }

        public int get_CfgType(ref Guid iidCfg, out IntPtr ppCfg)
        {
            ppCfg = IntPtr.Zero;

            if (iidCfg.Equals(typeof(IVsDebuggableProjectCfg).GUID) && _innerDebuggableProjCfg != null)
                ppCfg = Marshal.GetComInterfaceForObject(this, typeof(IVsDebuggableProjectCfg));
            else if (iidCfg.Equals(typeof(IVsDebuggableProjectCfg2).GUID) && _innerDebuggableProjCfg != null)
                ppCfg = Marshal.GetComInterfaceForObject(this, typeof(IVsDebuggableProjectCfg2));

            if (ppCfg != IntPtr.Zero)
                return VSConstants.S_OK;

            return VSConstants.E_NOINTERFACE;
        }



        #region IVsDebuggableProjectCfg

        public int DebugLaunch(uint grfLaunch)
        {
            int result = InnerDebuggableProjCfg.DebugLaunch(grfLaunch);

            if (result == VSConstants.S_OK)
            {
                var url = _project.Url;
                if (url != null)
                {
                    var sp = (System.IServiceProvider)_project;
                    VsDebugTargetInfo info = new VsDebugTargetInfo();
                    info.dlo = DEBUG_LAUNCH_OPERATION.DLO_CreateProcess;
                    info.bstrExe = url;
                    info.bstrCurDir = null;
                    info.bstrArg = null;
                    info.bstrRemoteMachine = null;
                    info.fSendStdoutToOutputWindow = 0;
                    info.clsidCustom = new Guid("{88EBAE0F-1B09-4c57-9463-799D483E4097}");
                    info.clsidPortSupplier = new Guid("{9D17E7CB-E7DB-4eb8-BF2C-DA2A454703AC}");
                    info.bstrPortName = "localhost";
                    info.grfLaunch = 0;
                    try
                    {
                        VsShellUtilities.LaunchDebugger(sp, info);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return result;
        }

        public int EnumOutputs(out IVsEnumOutputs ppIVsEnumOutputs)
        {
            return InnerDebuggableProjCfg.EnumOutputs(out ppIVsEnumOutputs);
        }

        public int OpenOutput(string szOutputCanonicalName, out IVsOutput ppIVsOutput)
        {
            return InnerDebuggableProjCfg.OpenOutput(szOutputCanonicalName, out ppIVsOutput);
        }

        public int QueryDebugLaunch(uint grfLaunch, out int pfCanLaunch)
        {
            return InnerDebuggableProjCfg.QueryDebugLaunch(grfLaunch, out pfCanLaunch);
        }

        public int get_BuildableProjectCfg(out IVsBuildableProjectCfg ppIVsBuildableProjectCfg)
        {
            return InnerDebuggableProjCfg.get_BuildableProjectCfg(out ppIVsBuildableProjectCfg);
        }

        public int get_CanonicalName(out string pbstrCanonicalName)
        {
            return InnerDebuggableProjCfg.get_CanonicalName(out pbstrCanonicalName);
        }

        public int get_DisplayName(out string pbstrDisplayName)
        {
            return InnerDebuggableProjCfg.get_DisplayName(out pbstrDisplayName);
        }

        public int get_IsDebugOnly(out int pfIsDebugOnly)
        {
            return InnerDebuggableProjCfg.get_IsDebugOnly(out pfIsDebugOnly);
        }

        public int get_IsPackaged(out int pfIsPackaged)
        {
            return InnerDebuggableProjCfg.get_IsPackaged(out pfIsPackaged);
        }

        public int get_IsReleaseOnly(out int pfIsReleaseOnly)
        {
            return InnerDebuggableProjCfg.get_IsReleaseOnly(out pfIsReleaseOnly);
        }

        public int get_IsSpecifyingOutputSupported(out int pfIsSpecifyingOutputSupported)
        {
            return InnerDebuggableProjCfg.get_IsSpecifyingOutputSupported(out pfIsSpecifyingOutputSupported);
        }

        public int get_Platform(out Guid pguidPlatform)
        {
            return InnerDebuggableProjCfg.get_Platform(out pguidPlatform);
        }

        public int get_ProjectCfgProvider(out IVsProjectCfgProvider ppIVsProjectCfgProvider)
        {
            return InnerDebuggableProjCfg.get_ProjectCfgProvider(out ppIVsProjectCfgProvider);
        }

        public int get_RootURL(out string pbstrRootURL)
        {
            return InnerDebuggableProjCfg.get_RootURL(out pbstrRootURL);
        }

        public int get_TargetCodePage(out uint puiTargetCodePage)
        {
            return InnerDebuggableProjCfg.get_TargetCodePage(out puiTargetCodePage);
        }

        public int get_UpdateSequenceNumber(ULARGE_INTEGER[] puliUSN)
        {
            return InnerDebuggableProjCfg.get_UpdateSequenceNumber(puliUSN);
        }

        public int OnBeforeDebugLaunch(uint grfLaunch)
        {
            return InnerDebuggableProjCfg.OnBeforeDebugLaunch(grfLaunch);
        }

        #endregion
    }

}
