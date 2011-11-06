var NWS = {};

var $dbgP = function(id, data) { return true; };
var $dbgE = function(name, data) { };
var $dbgL = function(v) { return v; };

NWS.CreateDocument = function (name) {
    if (document.implementation && document.implementation.createDocument) {
        return document.implementation.createDocument('', name, null);
    }
    else if (window.ActiveXObject) {
        var doc = new ActiveXObject("Microsoft.XMLDOM");
        var root = doc.createElement(name);
        doc.appendChild(root);
        return doc;
    }
    else {
        throw new Error('Unsupported');
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


