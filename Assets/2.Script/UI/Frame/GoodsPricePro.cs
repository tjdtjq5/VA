using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class GoodsPricePro : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }

    public void UISet(string itemCode)
    {
	    GetImage(UIImageE.Image).sprite = Managers.Atlas.GetItem(itemCode, false);
	    GetImage(UIImageE.Image).SetNativeSize();
    }
    
    public void UISet(Sprite sprite)
    {
	    GetImage(UIImageE.Image).sprite = sprite;
	    GetImage(UIImageE.Image).SetNativeSize();
    }

    public void SetCount(BBNumber count, bool isAlphabet)
    {
	    GetTextPro(UITextProE.Count).text = isAlphabet ? count.Alphabet() : count.ToInt().ToString(); 
    }
    public void SetText(string text)
    {
      GetTextPro(UITextProE.Count).text = text;
    }
    
	public enum UIImageE
    {
		Image,
    }
	public enum UITextProE
    {
		Count,
    }
}
