using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace NetWebScript.Debug.Launcher
{
    [Guid("C3B2BB31-36EC-4967-BDC5-2DA982DE928D")]
    //[PackageRegistration(UseManagedResourcesOnly = true, RegisterUsing = RegistrationMethod.CodeBase)]
    [DefaultRegistryRoot(@"Software\Microsoft\VisualStudio\10.0")]
    public class NwsFlavorPackage : Package
    {
        protected override void Initialize() 
        { 
            base.Initialize(); 
            RegisterProjectFactory(new NwsFlavorProjectFactory(this)); 
        }
    }
}
