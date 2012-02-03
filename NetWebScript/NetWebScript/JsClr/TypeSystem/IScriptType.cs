using System;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;
using NetWebScript.Metadata;
using NetWebScript.JsClr.ScriptWriter.Declaration;

namespace NetWebScript.JsClr.TypeSystem
{
    public interface IScriptType
    {
        IScriptConstructor GetScriptConstructor(ConstructorInfo method);

        IScriptMethod GetScriptMethod(MethodInfo method);

        IScriptMethod GetScriptMethodIfUsed(MethodInfo method);

        IScriptField GetScriptField(FieldInfo field);

        Type Type { get; }

        string TypeId { get; }

        ITypeBoxing Boxing { get; }

        IValueSerializer Serializer { get; }

        /// <summary>
        /// Runtime can determines if an object is of type. <br :>
        /// Usually it mean that <see cref="TypeId"/> is enough to determines if an an object is an instance of <see cref="Type"/>. <br />
        /// This requires : <br />
        /// - Prototype of intances of <see cref="Type"/> is <see cref="TypeId"/>. <br />
        /// - <see cref="TypeId"/> is prototype of only of instance of <see cref="Type"/>.<br />
        /// </summary>
        bool HaveCastInformation { get; }

        /// <summary>
        /// Default constructor of type if any (may be null).
        /// </summary>
        IScriptConstructor DefaultConstructor { get; }

        /// <summary>
        /// Optional runtime metadata
        /// </summary>
        TypeMetadata Metadata { get; }

        void RegisterChildType(IScriptType type);

        IScriptType BaseType { get; }
    }
}
