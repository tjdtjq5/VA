using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class EquipPercentCard : EquipCard
{
    [SerializeField] UITextPro _percentText;

    public override void Setting(ICardData data)
    {
        EquipPercentCardData cardData = data as EquipPercentCardData;

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
            PercentSet(0);
            return;
        }

        Equip equip = Managers.SO.GetEquip(gachaDto.ItemCode);

        GradeSet(EquipFomula.GetGrade(gachaDto.Grade));
        LevelSet(0);
        IconSet(equip);
        TypeSet(equip);
        SpecialSet(equip);
        PercentSet(gachaDto.Percent);
    }

    private void PercentSet(float percent)
    {
        _percentText.text = percent.ToString("F2") + "%";
    }
}
public class EquipPercentCardData : ICardData
{
    public TableGachaDto GachaDto { get; set; }
}