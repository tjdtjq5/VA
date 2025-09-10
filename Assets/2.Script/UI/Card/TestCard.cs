using UnityEngine;
using UnityEngine.UI;

public class TestCard : UICard
{
    TestCardData _data;

	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIText>(typeof(UITextE));
    }

    enum Texts
    {
        NumbrerText
    }

    public override void Setting(ICardData data)
    {
	    _data = (TestCardData)data;
        Get<UIText>(Texts.NumbrerText).text = _data.Id.ToString();
    }
	public enum UITextE
    {
		NumbrerText,
    }
}
