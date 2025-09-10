using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using UnityEngine;

public class UseItemButton : UIButton
{
    [SerializeField] private Item _item;
    [SerializeField] private int _count;
    [SerializeField] private ContentSizeRectTransform _csrt;

    private readonly string _countText = "x{0}";

    protected override void Initialize()
    {
        Bind<UITextPro>(typeof(UITextProE));
        Bind<UIImage>(typeof(UIImageE));
        Bind<UIText>(typeof(UITextE));

        GetImage(UIImageE.Goods_Icon).sprite = _item.Icon;
        GetImage(UIImageE.Goods_Icon).SetNativeSize();
        GetText(UITextE.Goods_Count).text = CSharpHelper.Format_H(_countText, _count);

        _csrt.SetFitHorizontal();

        base.Initialize();
    }

	public enum UITextProE
    {
		Text,
    }
	public enum UIImageE
    {
		Goods_Icon,
    }
	public enum UITextE
    {
		Goods_Count,
    }
}