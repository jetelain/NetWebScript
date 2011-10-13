using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;
using NetWebScript.Remoting;
using NetWebScript.Remoting;

namespace NetWebScript.Test.Remoting
{
    [ScriptAvailable]
    public class RemoteA
    {
        [ServerSide]
        public void Remote(ClassA a, string b)
        {

        }
    }
}
