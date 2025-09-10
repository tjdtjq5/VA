using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefMapIconButton : UIButton
{
    public void UISet(Sprite icon)
    {
	    Image.sprite = icon;
	    Image.SetNativeSize();
    }
}
