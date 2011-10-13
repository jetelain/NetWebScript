using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Debug
{
    class TrivialExpression : IDebugExpression2
    {
        private readonly Property property;

        public TrivialExpression(Property property)
        {
            this.property = property;
        }

        #region IDebugExpression2 Members

        public int Abort()
        {
            throw new NotImplementedException();
        }

        public int EvaluateAsync(enum_EVALFLAGS dwFlags, IDebugEventCallback2 pExprCallback)
        {
            throw new NotImplementedException();
        }

        public int EvaluateSync(enum_EVALFLAGS dwFlags, uint dwTimeout, IDebugEventCallback2 pExprCallback, out IDebugProperty2 ppResult)
        {
            ppResult = property;
            return Constants.S_OK;
        }

        #endregion
    }
}
