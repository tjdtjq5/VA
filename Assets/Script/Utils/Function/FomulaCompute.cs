using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;

public static class FomulaCompute
{
    const string Plus = "#";
    const string Minus = "-";
    const string Multiple = "*";
    const string Divide = "/";
    const string Root = "^";
    const string ParenthesesLeft = "(";
    const string ParenthesesRight = ")";

    public static BBNumber Compute(string expression, params BBNumber[] filter)
    {
        expression = expression.Replace("+", Plus);
        expression = CSharpHelper.Format_H(expression, filter);
        expression = MatchExprs(expression);

        return Compute(expression);
    }
    static BBNumber Compute(string expression)
    {
        string[] exprs = Exprs(expression);

        if (exprs.Contains(Multiple) || exprs.Contains(Divide) || exprs.Contains(Root))
            exprs = ExprsCalculate(exprs, Multiple, Divide, Root);

        if (exprs.Contains(Plus) || exprs.Contains(Minus))
            exprs = ExprsCalculate(exprs, Plus, Minus);

        if (exprs.Length == 1)
            return Calculate(exprs[0]);
        else
        {
            UnityHelper.Error_H($"The exprs is incorrect\nlast epxrs : {exprs[exprs.Length - 1]}");
            return 0;
        }
    }

    static string MatchExprs(string expression)
    {
        Regex reg = new Regex(@"\(([^)]*)\)");
        MatchCollection resultColl = reg.Matches(expression);

        while (resultColl.Count > 0)
        {
            foreach (Match match in resultColl)
            {
                Group g = match.Groups[0];
                string gStr = g.Value;

                gStr = gStr.Replace(ParenthesesLeft, string.Empty);
                gStr = gStr.Replace(ParenthesesRight, string.Empty);

                BBNumber gExpression = Compute(gStr);
                gStr = $"{ParenthesesLeft}{gStr}{ParenthesesRight}";
                expression = expression.Replace(gStr, gExpression.ToString());
            }

            resultColl = reg.Matches(expression);
        }

        return expression;
    }
    static string[] Exprs(string expression) 
    {
        expression = expression.Replace(" ", "");

        List<string> results = new List<string>();

        string exp = "";
        for (int i = 0; i < expression.Length; i++) 
        {
            if (IsSignCheck(expression[i].ToString()))
            {
                results.Add(exp);
                results.Add(expression[i].ToString());
                exp = "";
            }
            else
            {
                exp += expression[i];
            }
        }
        results.Add(exp);

        return results.ToArray();
    }
    static string[] ExprsCalculate(string[] exprs, params string[] sign)
    {
        if (exprs.Length <= 1 || sign.Length < 1)
            return exprs;

        if (exprs.Length % 2 == 0)
        {
            UnityHelper.Error_H($"The exprs is incorrect\nlast epxrs : {exprs[exprs.Length - 1]}");
            return exprs;
        }

        List<string> results = new List<string>();
        for (int i = 0; i < exprs.Length; i ++)
        {
            string exp = exprs[i];
            if (sign.Contains(exp))
            {
                if (results.Count != 0)
                    results.RemoveAt(results.Count - 1);

                string expr = exprs[i - 1] + exp + exprs[i + 1];
                results.Add(Calculate(expr).ToString());

                i++;
                exprs[i] = Calculate(expr).ToString();
            }
            else
            {
                results.Add(exp);
            }
        }

        return results.ToArray();
    }
    static BBNumber Calculate(string expr)
    {
        int findIndex = -1;

        if (expr.Contains(Plus))
            findIndex = expr.IndexOf(Plus);
        else if (expr.Contains(Minus))
            findIndex = expr.IndexOf(Minus);
        else if (expr.Contains(Multiple))
            findIndex = expr.IndexOf(Multiple);
        else if (expr.Contains(Divide))
            findIndex = expr.IndexOf(Divide);
        else if (expr.Contains(Root))
            findIndex = expr.IndexOf(Root);

        if (findIndex == -1)
            return expr.ToBBNumber();

        string aStr = expr.Substring(0, findIndex);
        string bStr = expr.Substring(findIndex + 1, expr.Length - findIndex - 1);
        BBNumber a = 0;
        BBNumber b = 0;

        if (IsSignSplit(aStr))
            a = Calculate(aStr);
        else
            a = aStr.ToBBNumber();

        if (IsSignSplit(bStr))
            b = Calculate(bStr);
        else
            b = bStr.ToBBNumber();

        if (expr.Contains(Plus))
            return PlusC(a, b);
        else if (expr.Contains(Minus))
            return MinusC(a, b);
        else if (expr.Contains(Multiple))
            return MultipleC(a, b);
        else if (expr.Contains(Divide))
            return DivideC(a, b);
        else if (expr.Contains(Root))
            return RootC(a, b.ToFloat());

        return 0;
    }

    static BBNumber PlusC(BBNumber a, BBNumber b) => a + b;
    static BBNumber MinusC(BBNumber a, BBNumber b) => a - b;
    static BBNumber MultipleC(BBNumber a, BBNumber b) => a * b;
    static BBNumber DivideC(BBNumber a, BBNumber b) => a / b;
    static BBNumber RootC(BBNumber a, float b) => BBNumber.Pow(a,b);

    static bool IsSignSplit(string expr) => expr.Contains(Plus) || expr.Contains(Minus) || expr.Contains(Multiple) || expr.Contains(Divide) || expr.Contains(Root);
    static bool IsSignCheck(string expr) => expr.Equals(Plus) || expr.Equals(Minus) || expr.Equals(Multiple) || expr.Equals(Divide) || expr.Equals(Root);
}
