﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem.Standard
{
    public class ScriptInterfaceMapping
    {
        private readonly Dictionary<string, IScriptMethod> mapping = new Dictionary<string, IScriptMethod>();

        public ScriptInterfaceMapping(ScriptSystem system, IScriptType scriptType)
        {
            Type type = scriptType.Type;
            if (!type.IsInterface)
            {
                foreach (var itf in type.GetInterfaces())
                {
                    var scriptItf = system.GetScriptType(itf);
                    if (scriptItf != null)
                    {
                        var map = type.GetInterfaceMap(itf);
                        ProcessInterface(system, map, scriptItf, scriptType);
                    }
                }
            }
            // TODO: check that interfaces slots does not create conflicts with type own slots and implementations
        }

        private void ProcessInterface(ScriptSystem system, InterfaceMapping map, IScriptType scriptItf, IScriptType scriptType)
        {
            for (int i = 0; i < map.InterfaceMethods.Length; ++i)
            {
                var interfaceMethod = map.InterfaceMethods[i];
                if (!interfaceMethod.IsGenericMethodDefinition)
                {
                    var targetMethod = map.TargetMethods[i];

                    IScriptMethod interfaceScriptMethod = scriptItf.GetScriptMethodIfUsed(interfaceMethod);
                    if (interfaceScriptMethod != null)
                    {
                        IScriptMethod targetScriptMethod;
                        if ( targetMethod.DeclaringType == scriptType.Type)
                        {
                            targetScriptMethod = scriptType.GetScriptMethod(targetMethod);
                        }
                        else
                        {
                            targetScriptMethod = system.GetScriptMethod(targetMethod);
                        }
                        
                        if (targetScriptMethod == null)
                        {
                            throw new Exception(string.Format("'{0}' cannot implements interface '{1}' : method '{2}' is not script available.", scriptType.Type.FullName, scriptItf.Type.FullName, targetMethod.ToString()));
                        }
                        if (targetScriptMethod.ImplId == null)
                        {
                            throw new Exception(string.Format("'{0}' cannot implements interface '{1}' : method '{2}' is script inlined or script sealed and thus cannot implemented.", scriptType.Type.FullName, scriptItf.Type.FullName, targetMethod.ToString()));
                        }

                        IScriptMethod potentialConflict;
                        if (mapping.TryGetValue(interfaceScriptMethod.SlodId, out potentialConflict))
                        {
                            if (potentialConflict != targetScriptMethod)
                            {
                                // TODO: determine conflict interface
                                throw new Exception();
                            }
                        }
                        else
                        {
                            mapping.Add(interfaceScriptMethod.SlodId, targetScriptMethod);
                        }

                    }
                }
            }
        }

        public IDictionary<string, IScriptMethod> Mapping
        {
            get { return mapping; }
        }

        public IEnumerable<ScriptSlotImplementation> InterfacesMapping
        {
            get
            {
                foreach (var pair in mapping)
                {
                    yield return new ScriptSlotImplementation()
                    {
                        SlotId = pair.Key,
                        ImplId = pair.Value.ImplId
                    };
                }
            }
        }

    }
}
