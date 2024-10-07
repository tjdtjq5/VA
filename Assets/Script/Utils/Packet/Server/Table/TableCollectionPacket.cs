public class TableCollectionPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();
    static string enumName = "TableCollection";

    public static void Add(string tableName)
    {
        string file = GetFile();
        string tableF = CSharpHelper.Format_H(tableEnumDataFormat, tableName);
        SimpleFormat.InnerEnumUnderAdd(file, enumName, tableF);
    }
    public static void Remove(string tableName)
    {
        string file = GetFile();
        SimpleFormat.InnerEnumDataRemove(file, enumName, tableName);
    }
    public static bool Exist(string tableName)
    {
        string file = GetFile();
        return SimpleFormat.InnerEnumDataExist(file, enumName, tableName);
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
    static string tableEnumDataFormat =
@"    {0},";
    #endregion
}
