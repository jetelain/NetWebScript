using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetWebScript.Script;

namespace NetWebScript.Equivalents
{
    [ScriptAvailable]
    [ScriptEquivalent(typeof(System.Math))]
    public static class MathEquiv
    {
        //public static decimal Abs(decimal value);

        public static double Abs(double value)
        {
            return JSMath.Abs(value);
        }

        //public static float Abs(float value);

        //public static int Abs(int value);

        //public static long Abs(long value);

        //public static sbyte Abs(sbyte value);

        //public static short Abs(short value);

        public static double Acos(double d)
        {
            return JSMath.Acos(d);
        }

        public static double Asin(double d)
        {
            return JSMath.Asin(d);
        }

        public static double Atan(double d)
        {
            return JSMath.Atan(d);
        }

        public static double Atan2(double y, double x)
        {
            return JSMath.Atan2(y,x);
        }

        //public static long BigMul(int a, int b);

        //public static decimal Ceiling(decimal d);

        public static double Ceiling(double a)
        {
            return JSMath.Ceil(a);
        }

        public static double Cos(double d)
        {
            return JSMath.Cos(d);
        }

        public static double Cosh(double value)
        {
            return (JSMath.Exp(value) + JSMath.Exp(-value)) / 2;
        }

        //public static int DivRem(int a, int b, out int result);

        //public static long DivRem(long a, long b, out long result);

        public static double Exp(double d)
        {
            return JSMath.Exp(d);
        }

        //public static decimal Floor(decimal d);

        public static double Floor(double d)
        {
            return JSMath.Floor(d);
        }

        //public static double IEEERemainder(double x, double y);

        public static double Log(double d)
        {
            return JSMath.Log(d);
        }

        public static double Log(double a, double newBase)
        {
            return JSMath.Log(a) / JSMath.Log(newBase);
        }

        public static double Log10(double d)
        {
            return JSMath.Log(d) / JSMath.Log(10);
        }

        //public static byte Max(byte val1, byte val2);

        //public static decimal Max(decimal val1, decimal val2);

        public static double Max(double val1, double val2)
        {
            return JSMath.Max(val1, val2);
        }

        public static float Max(float val1, float val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static int Max(int val1, int val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static long Max(long val1, long val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static sbyte Max(sbyte val1, sbyte val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static short Max(short val1, short val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static uint Max(uint val1, uint val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static ulong Max(ulong val1, ulong val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        [CLSCompliant(false)]
        public static ushort Max(ushort val1, ushort val2)
        {
            return val1 > val2 ? val1 : val2;
        }

        public static byte Min(byte val1, byte val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        //public static decimal Min(decimal val1, decimal val2);

        public static double Min(double val1, double val2)
        {
            return JSMath.Min(val1, val2);
        }

        public static float Min(float val1, float val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        public static int Min(int val1, int val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        public static long Min(long val1, long val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        [CLSCompliant(false)]
        public static sbyte Min(sbyte val1, sbyte val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        public static short Min(short val1, short val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        [CLSCompliant(false)]
        public static uint Min(uint val1, uint val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        [CLSCompliant(false)]
        public static ulong Min(ulong val1, ulong val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        [CLSCompliant(false)]
        public static ushort Min(ushort val1, ushort val2)
        {
            return val1 > val2 ? val2 : val1;
        }

        public static double Pow(double x, double y)
        {
            return JSMath.Pow(x, y);
        }

        //public static decimal Round(decimal d);

        public static double Round(double value)
        {
            if (value <= 0)
            {
                return JSMath.Round(value);
            }
            double integer = JSMath.Floor(value);
            if (value - integer > 0.5)
            {
                return integer + 1;
            }
            return integer;
        }

        private static double RoundAwayFromZero(double value)
        {
            if (value >= 0)
            {
                return JSMath.Round(value);
            }
            double integer = JSMath.Ceil(value);
            if (JSMath.Abs(value - integer) >= 0.5)
            {
                return integer - 1;
            }
            return integer;
        }

        //public static decimal Round(decimal d, int decimals);

        //public static decimal Round(decimal d, MidpointRounding mode);

        public static double Round(double value, int digits)
        {
            var f = JSMath.Pow(10, digits);
            return Round(value * f) / f;
        }

        public static double Round(double value, MidpointRounding mode)
        {
            if (mode == MidpointRounding.AwayFromZero)
            {
                return RoundAwayFromZero(value);
            }
            else
            {
                return Round(value);
            }
        }

        //public static decimal Round(decimal d, int decimals, MidpointRounding mode);

        public static double Round(double value, int digits, MidpointRounding mode)
        {
            var f = JSMath.Pow(10, digits);
            return Round(value * f, mode) / f;
        }

        //public static int Sign(decimal value);

        public static int Sign(double value)
        {
            if (value < 0.0)
            {
                return -1;
            }
            if (value > 0.0)
            {
                return 1;
            }
            if (value == 0.0)
            {
                return 0;
            }
            throw new System.Exception("Not a number");
        }

        public static int Sign(float value)
        {
            return Sign((double)value);
        }

        public static int Sign(int value)
        {
            if (value < 0)
            {
                return -1;
            }
            if (value > 0)
            {
                return 1;
            }
            return 0;
        }

        public static int Sign(long value)
        {
            if (value < 0)
            {
                return -1;
            }
            if (value > 0)
            {
                return 1;
            }
            return 0;
        }

        [CLSCompliant(false)]
        public static int Sign(sbyte value)
        {
            if (value < 0)
            {
                return -1;
            }
            if (value > 0)
            {
                return 1;
            }
            return 0;
        }

        public static int Sign(short value)
        {
            if (value < 0)
            {
                return -1;
            }
            if (value > 0)
            {
                return 1;
            }
            return 0;
        }

        public static double Sin(double a)
        {
            return JSMath.Sin(a);
        }

        public static double Sinh(double value)
        {
            return (JSMath.Exp(value) - JSMath.Exp(-value)) / 2;
        }

        public static double Sqrt(double d)
        {
            return JSMath.Sqrt(d);
        }

        public static double Tan(double a)
        {
            return JSMath.Tan(a);
        }

        //public static double Tanh(double value);

        //public static decimal Truncate(decimal d);

        public static double Truncate(double d)
        {
            if (JSNumber.IsNaN(d)) 
            {
                return JSNumber.NaN; 
            }
            return d < 0 ? JSMath.Ceil(d) : JSMath.Floor(d);
        }
    }
}
