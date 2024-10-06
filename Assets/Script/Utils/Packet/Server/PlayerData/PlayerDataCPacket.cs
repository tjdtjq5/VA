public class PlayerDataCPacket
{
    public static void Create(string playerDataName)
    {
        if (Exist(playerDataName))
            return;

        string filePathName = FilePathName(playerDataName);
        string text = CSharpHelper.Format_H(filePacket, playerDataName, playerDataName.ToLower_H(1));

        FileHelper.Write(filePathName, text, true);
    }
    public static void Remove(string playerDataName)
    {
        if (!Exist(playerDataName))
            return;

        string filePathName = FilePathName(playerDataName);

        FileHelper.FileDelete(filePathName, true);
    }
    public static bool Exist(string playerDataName)
    {
        return FileHelper.ScriptExist(TypeName(playerDataName));
    }
    public static string FilePathName(string playerDataName)
    {
        string filePath = FileHelper.GetScriptPath(typeof(PlayerDataClassSelecter));
        return filePath.Replace("PlayerDataClassSelecter.cs", $"{TypeName(playerDataName)}.cs");
    }
    static string TypeName(string playerDataName)
    {
        return $"{playerDataName}PlayerDataC";
    }

    #region Format
    // {0} : PlayerDataName
    // {1} : PlayerDataName _ First Lower
    static string filePacket =
@"using System;
using System.Collections.Generic;

public class {0}PlayerDataC : PlayerDataC<{0}PlayerData>
{{
    public override void DbGets(Action<List<{0}PlayerData>> result)
    {{
        Managers.Web.SendGetRequest<List<{0}PlayerData>>(""{1}PlayerData/gets"", (_result) =>
        {{
            datas = _result;
            result.Invoke(_result);
        }});
    }}

    public override List<Stat> GetStats()
    {{
        throw new NotImplementedException();
    }}
}}";
    #endregion
}
