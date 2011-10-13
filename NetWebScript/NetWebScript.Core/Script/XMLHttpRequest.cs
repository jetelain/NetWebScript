using System;

namespace NetWebScript.Script
{
    [Imported]
    public sealed class XMLHttpRequest
    {
        internal XMLHttpRequest()
        {

        }

        [IntrinsicProperty]
        public String ResponseText
        {
            get { throw new NotImplementedException(); }
        }

        [IntrinsicProperty]
        public string Status
        {
            get { throw new NotImplementedException(); }
        }
    }
}
