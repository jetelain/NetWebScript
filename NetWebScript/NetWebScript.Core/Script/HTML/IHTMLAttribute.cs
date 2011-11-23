
namespace NetWebScript.Script.HTML
{
    [Imported]
    public interface IHTMLAttribute
    {
        [IntrinsicProperty]
        string Name { get; }

        [IntrinsicProperty]
        bool Specified { get; }

        [IntrinsicProperty]
        string Value { get; set; }
    }
}

