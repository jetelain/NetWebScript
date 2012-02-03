using System;
using System.Text;
using System.Diagnostics.Contracts;

namespace NetWebScript.JsClr.TypeSystem
{
    /// <summary>
    /// Generateur incremental d'identifiants alphabetiques 
    /// a -> z, aa -> az, ba -> bz ...
    /// </summary>
    internal class IdentifierGenerator
    {
        private readonly char firstChar;
        private readonly int range;

        public IdentifierGenerator()
            : this(false, 26)
        {
        
        }

        public IdentifierGenerator ( bool uppercase, int range )
        {
            Contract.Requires(range > 0 && range <= 26);
            this.firstChar = uppercase ? 'A' : 'a';
            this.range = range;
        }

        private int current;

        
        internal void Increment()
        {
            current++;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            int number = current;
            while (number >= 0)
            {
                builder.Insert(0, (char)(firstChar + (number % range)));
                number = (number / range) - 1;
            }
            return builder.ToString();
        }

        internal String TakeOne()
        {
            String value = ToString();
            Increment();
            return value;
        }
    }
}