using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script
{
    [ScriptAvailable]
    internal sealed class JQueryEnumerator : IEnumerator<JQuery>
    {
        private readonly JQuery jquery;
        private int index = -1;

        internal JQueryEnumerator(JQuery jquery)
        {
            this.jquery = jquery;
        }

        public JQuery Current
        {
            get { return JQuery.Query(jquery.Get(index)); }
        }

        public void Dispose()
        {

        }

        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }

        public bool MoveNext()
        {
            index++;
            return index < jquery.Length;
        }

        public void Reset()
        {
            index = -1;
        }
    }

    public sealed partial class JQuery : IEnumerable<JQuery>
    {
        [ImportedExtension]
        public IEnumerator<JQuery> GetEnumerator()
        {
            return new JQueryEnumerator(this);
        }

        [ImportedExtension]
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
