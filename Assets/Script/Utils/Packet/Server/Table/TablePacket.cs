using System.Collections.Generic;

public class TablePacket
{
    public static void Add(string tableName, string tableData)
    {
        string file = Path(tableName);

        if (!Exist(tableName))
        {
            string lowerTableName = tableName.ToLower_H();
            string format = CSharpHelper.Format_H(TableScriptFormat, tableName, lowerTableName);

            FileHelper.Write(file, format, true);
        }

        string initCheckF = CSharpHelper.Format_H(initCheckFormat, tableName);
        SimpleFormat.RemoveStructInner(file, initCheckF);

        int dataCount = GoogleSpreadSheetUtils.GetDataCount(tableData);
        List<string> variables = GoogleSpreadSheetUtils.GetVariables(tableData);

        for (int i = 0; i < dataCount; i++)
        {
            string tdFormats = "";
            for (int j = 0; j < variables.Count; j++)
            {
                string variable = variables[j].ToLower_H();
                string value = GoogleSpreadSheetUtils.GetValueData(tableData, i, j);

                if (!value.IsNumber())
                    value = $"\"{value}\"";

                string tdFormat = CSharpHelper.Format_H(tableDataFormat, variable, value);
                tdFormats += $"{tdFormat} ";
            }

            string initForm = CSharpHelper.Format_H(initFormat, tableName, tdFormats);
            SimpleFormat.InnerUnderAdd(file, initCheckF, initForm);
        }
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
    protected override string TableName => ""{0}"";

    public override void DbGets(Action<List<{0}TableData>> result)
    {{
        Managers.Web.SendGetRequest<{0}TableGetsResponse>(""{1}Table/gets"", (_result) =>
        {{
            datas = _result.datas;
            result.Invoke(_result.datas);
        }});
    }}

    public override void InitialData()
    {{
        List<{0}TableData> datas = new List<{0}TableData>()
        {{
        }};
        Push(datas);
    }}
}}
";
    // {0} TableName
    static string initCheckFormat =
@"List<{0}TableData> datas = new List<{0}TableData>()";
    // {0} TableName
    // {1} TableFormatData
    static string initFormat =
@"            new {0}TableData() {{ {1} }},";
    // {0} Variable
    // {1} Value
    static string tableDataFormat =
@"{0} = {1},";
}
