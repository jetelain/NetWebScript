using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSProgram
    {
        ICollection<IJSThread> Threads { get; }

        int Id { get; }

        Uri Uri { get; }

        IList<ModuleInfo> Modules { get; }

        void AddBreakPoint(String uid);

        void RemoveBreakPoint(String uid);

        void DetachAll();

        void RegisterCallback ( IJSProgramCallback callback );

        void UnRegisterCallback(IJSProgramCallback callback);
    }
}
