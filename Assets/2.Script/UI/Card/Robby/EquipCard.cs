using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Player;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class EquipCard : UICard
{
    private readonly Dictionary<Grade, string> _gradeColor = new Dictionary<Grade, string>
    {
        { Grade.D, "#9D9D9DFF" },
        { Grade.C, "#66CF29FF" },
        { Grade.B, "#4FB1DDFF" },
        { Grade.A, "#C958E4FF" },
        { Grade.S, "#EAAE34FF" },
        { Grade.SS, "#EA4F33FF" },
        { Grade.SSS, "FFFFFFFF" },
    };

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }
    public override void Setting(ICardData data)
    {
        EquipCardData equipCardData = data as EquipCardData;

        if (equipCardData == null)
            return;

        UISet(equipCardData.PlayerEquipDto, equipCardData.Equip);
    }

    public void UISet(PlayerEquipDto playerEquipDto, Equip equip)
    {
        if (playerEquipDto == null || equip == null)
        {
            GradeSet(EquipGrade.D);
            LevelSet(0);
            IconSet(null);
            TypeSet(null);
            SpecialSet(null);
            return;
        }

        GradeSet(playerEquipDto.Grade);
        LevelSet(playerEquipDto.Level);
        IconSet(equip);
        TypeSet(equip);
        SpecialSet(equip);
    }

    private void GradeSet(EquipGrade equipGrade)
    {
        Grade grade = EquipFomula.GetGrade(equipGrade);

        GetImage(UIImageE.BG).sprite = Managers.Atlas.GetEquipGradeBg(grade);
        GetImage(UIImageE.Type).SetColor(_gradeColor[grade]);
        GetImage(UIImageE.GradeLevel).SetColor(_gradeColor[grade]);

        GetImage(UIImageE.GradeLevel).Fade(1);

        switch (equipGrade)
        {
            case EquipGrade.A1:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{1}";
                break;
            case EquipGrade.A2:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{2}";
                break;
            case EquipGrade.S1:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{1}";
                break;
            case EquipGrade.S2:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{2}";
                break;
            case EquipGrade.S3:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{3}";
                break;
            case EquipGrade.SS1:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{1}";
                break;
            case EquipGrade.SS2:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{2}";
                break;
            case EquipGrade.SS3:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{3}";
                break;
            case EquipGrade.SS4:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"+{4}";
                break;
            default:
                GetTextPro(UITextProE.GradeLevel_Text).text = $"";
                GetImage(UIImageE.GradeLevel).Fade(0);
                break;
        }
    }
    private void LevelSet(int level)
    {
        GetTextPro(UITextProE.Level).text = level <= 0 ? "" : $"LV.{level}";
    }
    private void IconSet(Equip equip)
    {
        if (equip == null)
        {
            GetImage(UIImageE.Icon).sprite = null;
            return;
        }
        GetImage(UIImageE.Icon).sprite = equip.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();
    }
    private void SpecialSet(Equip equip)
    {
        if (equip != null && equip.IsSpecial)
        {
            GetImage(UIImageE.Special).Fade(1);
        }
        else
        {
            GetImage(UIImageE.Special).Fade(0);
        }
    }
    private void TypeSet(Equip equip)
    {
        if (equip == null)
        {
            GetImage(UIImageE.Type_Icon).sprite = null;
            GetImage(UIImageE.Type).Fade(0);
            return;
        }

        GetImage(UIImageE.Type).Fade(1);
        GetImage(UIImageE.Type_Icon).sprite = Managers.Atlas.GetEquipType(equip.Type);
        GetImage(UIImageE.Type_Icon).SetNativeSize();
    }

	public enum UIImageE
    {
		BG,
		Icon,
		Type,
		Type_Icon,
		GradeLevel,
        Special,
    }
	public enum UITextProE
    {
		GradeLevel_Text,
		Level,
    }
}

public class EquipCardData : ICardData
{
    public PlayerEquipDto PlayerEquipDto { get; set; }
    public Equip Equip { get; set; }

    public EquipCardData(PlayerEquipDto playerEquipDto, Equip equip)
    {
        PlayerEquipDto = playerEquipDto;
        Equip = equip;
    }
}