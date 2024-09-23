public class MasterTableServicePacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void Create(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return;
        }

        string checkDbSetFormat = $"public void {DbRadisSetCheck}()";
        string dbSetCheckLineFormat = CSharpHelper.Format_H(DbRadisSetCheckLine, tableName, LowerTableName(tableName));
        bool checkDbSet = SimpleFormat.InnerExist(file, checkDbSetFormat, dbSetCheckLineFormat);

        if (!checkDbSet)
        {
            string dbSetFormat = CSharpHelper.Format_H(DbRadisSetFormat, tableName, LowerTableName(tableName));
            SimpleFormat.InnerUnderAdd(file, checkDbSetFormat, dbSetFormat);
        }

        string checkDbNullFormat = $"public void {DbRadisNullSetCheck}()";
        string dbNullCheckLineFormat = CSharpHelper.Format_H(DbRadisNullSetCheckLine, tableName, LowerTableName(tableName));
        bool checkDbNull = SimpleFormat.InnerExist(file, checkDbNullFormat, dbNullCheckLineFormat);

        if (!checkDbNull)
        {
            string dbNullFormat = CSharpHelper.Format_H(DbRadisNullSetFormat, tableName, LowerTableName(tableName));
            SimpleFormat.InnerUnderAdd(file, checkDbNullFormat, dbNullFormat);
        }
    }
    public static void Remove(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return;
        }

        string checkDbSetFormat = $"public void {DbRadisSetCheck}()";
        string dbSetCheckLineFormat = CSharpHelper.Format_H(DbRadisSetCheckLine, tableName, LowerTableName(tableName));
        bool checkDbSet = SimpleFormat.InnerExist(file, checkDbSetFormat, dbSetCheckLineFormat);

        if (checkDbSet)
        {
            string dbSetFormat = CSharpHelper.Format_H(DbRadisSetFormat, tableName, LowerTableName(tableName));
            SimpleFormat.RemoveLine(file, dbSetFormat);
        }

        string checkDbNullFormat = $"public void {DbRadisNullSetCheck}()";
        string dbNullCheckLineFormat = CSharpHelper.Format_H(DbRadisNullSetCheckLine, tableName, LowerTableName(tableName));
        bool checkDbNull = SimpleFormat.InnerExist(file, checkDbNullFormat, dbNullCheckLineFormat);

        if (checkDbNull)
        {
            string dbNullFormat = CSharpHelper.Format_H(DbRadisNullSetFormat, tableName, LowerTableName(tableName));
            SimpleFormat.RemoveLine(file, dbNullFormat);
        }
    }
    public static bool Exist(string tableName)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return false;
        }

        string checkDbSetFormat = $"public void {DbRadisSetCheck}()";
        string dbSetCheckLineFormat = CSharpHelper.Format_H(DbRadisSetCheckLine, tableName, LowerTableName(tableName));
        bool checkDbSet = SimpleFormat.InnerExist(file, checkDbSetFormat, dbSetCheckLineFormat);

        string checkDbNullFormat = $"public void {DbRadisNullSetCheck}()";
        string dbNullCheckLineFormat = CSharpHelper.Format_H(DbRadisNullSetCheckLine, tableName, LowerTableName(tableName));
        bool checkDbNull = SimpleFormat.InnerExist(file, checkDbNullFormat, dbNullCheckLineFormat);

        return checkDbSet && checkDbNull;
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

    #region Format
    // {0} Table Name
    // {1} Lower Table Name
    static string DbRadisSetCheck =
@"DbRadisSet";
    static string DbRadisSetCheckLine =
@"List<{0}TableData> {1}s = new List<{0}TableData>();";
    static string DbRadisSetFormat =
@"        List<{0}TableData> {1}s = new List<{0}TableData>();
        {1}s.SetCopyValue(_context.{0}s.ToList());
        _cache.SetValue_H(RadisKey.GetTableKey(""{0}""), CSharpHelper.SerializeObject({1}s), _logger);";
    static string DbRadisNullSetCheck =
@"DbRadisNullSet";
    static string DbRadisNullSetCheckLine =
@"string? {1}TableData = _cache.GetValue_H<string>(RadisKey.GetTableKey(""{0}""), _logger);";
    static string DbRadisNullSetFormat =
@"        string? {1}TableData = _cache.GetValue_H<string>(RadisKey.GetTableKey(""{0}""), _logger);
        if (string.IsNullOrEmpty({1}TableData))
        {{
            List<{0}TableData> {1}s = new List<{0}TableData>();
            {1}s.SetCopyValue(_context.{0}s.ToList());
            _cache.SetValue_H(RadisKey.GetTableKey(""{0}""), CSharpHelper.SerializeObject({1}s), _logger);
        }}";
    #endregion
}
