using System;

public class PlayerDataManager
{
    ItemPlayerDataC _item = new ItemPlayerDataC(); 
    CharacterPlayerDataC _character = new CharacterPlayerDataC(); 
    public void DbGets(Action callback = null)
    {
        Managers.Web.SendGetRequest<PlayerDataGetsResponse>("playerData/gets", (_result) =>
        {
            _item.Sets(_result.Items);
            _character.Sets(_result.Characters);
            callback.Invoke();
        });
    }
    public CharacterPlayerDataC Character => _character;
    public ItemPlayerDataC Item => _item;
}