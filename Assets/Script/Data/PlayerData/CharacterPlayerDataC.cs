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

    public override void InitialData()
    {
        List<CharacterPlayerData> datas = new List<CharacterPlayerData>()
        {
            new CharacterPlayerData() { Code = "Pirate_001", Level = 1 },
            new CharacterPlayerData() { Code = "Cat_001", Level = 1 },
            new CharacterPlayerData() { Code = "Dragon_001", Level = 1 },
            new CharacterPlayerData() { Code = "Robot_001", Level = 1 },
            new CharacterPlayerData() { Code = "Thief_001", Level = 1 },
            new CharacterPlayerData() { Code = "Druid_001", Level = 1 },
        };
        Sets(datas);
    }
}