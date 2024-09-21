using System;
using System.Collections.Generic;

public class ItemTable : Table<ItemTableData>
{
    public override void DbGets(Action<List<ItemTableData>> result)
    {
        Managers.Web.SendGetRequest<ItemTableGetsResponse>("itemTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }
}
