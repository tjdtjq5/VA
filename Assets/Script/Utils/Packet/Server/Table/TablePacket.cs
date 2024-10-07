using Unity.VisualScripting;

public class TablePacket
{
    public static void Add(string tableName)
    {
        if (Exist(tableName))
            return;

        string file = Path(tableName);

        string lowerTableName = tableName.FirstCharacterToLower();
        string format = CSharpHelper.Format_H(TableScriptFormat, tableName, lowerTableName);

        FileHelper.Write(file, format, true);
    }
    public static void Remove(string tableName)
    {
        string file = Path(tableName);
        bool exist = FileHelper.FileExist(file);

        if (exist)
            FileHelper.FileDelete(file, true);
    }
    static bool Exist(string tableName)
    {
        string path = FileHelper.GetScriptPath($"{tableName}Table");
        return !string.IsNullOrEmpty(path);
    }
    static string Path(string tableName)
    {
        string tablePath = FileHelper.GetScriptPath("Table");
        tablePath = tablePath.Replace("/Table.cs", "");
        string resultPath = tablePath + $"/{tableName}Table.cs";
        return resultPath;
    }

    // {0} TableName
    // {1} Lower TableName
    static string TableScriptFormat =
@"using System;
using System.Collections.Generic;

public class {0}Table : Table<{0}TableData>
{{
    public override void DbGets(Action<List<{0}TableData>> result)
    {{
        Managers.Web.SendGetRequest<{0}TableGetsResponse>(""{1}Table/gets"", (_result) =>
        {{
            datas = _result.datas;
            result.Invoke(_result.datas);
        }});
    }}
}}
";
}
