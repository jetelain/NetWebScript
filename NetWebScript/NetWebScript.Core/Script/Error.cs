using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script
{
    [IgnoreNamespace, Imported]
    public class Error
    {
        public Error()
        {
        }

        [IntrinsicProperty]
        public string Message { get; set; }

    }
}
