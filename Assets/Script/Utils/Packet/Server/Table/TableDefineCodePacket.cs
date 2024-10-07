using System.Collections.Generic;

public class TableDefineCodePacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void Update(string tableName, List<string> codeList)
    {
        Remove(tableName);

        string file = GetFile();
        string enumName = CSharpHelper.Format_H(enumNameFormat, tableName);

        string enumDatas = "";
        for (int i = 0; i < codeList.Count; i++)
        {
            enumDatas += CSharpHelper.Format_H(tableEnumDataFormat, codeList[i]) + "\n";
        }
        enumDatas = enumDatas.Substring(0, enumDatas.Length - 1);

        string formatData = CSharpHelper.Format_H(enumCreateFormat, enumName, enumDatas);

        SimpleFormat.OuterCreate(file, formatData);
    }
    public static void Remove(string tableName)
    {
        string file = GetFile();
        string enumName = CSharpHelper.Format_H(enumNameFormat, tableName);
        SimpleFormat.EnumRemove(file, enumName);
    }
    static string GetFile()
    {
        string file = secretFile.Read("DefineTablePath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"DefineTable GetTableFile No Linked File");
            return "";
        }
    }

    #region Format
    // {0} TableName
    static string enumNameFormat =
@"{0}TableCodeDefine";
    // {0} EnumName
    // {1} TableEnumDataList
    static string enumCreateFormat =
@"public enum {0}
{{
{1}
}}";
    static string tableEnumDataFormat =
@"    {0},";
    #endregion
}
