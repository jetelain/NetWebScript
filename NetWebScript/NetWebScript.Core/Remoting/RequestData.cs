using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;

namespace NetWebScript.Remoting
{
    [AnonymousObject]
    internal class RequestData
    {
        public string Type;
        public string Method;
        public object[] Parameters;
    }
}
