using System;
using System.Globalization;
using System.Numerics;
using Newtonsoft.Json.Linq;
using UnityEngine;

[Serializable]
public struct BBNumber : IComparable<BBNumber>, IEquatable<BBNumber> {
    
    [SerializeField]
    internal double significand;
    [SerializeField]
    internal double exponent;

    public static readonly BBNumber Zero = new BBNumber(0.0);

    public static readonly BBNumber One = new BBNumber(1.0);

    public static readonly BBNumber MinusOne = new BBNumber(-1.0);

    public static readonly BBNumber Epsilon = new BBNumber(4.94065645841247E-324);

    private static BBNumber MAX_INT = new BBNumber(2147483647.0);

    private static BBNumber MIN_INT = new BBNumber(-2147483648.0);

    private static BBNumber MAX_LONG = new BBNumber(9.2233720368547758E+18);

    private static BBNumber MIN_LONG = new BBNumber(-9.2233720368547758E+18);

    private static BBNumber MAX_FLOAT = new BBNumber(3.4028234663852886E+38);

    private static BBNumber MIN_FLOAT = new BBNumber(-3.4028234663852886E+38);

    private static BBNumber MAX_DOUBLE = new BBNumber(1.7976931348623157E+308);

    private static BBNumber MIN_DOUBLE = new BBNumber(-1.7976931348623157E+308);

    private static int ROUND_SIG_DIGITS = 8;

    private static MidpointRounding ROUND_MODE = MidpointRounding.AwayFromZero;

    static string zeroStr = "0";
    static string oneStr = "1";
    static string commaStr = ".";
    static string commaStr2 = ",";
    static string blankStr = "";
    static char eChar = 'E';

    public bool IsZero {
        get {
            return this.significand == 0.0;
        }
    }

    public BBNumber(BBNumber value) {
        this.significand = value.significand;
        this.exponent = value.exponent;
    }

    public BBNumber(JObject json) {
        this.significand = (double)json["significand"];
        this.exponent = (double)json["exponent"];
    }

    public BBNumber(float value) {
        if (value == 0.0f) {
            this.significand = 0.0;
            this.exponent = 0.0;
        } else {
            this.exponent = Math.Floor(Math.Log10(Math.Abs(value)));
            this.significand = value * Math.Pow(0.1, this.exponent);
        }
        this.significand = Math.Round(this.significand, 7);
    }

    public BBNumber(double value) {
        if (value == 0.0) {
            this.significand = 0.0;
            this.exponent = 0.0;
        } else {
            this.exponent = Math.Floor(Math.Log10(Math.Abs(value)));
            this.significand = value * Math.Pow(0.1, this.exponent);
        }
    }

    public BBNumber(double significand, double exponent) {
        if (significand == 0.0) {
            this.significand = 0.0;
            this.exponent = 0.0;
        } else {
            exponent += Math.Log10(Math.Abs(significand));
            this.exponent = Math.Floor(exponent);
            this.significand = Math.Pow(10.0, exponent - this.exponent);
            this.significand *= (double)((significand < 0.0) ? -1 : 1);
            if (BBNumber.Abs(this) < BBNumber.Epsilon) {
                this.significand = 0.0;
                this.exponent = 0.0;
            }
        }
    }

    public BBNumber(String value) {

        if (string.IsNullOrEmpty(value))
        {
            value = zeroStr;
        }

        double split0 = 0.0;
        double split1 = 0.0;

        value = value.Replace(commaStr2, commaStr);
        try {
            String[] split = value.Split(eChar);
            split0 = double.Parse(split[0], CultureInfo.CurrentCulture);
            if (split.Length > 1 && !split[1].Equals(blankStr)) {
                split1 = Double.Parse(split[1], CultureInfo.CurrentCulture);
            }
        } catch (Exception ex) {
            try {
                String[] split = value.Split(eChar);
                split0 = double.Parse(split[0]);
                if (split.Length > 1 && !split[1].Equals(blankStr)) {
                    split1 = Double.Parse(split[1]);
                }
            } catch (Exception ex2) {
                try
                {
                    String[] split = value.Split(eChar);
                    split0 = double.Parse(split[0], CultureInfo.InvariantCulture);
                    if (split.Length > 1 && !split[1].Equals(blankStr))
                    {
                        split1 = Double.Parse(split[1], CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex3)
                {
                    throw new FormatException("BBNumber String parsing failed : " + ex3.Message + "  value : " + value);
                }


            }
        }

        if (split0 == 0.0) {
            this.significand = 0.0;
            this.exponent = 0.0;
        } else {
            split1 += Math.Log10(Math.Abs(split0));
            this.exponent = Math.Floor(split1);
            this.significand = Math.Pow(10.0, split1 - this.exponent);
            this.significand = Math.Round(significand, 13);     //to remove tailing number
            this.significand *= (double)((split0 < 0.0) ? -1 : 1);
            if (BBNumber.Abs(this) < (BBNumber.Epsilon)) {
                this.significand = 0.0;
                this.exponent = 0.0;
            }
        }
    }

    public override bool Equals(object obj) {
        return obj is BBNumber && this.Equals((BBNumber)obj);
    }

    public bool Equals(BBNumber other) {
        return this.CompareTo(other) == 0;
    }

    public int CompareTo(object obj) {
        if (obj == null) {
            return 1;
        }
        if (!(obj is BBNumber)) {
            throw new ArgumentException("OBJECT_NOT_GHDOUBLE");
        }
        return this.CompareTo((BBNumber)obj);
    }

    public int CompareTo(BBNumber other) {
        if (this.IsZero && other.IsZero) {
            return 0;
        }
        if (this.IsZero) {
            return (other.significand >= 0.0) ? -1 : 1;
        }
        if (other.IsZero) {
            return (this.significand <= 0.0) ? -1 : 1;
        }
        if (this.significand > 0.0 && other.significand < 0.0) {
            return 1;
        }
        if (this.significand < 0.0 && other.significand > 0.0) {
            return -1;
        }
        if (Math.Round(this.exponent, BBNumber.ROUND_SIG_DIGITS, BBNumber.ROUND_MODE) != Math.Round(other.exponent, BBNumber.ROUND_SIG_DIGITS, BBNumber.ROUND_MODE)) {
            if (this.significand > 0.0) {
                return (this.exponent <= other.exponent) ? -1 : 1;
            }
            return (this.exponent >= other.exponent) ? -1 : 1;
        } else {
            if (Math.Round(this.significand, BBNumber.ROUND_SIG_DIGITS, BBNumber.ROUND_MODE) == Math.Round(other.significand, BBNumber.ROUND_SIG_DIGITS, BBNumber.ROUND_MODE)) {
                return 0;
            }
            return (this.significand <= other.significand) ? -1 : 1;
        }
    }

    public override string ToString() {
        if (BBNumber.IsNaN(this)) {
            return double.NaN.ToString();
        }
        if (BBNumber.IsInfinity(this)) {
            return (this.significand <= 0.0) ? double.NegativeInfinity.ToString() : double.PositiveInfinity.ToString();
        }
        return this.ToString("6");
    }

    public string ToString(string format) {
        return this.ToString(format, CultureInfo.CurrentCulture);//임시
    }

    public string ToString(string format, IFormatProvider provider) {
        int num = 6;
        if (int.TryParse(format, out num)) {
            return this.significand.ToString("F" + num, provider) + "E" + ((this.exponent < 0.0) ? string.Empty : "+") + this.exponent.ToString("000", provider);
        }
        if (!string.IsNullOrEmpty(format)) {
            if (format.Equals("N0")) {
                return ((double)this).ToString("N0", provider);
            }
            if (format.StartsWith("f") || format.StartsWith("F")) {
                return ((double)this).ToString(format, provider);
            }
            if (format.StartsWith("d") || format.StartsWith("D"))
            {
                return ((long)this).ToString(format, provider);
            }
        }
        throw new FormatException("FORMAT_NOT_SUPPORTED");
    }

    public static BBNumber Pow(BBNumber value, double exponent) {
        if (exponent == 0.0) {
            return BBNumber.One;
        }
        if (value.significand == 0.0) {
            return BBNumber.Zero;
        }
        if (double.IsNaN(Math.Pow(value.significand, exponent))) {
            return double.NaN;
        }
        value.exponent += Math.Log10(Math.Abs(value.significand));
        value.exponent *= exponent;
        double num = Math.Floor(value.exponent);
        value.significand = (double)((value.significand < 0.0) ? -1 : 1);
        value.significand *= Math.Pow(10.0, value.exponent - num);
        value.exponent = num;
        if (BBNumber.Abs(value) < BBNumber.Epsilon) {
            value = BBNumber.Zero;
        }
        return value;
    }

    public static BBNumber Log10(BBNumber val) {
        val.exponent += Math.Log10(val.significand);
        return new BBNumber(val.exponent);
    }

    public static BBNumber Log(BBNumber log_base, BBNumber val) {
        BBNumber operand_one = Log10(val);
        BBNumber operand_two = Log10(log_base);
        return new BBNumber(operand_one / operand_two);
    }

    public static BBNumber Round(BBNumber val, int digits = 0) {
        val.significand = Math.Round(val.significand, digits, BBNumber.ROUND_MODE);
        return val;
    }

    public static BBNumber Max(BBNumber val1, BBNumber val2) {
        return (!(val1 >= val2)) ? val2 : val1;
    }

    public static BBNumber Min(BBNumber val1, BBNumber val2) {
        return (!(val1 >= val2)) ? val1 : val2;
    }

    public static BBNumber Ceiling(BBNumber val) {
        if (val.exponent > (double)BBNumber.ROUND_SIG_DIGITS) {
            return val;
        }
        return new BBNumber(Math.Ceiling(Math.Round((double)val, BBNumber.ROUND_SIG_DIGITS, BBNumber.ROUND_MODE)));
    }

    public static BBNumber Floor(BBNumber val) {
        if (val.exponent > (double)BBNumber.ROUND_SIG_DIGITS) {
            return val;
        }
        return new BBNumber(Math.Floor(Math.Round((double)val, BBNumber.ROUND_SIG_DIGITS, BBNumber.ROUND_MODE)));
    }

    public static BBNumber Clamp(BBNumber targetValue, BBNumber min, BBNumber max) {
        BBNumber nsdouble = targetValue;
        if (nsdouble < min) { nsdouble = min; }
        if (nsdouble > max) { nsdouble = max; }
        return nsdouble;
    }

    public static BBNumber Abs(BBNumber val) {
        val.significand = Math.Abs(val.significand);
        return val;
    }

    public static BBNumber Rebalance(BBNumber val) {
        double num = val.significand;
        if (num >= 10.0 || num < 1.0) {
            val = new BBNumber(val.significand, val.exponent);
        }
        return val;
    }

    public static bool IsInfinity(BBNumber value) {
        return double.IsInfinity(value.significand) || double.IsInfinity(value.exponent);
    }

    public static bool IsNaN(BBNumber value) {
        return double.IsNaN(value.significand) || double.IsNaN(value.exponent);
    }
    public static bool Approximately(BBNumber a, BBNumber b)
    {
        bool sa = Math.Abs(a.significand - b.significand) < 0.0000001;
        bool ea = Math.Abs(a.exponent - b.exponent) < 0.0000001;
        return sa && ea;
    }

    public static BBNumber operator +(BBNumber value) {
        return value;
    }

    public static BBNumber operator +(BBNumber x, BBNumber y) {
        if (x.IsZero) {
            return y;
        }
        if (y.IsZero) {
            return x;
        }

        //두 수의 차이가 10^(ROUND_SIG_DIGITS + 1)배를 넘는 경우 연산을 무시하고 큰수 반환
        if (x.exponent - y.exponent > ROUND_SIG_DIGITS + 1) {
            return x;
        }
        if (y.exponent - x.exponent > ROUND_SIG_DIGITS + 1) {
            return y;
        }

        if (x.exponent < y.exponent) {
            x.significand *= Math.Pow(0.1, y.exponent - x.exponent);
            x.exponent = y.exponent;
        } else if (x.exponent > y.exponent) {
            y.significand *= Math.Pow(0.1, x.exponent - y.exponent);
        }
        x.significand += y.significand;
        y.exponent = Math.Floor(Math.Log10(Math.Abs(x.significand)));
        x.significand *= Math.Pow(0.1, y.exponent);
        x.exponent += y.exponent;
        if (BBNumber.Abs(x) < BBNumber.Epsilon) {
            x = BBNumber.Zero;
        }
        if (BBNumber.IsNaN(x)) {
            x = BBNumber.Zero;
        }
        return x;
    }

    public static BBNumber operator -(BBNumber value) {
        value.significand = -value.significand;
        return value;
    }

    public static BBNumber operator -(BBNumber x, BBNumber y) {
        if (y.IsZero) {
            return x;
        }
        if (x.IsZero) {
            return -y;
        }

        //두 수의 차이가 10^(ROUND_SIG_DIGITS + 1)배를 넘는 경우 연산을 무시하고 큰수 반환
        if (x.exponent - y.exponent > ROUND_SIG_DIGITS + 1) {
            return x;
        }
        if (y.exponent - x.exponent > ROUND_SIG_DIGITS + 1) {
            return -y;
        }

        if (x.exponent < y.exponent) {
            x.significand *= Math.Pow(0.1, y.exponent - x.exponent);
            x.exponent = y.exponent;
        } else if (x.exponent > y.exponent) {
            y.significand *= Math.Pow(0.1, x.exponent - y.exponent);
        }
        x.significand -= y.significand;
        double num = Math.Abs(x.significand);
        if (num >= 10.0) {
            x.significand *= 0.1;
            x.exponent += 1.0;
        } else if (num < 1.0 && num > 0.0) {
            int sigDiff = (int)(Math.Log10(1 / num) + 1);
            x.significand *= (Math.Pow(10, sigDiff));
            x.exponent -= (1.0 * sigDiff);
        }
        if (BBNumber.Abs(x) < BBNumber.Epsilon) {
            x = BBNumber.Zero;
        }
        if (BBNumber.IsNaN(x)) {
            x = BBNumber.Zero;
        }
        return x;
    }

    public static BBNumber operator *(BBNumber x, BBNumber y) {
        if (x.IsZero || y.IsZero) {
            return BBNumber.Zero;
        }
        x.significand *= y.significand;
        x.exponent += y.exponent;
        double num = Math.Abs(x.significand);
        if (num >= 10.0) {
            x.significand *= 0.1;
            x.exponent += 1.0;
        } else if (num < 1.0) {
            x.significand *= 10.0;
            x.exponent -= 1.0;
        }
        if (BBNumber.Abs(x) < BBNumber.Epsilon) {
            x = BBNumber.Zero;
        }
        return x;
    }

    public static BBNumber operator /(BBNumber x, BBNumber y) {
        if (y.IsZero) {
            //throw new ArgumentException("CANNOT_DIVIDE_BY_ZERO");
            return 0;
        }
        if (y == BBNumber.One) {
            return x;
        }
        if (y == BBNumber.MinusOne) {
            return -x;
        }
        x.significand /= y.significand;
        x.exponent -= y.exponent;
        double num = Math.Abs(x.significand);
        if (num >= 10.0) {
            x.significand *= 0.1;
            x.exponent += 1.0;
        } else if (num < 1.0) {
            x.significand *= 10.0;
            x.exponent -= 1.0;
        }
        if (BBNumber.Abs(x) < BBNumber.Epsilon) {
            x = BBNumber.Zero;
        }
        return x;
    }

    public static bool operator ==(BBNumber x, BBNumber y) {
        return object.Equals(x, y);
    }

    public static bool operator !=(BBNumber x, BBNumber y) {
        return !object.Equals(x, y);
    }

    public static bool operator <(BBNumber x, BBNumber y) {
        return x.CompareTo(y) < 0;
    }

    public static bool operator <=(BBNumber x, BBNumber y) {
        return x.CompareTo(y) <= 0;
    }

    public static bool operator >(BBNumber x, BBNumber y) {
        return x.CompareTo(y) > 0;
    }

    public static bool operator >=(BBNumber x, BBNumber y) {
        return x.CompareTo(y) >= 0;
    }

    public static implicit operator BBNumber(int value) {
        return new BBNumber((double)value);
    }

    public static implicit operator BBNumber(long value) {
        return new BBNumber((double)value);
    }

    public static implicit operator BBNumber(float value) {
        return new BBNumber((double)value);
    }

    public static implicit operator BBNumber(double value) {
        return new BBNumber(value);
    }

    public static explicit operator int(BBNumber value) {
        if (value >= BBNumber.MAX_INT) {
            return 2147483647;
        }
        if (value <= BBNumber.MIN_INT) {
            return -2147483648;
        }
        return (int)(value.significand * Math.Pow(10.0, value.exponent));
    }

    public static explicit operator long(BBNumber value) {
        if (value >= BBNumber.MAX_LONG) {
            return 9223372036854775807L;
        }
        if (value <= BBNumber.MIN_LONG) {
            return -9223372036854775808L;
        }
        return (long)(value.significand * Math.Pow(10.0, value.exponent));
    }

    public static explicit operator float(BBNumber value) {
        if (value >= BBNumber.MAX_FLOAT) {
            return 3.40282347E+38f;
        }
        if (value <= BBNumber.MIN_FLOAT) {
            return -3.40282347E+38f;
        }
        return (float)(value.significand * Math.Pow(10.0, value.exponent));
    }

    public static explicit operator double(BBNumber value) {
        if (value >= BBNumber.MAX_DOUBLE) {
            return 1.7976931348623157E+308;
        }
        if (value <= BBNumber.MIN_DOUBLE) {
            return -1.7976931348623157E+308;
        }
        return value.significand * Math.Pow(10.0, value.exponent);
    }



    public BigInteger ToStr()
    {
        if (significand < 0 || exponent < 0)
        {
            return BigInteger.Parse(zeroStr, CultureInfo.InvariantCulture);
        }

        string signStr = significand.ToString(CultureInfo.InvariantCulture).Replace(commaStr, blankStr);
        string s = oneStr;
        int expCount = (int)exponent;
        int zeroCount = expCount - (signStr.Length - 1);
        int signCount = expCount + 1;
        for (int i = 0; i < zeroCount; i++)
        {
            s += zeroStr;
        }
        signCount = signStr.Length > signCount ? signCount : signStr.Length;
        signStr = signStr.Substring(0, signCount);

        BigInteger sBig = BigInteger.Parse(s, CultureInfo.InvariantCulture);
        BigInteger signBig = BigInteger.Parse(signStr, CultureInfo.InvariantCulture);

        return sBig * signBig;
    }
    public double GetDouble()
    {
        try
        {
            if (exponent < -5)
            {
                return 0;
            }

            double p = Math.Pow(10.0, exponent);
            return significand * p;
        }
        catch
        {
            return 0;
        }
    }
    public float GetFloat()
    {
        try
        {
            if (exponent < -5)
            {
                return 0;
            }

            float p = (float)Math.Pow(10.0, exponent);
            return (float)significand * p;
        }
        catch
        {
            return 0;
        }
    }
    public string ToCountString()
    {
        if (exponent <= 10)
        {
            return this.ToString("D");
        }
        else
        {
            return this.ToString();
        }
    }
    public static BBNumber ToBBStr(string value)
    {
        try
        {
            return new BBNumber(value);
        }
        catch
        {
            int length = value.Length - 1;
            string sValueStr = value.Replace(zeroStr, "");

            double s = double.Parse(sValueStr, CultureInfo.InvariantCulture);
            double e = length;

            BBNumber bBNumber = new BBNumber();
            bBNumber.significand = s;
            bBNumber.exponent = e;

            return bBNumber;
        }
    }
}
