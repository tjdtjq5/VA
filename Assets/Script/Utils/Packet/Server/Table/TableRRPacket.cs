using System;
using System.Collections.Generic;

public class TableRRPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void Create(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"TableRRPacket Create Error Must Link TableDbData.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (isExist)
        {
            UnityHelper.LogError_H($"TableRRPacket Create Error Alread Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";

        foreach (var line in FileHelper.ReadLines(file))
        {
            text += $"{line}\n";
        }

        text += '\n';

        string classFormat = CSharpHelper.Format_H(tableClassFormat, tableName);

        text += $"{classFormat}";

        FileHelper.Write(file, text, false);
    }
    public static void Remove(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableRR.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"TableRRPacket Create Error Not Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;
        string tableCheckF = CSharpHelper.Format_H(tableCheckFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(tableCheckF))
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
        FileHelper.Write(file, text, false);
    }
    public static void Modify(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableRR.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"TableRRPacket Create Error Not Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;
        string tableCheckF = CSharpHelper.Format_H(tableCheckFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(tableCheckF))
            {
                readCheck = true;

                string classFormat = CSharpHelper.Format_H(tableClassFormat, tableName);
                if (string.IsNullOrEmpty(classFormat))
                {
                    return;
                }

                text += $"{classFormat}\n";
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
        FileHelper.Write(file, text, false);
    }
    public static bool Exist(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableRR.cs");
            return false;
        }

        string tableCheckF = CSharpHelper.Format_H(tableCheckFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(tableCheckF))
            {
                return true;
            }
        }

        return false;
    }

    public static string GetTableFile()
    {
        string file = secretFile.Read("TableRRPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"TableRRPacket GetTableFile No Linked File");
            return "";
        }
    }

    #region Format

    static string tableCheckFormat =
@"public class {0}TableResponse";

    // {0} TableName
    static string tableClassFormat =
@"public class {0}TableResponse
{{
    public string tableName {{ get; set; }}
    public List<string> changeDatas {{ get; set; }}
}}";

    #endregion
}
