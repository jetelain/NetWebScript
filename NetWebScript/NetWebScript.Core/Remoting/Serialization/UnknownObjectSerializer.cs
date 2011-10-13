using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.Runtime.Serialization;
using NetWebScript.Runtime;

namespace NetWebScript.Remoting.Serialization
{
    internal class UnknownObjectSerializer : IObjectSerializer
    {
        internal static readonly UnknownObjectSerializer Instance = new UnknownObjectSerializer();
        
        private UnknownObjectSerializer()
        {
        }

        #region IObjectSerializer Members

        public void Serialize(object source, System.Runtime.Serialization.SerializationInfo target)
        {
            SerializationHelper.Serialize((JSObject)source, target);
        }

        public object CreateAndDeserialize(System.Runtime.Serialization.SerializationInfo source)
        {
            var obj = new JSObject();
            SerializationHelper.Deserialize(obj, source);
            return obj;
        }

        #endregion

        #region IObjectSerializer Members


        public void WriteScriptStart(System.IO.TextWriter writer)
        {
        }

        public void WriteScriptEnd(System.IO.TextWriter writer)
        {
        }

        #endregion

    }
}
