using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NetWebScript.Metadata;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Remoting
{
    public abstract class PortBase
    {
        private readonly SerializerCache cache;

        private ResponseData Invoke(RequestData request)
        {
            var scriptType = cache.GetTypeMetadataByScriptName(request.Type);
            var scriptMethod = scriptType.Methods.First(m => m.Name == request.Method);
            var method = (MethodInfo)CRefToolkit.Resolve(scriptMethod.CRef);
            var response = new ResponseData();
            try
            {
                var def = method.GetParameters();
                for(int i=0;i<request.Parameters.Length;++i)
                {
                    request.Parameters[i] = cache.Converter.Ensure(request.Parameters[i], def[i].ParameterType);
                }
                response.Result = method.Invoke(request.Target, request.Parameters);
            }
            catch (Exception e)
            {
                response.Exception = e;
            }
            if ( request.Target != null )
            {
                // Serialize data to update target instance members
                var targetType = request.Target.GetType();
                var serializer = cache.GetSerializer(targetType);
                if ( serializer != null )
                {
                    response.Target = request.Target;
                }
            }
            return response;
        }

        protected void Process(TextReader reader, TextWriter writer)
        {
            ResponseData response;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                RequestData request = new XmlDeserializer(cache).Deserialize<RequestData>((XmlElement)doc.DocumentElement.SelectSingleNode("*"));
                response = Invoke(request);
            }
            catch (Exception e)
            {
                response = new ResponseData() { Exception = e };
            }
            new EvaluableSerializer(cache).Serialize(writer, response);
        }


    }
}
