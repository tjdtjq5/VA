using System.Collections.Generic;

public class GameOptionManager
{
    static OptionFile optionFile = new OptionFile();
    static Dictionary<ServerUrlType, string> serverUrlDics = new Dictionary<ServerUrlType, string>();

	public static ServerUrlType ServerUrlType { get; } = ServerUrlType.ReleaseUrl;
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
            string url = optionFile.Read<string>(serverUrlType.ToString());
            serverUrlDics.Add(serverUrlType, url);
            return url;
        }
    }

	public static void ChangeServerUrl(ServerUrlType changeUrlType)
    {
        ServerUrlType beforeUrl = ServerUrlType;

        AndroidManifestFormat.DeepLinkSetting(beforeUrl, changeUrlType);
        GoogleServiceFormat.RedirectUrlSetting(beforeUrl, changeUrlType);
        GameOptionManagerPacket.ServerUrlChange(changeUrlType);
	}
}
