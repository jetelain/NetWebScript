﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;
using System.Diagnostics;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    public class Enum
    {
        protected string name;
        protected double value;

        public static Enum ToObject(Type enumType, double value)
        {
            Enum[] table = GetValues(enumType);
            for (int i=0;i<table.Length;++i) 
            {
                if (table[i].value == value)
                {
                    return table[i];
                }
            }
            Enum obj = CreateInstance(enumType);
            obj.value = value;
            return obj;
        }

        public static double ToValue(Enum enumObject)
        {
            return enumObject.value;
        }

        [ScriptBody(Inline = "enumType.$v")]
        private static Enum[] GetValues(Type enumType)
        {
 	        throw new System.NotImplementedException();
        }

        [ScriptBody(Inline = "enumType.$v=values")]
        private static void SetValues(Type enumType, Enum[] values)
        {
            throw new System.NotImplementedException();
        }

        [ScriptBody(Inline = "new t()")]
        private static Enum CreateInstance(Type t)
        {
            throw new System.NotImplementedException();
        }

        [DebuggerHidden]
        public static Type CreateEnumType(string name, EnumData[] data)
        {
            var t = TypeSystemHelper.CreateType(name, typeof(Enum), null);
            Enum[] values = new Enum[data.Length];
            for (var i = 0; i < data.Length; ++i) {
                var e = data[i];
                Enum o = CreateInstance(t);
                o.value = e.v;
                o.name = e.n;
                values[i] = o;
            }
            SetValues(t, values);
            return t;
        }

        public override string ToString()
        {
            return name;
        }

        [AnonymousObject]
        public class EnumData
        {
            public double v;
            public string n;
        }
    }
}
