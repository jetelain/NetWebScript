using System;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using NetWebScript.Metadata;
using NetWebScript.Remoting;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Web.Remoting
{
    /// <summary>
    /// Handles NetWebScript remoting requests (from <see cref="RemoteInovker"/>).
    /// </summary>
    /// <example>
    /// To allow NetWebScript remoting in a web site, first add a file name "nws.ashx" with the following content :
    /// <code>
    /// &lt;%@ WebHandler Language="C#" Class="NetWebScript.Web.Remoting.RemoteInvokerHandler" %&gt;
    /// </code>
    /// In the "web.config" file, add the following section:
    /// <code>
    /// &lt;appSettings%&gt;
    ///   &lt;add key="netwebscript.metadatafile" value="~/[name of project].js.xml"/%&gt;
    /// &lt;/appSettings%&gt;
    /// </code>
    /// </example>
    public sealed class RemoteInvokerHandler : IHttpHandler
    {
        private readonly PortBase port;

        /// <summary>
        /// Creates an instance of <see cref="RemoteInvokerHandler" />.
        /// </summary>
        public RemoteInvokerHandler()
        {
            var appSettings = WebConfigurationManager.AppSettings;
            if (appSettings.Count == 0)
            {
                throw new Exception("appSettings section missing");
            }
            var customSetting = appSettings["netwebscript.metadatafile"];
            if (customSetting == null)
            {
                throw new Exception("add key='netwebscript.metadatafile' entry missing");
            }
            var physicalFile = HostingEnvironment.MapPath((string)customSetting);
            ModuleMetadata metadata;
            using (var reader = File.OpenText(physicalFile))
            {
                metadata = ModuleMetadataSerializer.Read(reader);
            }
            port = new PortBase(new MetadataProvider(metadata));
        }

        /// <inheritdoc />
        public bool IsReusable
        {
            get { return true; }
        }

        /// <inheritdoc />
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.HttpMethod != "POST")
            {
                throw new HttpException(400, "Invalid request");
            }
            try
            {
                using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                {
                    using (var writer = context.Response.Output)
                    {
                        port.Process(reader, writer);
                    }
                }
            }
            catch (RemotingSecurityException rsc)
            {
                // Report RemotingSecurityException as 403 errors
                throw new HttpException(403, rsc.Message, rsc);
            }
        }
    }
}
