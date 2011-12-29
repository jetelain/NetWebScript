using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using NetWebScript.Equivalents.Globalization;

namespace NetWebScript.Runtime
{
    [ScriptAvailable]
    [Exported(IgnoreNamespace=true)]
    public static class Culture
    {

        public static void SetCurrent(string cultureName)
        {
            CultureInfoEquiv.CurrentCulture = CultureInfoEquiv.GetCulture(cultureName);
        }

        public static void Add(CultureInfoEquiv culture)
        {
            CultureInfoEquiv.AddCulture(culture);
        }
    }
}
