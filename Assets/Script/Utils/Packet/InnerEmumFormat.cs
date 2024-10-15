using System;

public class InnerEmumFormat
{
    public static void Set(Type type, string enumName, string[] enumValues)
    {
        if (string.IsNullOrEmpty(enumName) || enumValues.Length <= 0)
        {
            UnityHelper.Error_H($"InnerEmumFormat EnumName Or Value Null Error");
            return;
        }

        Remove(type, enumName);

        string file = $"{FileHelper.GetScriptPath(type)}";

        string text = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool writeCheck = false;

        foreach (string line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Contains(CSharpHelper.Format_H(classFormat,type.Name)))
            {
                readCheck = false;
            }

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = true;
                    writeCheck = true;

                    string valueFormats = "";
                    for (int i = 0; i < enumValues.Length; i++) 
                    {
                        string value = enumValues[i];
                        string valueF = "\t" + CSharpHelper.Format_H(enumValueFormat, value);
                        valueFormats += "\t" + valueF + "\n";
                    }
                    valueFormats = valueFormats.Substring(0, valueFormats.Length - 1);

                    string enumF = CSharpHelper.Format_H(enumFormat, enumName, valueFormats);

                    text += "\t" + enumF;
                    text += line + "\n";
                }
            }

            if (!writeCheck)
            {
                text += line + "\n";
            }
            else
                writeCheck = false;
        }

        FileHelper.Write(file, text, true);
    }
    static void Remove(Type type, string enumName)
    {
        string file = $"{FileHelper.GetScriptPath(type)}";

        string text = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool writeCheck = false;

        int leftBracketCount_check = 0;
        int rightBracketCount_check = 0;

        foreach (string line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Contains(CSharpHelper.Format_H(classFormat, type.Name)))
            {
                readCheck = false;
            }

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (line.Trim().Equals(CSharpHelper.Format_H(enumCheckFormat, enumName)))
                {
                    writeCheck = true;
                }

                if (writeCheck)
                {
                    if (line.Contains('{'))
                        leftBracketCount_check++;
                    if (line.Contains('}'))
                        rightBracketCount_check++;

                    if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                    {
                        writeCheck = false;
                    }
                }

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = true;
                }
            }

            if (!writeCheck)
                text += line + '\n';
        }

        FileHelper.Write(file, text, true);
    }

    // {0} : TypeName
    public static string classFormat =
@"public class {0} :";

    // {0} : EnumName
    public static string enumCheckFormat =
@"public enum {0}";

    // {0} : EnumName
    // {1} : ValueFormatList
    public static string enumFormat =
@"public enum {0}
    {{
{1}
    }}
";
    // {0} EnumValue
    public static string enumValueFormat =
@"{0},";
}
