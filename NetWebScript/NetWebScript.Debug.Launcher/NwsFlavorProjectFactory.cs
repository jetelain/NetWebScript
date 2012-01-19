using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell.Flavor;

namespace NetWebScript.Debug.Launcher
{
    [Guid("05A87499-F343-4FCA-BCD0-B90EF6FC4558")]
    public class NwsFlavorProjectFactory : FlavoredProjectFactoryBase
    {
        private readonly NwsFlavorPackage package;

        public NwsFlavorProjectFactory(NwsFlavorPackage package)
        {
            this.package = package;
        }

        protected override object PreCreateForOuter(IntPtr outerProjectIUnknown)
        {
            return new NwsFlavorProject(package);
        }

    }
}
