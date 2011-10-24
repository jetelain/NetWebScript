var NWS = {};

// Object.GetHashCode()
//Object.prototype.$ghc = (function () { var c = 0; return function () { if (this.$hc!==undefined) return this.$hc; return this.$hc=++c; }; });
//Object.prototype.$eq = function (o) { return o == this; };

var $dbgP = function(id, data) { return true; };
var $dbgE = function(name, data) { };
var $dbgL = function(v) { return v; };

NWS.CreateDocument = function(name) {
    if (window.ActiveXObject) {
        var doc = new ActiveXObject("Microsoft.XMLDOM");
        var root = doc.createElement(name);
        doc.appendChild(root);
        return doc;
    }
    else {
        return document.implementation.createDocument('', name, null);
    }
}

NWS.ToXml = function(doc) {
    if (typeof XMLSerializer !== 'undefined') {
        return (new XMLSerializer()).serializeToString(doc);
    }
    return doc.xml;
}

NWS.$Modules = [];

NWS.$RegMod = function(name, version, filename) {
    var module = { Name: name, Version: version, Filename: filename };
    NWS.$Modules.push(module);
    return module;
}


