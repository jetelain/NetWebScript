
namespace NetWebScript.Script.HTML
{
    [Imported]
    public interface IHTMLAttributeCollection
    {
        IHTMLAttribute GetNamedItem(string name);

        IHTMLAttribute RemoveNamedItem(string name);

        IHTMLAttribute SetNamedItem(IHTMLAttribute attribute);

        [IntrinsicProperty]
        int Length { get; }
    }
}

