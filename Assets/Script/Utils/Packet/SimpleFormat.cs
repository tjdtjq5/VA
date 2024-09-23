using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Timeline;

public class SimpleFormat
{
    public static void OuterCreate(Type scriptType, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat OuterCreate Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        OuterCreate(file, format);
    }
    public static void OuterCreate(string file, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat OuterCreate Not Found File Error\nfile : {file}");
            return;
        }

        string text = "";

        foreach (var line in FileHelper.ReadLines(file))
        {
            text += $"{line}\n";
        }

        text += '\n';

        text += $"{format}";

        FileHelper.Write(file, text, true);
    }

    public static void InnerTypeUpperAdd(Type scriptType, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerClassUpperAdd Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        string className = scriptType.Name;
        InnerTypeUpperAdd(file, className, format);
    }
    public static void InnerTypeUpperAdd(string file, string className, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerClassUpperAdd Not Found File Error\nfile : {file}");
            return;
        }

        string checkClassFormat = $"public class {className}";

        InnerUpperAdd(file, checkClassFormat, format);
    }
    public static void InnerTypeUnderAdd(Type scriptType, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerClassUnderAdd Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        string className = scriptType.Name;
        InnerTypeUnderAdd(file, className, format);
    }
    public static void InnerTypeUnderAdd(string file, string className, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerClassUpperAdd Not Found File Error\nfile : {file}");
            return;
        }

        string checkClassFormat = $"public class {className}";

        InnerUnderAdd(file, checkClassFormat, format);
    }

    public static void InnerEnumUpperAdd(Type scriptType, string enumName, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerEnumUpperAdd Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        InnerEnumUpperAdd(file, enumName, format);
    }
    public static void InnerEnumUpperAdd(string file, string enumName, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerEnumUpperAdd Not Found File Error\nfile : {file}");
            return;
        }

        string checkEnumFormat = $"public enum {enumName}";

        InnerUpperAdd(file, checkEnumFormat, format);
    }
    public static void InnerEnumUnderAdd(Type scriptType, string enumName, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerEnumUnderAdd Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        InnerEnumUnderAdd(file, enumName, format);
    }
    public static void InnerEnumUnderAdd(string file, string enumName, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerEnumUnderAdd Not Found File Error\nfile : {file}");
            return;
        }

        string checkEnumFormat = $"public enum {enumName}";

        InnerUnderAdd(file, checkEnumFormat, format);
    }

    public static void InnerUpperAdd(string file, string checkFormat, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerUpperAdd Not Found File Error\nfile : {file}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;
        bool isWrite = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(checkFormat))
            {
                readCheck = true;
            }

            text += $"{line}\n";

            if (readCheck && !isWrite)
            {
                if (line.Contains("{"))
                {
                    text += $"{format}" + "\n";
                    isWrite = true;
                }
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;
                }
            }
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, true);
    }
    public static void InnerUnderAdd(string file, string checkFormat, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerClassUpperAdd Not Found File Error\nfile : {file}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(checkFormat))
            {
                readCheck = true;
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;

                    if (line.Contains("}"))
                    {
                        text += $"{format}" + "\n";
                    }
                }
            }

            text += $"{line}\n";
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, true);
    }

    public static void InnerTypeDataRemove(Type scriptType, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerTypeDataRemove Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        string className = scriptType.Name;
        string checkClassFormat = $"public class {className}";

        InnerRemove(file, checkClassFormat, format);
    }
    public static void InnerEnumDataRemove(string file, string enumName, string enumData)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerEnumDataRemove Not Found File Error\nfile : {file}");
            return;
        }

        string checkEnumFormat = $"public enum {enumName}";

        InnerRemove(file, checkEnumFormat, $" {enumData}");
    }

    static void InnerRemove(string file, string checkFormat, string formatLine)
    {
        if(string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerRemove Not Found File Error\nfile : {file}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(checkFormat))
            {
                readCheck = true;
            }

            if (readCheck)
            {
                if (!line.Contains(formatLine))
                {
                    text += $"{line}\n";
                }

                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;
                }
            }
            else
            {
                text += $"{line}\n";
            }
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, true);
    }

    public static bool InnerEnumDataExist(string file, string enumName, string enumData)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerEnumDataExist Not Found File Error\nfile : {file}");
            return false;
        }

        string checkEnumFormat = $"public enum {enumName}";

        return InnerExist(file, checkEnumFormat, $" {enumData}");
    }

    public static bool InnerExist(Type scriptType, string checkFormat, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerExist Not Found File Error\nscriptType : {scriptType.Name}");
            return false;
        }

        return InnerExist(file, checkFormat, format);
    }
    public static bool InnerExist(string file, string checkFormat, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat InnerRemove Not Found File Error\nfile : {file}");
            return false;
        }

        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(checkFormat))
            {
                readCheck = true;
            }

            if (readCheck)
            {
                if (line.Contains(format))
                {
                    return true;
                }

                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;
                }
            }
        }

        return false;
    }

    public static bool Exist(Type scriptType, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Exist Not Found File Error\nscriptType : {scriptType.Name}");
            return false;
        }

        return Exist(file, format);
    }
    public static bool Exist(string file, string format)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Exist Not Found File Error\nfile : {file}");
            return false;
        }

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(format))
            {
                return true;
            }
        }

        return false;
    }

    public static void Replace(Type scriptType, string originFormat, string replaceFormat)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Replace Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        Replace(file, originFormat, replaceFormat);
    }
    public static void Replace(string file, string originFormat, string replaceFormat)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Replace Not Found File Error\nfile : {file}");
            return;
        }

        string readAll = FileHelper.ReadAll(file);

        if (!readAll.Contains(originFormat))
        {
            UnityHelper.LogError_H($"SimpleFormat Replace Not Found originFormat Error\noriginFormat : {originFormat}");
            return;
        }

        readAll = readAll.Replace(originFormat, replaceFormat);
        FileHelper.Write(file, readAll, true);
    }

    public static void Modify(Type scriptType, string lineCheckFormat, string modifyFormat)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Modify Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        Modify(file, lineCheckFormat, modifyFormat);
    }
    public static void Modify(string file, string lineCheckFormat, string modifyFormat)
    {
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Modify Not Found File Error\nfile : {file}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(lineCheckFormat))
            {
                readCheck = true;

                text += $"{modifyFormat}\n";
            }

            if (!readCheck)
            {
                text += $"{line}\n";
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;
                }
            }
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, true);
    }

    public static void RemoveStruct(Type scriptType, string lineFormat)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Remove Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        RemoveStruct(file, lineFormat);
    }
    public static void RemoveStruct(string file, string format)
    {
        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(format))
            {
                readCheck = true;
            }

            if (!readCheck)
            {
                text += $"{line}\n";
            }

            if (readCheck)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    readCheck = false;
                }
            }
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, true);
    }

    public static void RemoveLine(Type scriptType, string format)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat RemoveLine Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        RemoveLine(file, format);
    }
    public static void RemoveLine(string file, string format)
    {
        string text = "";
        bool readCheckFlag = false;

        string[] exCheck = new string[] { "if (" };
        bool exCheckFlag = false;
        int leftBracketCount = 0;
        int rightBracketCount = 0;

        List<string> lineFormats = format.Split('\n').ToList();
        for (int i = 0; i < lineFormats.Count; i++)
        {
            string lf = lineFormats[i].Trim();
            if (!lf.Equals("{") && !lf.Equals("}"))
                lineFormats[i] = lineFormats[i].Trim();
        }

        foreach (var line in FileHelper.ReadLines(file))
        {
            string checkLine = line;
            checkLine = checkLine.Trim();

            for (int i = 0; i < lineFormats.Count; i++)
            {
                if (checkLine.Equals(lineFormats[i]))
                {
                    for (int j = 0; j < exCheck.Length; j++)
                    {
                        if (lineFormats[i].Contains(exCheck[j]))
                        {
                            exCheckFlag = true;
                        }
                    }
                    readCheckFlag = true;
                }
            }

            if (checkLine.Equals(format))
                readCheckFlag = true;

            if (!readCheckFlag && !exCheckFlag)
                text += $"{line}\n";

            if (exCheckFlag)
            {
                if (line.Contains('{'))
                    leftBracketCount++;

                if (line.Contains('}'))
                    rightBracketCount++;

                if (leftBracketCount != 0 && leftBracketCount == rightBracketCount)
                {
                    exCheckFlag = false;
                }
            }

            readCheckFlag = false;
        }

        text = text.Substring(0, text.Length - 1);
        FileHelper.Write(file, text, true);
    }
}
