using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Table;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class EquipGachaResultCard : EquipCard
{
    public override void Setting(ICardData data)
    {
        EquipGachaResultCardData cardData = data as EquipGachaResultCardData;

        if (cardData == null)
            return;

        UISet(cardData.GachaDto);
    }
    public void UISet(TableGachaDto gachaDto)
    {
        if (gachaDto == null)
        {
            GradeSet(EquipGrade.D);
            LevelSet(0);
            IconSet(null);
            TypeSet(null);
            SpecialSet(null);
            return;
        }

        Equip equip = Managers.SO.GetEquip(gachaDto.ItemCode);

        GradeSet(EquipFomula.GetGrade(gachaDto.Grade));
        LevelSet(0);
        IconSet(equip);
        TypeSet(equip);
        SpecialSet(equip);
    }
}
public class EquipGachaResultCardData : ICardData
{
    public TableGachaDto GachaDto { get; set; }
}