using System.Collections.Generic;

public class TableServicePacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();
    public static void Create(string tableName, string tableData)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"TableServicePacket Create Error Must Link TableService.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (isExist)
        {
            UnityHelper.LogError_H($"TableServicePacket Create Error Alread Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";

        foreach (var line in FileHelper.ReadLines(file))
        {
            text += $"{line}\n";
        }

        text += '\n';

        string classF = CSharpHelper.Format_H(classLineFormat, tableName);
        text += $"{classF}\n{{\n";

        string contextF = CSharpHelper.Format_H(contextFormat, tableName);
        text += $"\t{contextF}\n";

        string createF = CSharpHelper.Format_H(createrFormat, tableName);
        text += $"\t{createF}\n";

        string updateF = CSharpHelper.Format_H(updateFormat, tableName, LowerTableName(tableName), KeyValueName(tableData));
        text += $"\t{updateF}\n";

        string getsF = CSharpHelper.Format_H(getsFormat, tableName);
        text += $"\t{getsF}\n";

        string dbDataF = CSharpHelper.Format_H(getDbDataByTableDataFormat, tableName);
        text += $"\t{dbDataF}\n";

        text += "}";


        FileHelper.Write(file, text, false);
    }
    public static void Remove(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableService.cs");
            return;
        }

        bool isExist = Exist(tableName);

        if (!isExist)
        {
            UnityHelper.LogError_H($"TableServicePacket Remove Error Not Exist Table\nTable : {tableName}");
            return;
        }

        string text = "";
        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;
        string checkF = CSharpHelper.Format_H(classLineFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Contains(checkF))
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
    public static bool Exist(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return false;
        }

        string tableCheckF = CSharpHelper.Format_H(classLineFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(tableCheckF))
            {
                return true;
            }
        }

        return false;
    }

    static string GetFile()
    {
        string file = secretFile.Read("TableServicePath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"TableServicePacket GetTableFile No Linked File");
            return "";
        }
    }
    static string LowerTableName(string tableName)
    {
        return CSharpHelper.StartCharToLower(tableName);
    }
    static string KeyValueName(string tableData)
    {
        List<string> variableNames = VariableNames(tableData);
        if (variableNames.Count >= 1)
        {
            return CSharpHelper.StartCharToLower(variableNames[0]);
        }
        else
        {
            return "";
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

    #region Format

    // {0} Table Name
    static string classLineFormat =
@"public class {0}TableService";

    // {0} Table Name
    static string contextFormat =
@"ApplicationDbContext _context;
    ILogger<{0}TableService> _logger;";

    // {0} Table Name
    static string createrFormat =
@"public {0}TableService(ApplicationDbContext context, ILogger<{0}TableService> logger)
    {{
        this._context = context;
        this._logger = logger;
    }}";

    // {0} Table Name
    // {1} Lower Table Name
    // {2} Key Value Name
    static string updateFormat =
@"public List<string> Update(List<{0}TableData> allDatas)
    {{
        var {1}s = _context.{0}s;

        List<string> changeDatas = new List<string>();
        List<{0}Db> removeDbs = new List<{0}Db>();
        List<{0}Db> addDbs = new List<{0}Db>();

        foreach (var {1} in {1}s)
        {{
            // keyValue
            string {2} = {1}.{2};

            int findIndex = allDatas.FindIndex(d => d.{2} == {2});

            if (findIndex < 0)
            {{
                removeDbs.Add({1});
                changeDatas.Add({1}Code);
            }}
            else
            {{
                {0}TableData tableData = allDatas[findIndex];
                bool isDeferenceCheck = {1}.SetCopyValue(tableData);

                if (isDeferenceCheck)
                    changeDatas.Add({1}Code);

                allDatas.RemoveAt(findIndex);
            }}
        }}

        _context.{0}s.RemoveRange_H(removeDbs, true, _logger);

        for (int i = 0; i < allDatas.Count; i++)
        {{
            changeDatas.Add(allDatas[i].{1}Code);
            addDbs.Add(GetDbDataByTableData(allDatas[i]));
        }}

        _context.{0}s.AddRange_H(addDbs, true, _logger);

        {1}s = _context.{0}s;

        _context.SaveChanges_H(true, _logger);

        return changeDatas;
    }}";

    // {0} Table Name
    static string getsFormat =
@"public List<{0}Db> Gets()
    {{
        List<{0}TableData> results = new List<{0}TableData>();
        var datas = _context.{0}s;

        results.SetCopyValue(datas.ToList());

        return results;
    }}
";

    // {0} Table Name
    static string getDbDataByTableDataFormat =
@"{0}Db GetDbDataByTableData({0}TableData tableData)
    {{
        {0}Db result = new {0}Db();
        result.SetCopyValue(tableData);
        return result;
    }}";
    #endregion
}
