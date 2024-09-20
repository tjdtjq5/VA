using System.Collections.Generic;

public class GameOptionManager
{
    static OptionFile optionFile = new OptionFile();
    static Dictionary<ServerUrlType, string> serverUrlDics = new Dictionary<ServerUrlType, string>();

    public static bool IsRelease { get; } = false;

	public static ServerUrlType ServerUrlType { get; } = ServerUrlType.DebugUrl;
	public static string GetCurrentServerUrl
	{
		get
		{
			return GetServerUrl(ServerUrlType);
        }
	}
	public static string GetServerUrl(ServerUrlType serverUrlType)
	{
        if (serverUrlDics.TryGetValue(serverUrlType, out string result))
        {
            return result;
        }
        else
        {
            string url = optionFile.Read(serverUrlType.ToString());
            serverUrlDics.Add(serverUrlType, url);
            return url;
        }
    }
    public static string GetHost(ServerUrlType serverUrlType)
    {
        string url = GetServerUrl(serverUrlType);
        string result = url.Replace("http://", "").Replace("https://", "");
        if (result.Contains(':'))
        {
            int index = result.IndexOf(":");
            result = result.Substring(0, index);
        }

        return result;
    }
    public static string GetScheme(ServerUrlType serverUrlType)
    {
        string url = GetServerUrl(serverUrlType);
        string[] datas = url.Split("://");
        return datas[0];
    }


    public static void ChangeServerUrl(ServerUrlType changeUrlType)
    {
        ServerUrlType beforeUrl = ServerUrlType;

        GameOptionManagerPacket.ServerUrlChange(changeUrlType);
	}
    public static void SetRelease(bool flag)
    {
        GameOptionManagerPacket.SetRelease(flag);
    }
}
