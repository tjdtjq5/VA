using Shared.BBNumber;
using UnityEngine;

public class NeedItem : UIFrame
{
    [SerializeField] private ContentSizeRectTransform _csrt;

    protected override void Initialize()
    {
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }
    public void UISet(ItemValue itemValue)
    {
        GetImage(UIImageE.Icon).sprite = itemValue.item.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();

        bool isAlphabet = itemValue.item.IsAlphabet;

        BBNumber playerItemCount = Managers.PlayerData.GetPlayerItemCount(itemValue.item.CodeName);
        GetTextPro(UITextProE.Text).text = $"{(isAlphabet ? playerItemCount.Alphabet() : playerItemCount.ToInt())} / {(isAlphabet ? itemValue.value.Alphabet() : itemValue.value.ToInt())}";
    
        _csrt.SetFitHorizontal();
    }

	public enum UIImageE
    {
		Icon,
    }
	public enum UITextProE
    {
		Text,
    }
}