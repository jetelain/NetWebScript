using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.Page
{
    public interface IScriptDependencies
    {
        void AddType(Type info);
    }
}
