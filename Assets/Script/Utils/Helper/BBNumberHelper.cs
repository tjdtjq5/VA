using System;
using System.Globalization;

public static class BBNumberHelper
{
    private static string ZERO = "0";
    private static int UNICODE_A = 97;
    private static int UNICODE_ALPHABET_COUNT = 26;
    private static int ALPHABET_UNIT = 3;

    public static string Alphabet(this BBNumber bbNumber)
    {
        bbNumber = new BBNumber(bbNumber.significand, bbNumber.exponent);
        double e = bbNumber.exponent;

        int ei = (int)e;
        int en = ei / ALPHABET_UNIT;

        double s = AlphabetSign(bbNumber);

        string a = "";

        while (en > 0)
        {
            en--;
            Tuple<int, int> enTuple = AlphabetExp(en);
            en = enTuple.Item1;
            int enn = enTuple.Item2;
            int unicode = enn + UNICODE_A;
            a = Convert.ToChar(unicode) + a;
        }

        return $"{s}{a}";
    }
    public static double AlphabetSign(BBNumber bbNumber)
    {
        double s = bbNumber.significand;
        double e = bbNumber.exponent;

        int ei = (int)e;
        int enn = ei % ALPHABET_UNIT;

        s *= Math.Pow(10.0f, enn);
        s = Math.Round(s, 2);

        return s;
    }
    public static Tuple<int, int> AlphabetExp(int endm)
    {
        return Tuple.Create(endm / UNICODE_ALPHABET_COUNT, endm % UNICODE_ALPHABET_COUNT);
    }
    public static double ToDouble(this BBNumber bbNumber)
    {
        try
        {
            if (bbNumber.exponent < -5)
            {
                return 0;
            }

            double p = Math.Pow(10.0, bbNumber.exponent);
            return bbNumber.significand * p;
        }
        catch (Exception e)
        {
            UnityHelper.Error_H($"BBNumberHelper ToDouble Error\ne : {e.Message}");
            throw;
        }
    }
    public static float ToFloat(this BBNumber bbNumber)
    {
        try
        {
            if (bbNumber.exponent < -5)
                return 0;

            float p = (float)Math.Pow(10.0, bbNumber.exponent);
            return (float)bbNumber.significand * p;
        }
        catch (Exception e)
        {
            UnityHelper.Error_H($"BBNumberHelper ToFloat Error\ne : {e.Message}");
            throw;
        }
    }
    public static int ToInt(this BBNumber bbNumber)
    {
        try
        {
            if (bbNumber.exponent < -5)
                return 0;

            int p = (int)Math.Pow(10.0, bbNumber.exponent);
            return (int)bbNumber.significand * p;
        }
        catch(Exception e)
        {
            UnityHelper.Error_H($"BBNumberHelper ToInt Error\ne : {e.Message}");
            throw;
        }
    }
    public static string ToCountString(this BBNumber bbNumber)
    {
        if (bbNumber.exponent <= 10)
            return bbNumber.ToString("D");
        else
            return bbNumber.ToString();
    }
    public static BBNumber ToBBNumber(this string value)
    {
        try
        {
            return new BBNumber(value);
        }
        catch
        {
            int length = value.Length - 1;
            string sValueStr = value.Replace(ZERO, "");

            double s = double.Parse(sValueStr, CultureInfo.InvariantCulture);
            double e = length;

            BBNumber bBNumber = new BBNumber();
            bBNumber.significand = s;
            bBNumber.exponent = e;

            return bBNumber;
        }
    }
}
