public class PlayerDataManagerPacket
{
    public static void Add(string playerDataName)
    {
        string lower = playerDataName.ToLower_H();

        string variF = CSharpHelper.Format_H(variFormat, playerDataName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, playerDataName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, playerDataName, lower);
        string initF = CSharpHelper.Format_H(initFormat, lower);

        string check = "public class PlayerDataManager";
        bool variExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, dbF);
        bool initExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, initF);

        if (!variExist)
        {
            SimpleFormat.InnerTypeUpperAdd(typeof(PlayerDataManager), variF);
        }

        if (!funcExist)
        {
            SimpleFormat.InnerTypeUnderAdd(typeof(PlayerDataManager), funcF);
        }

        string file = $"{FileHelper.GetScriptPath(typeof(PlayerDataManager))}";

        if (!dbExist)
        {
            SimpleFormat.InnerUnderAdd(file, dbGetsCheckFormat, dbGetsEndCheckFormat, dbF);
        }

        if (!initExist)
        {
            SimpleFormat.InnerUnderAdd(file, initCheckFormat, initF);
        }
    }
    public static void Remove(string playerDataName)
    {
        string lower = playerDataName.ToLower_H();

        string variF = CSharpHelper.Format_H(variFormat, playerDataName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, playerDataName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, playerDataName, lower);
        string initF = CSharpHelper.Format_H(initFormat, lower);

        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), variF);
        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), funcF);
        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), dbF);
        SimpleFormat.InnerTypeDataRemove(typeof(PlayerDataManager), initF);
    }
    public static bool Exist(string playerDataName)
    {
        string lower = playerDataName.ToLower_H();

        string check = "public class PlayerDataManager";

        string variF = CSharpHelper.Format_H(variFormat, playerDataName, lower);
        string funcF = CSharpHelper.Format_H(funcFormat, playerDataName, lower);
        string dbF = CSharpHelper.Format_H(dbGetsFormat, playerDataName, lower);
        string initF = CSharpHelper.Format_H(initFormat, lower);

        bool variExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, variF);
        bool funcExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, funcF);
        bool dbExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, dbF);
        bool initExist = SimpleFormat.InnerExist(typeof(PlayerDataManager), check, initF);

        return variExist && funcExist && dbExist && initExist;
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
    static string initCheckFormat =
@"public void Initialize()";
    // {0} PlayerData Name ToLower
    static string initFormat =
@"        _{0}.InitialData();";
    #endregion
}
