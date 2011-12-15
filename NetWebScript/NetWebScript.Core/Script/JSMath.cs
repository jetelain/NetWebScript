using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script
{
    [Imported(Name = "Math", IgnoreNamespace = true)]
    public static class JSMath
    {
        public static double Abs(double value)
        {
            return Math.Abs(value);
        }

        public static double Acos(double value)
        {
            return Math.Acos(value);
        }

        public static double Asin(double value)
        {
            return Math.Asin(value);
        }

        public static double Atan(double value)
        {
            return Math.Atan(value);
        }

        public static double Atan2(double y, double x)
        {
            return Math.Atan2(y, x);
        }

        public static double Ceil(double value)
        {
            return Math.Ceiling(value);
        }

        public static double Cos(double value)
        {
            return Math.Cos(value);
        }

        public static double Exp(double value)
        {
            return Math.Exp(value);
        }

        public static double Floor(double value)
        {
            return Math.Floor(value);
        }

        public static double Log(double value)
        {
            return Math.Log(value);
        }

        public static double Max(double a, double b)
        {
            return Math.Max(a, b);
        }

        public static double Min(double a, double b)
        {
            return Math.Min(a, b);
        }

        public static double Pow(double x, double exponent)
        {
            return Math.Pow(x, exponent);
        }

        private static Random generator;

        public static double Random()
        {
            if ( generator == null )
            {
                generator = new Random();
            }
            return generator.NextDouble();
        }

        public static double Sin(double value)
        {
            return Math.Sin(value);
        }

        public static double Sqrt(double value)
        {
            return Math.Sqrt(value);
        }

        public static double Tan(double value)
        {
            return Math.Tan(value);
        }

        public static double Round(double a)
        {
            return Math.Round(a);
        }
    }
}
