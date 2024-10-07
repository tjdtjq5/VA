using System;

public class PlayerDataManager
{
    CharacterPlayerDataC _character = new CharacterPlayerDataC();
    public void DbGets(Action callback = null)
    {
        Managers.Web.SendGetRequest<PlayerDataGetsResponse>("playerData/gets", (_result) =>
        {
            _character.Sets(_result.Characters);
            callback.Invoke();
        });
    }
    public CharacterPlayerDataC Character => _character;
}