using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using UnityEngine;

public class GrowSector : UIFrame
{
    protected override void Initialize()
    {
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }

    public void UISet(TableGrowRewardDto tableData)
    {
        GetTextPro(UITextProE.Count).text = $"{tableData.Level}";
    }

	public enum UITextProE
    {
		Count,
    }
}