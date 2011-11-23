using System;
using NetWebScript;
using NetWebScript.Script;
using NetWebScript.Remoting.Serialization;

namespace NetWebScript.Remoting
{
    [ScriptAvailable]
    public static class RemoteInvoker
    {
        public static object Invoke(string type, string method, object target, object[] parameters)
        {
            var request = new RequestData();
            request.Type = type;
            request.Method = method;
            request.Parameters = parameters;
            request.Target = target;

            var ajaxRequest = new JQueryAjaxSettings();
            ajaxRequest.Type = "POST";
            ajaxRequest.Data = XmlSerializer.Serialize(request);
            ajaxRequest.Async = false;
            ajaxRequest.Cache = false;
            ajaxRequest.Url = RemotePortUrl;
            
            var xhr = JQuery.Ajax(ajaxRequest);

            var response = (ResponseData)Global.Eval(xhr.ResponseText);
            if (response.Target != null)
            {
                XmlSerializer.CopyTo(target, response.Target);
            }
            if (response.Exception != null)
            {
                throw response.Exception;
            }
            return response.Result;
        }

        /// <summary>
        /// Url to the remoting connector
        /// </summary>
        public static string RemotePortUrl;
    }
}
