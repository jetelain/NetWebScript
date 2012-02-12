using System;

namespace NetWebScript.Globalization
{
    /// <summary>
    /// Specifies in wich cultures script should be generated. <br />
    /// For each culture a satellite JavaScript file will be produced with culture specific informations. <br />
    /// Only cultures listed by this attribute will be available in web browser runtime, all other cultures will not exists.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
    public sealed class CulturesAttribute : Attribute
    {
        private readonly string[] cultures;

        /// <summary>
        /// See <see cref="CulturesAttribute"/>.
        /// </summary>
        /// <param name="cultures">Cultures supported by application.</param>
        [CLSCompliant(false)]
        public CulturesAttribute(params string[] cultures)
        {
            this.cultures = cultures;
        }

        /// <summary>
        /// See <see cref="CulturesAttribute"/>.
        /// </summary>
        /// <param name="cultures">Comma sperarated cultures supported by application.</param>
        public CulturesAttribute(string cultures)
        {
            this.cultures = cultures.Split(',');
        }

        /// <summary>
        /// Array of cultures names.
        /// </summary>
        public string[] Cultures
        {
            get { return cultures; }
        }
    }
}
