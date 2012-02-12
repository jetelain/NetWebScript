using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Metadata;

namespace NetWebScript.JsClr.TypeSystem
{
    public static class MetadataHelper
    {
        public static TypeMetadata CreateTypeMetadata(ScriptSystem system, IScriptType type)
        {
            var meta = new TypeMetadata();
            meta.Module = system.Metadata;
            meta.Name = type.TypeId;
            meta.CRef = CRefToolkit.GetCRef(type.Type);
            if (type.BaseType != null)
            {
                meta.BaseTypeName = type.BaseType.TypeId;
            }
            system.Metadata.Types.Add(meta);
            return meta;
        }

        public static MethodBaseMetadata CreateMethodMetadata(IScriptMethodBase method)
        {
            if (method.OwnerScriptType.Metadata == null)
            {
                return null;
            }
            var meta = new MethodBaseMetadata();
            meta.Type = method.OwnerScriptType.Metadata;
            meta.Name = method.ImplId;
            meta.CRef = CRefToolkit.GetCRef(method.Method);
            method.OwnerScriptType.Metadata.Methods.Add(meta);
            return meta;
        }
    }
}
