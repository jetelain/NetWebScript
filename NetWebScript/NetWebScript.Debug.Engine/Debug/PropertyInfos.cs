using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Debugger.Interop;

namespace NetWebScript.Debug.Engine.Debug
{
    class PropertyInfos : BaseEnumerable<DEBUG_PROPERTY_INFO, IEnumDebugPropertyInfo2>, IEnumDebugPropertyInfo2
    {
        public PropertyInfos(DEBUG_PROPERTY_INFO[] properties)
            : base(properties)
        {

        }
    }
}
