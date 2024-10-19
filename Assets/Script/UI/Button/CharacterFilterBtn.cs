using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterFilterBtn : UIButton
{
    public CharacterFilterType FilterType { get; set; } = CharacterFilterType.Grade;
    public Action<CharacterFilterType> FilterHandler;

    HorizontalLayoutGroup _hlg;
	ContentSizeFitter _ccf;

	protected override void Initialize()
	{
		base.Initialize();

		_hlg = this.gameObject.GetComponent<HorizontalLayoutGroup>();
		_ccf = this.gameObject.FindChildByPath<ContentSizeFitter>("Text");

        Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

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
        LayoutSet();
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

        GetText(UITextE.Text).text = text;
    }

    void LayoutSet()
	{
        _ccf.SetLayoutHorizontal();
        _hlg.SetLayoutHorizontal();
    }

	public enum UIImageE
    {
		Icon,
    }
	public enum UITextE
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
