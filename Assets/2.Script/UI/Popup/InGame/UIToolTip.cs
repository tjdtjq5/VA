using System;
using UnityEngine;

public class UIToolTip : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UITextPro>(typeof(UITextProE));

		
		GetButton(UIButtonE.CloseButton).AddClickEvent((ped) => ClosePopupUIPlayAni());

        base.Initialize();
    }
    [SerializeField] private Transform main;

    public void UISet(IdentifiedObject identifiedObject, Transform target, TooltipArrow tooltipArrow)
    {
	    main.transform.position = GetPosition(target, tooltipArrow);

	    GetText(UITextE.Main_Title).text = identifiedObject.DisplayName;
	    GetTextPro(UITextProE.Main_Descript).text = identifiedObject.Description;

	    SetArrow(tooltipArrow);
    }

    void SetArrow(TooltipArrow tooltipArrow)
    {
	    GetImage(UIImageE.Main_Arrow_Up).gameObject.SetActive(false);
	    GetImage(UIImageE.Main_Arrow_Left).gameObject.SetActive(false);
	    GetImage(UIImageE.Main_Arrow_Down).gameObject.SetActive(false);
	    GetImage(UIImageE.Main_Arrow_Right).gameObject.SetActive(false);
	    
	    switch (tooltipArrow)
	    {
		    case TooltipArrow.Up:
			    GetImage(UIImageE.Main_Arrow_Up).gameObject.SetActive(true);
			    break;
		    case TooltipArrow.Down:
			    GetImage(UIImageE.Main_Arrow_Down).gameObject.SetActive(true);
			    break;
		    case TooltipArrow.Right:
			    GetImage(UIImageE.Main_Arrow_Right).gameObject.SetActive(true);
			    break;
		    case TooltipArrow.Left:
			    GetImage(UIImageE.Main_Arrow_Left).gameObject.SetActive(true);
			    break;
	    }
    }
    Vector3 GetPosition(Transform target, TooltipArrow tooltipArrow)
    {
	    Vector3 result = target.position;
	    RectTransform targetRectTr = target.GetComponent<RectTransform>();
	    RectTransform mainRectTr = main.GetComponent<RectTransform>();

	    switch (tooltipArrow)
	    {
		    case TooltipArrow.Up:
			    result.y -= targetRectTr.rect.height * 0.5f;
			    result.y -= mainRectTr.rect.height * 0.5f;
			    break;
		    case TooltipArrow.Down:
			    result.y += targetRectTr.rect.height * 0.5f;
			    result.y += mainRectTr.rect.height * 0.5f;
			    break;
		    case TooltipArrow.Right:
			    result.x -= targetRectTr.rect.width * 0.5f;
			    result.x -= mainRectTr.rect.width * 0.5f;
			    break;
		    case TooltipArrow.Left:
			    result.x += targetRectTr.rect.width * 0.5f;
			    result.x += mainRectTr.rect.width * 0.5f;
			    break;
	    }
	    
	    return result;
    }
	public enum UIButtonE
    {
		CloseButton,
    }
	public enum UIImageE
    {
		Main_Bg,
		Main_Arrow_Up,
		Main_Arrow_Left,
		Main_Arrow_Down,
		Main_Arrow_Right,
    }
	public enum UITextE
    {
		Main_Title,
    }
	public enum UITextProE
    {
		Main_Descript,
    }
}

public enum TooltipArrow
{
	Up,
	Down,
	Right,
	Left,
}