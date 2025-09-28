using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Table;
using Shared.Enums;
using UnityEngine;

public class UIPercent : UIPopup
{
    [SerializeField] protected Transform _boxRoot;

    protected override void Initialize()
    {
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.Main_CloseButton).AddClickEvent((ped) => ClosePopupUIPlayAni());

        base.Initialize();
    }

    public virtual void UISet(List<TableGachaDto> gachaDtos)
    {
        for (int i = 0; i < _boxRoot.childCount; i++)
        {
            PercentBox percentBox = _boxRoot.GetChild(i).GetComponent<PercentBox>();
            percentBox.gameObject.SetActive(true);

            Grade boxGrade = percentBox.Grade;
            List<TableGachaDto> dtos = gachaDtos.Where(g => g.Grade == boxGrade).ToList();

            if (dtos.Count == 0)
            {
                percentBox.gameObject.SetActive(false);
                continue;
            }

            _boxRoot.GetChild(i).GetComponent<PercentBox>().UISet(dtos);
        }
    }
    
	public enum UITextProE
    {
		Main_Title_Text,
    }
	public enum UIButtonE
    {
		Main_CloseButton,
    }
}