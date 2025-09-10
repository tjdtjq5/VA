using Shared.BBNumber;
using UnityEngine;

public class GoodsPrice : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

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
	    GetText(UITextE.Count).text = isAlphabet ? count.Alphabet() : count.ToInt().ToString(); 
    }
    
	public enum UIImageE
    {
		OutLine,
		Image,
    }
	public enum UITextE
    {
		Count,
    }
}