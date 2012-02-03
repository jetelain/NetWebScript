using NetWebScript.Equivalents.Globalization;

namespace NetWebScript.Globalization
{
    /// <summary>
    /// Globalization runtime helper class.
    /// </summary>
    [ScriptAvailable]
    [Exported(IgnoreNamespace=true)]
    public static class Culture
    {
        /// <summary>
        /// Set the current culture by name. You should usually call this method at the beginning of your HTML integrated script.
        /// </summary>
        /// <param name="cultureName">Culture name</param>
        public static void SetCurrent(string cultureName)
        {
            var culture = CultureInfoEquiv.GetCulture(cultureName);
            CultureInfoEquiv.CurrentCulture = culture;
        }

        /// <summary>
        /// Infrastructure method. You should never call this method by yourself.<br />
        /// Register a culture into runtime. 
        /// </summary>
        /// <param name="culture">Culture to register</param>
        public static void Register(CultureInfoEquiv culture)
        {
            CultureInfoEquiv.AddCulture(culture);
        }
    }
}
