using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBarCard : UIFrame
{
	public Action<BuffBarCard> OnCardDestroy;
	
	public Buff Buff { get; set; }

	private readonly int _maxIconSize = 40;
	
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }

    public void Initialize(Buff buff)
    {
	    if (!IsInitialized)
		    Initialize();
	    
	    this.Buff = buff;
	    
		GetImage(UIImageE.Icon).RectTransform.localScale = Vector2.one;
	    GetImage(UIImageE.Icon).sprite = Buff.BuffIcon;
	    GetImage(UIImageE.Icon).SetNativeSize();

		Vector2 iconSize = GetImage(UIImageE.Icon).RectTransform.sizeDelta;
		float iconSizeMax = Mathf.Max(iconSize.x, iconSize.y);

		if (iconSizeMax > _maxIconSize)
		{
			float scale = _maxIconSize / iconSizeMax;
			GetImage(UIImageE.Icon).RectTransform.localScale = new Vector2(scale, scale);
		}
	    
	    Buff.OnCountChange -= Set;
		Buff.OnCountChange += Set;
	    
	    Set(Buff.Count);
    }

    public void Set(int count)
    {
	    if (count < 1)
	    {
		    Destroy();
		    return;
	    }
	    
	    GetTextPro(UITextProE.Count).text = count.ToString();
    }

    public void Destroy()
    {
	    OnCardDestroy?.Invoke(this);
	    OnCardDestroy = null;
	    Managers.Resources.Destroy(this.gameObject);
    }

	public enum UIImageE
    {
		Icon,
    }
	public enum UITextProE
    {
		Count,
    }
}