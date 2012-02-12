using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using NetWebScript.Metadata;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Remoting
{
    public class PortBase
    {
        private readonly SerializerCache cache;

        public PortBase ( MetadataProvider metdataProvider )
        {
            cache = new SerializerCache(metdataProvider);
        }

        private ResponseData Invoke(RequestData request)
        {
            var scriptType = cache.MetadataProvider.GetTypeMetadataByScriptName(request.Type);

            var scriptMethod = scriptType.Methods.First(m => m.Name == request.Method);

            var type = CRefToolkit.ResolveType(scriptType.CRef); 
            if (type == null || !Attribute.IsDefined(type, typeof(ServerSideAttribute)))
            {
                // Type must be marked with ServerSideAttribute, otherwise, rise an exception
                throw new RemotingSecurityException("Targeted type is not accessible.");
            }

            var method = (MethodInfo)CRefToolkit.ResolveMethod(scriptMethod.CRef);
            if (method == null || method.IsPrivate || method.IsFamily )
            {
                // Method must be public or internal, otherwise, rise an exception
                throw new RemotingSecurityException("Targeted method is not accessible.");
            }

            var response = new ResponseData();
            try
            {
                var def = method.GetParameters();
                for(int i=0;i<request.Parameters.Length;++i)
                {
                    request.Parameters[i] = cache.Converter.Ensure(request.Parameters[i], def[i].ParameterType);
                }
                response.Result = method.Invoke(Activator.CreateInstance(type), request.Parameters);
            }
            catch (Exception e)
            {
                response.Exception = e;
            }
            return response;
        }

        public void Process(TextReader reader, TextWriter writer)
        {
            ResponseData response;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                RequestData request = new XmlDeserializer(cache).Deserialize<RequestData>((XmlElement)doc.DocumentElement.SelectSingleNode("*"));
                response = Invoke(request);
            }
            catch (RemotingSecurityException)
            {
                throw;
            }
            catch (Exception e)
            {
                response = new ResponseData() { Exception = e };
            }
            new EvaluableSerializer(cache).Serialize(writer, response);
        }


    }
}
