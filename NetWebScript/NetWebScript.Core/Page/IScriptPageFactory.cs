using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NetWebScript.Page
{
    public interface IScriptPageFactory
    {

        void Prepare(IEnumerable<Assembly> targetedAssemblies, IScriptDependencies dependencies);

        IScriptPage CreatePage();
    }
}
