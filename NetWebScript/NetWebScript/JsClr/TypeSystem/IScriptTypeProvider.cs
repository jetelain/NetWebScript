using System;
using System.Reflection;

namespace NetWebScript.JsClr.TypeSystem
{
    interface IScriptTypeProvider
    {
        /// <summary>
        /// Try to  create a <see cref="IScriptType" /> corresponding to the provided <see cref="Type" />.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="scriptType">Corresponding script type</param>
        /// <returns>true if provider has a correspondint script type, this implies that <paramref name="scriptType"/> is not null, false otherwise.</returns>
        bool TryCreate(Type type, out IScriptType scriptType);

        /// <summary>
        /// Notify that an other assembly should be considered in script type resolution.
        /// </summary>
        /// <param name="assembly"></param>
        void RegisterAssembly(Assembly assembly);
    }
}
