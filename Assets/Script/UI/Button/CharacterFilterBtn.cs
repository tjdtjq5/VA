using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterFilterBtn : UIButton
{
    public CharacterFilterType FilterType { get; set; } = CharacterFilterType.Grade;
    public Action<CharacterFilterType> FilterHandler;

	protected override void Initialize()
	{
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

		base.Initialize();

        AddClickEvent(OnClick);
	}

    protected override void UISet()
    {
        base.UISet();
        UISet(FilterType);
    }

    public void OnClick(PointerEventData ped)
    {
        FilterType = (CharacterFilterType)CSharpHelper.EnumInRemain<CharacterFilterType>((int)FilterType + 1, false);
        UISet(FilterType);

        if (FilterHandler != null)
            FilterHandler.Invoke(FilterType);
    }

    public void UISet(CharacterFilterType type)
    {
        TextSet(type);
    }

    void TextSet(CharacterFilterType type)
    {
        string text = "";
        switch (type)
        {
            case CharacterFilterType.Grade:
                text = "희귀도";
                break;
            case CharacterFilterType.Level:
                text = "레벨";
                break;
            case CharacterFilterType.Awake:
                text = "승급";
                break;
        }

        GetTextPro(UITextProE.Text).text = text;
    }

	public enum UIImageE
    {
		Icon,
    }
	public enum UITextProE
    {
		Text,
    }
}
public enum CharacterFilterType
{
    Grade,
    Level,
    Awake,
}