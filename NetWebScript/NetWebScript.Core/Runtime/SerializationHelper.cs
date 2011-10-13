using System.Runtime.Serialization;
using System;

namespace NetWebScript.Runtime
{
    internal static class SerializationHelper
    {
        public static void Deserialize(Script.JSObject obj, SerializationInfo info)
        {
            foreach (var pair in info)
            {
                obj.data.Add(pair.Name, pair.Value);
            }
        }

        public static void Serialize(Script.JSObject obj, SerializationInfo info)
        {
            foreach (var pair in obj.data)
            {
                info.AddValue(pair.Key, pair.Value);
            }
        }
    }
}
