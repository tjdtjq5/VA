using System;
using System.Collections.Generic;
using System.Linq;

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
            new FormulaTableData() { formulaCode = "Need_C_Awake_Card", fM = "2+2^{C_AWAKE}", tipName = "캐릭터승급에따라필요한카드수",  },
            new FormulaTableData() { formulaCode = "Ab_C_Level_Hp", fM = "{Default}+{C_LEVEL}*({Default}*0.1)", tipName = "캐릭터레벨별능력치_체력",  },
            new FormulaTableData() { formulaCode = "Ab_C_Level_Atk", fM = "{Default}+{C_LEVEL}*({Default}*0.1)", tipName = "캐릭터레벨별능력치_공격력",  },
            new FormulaTableData() { formulaCode = "Ab_C_Level_Def", fM = "{Default}+{C_LEVEL}*({Default}*0.1)", tipName = "캐릭터레벨별능력치_방어력",  },
        };
        Push(datas);
    }

    public BBNumber GetValue(FormulaTableCodeDefine code, Dictionary<FormulaKeyword, float> keywordDics)
    {
        string formulaData = GetData(code).fM;
        int keywordLen = CSharpHelper.GetEnumLength<FormulaKeyword>();

        try
        {
            for (int i = 0; i < keywordLen; i++)
            {
                FormulaKeyword keyword = (FormulaKeyword)i;
                string keywordStr = $"{{{keyword}}}";

                if (formulaData.Contains(keywordStr))
                {
                    if (keywordDics.ContainsKey(keyword))
                        formulaData = formulaData.Replace(keywordStr, keywordDics[keyword].ToString());
                    else
                        throw null;
                }
            }
        }
        catch (Exception e)
        {
            UnityHelper.Error_H($"FormulaTable GetValue Error\ne : {e.Message}");
            throw;
        }
       

        return FomulaCompute.Compute(formulaData);
    }
    private FormulaTableData GetData(FormulaTableCodeDefine code)
    {
        FormulaTableData data = datas.Where(d => d.formulaCode.Equals(code.ToString())).FirstOrDefault();
        try
        {
            if (data == null)
                throw null;
        }
        catch (Exception e)
        {
            UnityHelper.Error_H($"FormulaTable GetValue Error\ne : {e.Message}");
            throw;
        }

        return data;
    }
}