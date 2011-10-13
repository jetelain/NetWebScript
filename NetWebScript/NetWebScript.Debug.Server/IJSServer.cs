using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSServer
    {
        ICollection<IJSProgram> Programs { get; }

        void RegisterCallback(IJSServerCallback callback);

        void UnRegisterCallback(IJSServerCallback callback);
    }
}
