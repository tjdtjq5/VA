using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using UnityEngine;

public class EquipPercentBox : PercentBox
{
    private readonly string _cardPrefabPath = "Robby/Equip/EquipPercentCard";

    protected override void SetCards(List<TableGachaDto> gachaDtos)
    {
        List<ICardData> cardDatas = new List<ICardData>();
        for (int i = 0; i < gachaDtos.Count; i++)
        {
            cardDatas.Add(new EquipPercentCardData() { GachaDto = gachaDtos[i] });
        }
        
        GetScrollView(UIScrollViewE.ScrollView).UISet(
            UIScrollViewLayoutStartAxis.Vertical, 
            _cardPrefabPath, 
            cardDatas, 
            0,
            5,
            UIScrollViewLayoutStartCorner.Middle,
            10, 10, 0, 10);
    }
}
