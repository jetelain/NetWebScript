using System;
using NetWebScript.Remoting;

namespace NetWebScript.Test.Remoting
{
    [ServerSide]
    public class RemoteA : MarshalByRefObject
    {
        public void Remote(ClassA a, string b)
        {

        }
    }
}
