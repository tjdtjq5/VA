using System;
using System.Collections.Generic;

public class GameDefineTable : Table<GameDefineTableData>
{
    public override void DbGets(Action<List<GameDefineTableData>> result)
    {
        Managers.Web.SendGetRequest<GameDefineTableGetsResponse>("gameDefineTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }
}
