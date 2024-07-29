public class AndroidManifestFormat
{
    static SecretOptionFile secretOption = new SecretOptionFile();

    public static void DeepLinkSetting(ServerUrlType beforeUrlType, ServerUrlType afterUrlType)
    {
        string androidManifestFile = secretOption.Read<string>("AndroidManifestPath");
        string beforeHostF = GameOptionManager.GetHost(beforeUrlType);
        string afterHostF = GameOptionManager.GetHost(afterUrlType);

        string beforeFormat = CSharpHelper.Format_H(deepLinkGoogleLoginFormat, beforeHostF);
        string afterFormat = CSharpHelper.Format_H(deepLinkGoogleLoginFormat, afterHostF);

        string beforeScheme = GameOptionManager.GetScheme(beforeUrlType);
        string afterScheme = GameOptionManager.GetScheme(afterUrlType);

        string beforeSchemeFormat = CSharpHelper.Format_H(schemeFormat, beforeScheme);
        string afterSchemeFormat = CSharpHelper.Format_H(schemeFormat, afterScheme);

        SimpleFormat.Replace(androidManifestFile, beforeFormat, afterFormat);
        SimpleFormat.Replace(androidManifestFile, beforeSchemeFormat, afterSchemeFormat);
    }

    #region Format
    // {0} : Server Url
    public static string deepLinkGoogleLoginFormat =
@"android:host=""{0}""";

    // {0} : Scheme
    public static string schemeFormat =
@"android:scheme=""{0}""";
    #endregion
}
