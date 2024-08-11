using System;

public class SimpleFormat
{
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
}
