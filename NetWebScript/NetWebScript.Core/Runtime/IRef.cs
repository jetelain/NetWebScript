using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Runtime
{
    [ScriptAvailable]
    public interface IRef
    {
        object Set(object value);

        object Get();
    }
}
