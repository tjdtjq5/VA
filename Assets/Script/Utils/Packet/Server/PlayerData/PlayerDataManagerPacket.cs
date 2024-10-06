using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

public class PlayerDataManagerPacket
{
    public static void Add(string playerDataName)
    {
        string lower = playerDataName.ToLower_H();

        string variF = CSharpHelper.Format_H(variFormat, playerDataName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, playerDataName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, playerDataName, lower);

        string check = "public class PlayerDataManager";
        bool variExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, dbF);

        if (!variExist)
        {
            SimpleFormat.InnerTypeUpperAdd(typeof(PlayerDataManager), variF);
        }

        if (!funcExist)
        {
            SimpleFormat.InnerTypeUnderAdd(typeof(PlayerDataManager), funcF);
        }

        if (!dbExist)
        {
            string file = $"{FileHelper.GetScriptPath(typeof(PlayerDataManager))}";
            SimpleFormat.InnerUnderAdd(file, dbGetsCheckFormat, dbGetsEndCheckFormat, dbF);
        }
    }
    public static void Remove(string playerDataName)
    {
        string lower = playerDataName.ToLower_H();

        string variF = CSharpHelper.Format_H(variFormat, playerDataName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, playerDataName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, playerDataName, lower);

        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), variF);
        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), funcF);
        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), dbF);
    }
    public static bool Exist(string playerDataName)
    {
        string lower = playerDataName.ToLower_H();

        string check = "public class PlayerDataManager";

        string variF = CSharpHelper.Format_H(variFormat, playerDataName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, playerDataName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, playerDataName, lower);

        bool variExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, dbF);

        return variExist && funcExist && dbExist;
    }

    #region Format
    // {0} PlayerData Name
    // {1} PlayerData Name ToLower
    static string variFormat =
@"    {0}PlayerDataC _{1} = new {0}PlayerDataC();";
    static string funcFormat =
@"    public {0}PlayerDataC {0} => _{1};";
    static string dbGetsCheckFormat =
@"Managers.Web.SendGetRequest<PlayerDataGetsResponse>";
    static string dbGetsEndCheckFormat =
@"callback.Invoke();";
    static string dbGetsFormat =
@"            _{1}.Sets(_result.{0}s);";
    #endregion
}
