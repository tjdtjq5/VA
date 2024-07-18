using UnityEngine;
using UnityEngine.UI;

public class TestCard : UICard
{
    TestCardData _data;

    public override void Initialize()
    {
        base.Initialize();

        Bind<Text>(typeof(Texts));
    }

    enum Texts
    {
        NumbrerText
    }

    public override void Setting(ICardData data)
    {
        _data = (TestCardData)data;
        Get<Text>(Texts.NumbrerText).text = _data.Id.ToString();
    }
}
