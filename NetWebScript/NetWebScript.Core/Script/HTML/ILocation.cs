using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script.HTML
{
    [Imported]
    public interface ILocation
    {
        void Replace(string url);

        [IntrinsicProperty]
        string Hash { get; set; }

        [IntrinsicProperty]
        string Hostname { get; }

        [IntrinsicProperty]
        string Href { get; set; }

        [IntrinsicProperty]
        string Pathname { get; }

        [IntrinsicProperty]
        string Port {  get; }

        [IntrinsicProperty]
        string Protocol { get; }

        [IntrinsicProperty]
        string Search { get; }
    }
}
