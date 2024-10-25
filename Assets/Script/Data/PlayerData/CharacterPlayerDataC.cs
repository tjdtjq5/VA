using System;
using System.Collections.Generic;
using System.Linq;

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

    public override CharacterPlayerData Get(object key)
    {
        return Gets().Where(p => p.Code.Equals(key.ToString())).FirstOrDefault();
    }

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

        // Test
        // InitialData();

        var result = new List<Stat>();

        Stat atk = Managers.SO.GetStat("Atk");
        Stat hp = Managers.SO.GetStat("Hp");
        Stat def = Managers.SO.GetStat("Def");

        result.Add(atk);
        result.Add(hp);
        result.Add(def);

        Dictionary<FormulaKeyword, float> formulaKeywords = new Dictionary<FormulaKeyword, float>();

        List<CharacterTableData> tables = Managers.Table.CharacterTable.Gets();

        for (int i = 0; i < datas.Count; i++)
        {
            CharacterPlayerData data = datas[i];
            string code = data.Code;
            CharacterTableData table = tables.Find(t => t.characterCode.Equals(code));
            if(table == null)
            {
                UnityHelper.Error_H($"CharacterPlayerDataC GetStats Error Null Table\ncode : {code}");
                continue;
            }

            int level = data.Level;
            formulaKeywords.TryAdd_H(FormulaKeyword.C_LEVEL, level, true);

            float c_hp = table.hp;
            float c_atk = table.atk;
            float c_def = table.def;

            formulaKeywords.TryAdd_H(FormulaKeyword.Default, c_hp, true);
            hp.SetBonusValue(OnlyCharacterStatKey.CharacterLevel.ToString(), code, Managers.Table.FormulaTable.GetValue(FormulaTableCodeDefine.Ab_C_Level_Hp, formulaKeywords));

            formulaKeywords.TryAdd_H(FormulaKeyword.Default, c_atk, true);
            atk.SetBonusValue(OnlyCharacterStatKey.CharacterLevel.ToString(), code, Managers.Table.FormulaTable.GetValue(FormulaTableCodeDefine.Ab_C_Level_Atk, formulaKeywords));

            formulaKeywords.TryAdd_H(FormulaKeyword.Default, c_def, true);
            def.SetBonusValue(OnlyCharacterStatKey.CharacterLevel.ToString(), code, Managers.Table.FormulaTable.GetValue(FormulaTableCodeDefine.Ab_C_Level_Def, formulaKeywords));
        }

        return result;
    }

    public override void InitialData()
    {
        List<CharacterPlayerData> datas = new List<CharacterPlayerData>()
        {
            new CharacterPlayerData() { Code = "Pirate001", Level = 1 },
            new CharacterPlayerData() { Code = "Cat001", Level = 1 },
            new CharacterPlayerData() { Code = "Dragon001", Level = 1 },
            new CharacterPlayerData() { Code = "Robot001", Level = 1 },
            new CharacterPlayerData() { Code = "Thief001", Level = 1 },
            new CharacterPlayerData() { Code = "Druid001", Level = 1 },
        };
        Sets(datas);
    }
}