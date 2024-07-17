using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnumPacketFormat
{
    static string[] folders = new string[] { "Script", "Data" };
    static string fileName = "EnumData.cs";

    public static void Create(string enumName, List<string> values)
    {
        string file = $"{FileHelper.GetFile(folders, fileName)}";

        string total = "";

        if (Exist(enumName))
        {
            UnityHelper.LogError_H($"EnumPacketFormat Create Exist Error\nenumName : {enumName}");
            return;
        }

        string readAll = File.ReadAllText(file);
        total += readAll;

        string valueText = "";
        for (int i = 0; i < values.Count; i++)
        {
            valueText += '\t' + CSharpHelper.Format_H(enumValueFormat, values[i]) + '\n';
        }
        if (!string.IsNullOrEmpty(valueText))
        {
            valueText = valueText.Substring(0, valueText.Length - 1);
        }

        string markText = CSharpHelper.Format_H(markFormat, enumName);

        string enumNameText = CSharpHelper.Format_H(enumFormat, enumName, valueText, markText);

        total += enumNameText;

        FileHelper.Write(file, total,true);
    }
    public static void Delete(string enumName)
    {
        if (!Exist(enumName)) 
        {
            UnityHelper.LogError_H($"EnumPacketFormat Delete Not Exist Error\nenumName : {enumName}");
            return;
        }

        string file = $"{FileHelper.GetFile(folders, fileName)}";
        string total = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        foreach (string line in File.ReadAllLines(file))
        {
            string findMark = CSharpHelper.Format_H(markFormat, enumName);

            if (line.Contains(findMark))
            {
                readCheck = false;
            }

            if (readCheck)
                total += line + '\n';

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                    readCheck = true;
            }
        }

        FileHelper.Write(file, total, true);
    }
    public static List<string> GetValues(string enumName)
    {
        if (!Exist(enumName))
        {
            // UnityHelper.LogError_H($"EnumPacketFormat AddValue Not Exist Error\nenumName : {enumName}");
            return null;
        }

        string file = $"{FileHelper.GetFile(folders, fileName)}";
        string enumText = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        
        foreach (string line in File.ReadAllLines(file))
        {
            string findMark = CSharpHelper.Format_H(markFormat, enumName);

            if (line.Contains(findMark))
            {
                readCheck = false;
            }

            if (!readCheck)
                enumText += line;

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                    readCheck = true;
            }
        }

        int leftBracketIndex = enumText.IndexOf('{') + 1;

        enumText = enumText.Substring(leftBracketIndex, enumText.Length - leftBracketIndex);
        int rightBracketIndex = enumText.IndexOf('}');
        enumText = enumText.Substring(0, rightBracketIndex);

        string[] values = enumText.Split(',');
        for (int i = 0; i < values.Length; i++)
            values[i] = values[i].Trim();

        List<string> result = new List<string>();

        for (int i = 0; i < values.Length; i++)
        {
            if (!string.IsNullOrEmpty(values[i]))
            {
                result.Add(values[i]);
            }
        }

        return result;
    }
    public static void AddValue(string enumName, string value)
    {
        if (!Exist(enumName))
        {
            UnityHelper.LogError_H($"EnumPacketFormat AddValue Not Exist Error\nenumName : {enumName}");
            return;
        }

        string file = $"{FileHelper.GetFile(folders, fileName)}";
        string total = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool modifyCheck = false;
        foreach (string line in File.ReadAllLines(file))
        {
            string findMark = CSharpHelper.Format_H(markFormat, enumName);

            if (line.Contains(findMark))
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
                    string addLine = $"\t{value},\n" + "}\n";
                    addLine = line.Replace("}", addLine);
                    total += addLine;
                    modifyCheck = true;
                    readCheck = true;
                }
            }

            if (!modifyCheck)
            {
                total += line + '\n';
            }

            modifyCheck = false;
        }

        FileHelper.Write(file, total, true);
    }
    public static void RemoveValue(string enumName, string value) 
    {
        if (!Exist(enumName))
        {
            UnityHelper.LogError_H($"EnumPacketFormat RemoveValue Not Exist Error\nenumName : {enumName}");
            return;
        }

        string file = $"{FileHelper.GetFile(folders, fileName)}";
        string total = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool isNoReadCheck = false;
        string addReadLine = "";
        foreach (string line in File.ReadAllLines(file))
        {
            string findMark = CSharpHelper.Format_H(markFormat, enumName);

            if (line.Contains(findMark))
            {
                readCheck = false;
            }

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount > 0)
                {
                    addReadLine = "";

                    string tempLine = line.Replace("{", "");
                    tempLine = tempLine.Replace("}", "");
                    string[] lineValues = tempLine.Split(',');

                    for (int i = 0; i < lineValues.Length; i++) 
                    {
                        string lineValue = lineValues[i].Trim();
                        if (lineValue.Equals(value))
                        {
                            isNoReadCheck = true;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(lineValue))
                            {
                                addReadLine += "\t" + lineValue + "," + "\n";
                            }
                        }
                    }
            
                }

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = true;
                }

                if (readCheck)
                {
                    addReadLine = "{" + addReadLine + "}\n";
                }
            }

            if (!isNoReadCheck)
            {
                total += line + '\n';
            }
            else
            {
                total += addReadLine;
                isNoReadCheck = false;
            }
        }

        FileHelper.Write(file, total, true);
    }
    public static void ModifyValue(string enumName, string originValue, string nextValue)
    {
        if (!Exist(enumName))
        {
            UnityHelper.LogError_H($"EnumPacketFormat ModifyValue Not Exist Error\nenumName : {enumName}");
            return;
        }

        string file = $"{FileHelper.GetFile(folders, fileName)}";
        string total = "";

        bool readCheck = true;
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool isNoReadCheck = false;
        string addReadLine = "";
        foreach (string line in File.ReadAllLines(file))
        {
            string findMark = CSharpHelper.Format_H(markFormat, enumName);

            if (line.Contains(findMark))
            {
                readCheck = false;
            }

            if (!readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;
                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount > 0)
                {
                    addReadLine = "";

                    string tempLine = line.Replace("{", "");
                    tempLine = tempLine.Replace("}", "");
                    string[] lineValues = tempLine.Split(',');

                    for (int i = 0; i < lineValues.Length; i++)
                    {
                        string lineValue = lineValues[i].Trim();
                        if (lineValue.Equals(originValue))
                        {
                            isNoReadCheck = true;
                            addReadLine += "\t" + nextValue + "," + "\n";
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(lineValue))
                            {
                                addReadLine += "\t" + lineValue + "," + "\n";
                            }
                        }
                    }

                }

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = true;
                }

                if (readCheck)
                {
                    addReadLine = "{" + addReadLine + "}\n";
                }
            }

            if (!isNoReadCheck)
            {
                total += line + '\n';
            }
            else
            {
                total += addReadLine;
                isNoReadCheck = false;
            }
        }

        FileHelper.Write(file, total, true);
    }
    public static bool Exist(string enumName)
    {
        string file = $"{FileHelper.GetFile(folders, fileName)}";

        if (File.Exists(file))
        {
            string readAll = File.ReadAllText(file);
            string findMark = CSharpHelper.Format_H(markFormat, enumName);

            return readAll.Contains(findMark);
        }
        else
        {
            return false;
        }
    }

    #region Format
    // {0} : EnumName
    // {1} : ValueFormatList
    // {2} : markFormat
    public static string enumFormat =
@"public enum {0} // {2}
{{
{1}
}}
";
    // {0} EnumValue
    public static string enumValueFormat = 
@"{0},";

    static string markFormat = "BeginMark_{0}_EndMark";
    #endregion
}
