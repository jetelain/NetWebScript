using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSProgramCallback
    {

        void OnNewThread ( IJSThread thread );

        void OnNewModule(JSModuleInfo module);

        /// <summary>
        /// Called when module is updated (new code and metadata)
        /// </summary>
        /// <param name="module"></param>
        void OnModuleUpdate(JSModuleInfo module);
    }
}
