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
            new CharacterTableData() { characterCode = "Pirate001", tribeType = 0, job = 0, grade = 0, atk = 100, hp = 85, def = 80, tipName = "해적001",  },
            new CharacterTableData() { characterCode = "Pirate002", tribeType = 0, job = 1, grade = 1, atk = 80, hp = 90, def = 85, tipName = "해적002",  },
            new CharacterTableData() { characterCode = "Pirate003", tribeType = 0, job = 2, grade = 2, atk = 85, hp = 95, def = 90, tipName = "해적003",  },
            new CharacterTableData() { characterCode = "Pirate004", tribeType = 0, job = 0, grade = 3, atk = 90, hp = 100, def = 95, tipName = "해적004",  },
            new CharacterTableData() { characterCode = "Pirate005", tribeType = 0, job = 1, grade = 4, atk = 95, hp = 80, def = 100, tipName = "해적005",  },
            new CharacterTableData() { characterCode = "Pirate006", tribeType = 0, job = 2, grade = 5, atk = 100, hp = 85, def = 80, tipName = "해적006",  },
            new CharacterTableData() { characterCode = "Pirate007", tribeType = 0, job = 0, grade = 6, atk = 80, hp = 90, def = 85, tipName = "해적007",  },
            new CharacterTableData() { characterCode = "Pirate008", tribeType = 0, job = 1, grade = 6, atk = 85, hp = 95, def = 90, tipName = "해적008",  },
            new CharacterTableData() { characterCode = "Cat001", tribeType = 1, job = 2, grade = 0, atk = 90, hp = 100, def = 95, tipName = "고양이001",  },
            new CharacterTableData() { characterCode = "Cat002", tribeType = 1, job = 0, grade = 1, atk = 95, hp = 80, def = 100, tipName = "고양이002",  },
            new CharacterTableData() { characterCode = "Cat003", tribeType = 1, job = 1, grade = 2, atk = 100, hp = 85, def = 80, tipName = "고양이003",  },
            new CharacterTableData() { characterCode = "Cat004", tribeType = 1, job = 2, grade = 3, atk = 80, hp = 90, def = 85, tipName = "고양이004",  },
            new CharacterTableData() { characterCode = "Cat005", tribeType = 1, job = 0, grade = 4, atk = 85, hp = 95, def = 90, tipName = "고양이005",  },
            new CharacterTableData() { characterCode = "Cat006", tribeType = 1, job = 1, grade = 5, atk = 90, hp = 100, def = 95, tipName = "고양이006",  },
            new CharacterTableData() { characterCode = "Cat007", tribeType = 1, job = 2, grade = 6, atk = 95, hp = 80, def = 100, tipName = "고양이007",  },
            new CharacterTableData() { characterCode = "Cat008", tribeType = 1, job = 0, grade = 6, atk = 100, hp = 85, def = 80, tipName = "고양이008",  },
            new CharacterTableData() { characterCode = "Dragon001", tribeType = 2, job = 1, grade = 0, atk = 80, hp = 90, def = 85, tipName = "용001",  },
            new CharacterTableData() { characterCode = "Dragon002", tribeType = 2, job = 2, grade = 1, atk = 85, hp = 95, def = 90, tipName = "용002",  },
            new CharacterTableData() { characterCode = "Dragon003", tribeType = 2, job = 0, grade = 2, atk = 90, hp = 100, def = 95, tipName = "용003",  },
            new CharacterTableData() { characterCode = "Dragon004", tribeType = 2, job = 1, grade = 3, atk = 95, hp = 80, def = 100, tipName = "용004",  },
            new CharacterTableData() { characterCode = "Dragon005", tribeType = 2, job = 2, grade = 4, atk = 100, hp = 85, def = 80, tipName = "용005",  },
            new CharacterTableData() { characterCode = "Dragon006", tribeType = 2, job = 0, grade = 5, atk = 80, hp = 90, def = 85, tipName = "용006",  },
            new CharacterTableData() { characterCode = "Dragon007", tribeType = 2, job = 1, grade = 6, atk = 85, hp = 95, def = 90, tipName = "용007",  },
            new CharacterTableData() { characterCode = "Dragon008", tribeType = 2, job = 2, grade = 6, atk = 90, hp = 100, def = 95, tipName = "용008",  },
            new CharacterTableData() { characterCode = "Robot001", tribeType = 3, job = 0, grade = 0, atk = 95, hp = 80, def = 100, tipName = "로봇001",  },
            new CharacterTableData() { characterCode = "Robot002", tribeType = 3, job = 1, grade = 1, atk = 100, hp = 85, def = 80, tipName = "로봇002",  },
            new CharacterTableData() { characterCode = "Robot003", tribeType = 3, job = 2, grade = 2, atk = 80, hp = 90, def = 85, tipName = "로봇003",  },
            new CharacterTableData() { characterCode = "Robot004", tribeType = 3, job = 0, grade = 3, atk = 85, hp = 95, def = 90, tipName = "로봇004",  },
            new CharacterTableData() { characterCode = "Robot005", tribeType = 3, job = 1, grade = 4, atk = 90, hp = 100, def = 95, tipName = "로봇005",  },
            new CharacterTableData() { characterCode = "Robot006", tribeType = 3, job = 2, grade = 5, atk = 95, hp = 80, def = 100, tipName = "로봇006",  },
            new CharacterTableData() { characterCode = "Robot007", tribeType = 3, job = 0, grade = 6, atk = 100, hp = 85, def = 80, tipName = "로봇007",  },
            new CharacterTableData() { characterCode = "Robot008", tribeType = 3, job = 1, grade = 6, atk = 80, hp = 90, def = 85, tipName = "로봇008",  },
            new CharacterTableData() { characterCode = "Thief001", tribeType = 4, job = 2, grade = 0, atk = 85, hp = 95, def = 90, tipName = "도적001",  },
            new CharacterTableData() { characterCode = "Thief002", tribeType = 4, job = 0, grade = 1, atk = 90, hp = 100, def = 95, tipName = "도적002",  },
            new CharacterTableData() { characterCode = "Thief003", tribeType = 4, job = 1, grade = 2, atk = 95, hp = 80, def = 100, tipName = "도적003",  },
            new CharacterTableData() { characterCode = "Thief004", tribeType = 4, job = 2, grade = 3, atk = 100, hp = 85, def = 80, tipName = "도적004",  },
            new CharacterTableData() { characterCode = "Thief005", tribeType = 4, job = 0, grade = 4, atk = 80, hp = 90, def = 85, tipName = "도적005",  },
            new CharacterTableData() { characterCode = "Thief006", tribeType = 4, job = 1, grade = 5, atk = 85, hp = 95, def = 90, tipName = "도적006",  },
            new CharacterTableData() { characterCode = "Thief007", tribeType = 4, job = 2, grade = 6, atk = 90, hp = 100, def = 95, tipName = "도적007",  },
            new CharacterTableData() { characterCode = "Thief008", tribeType = 4, job = 0, grade = 6, atk = 95, hp = 80, def = 100, tipName = "도적008",  },
            new CharacterTableData() { characterCode = "Druid001", tribeType = 5, job = 1, grade = 0, atk = 100, hp = 85, def = 80, tipName = "드루이드001",  },
            new CharacterTableData() { characterCode = "Druid002", tribeType = 5, job = 2, grade = 1, atk = 80, hp = 90, def = 85, tipName = "드루이드002",  },
            new CharacterTableData() { characterCode = "Druid003", tribeType = 5, job = 0, grade = 2, atk = 85, hp = 95, def = 90, tipName = "드루이드003",  },
            new CharacterTableData() { characterCode = "Druid004", tribeType = 5, job = 1, grade = 3, atk = 90, hp = 100, def = 95, tipName = "드루이드004",  },
            new CharacterTableData() { characterCode = "Druid005", tribeType = 5, job = 2, grade = 4, atk = 95, hp = 80, def = 100, tipName = "드루이드005",  },
            new CharacterTableData() { characterCode = "Druid006", tribeType = 5, job = 0, grade = 5, atk = 100, hp = 85, def = 80, tipName = "드루이드006",  },
            new CharacterTableData() { characterCode = "Druid007", tribeType = 5, job = 1, grade = 6, atk = 80, hp = 90, def = 85, tipName = "드루이드007",  },
            new CharacterTableData() { characterCode = "Druid008", tribeType = 5, job = 2, grade = 6, atk = 85, hp = 95, def = 90, tipName = "드루이드008",  },
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