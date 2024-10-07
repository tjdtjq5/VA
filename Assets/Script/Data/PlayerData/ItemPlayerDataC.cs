using System;
using System.Collections.Generic;

public class ItemPlayerDataC : PlayerDataC<PlayerItemData>
{
    public override void DbGets(Action<List<PlayerItemData>> result)
    {
        Managers.Web.SendGetRequest<PlayerItemGetResponse>("playerItem/gets", (res) =>
        {
            datas = res.Datas;
            result.Invoke(res.Datas);
        });
    }

    public override List<Stat> GetStats()
    {
        return null;
    }
}
