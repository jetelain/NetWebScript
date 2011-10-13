using System;
using System.Text;

namespace NetWebScript.JsClr.TypeSystem
{
    /// <summary>
    /// Generateur incremental d'identifiants alphabetiques 
    /// a -> z, aa -> az, ba -> bz ...
    /// </summary>
    internal class IdentifierGenerator
    {
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
                builder.Insert(0, (char) ('a' + (number % 26)));
                number = (number / 26) - 1;
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