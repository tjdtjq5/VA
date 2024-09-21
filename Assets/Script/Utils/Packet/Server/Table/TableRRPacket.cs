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

        string classUpdateFormat = CSharpHelper.Format_H(tableClassUpdateFormat, tableName);
        SimpleFormat.OuterCreate(file, classUpdateFormat);

        string classGetsFormat = CSharpHelper.Format_H(tableClassGetsFormat, tableName);
        SimpleFormat.OuterCreate(file, classGetsFormat);
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

        string tableCheckUpdateF = CSharpHelper.Format_H(tableCheckUpdateFormat, tableName);
        SimpleFormat.Remove(file, tableCheckUpdateF);

        string tableCheckGetsF = CSharpHelper.Format_H(tableCheckGetsFormat, tableName);
        SimpleFormat.Remove(file, tableCheckGetsF);
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

        string tableUpdateCheckF = CSharpHelper.Format_H(tableCheckUpdateFormat, tableName);
        string classUpdateFormat = CSharpHelper.Format_H(tableClassUpdateFormat, tableName);

        string tableGetsCheckF = CSharpHelper.Format_H(tableCheckGetsFormat, tableName);
        string classGetsFormat = CSharpHelper.Format_H(tableClassGetsFormat, tableName);

        SimpleFormat.Modify(file, tableUpdateCheckF, classUpdateFormat);
        SimpleFormat.Modify(file, tableGetsCheckF, classGetsFormat);
    }
    public static bool Exist(string tableName)
    {
        string file = GetTableFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableRR.cs");
            return false;
        }

        string tableUpdateCheckF = CSharpHelper.Format_H(tableCheckUpdateFormat, tableName);
        bool checkUpdateFlag = SimpleFormat.Exist(file, tableUpdateCheckF);

        string tableGetsCheckF = CSharpHelper.Format_H(tableCheckGetsFormat, tableName);
        bool checkGetsFlag = SimpleFormat.Exist(file, tableGetsCheckF);

        return checkUpdateFlag && checkGetsFlag;
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

    static string tableCheckUpdateFormat =
@"public class {0}TableUpdateResponse";

    // {0} TableName
    static string tableClassUpdateFormat =
@"public class {0}TableUpdateResponse
{{
    public string tableName {{ get; set; }}
    public List<string> changeDatas {{ get; set; }}
}}";

    static string tableCheckGetsFormat =
@"public class {0}TableGetsResponse";

    // {0} TableName
    static string tableClassGetsFormat =
@"public class {0}TableGetsResponse
{{
    public string tableName {{ get; set; }}
    public List<{0}TableData> datas {{ get; set; }}
}}";

    #endregion
}
