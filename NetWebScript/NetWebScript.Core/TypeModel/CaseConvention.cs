namespace NetWebScript
{
    /// <summary>
    /// Naming convention used by imported type
    /// </summary>
    public enum CaseConvention : byte
    {
        /// <summary>
        /// C# convention : names in JavaScript are strictly the same
        /// </summary>
        CSharpConvention,

        /// <summary>
        /// JavaScript convention : members names in JavaScript have first letter lowsercase
        /// </summary>
        JavaScriptConvention
    }

    public static class CaseToolkit
    {
        public static string GetMemberName(CaseConvention convention, string name)
        {
            if (convention == CaseConvention.JavaScriptConvention)
            {
                return name.Substring(0, 1).ToLowerInvariant() + name.Substring(1);
            }
            return name;
        }

    }
}
