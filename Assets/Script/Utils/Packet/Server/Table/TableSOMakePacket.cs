public class TableSOMakePacket
{
    public static void ChangeUpdate(string tableName)
    {
        string tableCheckF = CSharpHelper.Format_H(tableCheckFormat, tableName);
        string file = FileHelper.GetScriptPath("TableSOMake");

        if (SimpleFormat.Exist(file, tableCheckF))
        {
            UnityHelper.Error_H($"TableSOMakePacket ChangeUpdateFunction Error Alread Setting\tableName : {tableName}");
            return;
        }

        SimpleFormat.InnerRemove(file, "public class TableSOMake", "public static string TableName");
        SimpleFormat.InnerTypeUpperAdd(file, "TableSOMake", $"   {tableCheckF}");

        SimpleFormat.RemoveStruct(file, removeCreateFormat);

        string createF =CSharpHelper.Format_H(createFormat, tableName);
        SimpleFormat.InnerTypeUnderAdd(file, "TableSOMake", createF);
    }
    public static bool IsCheckTable(string tableName)
    {
        string tableCheckF = CSharpHelper.Format_H(tableCheckFormat, tableName);
        string file = FileHelper.GetScriptPath("TableSOMake");
        return SimpleFormat.Exist(file, tableCheckF);
    }

    #region Format
    // {0} TableName
    static string tableCheckFormat =
@"public static string TableName {{ get => ""{0}""; }}";
    static string removeCreateFormat =
@"public static void CreateSO(string tableName, string tableData)";
    static string createFormat =
@"    public static void CreateSO(string tableName, string tableData)
    {{
        if (!tableName.Equals(TableName))
        {{
            UnityHelper.Error_H($""Error Deference Table CreateSO"");
            return;
        }}

        bool isExistSOEnum = CSharpHelper.ExistEnumData<SOTableType>(tableName);

        if (isExistSOEnum)
        {{
            List<string> tableKeys = GoogleSpreadSheetUtils.GetKeyDatas(tableData);
            for (int i = 0; i < tableKeys.Count; i++)
            {{
                string code = tableKeys[i];

                string path = DefinePath.TableSODirectory(tableName);
                if (!FileHelper.DirectoryExist(path))
                    FileHelper.DirectoryCreate(path);

                string soName = DefinePath.TableSOName(tableName, code);

                path = DefinePath.TableSOPath(tableName, code);
                var so = AssetDatabase.LoadAssetAtPath<{0}SO>(path);

                if (so == null)
                {{
                    so = CreateInstance<{0}SO>();
                    so.codeName = code;
                    AssetDatabase.CreateAsset(so, path);
                }}
            }}
        }}
    }}";
    #endregion
}
