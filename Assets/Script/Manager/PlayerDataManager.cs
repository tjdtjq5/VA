using System;
using System.Collections.Generic;

public class PlayerDataManager
{
    ItemPlayerDataC _item = new ItemPlayerDataC(); 
    CharacterPlayerDataC _character = new CharacterPlayerDataC(); 
    public void Initialize()
    {
        _item.InitialData();
        _character.InitialData();
    }
    public void DbGets(Action callback = null)
    {
        Managers.Web.SendGetRequest<PlayerDataGetsResponse>("playerData/gets", (_result) =>
        {
            _item.Sets(_result.Items);
            _character.Sets(_result.Characters);
            callback.Invoke();
        });
    }
    public List<Stat> GetStats()
    {
        List<Stat> result = new List<Stat>();

        result.AddRange(_item.GetStats());
        result.AddRange(_character.GetStats());

        return result;
    }
    public CharacterPlayerDataC Character => _character;
    public ItemPlayerDataC Item => _item;
}