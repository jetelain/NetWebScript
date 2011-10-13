NWS.$RegMod('NetWebScript.Test','0.0.0.0');

NWS = {};
NWS.UnitTest = {};
NWS.UnitTest.Data = [];
NWS.UnitTest.Add = function(obj) {
    NWS.UnitTest.Data.push(obj);
}

NWS.UnitTest.RunAll = function() {
    var body = $(document.body);
    var table = $("<table>");
    body.append(table);
    for (var i = 0; i < NWS.UnitTest.Data.length; ++i) {
        NWS.UnitTest.Run(NWS.UnitTest.Data[i], table);
    }


}

NWS.UnitTest.Run = function(obj, table) {
    var tr = $('<tr></tr>');
    tr.append($('<td></td>').attr('colspan', '3').text(obj.FullName));
    table.append(tr);

    var target = new obj.Ref();

    for (var i = 0; i < obj.Methods.length; ++i) {
        NWS.UnitTest.RunMethod(target, obj.Methods[i], table);
    }

}

NWS.UnitTest.RunMethod = function(target, name, table) {
    var tr = $('<tr></tr>');

    tr.append($('<td></td>').text(name));

    var ok = true;
    var start, time = '?';
    try {
        start = new Date();
        target[name]();
        time = new Date() - start;
    }
    catch (err) {
        time = new Date() - start;
        ok = false;
        tr.append($('<td style="color:red;">KO</td>'));
        tr.append($('<td></td>').text(err.name + ': ' + err.message));
    }

    if (ok) {
        tr.append($('<td style="color:green;">OK</td>'));
        tr.append($('<td>' + time + ' msec</td>'));
    }
    table.append(tr);
}

var AssertFailedException = function(message) { this.name = "AssertFailedException"; this.message = (message || ""); };
AssertFailedException.prototype = new Error();

var Assert =
{
    AreEqual: function(expected, effective, message) {
        if (expected != effective) {
            throw new AssertFailedException((message || "") + "Expected: '" + expected + "' Effective: '" + effective + "'");
        }
    },

    AreNotEqual: function(expected, effective, message) {
        if (expected == effective) {
            throw new AssertFailedException(message || "Expected not equal to " + expected);
        }
    },

    AreSame: function(expected, effective, message) {
        if (expected !== effective) {
            throw new AssertFailedException((message || "") + "Expected: " + expected + " Effective: " + effective);
        }
    },

    AreNotSame: function(expected, effective, message) {
        if (expected === effective) {
            throw new AssertFailedException(message || "Expected not same to " + expected);
        }
    },

    IsTrue: function(effective, message) {
        if (true !== effective) {
            throw new AssertFailedException(message || "Expected true Effective: " + effective);
        }
    },

    IsFalse: function(effective, message) {
        if (false !== effective) {
            throw new AssertFailedException(message || "Expected false Effective: " + effective);
        }
    },

    IsNull: function(effective, message) {
        if (null !== effective) {
            throw new AssertFailedException(message || "Expected null Effective: " + effective);
        }
    },

    IsNotNull: function(effective, message) {
        if (null === effective) {
            throw new AssertFailedException(message || "Expected not null");
        }
    },

    Fail: function(message) {
        throw new AssertFailedException(message || "Assert.Fail");
    }
};
