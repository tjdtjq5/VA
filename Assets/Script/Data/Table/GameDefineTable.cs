using System;
using System.Collections.Generic;
using System.Linq;

public class GameDefineTable : Table<GameDefineTableData>
{
    protected override string TableName => "GameDefine";

    public override void DbGets(Action<List<GameDefineTableData>> result)
    {
        Managers.Web.SendGetRequest<GameDefineTableGetsResponse>("gameDefineTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }

    public override GameDefineTableData Get(object key)
        => Gets().Where(d => d.gameDefineCode.Equals(key.ToString())).FirstOrDefault();

    public override void InitialData()
    {
        List<GameDefineTableData> datas = new List<GameDefineTableData>()
        {
            new GameDefineTableData() { gameDefineCode = "GN_C_Level_UP", value = "Elixir", tipName = "캐릭터레벨업할때필요한재화",  },
        };
        Push(datas);
    }
}