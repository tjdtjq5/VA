public class GoogleSheetFile : SecretFileTxt
{
    public override string FileName => "GoogleSheetFile.txt";

    public GoogleSheetFileModel Read(string key)
    {
        return base.Read<GoogleSheetFileModel>(key);
    }
}
public class GoogleSheetFileModel
{
    public long sheetId;
    public string tableName;
    public string range;

    public GoogleSheetFileModel(long sheetId, string tableName, string range)
    {
        this.sheetId = sheetId;
        this.range = range;
        this.tableName = tableName;
    }
}