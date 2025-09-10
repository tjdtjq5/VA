using UnityEngine;
using UnityEngine.UI;

public class PageCard : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }
    
    public int Index { get; private set; }
	    
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;

    public void Initialize(int index)
    {
	    this.Index = index;
	    GetText(UITextE.Text).text = (index + 1).ToString();
    }
    public void UISet(bool flag)
    {
	    this.GetComponent<Image>().color = flag ? onColor : offColor;
    }
    
	public enum UIImageE
    {
		OutLine,
    }
	public enum UITextE
    {
		Text,
    }
}