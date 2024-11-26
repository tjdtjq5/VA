using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayer : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    public void SetLeftDown(Action<PointerEventData> action)
    {
	    GetButton(UIButtonE.LeftArrow).AddPointDownEvent(action);
    }
    public void SetLeftUp(Action<PointerEventData> action)
    {
	    GetButton(UIButtonE.LeftArrow).AddPointUpEvent(action);
    }
    public void SetRightDown(Action<PointerEventData> action)
    {
	    GetButton(UIButtonE.RightArrow).AddPointDownEvent(action);
    }
    public void SetRightUp(Action<PointerEventData> action)
    {
	    GetButton(UIButtonE.RightArrow).AddPointUpEvent(action);
    }
	
	public enum UIButtonE
    {
		RightArrow,
		LeftArrow,
    }
	public enum UIImageE
    {
		Anger,
    }
}