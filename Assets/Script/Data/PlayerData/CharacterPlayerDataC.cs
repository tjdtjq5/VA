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

    // Dic<Stat Code , Bonus Data>
    // Dic<string, Tuple<string, string, BBNumber>> 
    public override List<Stat> GetStats()
    {
        // Main / Sub / BBnum

        /* [캐릭터 별 추가 능력] 
         * 
         * 1. 캐릭터 레벨 : 공격력 , 체력 , 방어력 
         * 2. 잠재능력
         * 3. 전용무기 레벨
         * 
         * [모든 캐릭터 추가 능력] 
         * 1. 승급 
         * 2. 전용무기 달성
         * 3. 도감
        */

        string CharacterLevelMainKey = "CharacterLevel";
        string CharacterPotentialMainKey = "CharacterPotential";
        string CharacterSpecialWeaponLevelMainKey = "CharacterSpecialWeaponLevel";

        for (int i = 0; i < datas.Count; i++)
        {
            CharacterPlayerData data = datas[i];

        }

        var result = new Tuple<string, string, BBNumber>("Main", "Sub", 1);

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