using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using NetWebScript.JsClr.TypeSystem.Invoker;

namespace NetWebScript.JsClr.TypeSystem.Native
{
    public class NativeMethod : IScriptMethod
    {
        private readonly IScriptType owner;
        private readonly MethodInfo method;
        private readonly string slotId;
        private readonly string implId;

        public NativeMethod(IScriptType owner, MethodInfo method, string name)
        {
            this.owner = owner;
            this.method = method;
            if (method.IsVirtual)
            {
                slotId = name;
                implId = name;
            }
            else
            {
                implId = name;
            }
        }


        #region IScriptMethod Members

        public string SlodId
        {
            get { return slotId; }
        }

        #endregion

        #region IScriptMethodBase Members

        public string ImplId
        {
            get { return implId; }
        }

        public MethodBase Method
        {
            get { return method; }
        }

        public IScriptType Owner
        {
            get { return owner; }
        }

        public IMethodInvoker Invoker
        {
            get { return StandardInvoker.Instance; }
        }

        #endregion
    }
}
