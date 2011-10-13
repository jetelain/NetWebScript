
using System;
using NetWebScript;

namespace NetWebScript.Test.Material
{
    [ScriptAvailable]
	public static class Flow
	{
        public static object If(bool cond, object a)
        {
            object result = null;
            if (cond)
            {
                result = a;
            }
            return result;
        }

        public static object IfRet(bool cond, object a)
        {
            if (cond)
            {
                return a;
            }
            return null;
        }

        public static object IfElse(bool cond, object a, object b)
        {
            object result;
            if (cond)
            {
                result = a;
            }
            else
            {
                result = b;
            }
            return result;
        }

        public static object IfElseRet(bool cond, object a, object b)
        {
            if (cond)
            {
                return a;
            }
            else
            {
                return b;
            }
        }

        public static int While(int a, int b)
        {
            int result = 0;
            int i = a;
            while ( i != b )
            {
                result++;
                i++;
            }
            return result;
        }

        public static int WhileBreakContinue(int a, int b, int c, int d)
        {
            int result = 0;
            int i = a;
            while (i != b)
            {
                if (i == c)
                {
                    continue;
                }
                result++;
                i++;
                if (i == d)
                {
                    break;
                }
            }
            return result;
        }

        public static int WhileReturn(int a, int b, int c)
        {
            int result = 0;
            int i = a;
            while (i != b)
            {
                if (i == c)
                {
                    return result;
                }
                result++;
                i++;
            }
            return result;
        }

        public static int Swicth(int a, int b)
        {
            int r = a;
            switch (a)
            {
                case 0: r = b + 3; break;
                case 1: r = b + 2; break;
                case 2: r = b + 1; break;
                case 3: r = b; break;
            }
            return r;
        }
        /*
        public static int DoWhile(int a, int b)
        {
            int result = 0;
            int i = a;
            do
            {
                result++;
                i++;
            }
            while (i != b);
            return result;
        }
        */
        /*public static int SwitchStrRet(String a)
        {
            switch (a)
            {
                case "0": return 0;
                case "1": return 1;
                case "2": return 2;
                case "3": return 3;
                case "4": return 4;
                case "5": return 5;
                case "6": return 6;
                case "7": return 7;
                case "8": return 8;
                case "9": return 9;
                case "10": return 10;
                default: return -1;
            }
        }*/
	}
}
