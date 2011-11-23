var Debugger = {
    Url: 'http://localhost:9090/',
    Tid: null,
    BP: {},
    Step: 0,

    Break: function (cmd, id) {
        var c = new Debugger.Channel('wait');
        c.SendPost(cmd, id, Debugger.DumpStack());
        while (true) {
            var msg = c.Get();
            if (!msg) {
                Debugger.Detach();
                return;
            }
            if (msg.cmd == 'continue') {
                Debugger.Step = 0;
                break;
            }
            else if (msg.cmd == 'step') {
                var mode = msg.data;
                if (mode == 'up') {
                    Debugger.Step = Debugger.Stack.length - 1;
                }
                else if (mode == 'dw') {
                    Debugger.Step = Debugger.Stack.length + 1;
                }
                else {
                    Debugger.Step = Debugger.Stack.length;
                }
                break;
            }
            else if (msg.cmd == 'detach') {
                Debugger.Detach();
                return;
            }
            else {
                Debugger.Process(c, msg.cmd, msg.data);

            }
        }
        c.ModeNoWait();
        c.Send('continueACK');
        Debugger.ProcessAll(c);
    },

    Start: function () {
        $.support.cors = true;
        var success = true;
        var xhr = $.ajax({
            type: 'POST',
            url: Debugger.Url + "?cmd=start",
            data: Debugger.Identity(),
            dataType: 'text',
            timeout: 500,
            async: false,
            cache: false,
            error: function () { success = false }
        });
        if (success) {
            var result = xhr.responseText;
            var i = result.indexOf(':');
            Debugger.Tid = result.substr(0, i);

            var c = new Debugger.Channel();
            Debugger.Process(c, 'listbp', result.substr(i + 1));
            Debugger.ProcessAll(c);

            var timer = setInterval(Debugger.QueryStatus, 5000);
            $(window).unload(function () {
                clearInterval(timer);
                var c = new Debugger.Channel();
                c.Send('stop');
            });
        }
        else {
            alert('Debugger unavailable Status=' + xhr.status);
        }
    },

    Detach: function () {
        Debugger.Step = 0;
        Debugger.BP = {};
    },

    QueryStatus: function () {
        $.ajax({
            url: Debugger.Url,
            data: { t: Debugger.Tid, cmd: 'status' },
            dataType: 'text',
            async: true,
            cache: false,
            success: Debugger.Status
        });
    },

    Status: function (hasp) {
        if (hasp == 'true') {
            var c = new Debugger.Channel();
            c.Send('ping');
            Debugger.ProcessAll(c);
        }
    },


    ProcessAll: function (c) {
        var msg = null;
        while (msg = c.Get()) {
            Debugger.Process(c, msg.cmd, msg.data);
        }
    },

    Process: function (c, cmd, data) {
        if (cmd == 'addbp') {
            Debugger.BP[data] = 'full';
            c.Send('bpACK', data);
        }
        else if (cmd == 'rmbp') {
            delete Debugger.BP[data];
            c.Send('rmbpACK', data);
        }
        else if (cmd == 'listbp') {
            var ids = data.split(',');
            for (var i = 0; i < ids.length; ++i) {
                Debugger.BP[ids[i]] = 'full';
            }
            c.Send('bpACK', data);
        }
        else if (cmd == 'detach') {
            Debugger.Detach();
        }
        else if (cmd == 'retreive') {
            var result;
            try {
                result = eval(data);
            }
            catch (e) {
                result = e.message;
            }
            c.SendPost('result', data, Debugger.DumpResult(result));
        }
    },

    Channel: function (m) {
        var q = [];
        var mode = m || 'nowait';
        this.ModeNoWait = function () {
            mode = 'nowait';
        };
        this.Send = function (cmd, data) {
            var success = true;
            var xhr = $.ajax({
                url: Debugger.Url,
                data: { t: Debugger.Tid, cmd: cmd, data: data || '', mode: mode },
                dataType: 'text',
                timeout: 2500,
                async: false,
                cache: false,
                error: function () { success = false }
            });
            if (success) this.Push(xhr.responseText);
            return success;
        };
        this.SendPost = function (cmd, data, postData) {
            var success = true;
            var xhr = $.ajax({
                type: 'POST',
                url: Debugger.Url + "?" + $.param({ t: Debugger.Tid, cmd: cmd, data: data || '', mode: mode }),
                data: postData,
                dataType: 'text',
                timeout: 2500,
                async: false,
                cache: false,
                error: function () { success = false }
            });
            if (success) this.Push(xhr.responseText);
            return success;
        };
        this.Push = function (result) {
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
        this.Get = function () {
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
    },

    Stack: [],

    Enter: function (name, data) {
        Debugger.Stack.push({ Id: Debugger.Stack.length, Name: name, Data: data });
    },

    Leave: function (v) {
        Debugger.Stack.pop();
        return v;
    },

    DumpStack: function () {
        var doc = NWS.CreateDocument('StackFrame');
        for (var i = 0; i < Debugger.Stack.length; ++i) {
            var frame = Debugger.Stack[i];
            var element = doc.createElement('Frame');
            element.setAttribute("Id", frame.Id);
            element.setAttribute("Name", frame.Name);
            element.setAttribute("Point", frame.Point);
            if (i == Debugger.Stack.length - 1) {
                element.appendChild(Debugger.DumpObject(frame.Data, doc, 0));
            }
            doc.documentElement.appendChild(element);
        }
        return NWS.ToXml(doc);
    },
    DumpResult: function (obj) {
        var doc = NWS.CreateDocument('Dump');
        doc.documentElement.appendChild(Debugger.DumpObject(obj, doc, 0));
        return NWS.ToXml(doc);
    },
    TypeNameOf: function (obj) {
        if (obj !== null && obj.constructor && obj.constructor.$n) {
            return obj.constructor.$n;
        }
        return typeof obj;
    },
    SafeToString: function (obj) {
        if (obj === undefined) return '(undefined)';
        if (obj === null) return '(null)';
        var value;
        try {
            value = obj.toString();
        }
        catch (e) {
            value = '(exception catched)';
        }
        return value;
    },
    DumpObject: function (obj, doc, depth) {
        var node = doc.createElement("P");
        node.setAttribute("Value", Debugger.SafeToString(obj));
        node.setAttribute("Type", Debugger.TypeNameOf(obj));
        if (typeof obj === "object") {
            for (var key in obj) {
                var value = obj[key];
                if (key == '$this') {
                    var value = Debugger.DumpObject(value, doc, 0);
                    value.setAttribute("Name", "this");
                    node.appendChild(value);
                }
                else if (!$.isFunction(value)) {
                    var vnode;
                    if (typeof value === "object") {
                        if (depth < 2) {
                            vnode = Debugger.DumpObject(value, doc, depth + 1);
                        }
                        else {
                            vnode = doc.createElement("P")
                            vnode.setAttribute("Value", Debugger.SafeToString(value));
                            vnode.setAttribute("Retreive", "true");
                            vnode.setAttribute("Type", Debugger.TypeNameOf(value));
                        }
                    }
                    else {
                        vnode = doc.createElement("P")
                        vnode.setAttribute("Value", Debugger.SafeToString(value));
                        vnode.setAttribute("Type", typeof value);
                    }
                    vnode.setAttribute("Name", key);
                    node.appendChild(vnode);
                }
            }
        }
        return node;
    },

    Identity: function () {
        var doc = NWS.CreateDocument('Identity');
        doc.documentElement.setAttribute("Url", document.location.href);
        for (var i = 0; i < NWS.$Modules.length; ++i) {
            var mod = NWS.$Modules[i];
            var element = doc.createElement('Module');
            element.setAttribute("Name", mod.Name);
            element.setAttribute("Version", mod.Version);
            element.setAttribute("Filename", mod.Filename);
            doc.documentElement.appendChild(element);
        }
        return NWS.ToXml(doc);
    }



};

var $dbgStart = Debugger.Start;
var $dbgE = Debugger.Enter;
var $dbgL = Debugger.Leave;
var $dbgP = function(id) {
	if ( Debugger.Stack.length > 0) {
		Debugger.Stack[Debugger.Stack.length-1].Point = id;
	}
    if ( Debugger.Step>0 && Debugger.Step >= Debugger.Stack.length ) {
        Debugger.Break('steped', id);
    }
    else if (Debugger.BP[id]) {
        Debugger.Break('reached', id);
    }
    return true;
}


