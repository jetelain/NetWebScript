using System;

namespace NetWebScript.JsClr.AstBuilder
{
    public sealed class AstBuilderException : Exception
    {

        internal AstBuilderException ( int ilOffset, string message )
            : base(message)
        {
            this.IlOffset = ilOffset;
        }

        internal AstBuilderException(int? ilOffset, string message)
            : base(message)
        {
            this.IlOffset = ilOffset;
        }

        internal AstBuilderException(string message, Exception e)
            : base(message, e)
        {

        }

        internal AstBuilderException(string message)
            : base(message)
        {

        }

        public int? IlOffset { get; private set; }
    }
}
