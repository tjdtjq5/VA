using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICheck : UIFrame
{
    protected virtual string Script { get; set; }

    public Action<bool> CheckHandler;
    public bool IsChecked { get; set; } = false;

    HorizontalLayoutGroup hlg;
	ContentSizeFitter main_csf;
	ContentSizeFitter text_csf;
    GameObject checkObj;

    protected override void Initialize()
	{
		base.Initialize();

        hlg = this.gameObject.FindChild<HorizontalLayoutGroup>(true);
        main_csf = GetComponent<ContentSizeFitter>();
        text_csf = this.gameObject.FindChildByPath<ContentSizeFitter>("Text");
        checkObj = this.gameObject.FindChildByPath<RectTransform>("Check/Obj").gameObject;

        Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIText>(typeof(UITextE));

        GetButton(UIButtonE.Check).AddClickEvent(OnClickCheck);
    }

    protected override void UISet()
    {
        base.UISet();
        GetText(UITextE.Text).text = Script;

        text_csf.SetLayoutHorizontal();
        hlg.SetLayoutHorizontal();
        main_csf.SetLayoutHorizontal();

        CheckSetting();
    }
    public void UISet(bool flag)
    {
        IsChecked = flag;
        CheckSetting();
    }
    void OnClickCheck(PointerEventData ped)
    {
        IsChecked = !IsChecked;
        CheckSetting();

        if (CheckHandler != null)
            CheckHandler.Invoke(IsChecked);
    }
    void CheckSetting()
    {
        checkObj.SetActive(IsChecked);
    }

    public enum UIImageE
    {
		Check,
    }
    public enum UIButtonE
    {
		Check,
    }
    public enum UITextE
    {
		Text,
    }
}
