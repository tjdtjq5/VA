using System;
using System.Collections.Generic;

public class CharacterPlayerDataC : PlayerDataC<CharacterPlayerData>
{
    public override void DbGets(Action<List<CharacterPlayerData>> result)
    {
        Managers.Web.SendGetRequest<List<CharacterPlayerData>>("characterPlayerData/gets", (_result) =>
        {
            datas = _result;
            result.Invoke(_result);
        });
    }

    public override List<Stat> GetStats()
    {
        throw new NotImplementedException();
    }
}