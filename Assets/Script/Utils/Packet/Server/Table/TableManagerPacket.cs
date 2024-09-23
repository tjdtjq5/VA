using Unity.VisualScripting;

public class TableManagerPacket
{
    public static void Add(string tableName)
    {
        string lower = tableName.FirstCharacterToLower();

        string variF = CSharpHelper.Format_H(variFormat, tableName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, tableName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, tableName, lower);

        bool variExist = SimpleFormat.Exist(typeof(TableManager), variF);
        bool funcExist = SimpleFormat.Exist(typeof(TableManager), funcF);
        bool dbExist = SimpleFormat.Exist(typeof(TableManager), dbF);

        if (!variExist)
        {
            SimpleFormat.InnerTypeUpperAdd(typeof(TableManager), variF);
        }

        if (!funcExist)
        {
            SimpleFormat.InnerTypeUnderAdd(typeof(TableManager), funcF);
        }

        if (!dbExist)
        {
            string file = $"{FileHelper.GetScriptPath(typeof(TableManager))}";
            SimpleFormat.InnerUpperAdd(file, dbGetsCheckFormat, dbF);
        }
    }
    public static void Remove(string tableName)
    {
        string lower = tableName.FirstCharacterToLower();

        string variF = CSharpHelper.Format_H(variFormat, tableName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, tableName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, tableName, lower);

        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), variF);
        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), funcF);
        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), dbF);
    }
    public static bool Exist(string tableName)
    {
        string lower = tableName.FirstCharacterToLower();

        string check = "public class TableManager";

        string variF = CSharpHelper.Format_H(variFormat, tableName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, tableName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, tableName, lower);

        bool variExist = SimpleFormat.InnerExist(typeof(TableManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(TableManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(TableManager), check, dbF);

        return variExist && funcExist && dbExist;
    }

    #region Format
    // {0} TableName
    // {1} Lower TableName
    static string variFormat =
@"    {0}Table _{1}Table = new {0}Table();";
    static string funcFormat =
@"    public {0}Table {0}Table => _{1}Table;";
    static string dbGetsCheckFormat =
@"switch (res.datas[i].tableName)";
    static string dbGetsFormat =
@"                    case ""{0}"": _{1}Table.Push(CSharpHelper.DeserializeObject<List<{0}TableData>>(res.datas[i].tableDatas)); break;";
    #endregion
}
