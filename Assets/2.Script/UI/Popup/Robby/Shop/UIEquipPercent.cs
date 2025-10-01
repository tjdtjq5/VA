using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using Shared.Enums;
using System.Linq;
using UnityEngine;

public class UIEquipPercent : UIPercent
{
    public override void UISet(List<TableGachaDto> gachaDtos)
    {
        List<TableGachaDto> basicDtos = new List<TableGachaDto>();
        List<TableGachaDto> specialDtos = new List<TableGachaDto>();

        for (int i = 0; i < gachaDtos.Count; i++)
        {
            string itemCode = gachaDtos[i].ItemCode;
            Equip equip = Managers.SO.GetEquip(itemCode);
            if (equip.IsSpecial)
                specialDtos.Add(gachaDtos[i]);
            else
                basicDtos.Add(gachaDtos[i]);
        }

        for (int i = 0; i < _boxRoot.childCount; i++)
        {
            PercentBox percentBox = _boxRoot.GetChild(i).GetComponent<PercentBox>();
            percentBox.gameObject.SetActive(true);

            Grade boxGrade = percentBox.Grade;
            List<TableGachaDto> dtos = new List<TableGachaDto>();

            if (percentBox.IsSpeical)
                dtos = specialDtos.Where(g => g.Grade == boxGrade).ToList();
            else
                dtos = basicDtos.Where(g => g.Grade == boxGrade).ToList();

            if (dtos.Count == 0)
            {
                percentBox.gameObject.SetActive(false);
                continue;
            }

            _boxRoot.GetChild(i).GetComponent<PercentBox>().UISet(dtos);
        }
    }
}
