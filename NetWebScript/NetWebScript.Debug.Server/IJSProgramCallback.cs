using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSProgramCallback
    {

        void OnNewThread ( IJSThread thread );

        void OnNewModule(ModuleInfo module);

    }
}
