
using System;
using System.Collections.Generic;
namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public abstract class Sequence
    {
        public virtual int LowestStack
        {
            get { throw new NotSupportedException(); }
        }

        public abstract IEnumerable<Sequence> Children
        {
            get;
        }
    }
}
