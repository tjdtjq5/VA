public class GameOptionManagerPacket
{
    static SecretOptionFile secretFile = new SecretOptionFile();

    public static void ServerUrlChange(ServerUrlType type)
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link GameOptionManager.cs");
            return;
        }

        bool isExist = Exist();

        if (!isExist)
        {
            UnityHelper.LogError_H($"GameOptionManagerPacket Error Not Exist Code");
            return;
        }

        string text = "";
        string checkF = serverUrlCheck;
        string serverUrlF = CSharpHelper.Format_H(serverUrlFormat, type.ToString());

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Contains(checkF))
            {
                text += $"\t{serverUrlF}\n";
            }
            else
            {
                text += $"{line}\n";
            }
        }

        FileHelper.Write(file, text, true);
    }
    public static bool Exist()
    {
        string file = GetFile();
        if (string.IsNullOrEmpty(file))
        {
            UnityHelper.LogError_H($"Must Link TableController.cs");
            return false;
        }

        string checkF = serverUrlCheck;

        foreach (var line in FileHelper.ReadLines(file))
        {
            if (line.Trim().Contains(checkF))
            {
                return true;
            }
        }

        return false;
    }
    static string GetFile()
    {
        string file = secretFile.Read("GameOptionManagerPath");

        if (FileHelper.FileExist(file))
        {
            return file;
        }
        else
        {
            UnityHelper.LogError_H($"GameOptionManagerPacket GetTableFile No Linked File\nfile : {file}");
            return "";
        }
    }

    #region Format
    static string serverUrlCheck =
@"public static ServerUrlType ServerUrlType";
    // {0} Server Url Type
    static string serverUrlFormat =
@"public static ServerUrlType ServerUrlType {{ get; }} = ServerUrlType.{0};";
    #endregion
}
