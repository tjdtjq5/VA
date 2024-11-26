using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerDataManager
{
    ItemPlayerDataC _item = new ItemPlayerDataC(); 
    public void Initialize()
    {
        _item.InitialData();
    }
    public void DbGets(Action callback = null)
    {
        Managers.Web.SendGetRequest<PlayerDataGetsResponse>("playerData/gets", (_result) =>
        {
            _item.Sets(_result.Items);
            callback.Invoke();
        });
    }
    public ItemPlayerDataC Item => _item;
}