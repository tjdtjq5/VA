public class TableFunctionPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void ChangeUpdateFunction(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"TableFunctionPacket ChangeUpdateFunction Error Must Link TableFunction.cs");
            return;
        }

        bool check = IsCheckUpdateTable(tableName);

        if (check)
        {
            UnityHelper.LogError_H($"TableFunctionPacket ChangeUpdateFunction Error Alread Setting\tableName : {tableName}");
            return;
        }

        string text = "";
        string checkUpdateTable = CSharpHelper.Format_H(checkUpdateTableFormat, tableName);
        string updateTableF = CSharpHelper.Format_H(updateTableFormat, tableName);

        int leftBracketCount = 0;
        int rightBracketCount = 0;
        bool readCheck = false;

        foreach (var line in FileHelper.ReadLines(file))
        {
            bool isWrite = false;

            if (line.Trim().Contains(checkUpdateTableCheckFormat))
            {
                isWrite = true;

                text += $"\t{checkUpdateTable}\n";
            }

            if (line.Trim().Equals(checkUpdateTableStartFormat))
            {
                readCheck = true;

                text += $"{line}\n";
                text += "\t{\n";

                text += $"{updateTableF}\n";

                text += "\t}\n";
            }

            if (readCheck)
            {
                isWrite = true;
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

            if (!isWrite)
            {
                text += $"{line}\n";
                isWrite = false;
            }
        }

        FileHelper.Write(file, text, true);
    }
    public static bool IsCheckUpdateTable(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableFunction.cs");
            return false;
        }

        string checkF = CSharpHelper.Format_H(checkUpdateTableFormat, tableName);

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Equals(checkF))
            {
                return true;
            }
        }

        return false;
    }

    static string GetFile()
    {
        string file = secretFile.Read<string>("TableFunctionPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"TableFunctionPacket GetTableFile No Linked File");
            return "";
        }
    }

    #region Format
    static string checkUpdateTableCheckFormat =
@"public static string GetUpdateTableName";
    // {0} Table Name
    static string checkUpdateTableFormat =
@"public static string GetUpdateTableName {{ get => ""{0}""; }}";
    static string checkUpdateTableStartFormat =
@"public static async void UpdateTable(string tableName, string tableData)";
    // {0} Table Name
    static string updateTableFormat =
@"        if (!tableName.Equals(GetUpdateTableName))
        {{
            UnityHelper.LogError_H($""Error Deference Table Function"");
            return;
        }}

        List<{0}TableData> tableDatas = GoogleSpreadSheetUtils.GetListTableDatas<{0}TableData>(tableName, tableData);

        string addUrl = $""{{CSharpHelper.StartCharToLower(tableName)}}Table/update"";
        var result = await WebTaskCall.Post<{0}TableResponse>(true, addUrl, tableDatas);

        string resultSeri = CSharpHelper.SerializeObject(result);
        UnityHelper.Log_H(resultSeri);";
    #endregion
}
