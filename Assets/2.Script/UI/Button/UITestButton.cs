using UnityEngine.EventSystems;

public class UITestButton : UIButton
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
	}

    protected override void UISet()
    {
        base.UISet();

		AddPressedEvent(PressEvent);
    }

	void PressEvent(PointerEventData ped)
	{
		UnityHelper.Log_H("Pressed");
	}

    public enum UIImageE
    {
		Icon,
		Name,
    }
	public enum UITextE
    {
		Name_Text,
    }
}
