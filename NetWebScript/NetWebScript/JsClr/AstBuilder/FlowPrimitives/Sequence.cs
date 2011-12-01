
using System;
namespace NetWebScript.JsClr.AstBuilder.Flow
{
    public abstract class Sequence
    {
        public virtual int LowestStack
        {
            get { throw new NotSupportedException(); }
        }
    }
}
