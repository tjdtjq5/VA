public class TableManagerPacket
{
    public static void Add(string tableName)
    {
        string lower = tableName.ToLower_H();

        string variF = CSharpHelper.Format_H(variFormat, tableName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, tableName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, tableName, lower);
        string initF = CSharpHelper.Format_H(initFormat, lower);

        string check = "public class TableManager";
        bool variExist = SimpleFormat.InnerExist(typeof(TableManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(TableManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(TableManager), check, dbF);
        bool initExist = SimpleFormat.InnerExist(typeof(TableManager), check, initF);

        if (!variExist)
        {
            SimpleFormat.InnerTypeUpperAdd(typeof(TableManager), variF);
        }

        if (!funcExist)
        {
            SimpleFormat.InnerTypeUnderAdd(typeof(TableManager), funcF);
        }

        string file = $"{FileHelper.GetScriptPath(typeof(TableManager))}";

        if (!dbExist)
        {
            SimpleFormat.InnerUpperAdd(file, dbGetsCheckFormat, dbF);
        }

        if (!initExist)
        {
            SimpleFormat.InnerUnderAdd(file, initCheckFormat, initF);
        }
    }
    public static void Remove(string tableName)
    {
        string lower = tableName.ToLower_H();

        string variF = CSharpHelper.Format_H(variFormat, tableName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, tableName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, tableName, lower);
        string initF = CSharpHelper.Format_H(initFormat, lower);

        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), variF);
        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), funcF);
        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), dbF);
        SimpleFormat.InnerTypeDataRemove(typeof(TableManager), initF);
    }
    public static bool Exist(string tableName)
    {
        string lower = tableName.ToLower_H();

        string check = "public class TableManager";

        string variF = CSharpHelper.Format_H(variFormat, tableName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, tableName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, tableName, lower);
        string initF = CSharpHelper.Format_H(initFormat, lower);

        bool variExist = SimpleFormat.InnerExist(typeof(TableManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(TableManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(TableManager), check, dbF);
        bool initExist = SimpleFormat.InnerExist(typeof(TableManager), check, initF);

        return variExist && funcExist && dbExist && initExist;
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
    static string initCheckFormat =
@"public void Initialize()";
    // {0} TableName ToLower
    static string initFormat =
@"        _{0}Table.InitialData();";
    #endregion
}
