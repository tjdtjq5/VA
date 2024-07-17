using System.Collections.Generic;

public class GameOptionManager
{
    static OptionFile optionFile = new OptionFile();
    static Dictionary<ServerUrlType, string> serverUrlDics = new Dictionary<ServerUrlType, string>();

	public static ServerUrlType ServerUrlType { get; } = ServerUrlType.LocalhostUrl;
	public static string GetServerUrl
	{
		get
		{
			if (serverUrlDics.TryGetValue(ServerUrlType, out string result))
			{
				return result;
			}
			else
			{
				string url = optionFile.Read<string>(ServerUrlType.ToString());
				serverUrlDics.Add(ServerUrlType, url);
				return url;
            }
		}
	}
}
