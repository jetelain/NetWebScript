using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSThread
    {
        void RegisterCallback(IJSThreadCallback callback);

        void UnRegisterCallback(IJSThreadCallback callback);

        void StepOver();

        void StepOut();

        void StepInto();

        void Continue();

        JSThreadState State { get; }

        int Id { get; }

        String UId { get; }
    }
}
