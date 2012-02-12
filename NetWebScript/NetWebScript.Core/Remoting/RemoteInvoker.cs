using System;
using NetWebScript;
using NetWebScript.Script;
using NetWebScript.Remoting.Serialization;
using NetWebScript.Script.Xml;

namespace NetWebScript.Remoting
{
    [ScriptAvailable]
    public sealed class RemoteInvoker : ScriptRealProxy
    {
        public RemoteInvoker()
        {
        }

        public override object Invoke(string typeId, string methodId, object[] parameters)
        {
            var request = new RequestData();
            request.Type = typeId;
            request.Method = methodId;
            request.Parameters = parameters;

            var ajaxRequest = new JQueryAjaxSettings();
            ajaxRequest.Type = "POST";
            ajaxRequest.Data = XmlToolkit.ToXml(XmlSerializer.Serialize(request));
            ajaxRequest.Async = false;
            ajaxRequest.Cache = false;
            ajaxRequest.Url = RemotePortUrl;
            
            var xhr = JQuery.Ajax(ajaxRequest);

            var response = (ResponseData)Global.Eval("("+xhr.ResponseText+")");
            if (response.Exception != null)
            {
                throw response.Exception;
            }
            return response.Result;
        }

        /// <summary>
        /// Url to the remoting connector
        /// </summary>
        public static string RemotePortUrl = "nws.ashx";

    }
}
