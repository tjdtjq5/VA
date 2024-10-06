public class PlayerDataControllerPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();
    public static void Add(string playerDataName)
    {
        string file = GetFile();
        string check = getsCheckFormat;

        string getsF = CSharpHelper.Format_H(getsFormat, playerDataName, playerDataName.ToLower_H(1));
        bool getsExist = SimpleFormat.InnerExist(file, check, getsF);

        if (!getsExist)
            SimpleFormat.InnerUnderAdd(file, getsCheckFormat, getsEndCheckFormat, getsF);
    }
    public static void Remove(string playerDataName)
    {
        string file = GetFile();

        string getsF = CSharpHelper.Format_H(getsFormat, playerDataName, playerDataName.ToLower_H(1));

        SimpleFormat.InnerRemove(file, getsCheckFormat, getsF);
    }
    public static bool Exist(string playerDataName)
    {
        string file = GetFile();

        string getsF = CSharpHelper.Format_H(getsFormat, playerDataName, playerDataName.ToLower_H(1));
        bool getsExist = SimpleFormat.InnerExist(file, getsCheckFormat, getsF);

        return getsExist;
    }
    public static string GetFile()
    {
        string file = secretFile.Read("PlayerDataControllerPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"PlayerDataControllerPacket GetFile No Linked File");
            return "";
        }
    }
    #region Format
    // {0} PlayerData Name
    // {1} PlayerData Name toLower
    static string getsCheckFormat =
@"public PlayerDataGetsResponse Gets()";
    static string getsEndCheckFormat =
@"return res;";
    static string getsFormat =
@"        res.{0}s = _{1}Service.DbGets(accountId);";
    #endregion
}
