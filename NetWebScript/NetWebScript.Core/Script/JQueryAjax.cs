using System;
using System.Collections.Generic;

namespace NetWebScript.Script
{
    [AnonymousObject(CaseConvention.JavaScriptConvention)]
    public sealed class JQueryAjax
    {
        public string Type;
        
        public string Url;

        public string DataType;

        public object Data;

        public int Timeout;

        public bool Async;

        public bool Cache;

        public Action Error;

        public Action<string> Success;
    }
}
