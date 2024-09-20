using System;

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

    public static void Remove(Type scriptType, string lineFormat)
    {
        string file = $"{FileHelper.GetScriptPath(scriptType)}";

        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"SimpleFormat Remove Not Found File Error\nscriptType : {scriptType.Name}");
            return;
        }

        Remove(file, lineFormat);
    }
    public static void Remove(string file, string lineFormat)
    {
        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(lineFormat))
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
}
