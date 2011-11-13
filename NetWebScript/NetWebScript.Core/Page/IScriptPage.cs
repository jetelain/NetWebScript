using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Page
{
    [ScriptAvailable]
    public interface IScriptPage
    {
        void OnLoad();
    }
}
