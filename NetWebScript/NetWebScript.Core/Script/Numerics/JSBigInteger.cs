//	JavaScript BigInteger library version 0.9
//	http://silentmatt.com/biginteger/
//
//	Copyright (c) 2009 Matthew Crumley <email@matthewcrumley.com>
//	Copyright (c) 2010,2011 by John Tobey <John.Tobey@gmail.com>
//	Licensed under the MIT license.
//
//	Support for arbitrary internal representation base was added by
//	Vitaly Magerya.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebScript.Script.Numercis
{
    [ScriptAvailable]
    public sealed class JSBigInteger
    {
        // Constant: ZERO
        // <BigInteger> 0.
        public static JSBigInteger ZERO = new JSBigInteger(new JSNumber[0], 0);

        // Constant: ONE
        // <BigInteger> 1.
        public static JSBigInteger ONE = new JSBigInteger(new JSNumber[] { 1 }, 1);

        // Constant: M_ONE
        // <BigInteger> -1.
        public static JSBigInteger M_ONE = new JSBigInteger(JSBigInteger.ONE._d, -1);

        /*
            Constant: small
            Array of <BigIntegers> from 0 to 36.

            These are used internally for parsing, but useful when you need a "small"
            <BigInteger>.

            See Also:

                <ZERO>, <ONE>, <_0>, <_1>
        */
        internal static JSBigInteger[] small = new []{
            JSBigInteger.ZERO,
            JSBigInteger.ONE,
            new JSBigInteger( new JSNumber[]{2},1),
            new JSBigInteger( new JSNumber[]{3},1),
            new JSBigInteger( new JSNumber[]{4},1),
            new JSBigInteger( new JSNumber[]{5},1),
            new JSBigInteger( new JSNumber[]{6},1),
            new JSBigInteger( new JSNumber[]{7},1),
            new JSBigInteger( new JSNumber[]{8},1),
            new JSBigInteger( new JSNumber[]{9},1),
            new JSBigInteger(new JSNumber[]{10},1),
            new JSBigInteger(new JSNumber[]{11},1),
            new JSBigInteger(new JSNumber[]{12},1),
            new JSBigInteger(new JSNumber[]{13},1),
            new JSBigInteger(new JSNumber[]{14},1),
            new JSBigInteger(new JSNumber[]{15},1),
            new JSBigInteger(new JSNumber[]{16},1),
            new JSBigInteger(new JSNumber[]{17},1),
            new JSBigInteger(new JSNumber[]{18},1),
            new JSBigInteger(new JSNumber[]{19},1),
            new JSBigInteger(new JSNumber[]{20},1),
            new JSBigInteger(new JSNumber[]{21},1),
            new JSBigInteger(new JSNumber[]{22},1),
            new JSBigInteger(new JSNumber[]{23},1),
            new JSBigInteger(new JSNumber[]{24},1),
            new JSBigInteger(new JSNumber[]{25},1),
            new JSBigInteger(new JSNumber[]{26},1),
            new JSBigInteger(new JSNumber[]{27},1),
            new JSBigInteger(new JSNumber[]{28},1),
            new JSBigInteger(new JSNumber[]{29},1),
            new JSBigInteger(new JSNumber[]{30},1),
            new JSBigInteger(new JSNumber[]{31},1),
            new JSBigInteger(new JSNumber[]{32},1),
            new JSBigInteger(new JSNumber[]{33},1),
            new JSBigInteger(new JSNumber[]{34},1),
            new JSBigInteger(new JSNumber[]{35},1),
            new JSBigInteger(new JSNumber[]{36},1)
        };

        private static JSRegExp[] radixRegex = new[] {
	        new JSRegExp("^$"),
	        new JSRegExp("^$"),
	        new JSRegExp("^[01]*$"),
	        new JSRegExp("^[012]*$"),
	        new JSRegExp("^[0-3]*$"),
	        new JSRegExp("^[0-4]*$"),
	        new JSRegExp("^[0-5]*$"),
	        new JSRegExp("^[0-6]*$"),
	        new JSRegExp("^[0-7]*$"),
	        new JSRegExp("^[0-8]*$"),
	        new JSRegExp("^[0-9]*$"),
	        new JSRegExp("^[0-9aA]*$"),
	        new JSRegExp("^[0-9abAB]*$"),
	        new JSRegExp("^[0-9abcABC]*$"),
	        new JSRegExp("^[0-9a-dA-D]*$"),
	        new JSRegExp("^[0-9a-eA-E]*$"),
	        new JSRegExp("^[0-9a-fA-F]*$"),
	        new JSRegExp("^[0-9a-gA-G]*$"),
	        new JSRegExp("^[0-9a-hA-H]*$"),
	        new JSRegExp("^[0-9a-iA-I]*$"),
	        new JSRegExp("^[0-9a-jA-J]*$"),
	        new JSRegExp("^[0-9a-kA-K]*$"),
	        new JSRegExp("^[0-9a-lA-L]*$"),
	        new JSRegExp("^[0-9a-mA-M]*$"),
	        new JSRegExp("^[0-9a-nA-N]*$"),
	        new JSRegExp("^[0-9a-oA-O]*$"),
	        new JSRegExp("^[0-9a-pA-P]*$"),
	        new JSRegExp("^[0-9a-qA-Q]*$"),
	        new JSRegExp("^[0-9a-rA-R]*$"),
	        new JSRegExp("^[0-9a-sA-S]*$"),
	        new JSRegExp("^[0-9a-tA-T]*$"),
	        new JSRegExp("^[0-9a-uA-U]*$"),
	        new JSRegExp("^[0-9a-vA-V]*$"),
	        new JSRegExp("^[0-9a-wA-W]*$"),
	        new JSRegExp("^[0-9a-xA-X]*$"),
	        new JSRegExp("^[0-9a-yA-Y]*$"),
	        new JSRegExp("^[0-9a-zA-Z]*$")
        };
        private static JSArray<string> digits = ((JSString)"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ").Split("");

        internal /*readonly*/ JSArray<JSNumber> _d;
        internal /*readonly*/ int _s;

        private const double @base = 10000000;
        private const int base_log10 = 7;

        internal JSBigInteger(JSArray<JSNumber> n, int s)
        {
            while (n.Length > 0 && (n[n.Length - 1] == null || n[n.Length - 1] == 0))
            {
                n.Splice(n.Length-1, 1);
            }
            this._d = n;
            this._s = n.Length != 0 ? (s == 0 ? 1 : s) : 0;
        }

        public string ToString(int toBase) {
	        if (toBase < 2 || toBase > 36) {
		        throw new Exception("illegal radix " + toBase + ".");
	        }
	        if (this._s == 0) {
		        return "0";
	        }
	        if (toBase == 10) {
		        var str = this._s < 0 ? "-" : "";
		        str += this._d[this._d.Length - 1].ToString();
		        for (var i = this._d.Length - 2; i >= 0; i--) {
			        var group = this._d[i].ToString();
			        while (group.Length < base_log10) group = '0' + group;
			        str += group;
		        }
		        return str;
	        }
	        else {
		        var numerals = digits;
		        JSBigInteger baseBig = small[toBase];
		        var sign = this._s;

                JSBigInteger n = Abs();
		        var numdDigits = new JSArray<string>();
                JSBigInteger digit;

		        while (n._s != 0) {
                    var divmod = n.DivRem(baseBig);
			        n = divmod[0];
			        digit = divmod[1];
			        // TODO: This could be changed to unshift instead of reversing at the end.
			        // Benchmark both to compare speeds.
                    numdDigits.Push(numerals[digit.ValueOfSmall()]);
		        }
                return (sign < 0 ? "-" : "") + numdDigits.Reverse().Join("");
	        }
        }

        private static string expandExponential(JSString str) {
		    str = str.Replace(new JSRegExp(@"\s*[*xX]\s*10\s*(\^|\*\*)\s*"), "e");

            return str.Replace(new JSRegExp(@"^([+\-])?(\d+)\.?(\d*)[eE]([+\-]?\d+)$"), new Func<string, string, string, string, string, string>((strX, s, n, f, cStr) =>
            {
                //int x = JSNumber.ParseInt(strX);
                int c = JSNumber.ParseInt(cStr);
                var l = c < 0 ? 1 : 0;
			    var i = n.Length + c;
			    int x = (c < 0 ? n : f).Length;
			    c = ((c = Math.Abs(c)) >= x ? (c - x + l) : 0);
			    var z = (new JSArray<object>(c + 1)).Join("0");
			    JSString r = n + f;
                if ((object)s == JSObject.Undefined || s == null)
                {
                    s = string.Empty;
                }
			    return s + (c < 0 ? r = z + r : r += z).Substr(0, i += c < 0 ? z.Length : 0) + (i < r.Length ? "." + r.Substr(i) : "");
		    }));
	    }
        public static JSBigInteger Parse(JSNumber s)
        {
            return Parse(s.ToString(), -1);
        }
        public static JSBigInteger Parse(string s)
        {
            return Parse(s, -1);
        }
        public static JSBigInteger FromInteger(int v)
        {
            if ( v >= 0)
            {
                if (v < small.Length)
                {
                    return small[v];
                }
                if (v < @base)
                {
                    return new JSBigInteger(new JSNumber[] { v }, 1);
                }
                return new JSBigInteger(new JSNumber[] { (int)(v % @base) & 0x7FFFFFFF, (int)(v / @base) & 0x7FFFFFFF }, 1);
            }
            
            return Parse(((JSNumber)v).ToString(), 10);
        }
        public static JSBigInteger Parse (string s, int fromBase) {
	        // Expands a number in exponential form to decimal form.
	        // expandExponential("-13.441*10^5") === "1344100";
	        // expandExponential("1.12300e-1") === "0.112300";
	        // expandExponential(1000000000000000000000000000000) === "1000000000000000000000000000000";
            var originalS = s;

	        if (fromBase == 10 || fromBase == -1) {
		        s = expandExponential(s);
	        }

            string prefixRE=string.Empty;
	        if ( fromBase == -1) {
		        prefixRE = "0[xcb]";
	        }
	        else if (fromBase == 16) {
		        prefixRE = "0x";
	        }
	        else if (fromBase == 8) {
		        prefixRE = "0c";
	        }
	        else if (fromBase == 2) {
		        prefixRE = "0b";
	        }

	        var parts = new JSRegExp("^([+\\-]?)(" + prefixRE + ")?([0-9a-z]*)(?:\\.\\d*)?$", "i").Exec(s);
            //var parts = new JSRegExp(@"^([+\-]?)(0[xXcCbB])?([0-9A-Za-z]*)(?:\.\d*)?$").Exec(s);
	        if (parts != null) {
		        var signPart = parts[1].Length == 0 ? "+" : parts[1];
		        var baseSection = parts[2];
		        var digitsPart = parts[3];
                if (fromBase == -1)
                {
                    if (baseSection == "0x" || baseSection == "0X") { // Hex
				        fromBase = 16;
			        }
			        else if (baseSection == "0c" || baseSection == "0C") { // Octal
				        fromBase = 8;
			        }
			        else if (baseSection == "0b" || baseSection == "0B") { // Binary
				        fromBase = 2;
			        }
			        else {
				        fromBase = 10;
			        }
                    
                }
		        if (fromBase < 2 || fromBase > 36) {
			        throw new Exception("Illegal radix " + fromBase + ".");
		        }
		        // Check for digits outside the range
		        if (!(radixRegex[fromBase].Test(digitsPart))) {
                    throw new Exception("Bad digit for radix " + fromBase + " ('" + originalS + "'=>'" + digitsPart + "')");
		        }

		        // Strip leading zeros, and convert to array
		        JSArray<string> digits = ((JSString)digitsPart.Replace(new JSRegExp("^0+"), "")).Split("");
		        if (digits.Length == 0) {
			        return ZERO;
		        }

		        // Get the sign (we know it's not zero)
		        var sign = (signPart == "-") ? -1 : 1;

		        // Optimize 10
		        if (fromBase == 10) {
			        var d = new JSArray<JSNumber>();
			        while (digits.Length >= base_log10) {
				        d.Push(JSNumber.ParseInt(digits.Splice(-base_log10).Join(""), 10));
			        }
                    if (digits.Length > 0)
                    {
                        d.Push(JSNumber.ParseInt(digits.Join(""), 10));
                    }
			        return new JSBigInteger(d, sign);
		        }

                //// Optimize base
                //if (@base === JSBigInteger.@base) {
                //    return new JSBigInteger(digits.Map(Number).Reverse(), sign);
                //}

		        // Do the conversion
		        var d2 = ZERO;
		        var baseBig = small[fromBase];
		        for (var i = 0; i < digits.Length; i++) {
			        d2 = d2.Multiply(baseBig).Add(small[(int)JSNumber.ParseInt(digits[i], 36)]);
		        }
		        return new JSBigInteger(d2._d, sign);
	        }
	        else {
		        throw new Exception("Invalid BigInteger format: " + s);
	        }
        }

        public JSBigInteger Add (JSBigInteger n) {
	        if (this._s == 0) {
		        return n;
	        }

	        if (n._s== 0) {
		        return this;
	        }
	        if (this._s != n._s) {
		        n = n.Negate();
		        return Subtract(n);
	        }
	        var a = this._d;
	        var b = n._d;
	        var al = a.Length;
	        var bl = b.Length;
	        var sum = new JSNumber[Math.Max(al, bl) + 1];
	        var size = Math.Min(al, bl);
            double carry = 0;
            double digit;
            int i;
	        for (i = 0; i < size; i++) {
		        digit = a[i] + b[i] + carry;
		        sum[i] = digit % @base;
                carry = (int)(digit / @base) & 0x7FFFFFFF;
	        }
	        if (bl > al) {
		        a = b;
		        al = bl;
	        }
	        for (i = size; carry > 0 && i < al; i++) {
		        digit = a[i] + carry;
		        sum[i] = digit % @base;
                carry = (int)(digit / @base) & 0x7FFFFFFF;
	        }
	        if (carry != 0) {
		        sum[i] = carry;
	        }
	        for ( ; i < al; i++) {
		        sum[i] = a[i];
	        }
	        return new JSBigInteger(sum, _s);
        }

        public JSBigInteger Negate () {
	        return new JSBigInteger(_d, -_s);
        }

        public JSBigInteger Abs () {
	        return (_s < 0) ? Negate() : this;
        }

        public JSBigInteger Subtract( JSBigInteger n) {
	        if (this._s == 0) {
		        return n.Negate();
	        }

	        if (n._s == 0) {
		        return this;
	        }
	        if (this._s != n._s) {
		        n = n.Negate();
		        return this.Add(n);
	        }

	        var m = this;
	        JSBigInteger t;
	        // negative - negative => -|a| - -|b| => -|a| + |b| => |b| - |a|
	        if (this._s < 0) {
		        t = m;
		        m = new JSBigInteger(n._d, 1);
		        n = new JSBigInteger(t._d, 1);
	        }

	        // Both are positive => a - b
	        var sign = m.CompareAbs(n);
	        if (sign == 0) {
		        return ZERO;
	        }
	        else if (sign < 0) {
		        // swap m and n
		        t = n;
		        n = m;
		        m = t;
	        }

	        // a > b
	        var a = m._d;
	        var b = n._d;
	        var al = a.Length;
	        var bl = b.Length;
	        var diff = new JSNumber[al]; // al >= bl since a > b
	        var borrow = 0;
	        int i;
	        JSNumber digit;

	        for (i = 0; i < bl; i++) {
		        digit = a[i] - borrow - b[i];
		        if (digit < 0) {
			        digit += @base;
			        borrow = 1;
		        }
		        else {
			        borrow = 0;
		        }
		        diff[i] = digit;
	        }
	        for (i = bl; i < al; i++) {
		        digit = a[i] - borrow;
		        if (digit < 0) {
                    digit += @base;
		        }
		        else {
			        diff[i++] = digit;
			        break;
		        }
		        diff[i] = digit;
	        }
	        for ( ; i < al; i++) {
		        diff[i] = a[i];
	        }

	        return new JSBigInteger(diff, sign);
        }

        public int CompareAbs (JSBigInteger n) {
	        if (this == n) {
		        return 0;
	        }

	        if (this._s == 0) {
		        return (n._s != 0) ? -1 : 0;
	        }
	        if (n._s == 0) {
		        return 1;
	        }

	        var l = this._d.Length;
	        var nl = n._d.Length;
	        if (l < nl) {
		        return -1;
	        }
	        else if (l > nl) {
		        return 1;
	        }

	        var a = this._d;
	        var b = n._d;
	        for (var i = l-1; i >= 0; i--) {
		        if (a[i] != b[i]) {
			        return a[i] < b[i] ? -1 : 1;
		        }
	        }

	        return 0;
        }

        public bool IsUnit(){
	        return this == ONE ||
		        this == M_ONE ||
		        (this._d.Length == 1 && this._d[0] == 1);
        }

        public JSBigInteger Multiply (JSBigInteger n) {
	        // TODO: Consider adding Karatsuba multiplication for large numbers
	        if (this._s == 0) {
		        return ZERO;
	        }

	        if (n._s == 0) {
		        return ZERO;
	        }
	        if (IsUnit()) {
		        if (this._s < 0) {
			        return n.Negate();
		        }
		        return n;
	        }
	        if (n.IsUnit()) {
		        if (n._s < 0) {
			        return this.Negate();
		        }
		        return this;
	        }
	        if (this == n) {
		        return this.Square();
	        }

	        var r = (this._d.Length >= n._d.Length);
	        var a = (r ? this : n)._d; // a will be longer than b
	        var b = (r ? n : this)._d;
	        var al = a.Length;
	        var bl = b.Length;

	        var pl = al + bl;
            var partial = new JSNumber[pl];
	        int i;
	        for (i = 0; i < pl; i++) {
		        partial[i] = 0;
	        }

	        for (i = 0; i < bl; i++) {
                double carry = 0;
                double bi = b[i];
                double jlimit = al + i;
                double digit;
                int j;
		        for (j = i; j < jlimit; j++) {
			        digit = partial[j] + bi * a[j - i] + carry;
			        carry = (int)(digit / @base) & 0x7FFFFFFF;
                    partial[j] = (int)(digit % @base) & 0x7FFFFFFF;
		        }
		        if (carry!=0) {
			        digit = partial[j] + carry;
                    carry = (int)(digit / @base) & 0x7FFFFFFF;
			        partial[j] = digit % @base;
		        }
	        }
	        return new JSBigInteger(partial, this._s * n._s);
        }

        private JSBigInteger MultiplySingleDigit (double n) {
	        if (n == 0 || this._s == 0) {
		        return ZERO;
	        }
	        if (n == 1) {
		        return this;
	        }

            double digit;
	        if (this._d.Length == 1) {
		        digit = this._d[0] * n;
		        if (digit >= @base) {
                    return new JSBigInteger(new JSNumber[]{(int)(digit % @base)& 0x7FFFFFFF,
					        (int)(digit / @base)& 0x7FFFFFFF}, 1);
		        }
		        return new JSBigInteger(new JSNumber[]{digit}, 1);
	        }

	        if (n == 2) {
		        return Add(this);
	        }
	        if (this.IsUnit()) {
		        return new JSBigInteger(new JSNumber[]{n}, 1);
	        }

	        var a = this._d;
	        var al = a.Length;

	        var pl = al + 1;
	        var partial = new JSNumber[pl];
	        for (var i = 0; i < pl; i++) {
		        partial[i] = 0;
	        }
            int j;
            double carry = 0;
	        for (j = 0; j < al; j++) {
		        digit = n * a[j] + carry;
                carry = (int)(digit / @base) & 0x7FFFFFFF;
                partial[j] = (int)(digit % @base) & 0x7FFFFFFF;
	        }
	        if (carry!=0) {
                partial[j] = carry;
	        }

	        return new JSBigInteger(partial, 1);
        }

        public JSBigInteger Square() {
	        // Normally, squaring a 10-digit number would take 100 multiplications.
	        // Of these 10 are unique diagonals, of the remaining 90 (100-10), 45 are repeated.
	        // This procedure saves (N*(N-1))/2 multiplications, (e.g., 45 of 100 multiplies).
	        // Based on code by Gary Darby, Intellitech Systems Inc., www.DelphiForFun.org

	        if (this._s == 0) {
		        return ZERO;
	        }
	        if (this.IsUnit()) {
		        return ONE;
	        }

	        var digits = this._d;
	        var length = digits.Length;
	        var imult1 = new JSNumber[length + length + 1];
	        double product, carry;
            int k;
	        int i;

	        // Calculate diagonal
	        for (i = 0; i < length; i++) {
		        k = i * 2;
		        product = digits[i] * digits[i];
                carry = (int)(product / @base) & 0x7FFFFFFF;
		        imult1[k] = product % @base;
		        imult1[k + 1] = carry;
	        }

	        // Calculate repeating part
	        for (i = 0; i < length; i++) {
		        carry = 0;
		        k = i * 2 + 1;
		        for (var j = i + 1; j < length; j++, k++) {
			        product = digits[j] * digits[i] * 2 + imult1[k] + carry;
                    carry = (int)(product / @base) & 0x7FFFFFFF;
			        imult1[k] = product % @base;
		        }
		        k = length + i;
		        var digit = carry + imult1[k];
                carry = (int)(digit / @base) & 0x7FFFFFFF;
		        imult1[k] = digit % @base;
		        imult1[k + 1] = (imult1[k + 1] ?? 0) + carry;
	        }

	        return new JSBigInteger(imult1, 1);
        }

        public JSBigInteger Quotient (JSBigInteger n) {
	        return this.DivRem(n)[0];
        }
        public JSBigInteger Remainder (JSBigInteger n) {
	        return this.DivRem(n)[1];
        }
        public JSBigInteger[] DivRem (JSBigInteger n) {
	        if (n._s == 0) {
		        throw new Exception("Divide by zero");
	        }
	        if (this._s == 0) {
		        return new JSBigInteger[]{ZERO, ZERO};
	        }
	        if (n._d.Length == 1) {
		        return this.DivRemSmall(n._s * n._d[0]);
	        }

	        // Test for easy cases -- |n1| <= |n2|
	        switch (this.CompareAbs(n)) {
	        case 0: // n1 == n2
		        return new JSBigInteger[]{this._s ==n._s ? ONE : M_ONE, ZERO};
	        case -1: // |n1| < |n2|
		        return new JSBigInteger[]{ZERO, this};
	        }

            var sign = this._s * n._s;
	        var a = n.Abs();
	        var b_digits = this._d;
	        var b_index = b_digits.Length;
	        var digits = n._d.Length;
	        var quot = new JSArray<JSNumber>();
	        double guess;
            JSBigInteger check;

	        var part = new JSBigInteger(new JSNumber[0], 0);
	        part._s = 1;

	        while (b_index >0) {
		        part._d.Unshift(b_digits[--b_index]);

		        if (part.CompareAbs(n) < 0) {
			        quot.Push(0);
			        continue;
		        }
		        if (part._s == 0) {
			        guess = 0;
		        }
		        else {
                    var xlen = part._d.Length;
                    var ylen = a._d.Length;
			        double highx = part._d[xlen-1]*@base + part._d[xlen-2];
                    double highy = a._d[ylen - 1] * @base + a._d[ylen - 2];
			        if (part._d.Length > a._d.Length) {
				        // The length of part._d can either match a._d length,
				        // or exceed it by one.
                        highx = (highx + 1) * @base;
			        }
			        guess = JSMath.Ceil(highx/highy);
		        }
		        do {
			        check = a.MultiplySingleDigit(guess);
			        if (check.CompareAbs(part) <= 0) {
				        break;
			        }
			        guess--;
                } while (guess != 0);

		        quot.Push(guess);
		        if (guess == 0) {
			        continue;
		        }
		        var diff = part.Subtract(check);
		        part._d = diff._d.Slice();
		        if (part._d.Length == 0) {
			        part._s = 0;
		        }
	        }


            return new JSBigInteger[]{new JSBigInteger(quot.Reverse(), sign),
                   new JSBigInteger(part._d, this._s)};
        }

        public JSBigInteger[] DivRemSmall (int n) {
            JSBigInteger r;
	        if (n == 0) {
		        throw new Exception("Divide by zero");
	        }

	        var n_s = n < 0 ? -1 : 1;
	        var sign = this._s * n_s;
	        n = Math.Abs(n);

	        if (n < 1 || n >= @base) {
		        throw new Exception("Argument out of range");
	        }

	        if (this._s == 0) {
		        return new JSBigInteger[]{ZERO, ZERO};
	        }

	        if (n == 1 || n == -1) {
		        return new JSBigInteger[]{(sign == 1) ? this.Abs() : new JSBigInteger(this._d, sign), ZERO};
	        }

	        // 2 <= n < BigInteger.base

	        // divide a single digit by a single digit
	        if (this._d.Length == 1) {
		        var q = new JSBigInteger(new JSNumber[]{(this._d[0] / n) & 0x7FFFFFFF}, 1);
		        r = new JSBigInteger(new JSNumber[]{(this._d[0] % n) & 0x7FFFFFFF}, 1);
		        if (sign < 0) {
			        q = q.Negate();
		        }
		        if (this._s < 0) {
			        r = r.Negate();
		        }
                return new JSBigInteger[] { q, r };
	        }

	        var digits = this._d.Slice();
	        var quot = new JSNumber[digits.Length];
            double part = 0;
	        double diff = 0;
	        var i = 0;
            double guess;

	        while (digits.Length>0) {
		        part = part * @base + digits[digits.Length - 1];
		        if (part < n) {
			        quot[i++] = 0;
			        digits.Pop();
			        diff = @base * diff + part;
			        continue;
		        }
		        if (part == 0) {
			        guess = 0;
		        }
		        else {
			        guess = (int)(part / n) & 0x7FFFFFFF;
		        }

		        var check = n * guess;
		        diff = part - check;
		        quot[i++] = guess;
		        if (guess==0) {
			        digits.Pop();
			        continue;
		        }

		        digits.Pop();
		        part = diff;
	        }

	        r = new JSBigInteger(new JSNumber[]{diff}, 1);
	        if (this._s < 0) {
		        r = r.Negate();
	        }
	        return new JSBigInteger[]{new JSBigInteger(((JSArray<JSNumber>)quot).Reverse(), sign), r};
        }

        public bool IsEven () {
	        var digits = this._d;
	        return this._s == 0 || digits.Length == 0 || (digits[0] % 2) == 0;
        }

        public bool IsOdd() {
	        return !this.IsEven();
        }

        public int Sign () {
	        return this._s;
        }

        public bool IsPositive () {
	        return this._s > 0;
        }

        public bool IsNegative () {
	        return this._s < 0;
        }

        public bool IsZero () {
	        return this._s == 0;
        }

        private const int MAX_EXP = 0x7FFFFFFF;

        public JSBigInteger Exp10 (int n) {
	        if (n == 0) {
		        return this;
	        }
	        if (Math.Abs(n) > MAX_EXP) {
		        throw new Exception("exponent too large in BigInteger.exp10");
	        }
	        if (n > 0) {
		        var k = new JSBigInteger(this._d.Slice(), this._s);

		        for (; n >= base_log10; n -= base_log10) {
			        k._d.Unshift(0);
		        }
		        if (n == 0)
			        return k;
		        k._s = 1;
		        k = k.MultiplySingleDigit((int)Math.Pow(10, n));
		        return (this._s < 0 ? k.Negate() : k);
	        } else if (-n >= this._d.Length*base_log10) {
		        return ZERO;
	        } else {
		        var k = new JSBigInteger(this._d.Slice(), this._s);

		        for (n = -n; n >= JSBigInteger.base_log10; n -= JSBigInteger.base_log10) {
			        k._d.Shift();
		        }
		        return (n == 0) ? k : k.DivRemSmall((int)Math.Pow(10, n))[0];
	        }
        }

        public JSBigInteger Pow(JSBigInteger n) {
	        if (this.IsUnit()) {
		        if (this._s > 0) {
			        return this;
		        }
		        else {
			        return n.IsOdd() ? this : this.Negate();
		        }
	        }

	        if (n._s == 0) {
		        return ONE;
	        }
	        else if (n._s < 0) {
		        if (this._s == 0) {
			        throw new Exception("Divide by zero");
		        }
		        else {
			        return ZERO;
		        }
	        }
	        if (this._s == 0) {
		        return ZERO;
	        }
	        if (n.IsUnit()) {
		        return this;
	        }

            //if (n.CompareAbs(MAX_EXP) > 0) {
            //    throw new Exception("exponent too large in BigInteger.pow");
            //}
	        var x = this;
	        var aux = ONE;
	        var two = small[2];

	        while (n.IsPositive()) {
		        if (n.IsOdd()) {
			        aux = aux.Multiply(x);
			        if (n.IsUnit()) {
				        return aux;
			        }
		        }
		        x = x.Square();
		        n = n.Quotient(two);
	        }

	        return aux;
        }
        private int ValueOfSmall() {
            if (this._s == 0)
            {
                return 0;
            }
            if (this._d.Length > 1)
            {
                throw new Exception("Not that small");
            }
            return this._s * this._d[0];
        }
        public JSNumber ValueOf()
        {
            return JSNumber.ParseInt(this.ToString(), 10);
        }
        public bool Equals(JSBigInteger n)
        {
            if (n == null)
            {
                return false;
            }
            return CompareAbs(n) == 0;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as JSBigInteger);
        }

        //public override int GetHashCode()
        //{
        //    throw new NotImplementedException();
        //}

        public override string ToString()
        {
            return ToString(10);
        }
    }
}
