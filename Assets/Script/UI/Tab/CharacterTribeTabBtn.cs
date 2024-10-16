using UnityEngine;
using UnityEngine.UI;

public class CharacterTribeTabBtn : UITabButton
{
    public bool IsAll;
    [ShowWhen("IsAll", false)] public Tribe Tribe;

    Text allText;
    Image tribeImg;

    Color onColor;
    Color offColor = new Color(0.5f, 0.44f, 0.39f, 1);

    protected override void Initialize()
    {
        if (IsAll)
        {
            allText = this.gameObject.FindChild<Text>();
            onColor = Color.white;
        }
        else
        {
            tribeImg = this.gameObject.FindChild<Image>();
            onColor = DefineColor.GetTribe(Tribe);
        }

        base.Initialize();
    }
    protected override void UIOnSet()
    {
        if (IsAll)
            allText.color = onColor;
        else
            tribeImg.color = onColor;
    }
    protected override void UIOffSet()
    {
        if (IsAll)
            allText.color = offColor;
        else
            tribeImg.color = offColor;
    }
}
