var TestRunner = {
    Url: '%%URL%%',
    Pid: navigator.appName + ' ' + $.browser.version,

    Start: function() {
        var c = new TestRunner.Channel();
        c.Send('ready');
        TestRunner.ProcessAll(c);
        var timer = setInterval(TestRunner.Update, 1000);
        $(window).unload(function() {
            clearInterval(timer);
            var c = new TestRunner.Channel();
            c.Send('stop');
        });
    },

    Update: function() {
        var c = new TestRunner.Channel('wait');
        c.Send('ping');
        c.ModeNoWait();
        TestRunner.ProcessAll(c);
    },
    
    ProcessAll: function(c) {
        var msg = null;
        while (msg = c.Get()) {
            TestRunner.Process(c, msg.cmd, msg.data);
        }
    },

    Process: function(c, cmd, data) {
        if (cmd == 'run') {

            var j = data.indexOf(':');
            var i = data.lastIndexOf('.', j);
            var cl = data.substring(0, i);
            var method = data.substring(i + 1, j);
            var script = data.substring(j + 1);

            var start = new Date();
            var result = 'OK';
            var message = '';
            var duration = 0;

            try {
                $.globalEval(script);
                var clref = eval(cl);
                (new clref())[method]();
            }
            catch (err) {
                result = 'KO';
                message = message + ' ' + err.name + ' ' + err.message;
            }
            duration = new Date() - start;
            c.Send('result', result + ':' + duration + ':' + message);
        }
    },

    Channel: function(m) {
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
    }
};
