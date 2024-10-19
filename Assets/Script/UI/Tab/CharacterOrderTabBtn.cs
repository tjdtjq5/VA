using UnityEngine;

public class CharacterOrderTabBtn : UITabButton
{
    RectTransform iconRect;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
        iconRect = GetImage(UIImageE.Icon).RectTransform;

        base.Initialize();

        Set(0, true);
    }

    protected override void UIOffSet()
    {
        base.UIOffSet();

        iconRect.rotation = Quaternion.Euler(0, 0, 0);
    }
    protected override void UIOnSet()
    {
        base.UIOnSet();

        iconRect.rotation = Quaternion.Euler(180, 0, 0);
    }
	public enum UIImageE
    {
		Icon,
    }
}