
namespace NetWebScript.Script.HTML
{
    /// <summary>
    /// List of <see cref="IHTMLElement"/>
    /// </summary>
    [Imported]
    public interface IHTMLElementCollection
    {
        /// <summary>
        /// Element
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns></returns>
        [IntrinsicProperty]
        IHTMLElement this[int index] { get; }

        /// <summary>
        /// Number of elements
        /// </summary>
        [IntrinsicProperty]
        int Length { get; }
    }
}

