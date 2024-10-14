using System;
using System.Collections.Generic;

public class FormulaTable : Table<FormulaTableData>
{
    protected override string TableName => "Formula";
    public override void DbGets(Action<List<FormulaTableData>> result)
    {
        Managers.Web.SendGetRequest<FormulaTableGetsResponse>("formulaTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }

    public override void InitialData()
    {
        List<FormulaTableData> datas = new List<FormulaTableData>()
        {
            new FormulaTableData() { formulaCode = "Stage_Get_Gesso", fM = "10+(10*{C_LEVEL})", tipName = "스테이지획득게소",  },
            new FormulaTableData() { formulaCode = "Stage_Get_Elixir", fM = "10+(10*{C_LEVEL})", tipName = "스테이지획득성장의비약",  },
            new FormulaTableData() { formulaCode = "Stage_Get_Exp", fM = "10+(10*{C_LEVEL})", tipName = "스테이지획득경험치",  },
            new FormulaTableData() { formulaCode = "Stage_Get_TC", fM = "10+(10*{C_LEVEL})", tipName = "일반던전획득재능변환기",  },
            new FormulaTableData() { formulaCode = "Stage_Get_Cobalt", fM = "10+(10*{C_LEVEL})", tipName = "랭킹던전획득코발트",  },
            new FormulaTableData() { formulaCode = "Stage_Get_Cubic", fM = "10+(10*{C_LEVEL})", tipName = "계정레벨업획득큐빅",  },
            new FormulaTableData() { formulaCode = "Stage_Get_MR", fM = "10+(10*{C_LEVEL})", tipName = "랭킹던전획득운석잔해",  },
            new FormulaTableData() { formulaCode = "Stage_Get_DreamC", fM = "10+(10*{C_LEVEL})", tipName = "일반던전획득꿈의결정",  },
            new FormulaTableData() { formulaCode = "Stage_Get_RH", fM = "10+(10*{C_LEVEL})", tipName = "일반던전획득복원망치",  },
            new FormulaTableData() { formulaCode = "Stage_Get_StarCandy", fM = "10+(10*{C_LEVEL})", tipName = "일반던전획득별사탕",  },
            new FormulaTableData() { formulaCode = "Stage_Get_LC", fM = "10+(10*{C_LEVEL})", tipName = "일반던전획득행운변환기",  },
            new FormulaTableData() { formulaCode = "Character_Level_Up", fM = "10+(10*{C_LEVEL})", tipName = "캐릭터레벨업",  },
        };
        Push(datas);
    }
}