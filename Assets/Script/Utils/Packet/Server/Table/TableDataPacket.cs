using System;
using System.Collections.Generic;

public class TableDataPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void Create(string tableName, string tableData)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"TableDataPacket Create Error Must Link TableDbData.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (isExist)
        {
            UnityHelper.LogError_H($"TableDataPacket Create Error Alread Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";

        foreach (var line in FileHelper.ReadLines(file))
        {
            text += $"{line}\n";
        }

        text += '\n';

        string classFormat = GetClassFormat(tableName, tableData);
        if (string.IsNullOrEmpty(classFormat))
        {
            return;
        }

        text += $"{classFormat}";

        FileHelper.Write(file, text, false);
    }
    public static void Remove(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableData.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"TableDataPacket Remove Error Not Exist Table\nTable : {tableName}");
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
    public static void Modify(string tableName, string tableData)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableData.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"TableDataPacket Modify Error Not Exist Table\nTable : {tableName}");
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

                string classFormat = GetClassFormat(tableName, tableData);
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
            UnityHelper.LogError_H($"Must Link TableData.cs");
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
        string file = secretFile.Read("TableDataPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"TableDataPacket GetTableFile No Linked File");
            return "";
        }
    }
    public static List<Type> TableTypes(string tableData)
    {
        string[] lineDatas = tableData.Split('\n');

        try
        {
            List<Type> result = new List<Type>();

            string dataLine = lineDatas[1];

            string[] datas = dataLine.Split('\t');
            for (int i = 0; i < datas.Length; i++)
            {
                result.Add(CSharpHelper.GetType(datas[i]));
            }

            return result;
        }
        catch
        {
            UnityHelper.LogError_H($"TableDataPacket TableTypes Error\ntableData : {tableData}");
            return null;
        }
    }
    public static List<string> VariableNames(string tableData)
    {
        string[] lineDatas = tableData.Split('\n');

        try
        {
            List<string> result = new List<string>();

            string variableNameLine = lineDatas[0];

            string[] variables = variableNameLine.Split('\t');
            for (int i = 0; i < variables.Length; i++)
            {
                result.Add(variables[i]);
            }

            return result;
        }
        catch
        {
            UnityHelper.LogError_H($"TableDataPacket TableTypes Error\ntableData : {tableData}");
            return null;
        }
    }

    public static string GetClassFormat(string tableName, string tableData)
    {
        string text = "";

        List<Type> types = TableTypes(tableData);
        List<string> variables = VariableNames(tableData);

        if (types.Count != variables.Count || types.Count == 0)
        {
            UnityHelper.LogError_H($"TableDataPacket Create Error Types Or Variable Exception\nTableData : {tableData}");
            return "";
        }

        string fieldDatasText = "";
        for (int i = 0; i < types.Count; i++)
        {
            Type tp = types[i];
            string tpStr = CSharpHelper.GetTypeString(tp);
            string variName = CSharpHelper.ToLower_H(variables[i].Trim());

            string fieldF = CSharpHelper.Format_H(fieldFormat, tpStr, variName);

            fieldDatasText += $"\t{fieldF}\n";
        }
        fieldDatasText = fieldDatasText.Substring(0, fieldDatasText.Length - 1);

        string tableClassF = CSharpHelper.Format_H(tableClassFormat, tableName, fieldDatasText);
        text += $"{tableClassF}";

        return text;
    }

    #region Format

    static string tableCheckFormat =
@"[Serializable] // {0}Table";

    // {0} TableName
    // {1} FieldDatas
    static string tableClassFormat =
@"[Serializable] // {0}Table
public class {0}TableData
{{
{1}
}}";

    // {0} Type String
    // {1} Variable
    static string fieldFormat =
@"public {0} {1} {{ get; set; }}";
    #endregion
}
