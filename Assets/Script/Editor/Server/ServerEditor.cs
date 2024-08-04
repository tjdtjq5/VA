using UnityEditor;
using UnityEngine;

public class ServerEditor
{
    static SecretOptionFile secretFileTxt = new SecretOptionFile();

    [MenuItem("Server/LocalhostStart")]
    public static void LocalhostStart()
    {
        FileHelper.ProcessStart(secretFileTxt.Read("LocalhostStartPath"));
    }
    [MenuItem("Server/ServerSlnFile")]
    public static void ServerSlnFile()
    {
        FileHelper.ProcessStart(secretFileTxt.Read("ServerSlnPath"));
    }
    [MenuItem("Server/NgrokStart")]
    public static void NgrokStart()
    {
        FileHelper.ProcessStart(secretFileTxt.Read("NgrokPath"));
    }
    [MenuItem("Server/DefineCopyStart")]
    public static void DefineCopyStart()
    {
        FileHelper.ProcessStart(secretFileTxt.Read("DefineCopyPath"));
    }
    public static void StartServer()
    {
        // 서버 실행 
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
