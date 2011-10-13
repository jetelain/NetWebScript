using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;
using NetWebScript.Script;

namespace NetWebScript.Test.Client
{
    /*Channel: function(m) {
        var q = [];
        var mode = m || 'nowait';
        this.ModeNoWait = function() {
            mode = 'nowait';
        };
        this.Send = function(cmd, data) {
            var success = true;
            var xhr = $.ajax({
                url: TestRunner.Url,
                data: { p: TestRunner.Pid, cmd: cmd, data: data || '', mode: mode },
                dataType: 'text',
                timeout: 2500,
                async: false,
                cache: false,
                error: function() { success = false }
            });
            if (success) this.Push(xhr.responseText);
            return success;
        };
        this.Push = function(result) {
            if (result && result != 'nop' && result != 'wait') {
                var i = result.indexOf(':');
                if (i == -1) {
                    q.push({ cmd: result, data: null });
                }
                else {
                    q.push({ cmd: result.substr(0, i), data: result.substr(i + 1) });
                }
            }
        };
        this.Get = function() {
            if (q.length > 0) {
                return q.shift();
            }
            if (mode == 'wait') {
                var z = 0;
                while (q.length == 0 && this.Send('ping'));
                if (q.length > 0) {
                    return q.shift();
                }
            }
            return null;
        };
    }*/

    [ScriptAvailable]
    public class Message
    {
        public String Cmd;
        public String Data;
    }

    [AnonymousObject]
    public class HttpMessage
    {
        public String P;
        public String Cmd;
        public String Data;
        public String Mode;
    }

    [ScriptAvailable]
    public class Channel
    {
        private readonly Queue<Message> queue = new Queue<Message>();
        private readonly String url;
        private readonly String p;
        private bool waitMode = false;

        public bool Send(String cmd, String data)
        {
            bool success = true;
            XMLHttpRequest request = JQuery.Ajax(new JQueryAjax()
            {
                Async = false,
                Cache = false,
                Url = url,
                Error = delegate() { success = false; },
                Timeout = 2500,
                DataType = "text",
                Data = new HttpMessage() { P = p, Cmd = cmd, Data = data, Mode = waitMode ? "wait" : "nowait" }
            });
            if (success)
            {
                Handle(request.ResponseText);
            }
            return success;
        }

        private void Handle(String result) 
        {
            if (!String.IsNullOrEmpty(result) && result != "nop" && result != "wait") 
            {
                var i = result.IndexOf(':');
                if (i == -1) 
                {
                    queue.Enqueue(new Message(){ Cmd = result });
                }
                else {
                    queue.Enqueue(new Message(){ Cmd = result.Substring(0, i), Data = result.Substring(i + 1) });
                }
            }
        }

        private Message Get()
        {
            if (queue.Count > 0)
            {
                return queue.Dequeue();
            }
            if (waitMode)
            {
                while (queue.Count == 0 )
                {
                    if (!Send("ping", String.Empty))
                    {
                        return null;
                    }
                }
                return queue.Dequeue();
            }
            return null;
        }

    }
}
