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

    public PlayerItemData GetItem(ItemTableCodeDefine item)
    {
        PlayerItemData getData = Gets().Find(i => i.ItemCode.Equals(item.ToString()));
        if (getData == null)
        {
            return new PlayerItemData() { ItemCode = item.ToString() };
        }
        else
        {
            return getData;
        }
    }

    public override void InitialData()
    {
        List<PlayerItemData> datas = new List<PlayerItemData>()
        {
            new PlayerItemData() { ItemCode = "Elixir", SignValue = 500000 },
            new PlayerItemData() { ItemCode = "TalentConverter", SignValue = 500000 },
            new PlayerItemData() { ItemCode = "Cobalt", SignValue = 500000 },
            new PlayerItemData() { ItemCode = "Gesso", SignValue = 500000 },
            new PlayerItemData() { ItemCode = "Cubic", SignValue = 500000 },
            new PlayerItemData() { ItemCode = "MeteoriteDebris", SignValue = 500000 },
        };
        Sets(datas);
    }
}
