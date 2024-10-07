using System;
using System.Collections.Generic;

public class CharacterTable : Table<CharacterTableData>
{
    public override void DbGets(Action<List<CharacterTableData>> result)
    {
        Managers.Web.SendGetRequest<CharacterTableGetsResponse>("characterTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }
}
