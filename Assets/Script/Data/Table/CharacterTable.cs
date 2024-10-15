using System;
using System.Collections.Generic;

public class CharacterTable : Table<CharacterTableData>
{
    Dictionary<string, CharacterSO> tableSOs { get; set; } = new();
    protected override string TableName => "Character";
    public override void DbGets(Action<List<CharacterTableData>> result)
    {
        Managers.Web.SendGetRequest<CharacterTableGetsResponse>("characterTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }
    public override void InitialData()
    {
        List<CharacterTableData> datas = new List<CharacterTableData>()
        {
            new CharacterTableData() { characterCode = "Pirate_001", tribeType = 0, grade = 0, tipName = "해적001",  },
            new CharacterTableData() { characterCode = "Pirate_002", tribeType = 0, grade = 1, tipName = "해적002",  },
            new CharacterTableData() { characterCode = "Pirate_003", tribeType = 0, grade = 2, tipName = "해적003",  },
            new CharacterTableData() { characterCode = "Pirate_004", tribeType = 0, grade = 3, tipName = "해적004",  },
            new CharacterTableData() { characterCode = "Pirate_005", tribeType = 0, grade = 4, tipName = "해적005",  },
            new CharacterTableData() { characterCode = "Pirate_006", tribeType = 0, grade = 5, tipName = "해적006",  },
            new CharacterTableData() { characterCode = "Pirate_007", tribeType = 0, grade = 6, tipName = "해적007",  },
            new CharacterTableData() { characterCode = "Pirate_008", tribeType = 0, grade = 6, tipName = "해적008",  },
            new CharacterTableData() { characterCode = "Cat_001", tribeType = 1, grade = 0, tipName = "고양이001",  },
            new CharacterTableData() { characterCode = "Cat_002", tribeType = 1, grade = 1, tipName = "고양이002",  },
            new CharacterTableData() { characterCode = "Cat_003", tribeType = 1, grade = 2, tipName = "고양이003",  },
            new CharacterTableData() { characterCode = "Cat_004", tribeType = 1, grade = 3, tipName = "고양이004",  },
            new CharacterTableData() { characterCode = "Cat_005", tribeType = 1, grade = 4, tipName = "고양이005",  },
            new CharacterTableData() { characterCode = "Cat_006", tribeType = 1, grade = 5, tipName = "고양이006",  },
            new CharacterTableData() { characterCode = "Cat_007", tribeType = 1, grade = 6, tipName = "고양이007",  },
            new CharacterTableData() { characterCode = "Cat_008", tribeType = 1, grade = 6, tipName = "고양이008",  },
            new CharacterTableData() { characterCode = "Dragon_001", tribeType = 2, grade = 0, tipName = "용001",  },
            new CharacterTableData() { characterCode = "Dragon_002", tribeType = 2, grade = 1, tipName = "용002",  },
            new CharacterTableData() { characterCode = "Dragon_003", tribeType = 2, grade = 2, tipName = "용003",  },
            new CharacterTableData() { characterCode = "Dragon_004", tribeType = 2, grade = 3, tipName = "용004",  },
            new CharacterTableData() { characterCode = "Dragon_005", tribeType = 2, grade = 4, tipName = "용005",  },
            new CharacterTableData() { characterCode = "Dragon_006", tribeType = 2, grade = 5, tipName = "용006",  },
            new CharacterTableData() { characterCode = "Dragon_007", tribeType = 2, grade = 6, tipName = "용007",  },
            new CharacterTableData() { characterCode = "Dragon_008", tribeType = 2, grade = 6, tipName = "용008",  },
            new CharacterTableData() { characterCode = "Robot_001", tribeType = 3, grade = 0, tipName = "로봇001",  },
            new CharacterTableData() { characterCode = "Robot_002", tribeType = 3, grade = 1, tipName = "로봇002",  },
            new CharacterTableData() { characterCode = "Robot_003", tribeType = 3, grade = 2, tipName = "로봇003",  },
            new CharacterTableData() { characterCode = "Robot_004", tribeType = 3, grade = 3, tipName = "로봇004",  },
            new CharacterTableData() { characterCode = "Robot_005", tribeType = 3, grade = 4, tipName = "로봇005",  },
            new CharacterTableData() { characterCode = "Robot_006", tribeType = 3, grade = 5, tipName = "로봇006",  },
            new CharacterTableData() { characterCode = "Robot_007", tribeType = 3, grade = 6, tipName = "로봇007",  },
            new CharacterTableData() { characterCode = "Robot_008", tribeType = 3, grade = 6, tipName = "로봇008",  },
            new CharacterTableData() { characterCode = "Thief_001", tribeType = 4, grade = 0, tipName = "도적001",  },
            new CharacterTableData() { characterCode = "Thief_002", tribeType = 4, grade = 1, tipName = "도적002",  },
            new CharacterTableData() { characterCode = "Thief_003", tribeType = 4, grade = 2, tipName = "도적003",  },
            new CharacterTableData() { characterCode = "Thief_004", tribeType = 4, grade = 3, tipName = "도적004",  },
            new CharacterTableData() { characterCode = "Thief_005", tribeType = 4, grade = 4, tipName = "도적005",  },
            new CharacterTableData() { characterCode = "Thief_006", tribeType = 4, grade = 5, tipName = "도적006",  },
            new CharacterTableData() { characterCode = "Thief_007", tribeType = 4, grade = 6, tipName = "도적007",  },
            new CharacterTableData() { characterCode = "Thief_008", tribeType = 4, grade = 6, tipName = "도적008",  },
            new CharacterTableData() { characterCode = "Druid_001", tribeType = 5, grade = 0, tipName = "드루이드001",  },
            new CharacterTableData() { characterCode = "Druid_002", tribeType = 5, grade = 1, tipName = "드루이드002",  },
            new CharacterTableData() { characterCode = "Druid_003", tribeType = 5, grade = 2, tipName = "드루이드003",  },
            new CharacterTableData() { characterCode = "Druid_004", tribeType = 5, grade = 3, tipName = "드루이드004",  },
            new CharacterTableData() { characterCode = "Druid_005", tribeType = 5, grade = 4, tipName = "드루이드005",  },
            new CharacterTableData() { characterCode = "Druid_006", tribeType = 5, grade = 5, tipName = "드루이드006",  },
            new CharacterTableData() { characterCode = "Druid_007", tribeType = 5, grade = 6, tipName = "드루이드007",  },
            new CharacterTableData() { characterCode = "Druid_008", tribeType = 5, grade = 6, tipName = "드루이드008",  },
        };
        Push(datas);
    }
    public CharacterSO GetTableSO(string code)
    {
        if (tableSOs.ContainsKey(code))
            return tableSOs[code];

        string path = DefinePath.TableSOResourcesPath(TableName, code);
        CharacterSO tableSO = Managers.Resources.Load<CharacterSO>(path);

        tableSOs.Add(code, tableSO);
        return tableSO;
    }
}