public class GoogleServiceFormat
{
    static SecretOptionFile secretOption = new SecretOptionFile();

    public static void RedirectUrlSetting(ServerUrlType beforeUrlType, ServerUrlType afterUrlType)
    {
        string file = secretOption.Read<string>("GoogleServicePath");
        string beforeUrl = GameOptionManager.GetServerUrl(beforeUrlType);
        string afterUrl = GameOptionManager.GetServerUrl(afterUrlType);

        string beforeFormat = CSharpHelper.Format_H(redirectUrlFormat, beforeUrl);
        string afterFormat = CSharpHelper.Format_H(redirectUrlFormat, afterUrl);

        SimpleFormat.Replace(file, beforeFormat, afterFormat);
    }

    #region Format
    // {0} : Server Url
    public static string redirectUrlFormat =
@"string _redirect_uri = ""{0}/auth/google/callback"";";
    #endregion
}
