
namespace NetWebScript.Script.HTML
{
    [Imported]
    public interface IHTMLImplementation
    {
        bool HasFeature(string feature);

        bool HasFeature(string feature, string version);
    }
}

