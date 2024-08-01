using UnityEditor;
using UnityEngine;

public class ServerEditor
{
    static SecretOptionFile secretFileTxt = new SecretOptionFile();

    [MenuItem("Server/Start")]
    public static void StartServer()
    {
        // ���� ���� 
        switch (GameOptionManager.ServerUrlType)
        {
            case ServerUrlType.DebugUrl:
                FileHelper.ProcessStart(secretFileTxt.Read("ServerSlnPath"));
                break;
            case ServerUrlType.LocalhostUrl:
                FileHelper.ProcessStart(secretFileTxt.Read("LocalhostStartPath"));
                break;
            case ServerUrlType.DebugNgrok:
                FileHelper.ProcessStart(secretFileTxt.Read("NgrokPath"));
                break;
            default:
                break;
        }
    }
}
