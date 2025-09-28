using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Table;
using Shared.Enums;
using UnityEngine;

public abstract class PercentBox : UIFrame
{
    [SerializeField] Grade _grade;
    [SerializeField] bool _isSpeical;

    public Grade Grade => _grade;
    public bool IsSpeical => _isSpeical;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIScrollView>(typeof(UIScrollViewE));

        base.Initialize();
    }

    public void UISet(List<TableGachaDto> gachaDtos)
    {
        SetGrade();
        SetTotalPercent(gachaDtos);
        SetCards(gachaDtos);
    }

    private void SetGrade()
    {
        GetTextPro(UITextProE.Total_Grade_Text).text = DefineScript.GetGradeText(Grade);
        GetImage(UIImageE.Total_Grade_Special).Fade(IsSpeical ? 1 : 0);
    }
    private void SetTotalPercent(List<TableGachaDto> gachaDtos)
    {
        float totalPercent = 0;
        for (int i = 0; i < gachaDtos.Count; i++)
        {
            totalPercent += gachaDtos[i].Percent;
        }
        GetTextPro(UITextProE.Total_Percent).text = $"{DefineScript.GetGradeText(Grade)}등급 전체 확률 {totalPercent.ToString("F2")}%";
    }
    protected abstract void SetCards(List<TableGachaDto> gachaDtos);
    
	public enum UIImageE
    {
		Total,
		Total_Grade,
		Total_Grade_Special,
    }
	public enum UITextProE
    {
		Total_Grade_Text,
		Total_Percent,
    }
	public enum UIScrollViewE
    {
		ScrollView,
    }
}