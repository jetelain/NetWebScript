using System;
using System.Linq;
using System.Reflection;
using NetWebScript.JsClr.AstBuilder.PdbInfo;

namespace NetWebScript.JsClr.AstBuilder.Cil
{
    /// <summary>
    /// Local variable of a <see cref="MethodCil"/>.
    /// </summary>
    public sealed class LocalVariable
    {
        /// <summary>
        /// Name of the variable. Generated if unknown.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// IL index of variable
        /// </summary>
        public int LocalIndex { get; private set; }

        /// <summary>
        /// Type of variable value
        /// </summary>
        public Type LocalType { get; private set; }

        /// <summary>
        /// Allow a RefExpression on variable
        /// </summary>
        public bool AllowRef { get; internal set; }

        /// <summary>
        /// Create a "fictive" <see cref="LocalVariable"/>.
        /// </summary>
        internal LocalVariable(string name, Type localType)
        {
            LocalIndex = -1;
            Name = name;
            LocalType = localType;
        }

        /// <summary>
        /// Create a <see cref="LocalVAriable"/>
        /// </summary>
        /// <param name="pdb">Eventuel PDB informations about the variable</param>
        /// <param name="info">Variable informations</param>
        internal LocalVariable(PdbMethod pdb, LocalVariableInfo info)
        {
            this.LocalIndex = info.LocalIndex;
            this.LocalType = info.LocalType;

            if (pdb != null)
            {
                PdbLocalVariable varpdb = pdb.GetVariable(LocalIndex);
                if (varpdb != null)
                {
                    Name = varpdb.Name;
                }
            }
            if ( Name == null || Name.Contains('$') || Name.Contains('<') )
            {
                Name = String.Format("v{0}", LocalIndex);
            }
        }

    }
}
