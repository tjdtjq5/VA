public class MainItemView : UIButton
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }

    public void UISet(ItemTableCodeDefine item)
    {
        GetImage(UIImageE.Icon).sprite = Managers.Atlas.GetItem(item);
        GetImage(UIImageE.Icon).SetNativeSize();

        string countStr = Managers.PlayerData.Item.GetItem(item).Count().Alphabet();
        GetText(UITextE.Bg_Count).text = countStr;
    }

	public enum UIImageE
    {
		Bg,
		Icon,
		Icon_Plus,
    }
	public enum UITextE
    {
		Bg_Count,
    }
}