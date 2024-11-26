using System;
using System.Collections.Generic;
using System.Linq;

public class ItemTable : Table<ItemTableData>
{
    protected override string TableName => "Item";

    public override void DbGets(Action<List<ItemTableData>> result)
    {
        Managers.Web.SendGetRequest<ItemTableGetsResponse>("itemTable/gets", (_result) =>
        {
            datas = _result.datas;
            result.Invoke(_result.datas);
        });
    }

    public override ItemTableData Get(object key)
        => Gets().Where(d => d.itemCode.Equals(key.ToString())).FirstOrDefault();

    public override void InitialData()
    {
        List<ItemTableData> datas = new List<ItemTableData>()
        {
            new ItemTableData() { itemCode = "Elixir", itemType = 0, tipName = "성장의비약",  },
            new ItemTableData() { itemCode = "TalentConverter", itemType = 0, tipName = "재능변환기",  },
            new ItemTableData() { itemCode = "Cobalt", itemType = 0, tipName = "코발트",  },
            new ItemTableData() { itemCode = "Gesso", itemType = 0, tipName = "게소",  },
            new ItemTableData() { itemCode = "Cubic", itemType = 0, tipName = "큐빅",  },
            new ItemTableData() { itemCode = "MeteoriteDebris", itemType = 0, tipName = "운석잔해",  },
            new ItemTableData() { itemCode = "RestorationHammer", itemType = 0, tipName = "복원망치",  },
            new ItemTableData() { itemCode = "StarCandy", itemType = 0, tipName = "별사탕",  },
            new ItemTableData() { itemCode = "LuckConverter", itemType = 0, tipName = "행운변환기",  },
            new ItemTableData() { itemCode = "Eight", itemType = 0, tipName = "에이트",  },
            new ItemTableData() { itemCode = "DreamCrystals", itemType = 0, tipName = "꿈의결정",  },
            new ItemTableData() { itemCode = "PureEight", itemType = 0, tipName = "순수에이트",  },
            new ItemTableData() { itemCode = "Mileage", itemType = 0, tipName = "마일리지",  },
            new ItemTableData() { itemCode = "Experience", itemType = 0, tipName = "경험치",  },
            new ItemTableData() { itemCode = "FragmentSpace", itemType = 0, tipName = "공간의파편",  },
            new ItemTableData() { itemCode = "FragmentTime", itemType = 0, tipName = "시간의파편",  },
            new ItemTableData() { itemCode = "GC_Artifact", itemType = 0, tipName = "유물가챠쿠폰",  },
            new ItemTableData() { itemCode = "GC_Permanent_P", itemType = 0, tipName = "상시펫가챠쿠폰",  },
            new ItemTableData() { itemCode = "GC_Pickup_P", itemType = 0, tipName = "픽업펫가챠쿠폰",  },
            new ItemTableData() { itemCode = "GC_Permanent_C", itemType = 0, tipName = "상시캐릭터가챠쿠폰",  },
            new ItemTableData() { itemCode = "GC_Pickup_C", itemType = 0, tipName = "픽업캐릭터가챠쿠폰",  },
            new ItemTableData() { itemCode = "Drill", itemType = 0, tipName = "드릴",  },
            new ItemTableData() { itemCode = "Dynamite", itemType = 0, tipName = "다이너마이트",  },
            new ItemTableData() { itemCode = "UndergroundRes", itemType = 0, tipName = "지하자원",  },
            new ItemTableData() { itemCode = "Shrimp", itemType = 0, tipName = "새우",  },
            new ItemTableData() { itemCode = "FantasyBait", itemType = 0, tipName = "환상의미끼",  },
            new ItemTableData() { itemCode = "Fish", itemType = 0, tipName = "물고기",  },
            new ItemTableData() { itemCode = "AdvertisingJamming", itemType = 1, tipName = "광고전파방해장치",  },
            new ItemTableData() { itemCode = "ElixirBox_Hour_1", itemType = 2, tipName = "성장의비약상자1시간",  },
            new ItemTableData() { itemCode = "ElixirBox_Hour_3", itemType = 2, tipName = "성장의비약상자3시간",  },
            new ItemTableData() { itemCode = "ElixirBox_Hour_6", itemType = 2, tipName = "성장의비약상자6시간",  },
            new ItemTableData() { itemCode = "ElixirBox_Hour_12", itemType = 2, tipName = "성장의비약상자12시간",  },
            new ItemTableData() { itemCode = "GessoBox_Hour_1", itemType = 2, tipName = "게소상자1시간",  },
            new ItemTableData() { itemCode = "GessoBox_Hour_3", itemType = 2, tipName = "게소상자3시간",  },
            new ItemTableData() { itemCode = "GessoBox_Hour_6", itemType = 2, tipName = "게소상자6시간",  },
            new ItemTableData() { itemCode = "GessoBox_Hour_12", itemType = 2, tipName = "게소상자12시간",  },
            new ItemTableData() { itemCode = "ExpBox_Hour_1", itemType = 2, tipName = "경험치상자1시간",  },
            new ItemTableData() { itemCode = "ExpBox_Hour_3", itemType = 2, tipName = "경험치상자3시간",  },
            new ItemTableData() { itemCode = "ExpBox_Hour_6", itemType = 2, tipName = "경험치상자6시간",  },
            new ItemTableData() { itemCode = "ExpBox_Hour_12", itemType = 2, tipName = "경험치상자12시간",  },
            new ItemTableData() { itemCode = "CSBox_Grade_A", itemType = 3, tipName = "캐릭터선택상자(A등급)",  },
            new ItemTableData() { itemCode = "CSBox_Grade_S", itemType = 3, tipName = "캐릭터선택상자(S등급)",  },
            new ItemTableData() { itemCode = "CSBox_Grade_SS", itemType = 3, tipName = "캐릭터선택상자(SS등급)",  },
            new ItemTableData() { itemCode = "CSBox_Grade_SSS", itemType = 3, tipName = "캐릭터선택상자(SSS등급)",  },
            new ItemTableData() { itemCode = "PSBox_Grade_A", itemType = 3, tipName = "펫선택상자(A등급)",  },
            new ItemTableData() { itemCode = "PSBox_Grade_S", itemType = 3, tipName = "펫선택상자(S등급)",  },
            new ItemTableData() { itemCode = "PSBox_Grade_SS", itemType = 3, tipName = "펫선택상자(SS등급)",  },
            new ItemTableData() { itemCode = "PSBox_Grade_SSS", itemType = 3, tipName = "펫선택상자(SSS등급)",  },
            new ItemTableData() { itemCode = "NormalChest", itemType = 4, tipName = "일반보물상자",  },
            new ItemTableData() { itemCode = "LuxuryChest", itemType = 4, tipName = "고급보물상자",  },
            new ItemTableData() { itemCode = "LegendChest", itemType = 4, tipName = "전설보물상자",  },
            new ItemTableData() { itemCode = "CRBox_Grade_A", itemType = 4, tipName = "캐릭터랜덤상자(A등급)",  },
            new ItemTableData() { itemCode = "CRBox_Grade_S", itemType = 4, tipName = "캐릭터랜덤상자(S등급)",  },
            new ItemTableData() { itemCode = "CRBox_Grade_SS", itemType = 4, tipName = "캐릭터랜덤상자(SS등급)",  },
            new ItemTableData() { itemCode = "CRBox_Grade_SSS", itemType = 4, tipName = "캐릭터랜덤상자(SSS등급)",  },
            new ItemTableData() { itemCode = "PRBox_Grade_A", itemType = 4, tipName = "펫랜덤상자(A등급)",  },
            new ItemTableData() { itemCode = "PRBox_Grade_S", itemType = 4, tipName = "펫랜덤상자(S등급)",  },
            new ItemTableData() { itemCode = "PRBox_Grade_SS", itemType = 4, tipName = "펫랜덤상자(SS등급)",  },
            new ItemTableData() { itemCode = "PRBox_Grade_SSS", itemType = 4, tipName = "펫랜덤상자(SSS등급)",  },
            new ItemTableData() { itemCode = "Ticket_Elixir", itemType = 5, tipName = "입장권-성장의비약",  },
            new ItemTableData() { itemCode = "Ticket_TC", itemType = 5, tipName = "입장권-재능변환기",  },
            new ItemTableData() { itemCode = "Ticket_G", itemType = 5, tipName = "입장권-게소",  },
            new ItemTableData() { itemCode = "Ticket_DC", itemType = 5, tipName = "입장권-꿈의결정",  },
            new ItemTableData() { itemCode = "Ticket_RH", itemType = 5, tipName = "입장권-복원망치",  },
            new ItemTableData() { itemCode = "Ticket_SC", itemType = 5, tipName = "입장권-별사탕",  },
            new ItemTableData() { itemCode = "Ticket_LC", itemType = 5, tipName = "입장권-행운변환기",  },
            new ItemTableData() { itemCode = "EssenceFire", itemType = 6, tipName = "불의정수",  },
            new ItemTableData() { itemCode = "EssenceWater", itemType = 6, tipName = "물의정수",  },
            new ItemTableData() { itemCode = "EssenceWind", itemType = 6, tipName = "바람의정수",  },
            new ItemTableData() { itemCode = "EssenceLight", itemType = 6, tipName = "빛의정수",  },
            new ItemTableData() { itemCode = "EssenceDarkness", itemType = 6, tipName = "어둠의정수",  },
            new ItemTableData() { itemCode = "EssenceLightning", itemType = 6, tipName = "번개의정수",  },
            new ItemTableData() { itemCode = "Pick", itemType = 7, tipName = "곡괭이",  },
            new ItemTableData() { itemCode = "Earthworm", itemType = 7, tipName = "지렁이",  },
        };
        Push(datas);
    }
}