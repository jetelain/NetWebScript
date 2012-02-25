﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Single))]
    internal sealed class SingleEquiv
    {
        private readonly JSNumber value;

        public SingleEquiv ( JSNumber value )
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public JSNumber Value { get { return value; } } 

    }
}
