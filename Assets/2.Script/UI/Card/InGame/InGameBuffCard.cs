using UnityEngine.UI;

public class InGameBuffCard : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }

    public void UISet(Buff buff)
    {
	    GetImage(UIImageE.Layout_Icon).sprite = buff.Icon;
	    GetImage(UIImageE.Layout_Icon).SetNativeSize();
	    GetText(UITextE.Layout_Text).text = buff.Description;
    }
    
	public enum UIImageE
    {
		Layout,
		Layout_Icon,
    }
	public enum UITextE
    {
		Layout_Text,
    }
}