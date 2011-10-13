using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript;

namespace NetWebScript.Test.Remoting
{
    [ScriptAvailable]
    public class ClassA
    {
        public string StringField;
        public int IntField;
        public double DoubleField;
        public ClassB BField;
        public ClassB[] BArrayField;
    }
}
