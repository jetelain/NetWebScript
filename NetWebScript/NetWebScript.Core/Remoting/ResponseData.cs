using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;

namespace NetWebScript.Remoting
{
    [AnonymousObject]
    internal class ResponseData
    {
        public Exception Exception;
        public object Result;
        public object Target;
    }
}
