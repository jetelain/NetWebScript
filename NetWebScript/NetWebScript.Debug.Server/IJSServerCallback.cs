using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Debug.Server
{
    public interface IJSServerCallback
    {
        void OnNewProgram(IJSProgram program);
    }
}
