public class AndroidManifestFormat
{
    static SecretOptionFile secretOption = new SecretOptionFile();

    public static void DeepLinkSetting(ServerUrlType beforeUrlType, ServerUrlType afterUrlType)
    {
        string androidManifestFile = secretOption.Read<string>("AndroidManifestPath");
        string beforeUrl = GameOptionManager.GetServerUrl(beforeUrlType);
        string afterUrl = GameOptionManager.GetServerUrl(afterUrlType);

        string beforeFormat = CSharpHelper.Format_H(deepLinkGoogleLoginFormat, beforeUrl);
        string afterFormat = CSharpHelper.Format_H(deepLinkGoogleLoginFormat, afterUrl);

        SimpleFormat.Replace(androidManifestFile, beforeFormat, afterFormat);
    }

    #region Format
    // {0} : Server Url
    public static string deepLinkGoogleLoginFormat =
@"					android:host=""{0}""";
    #endregion
}
