﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ScriptEquivalentAttribute : Attribute
    {
        private readonly Type type;

        public ScriptEquivalentAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}
