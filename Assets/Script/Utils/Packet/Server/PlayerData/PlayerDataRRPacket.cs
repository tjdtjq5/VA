using System;

public class PlayerDataRRPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();
    public static void Add(string playerDataName)
    {
        string file = GetFile();
        string check = responseCheckFormat;

        string resF = CSharpHelper.Format_H(resFormat, playerDataName);
        bool resExist = SimpleFormat.InnerExist(file, check, resF);

        if (!resExist)
            SimpleFormat.InnerUnderAdd(file, responseCheckFormat, resF);
    }
    public static void Remove(string playerDataName)
    {
        string file = GetFile();

        string resF = CSharpHelper.Format_H(resFormat, playerDataName);

        SimpleFormat.InnerRemove(file, responseCheckFormat, resF);
    }
    public static bool Exist(string playerDataName)
    {
        string file = GetFile();
        string check = responseCheckFormat;

        string resF = CSharpHelper.Format_H(resFormat, playerDataName);
        bool resExist = SimpleFormat.InnerExist(file, check, resF);

        return resExist;
    }
    public static string GetFile()
    {
        string file = secretFile.Read("PlayerDataRRPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"PlayerDataRRPacket GetFile No Linked File");
            return "";
        }
    }

    #region Format
    // {0} PlayerData Name
    static string responseCheckFormat =
@"public class PlayerDataGetsResponse";
    static string resFormat =
@"    public List<{0}PlayerData> {0}s {{ get; set; }}";
    #endregion
}
