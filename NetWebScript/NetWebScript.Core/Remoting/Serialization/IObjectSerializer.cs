using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace NetWebScript.Remoting.Serialization
{
    public interface IObjectSerializer
    {
        void Serialize(object source, SerializationInfo target);

        object CreateAndDeserialize(SerializationInfo source);

        void WriteScriptStart(TextWriter writer);

        void WriteScriptEnd(TextWriter writer);
    }
}
