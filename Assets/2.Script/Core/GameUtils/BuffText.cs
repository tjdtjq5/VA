using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffText : UIFrame
{
    [SerializeField] private Poolable pool;
    [SerializeField] private ContentSizeRectTransform _contentSizeRectTransform;

    protected override void Initialize()
    {
        Bind<UIImage>(typeof(UIImageE));
        Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }

    public void UISet(Stat stat)
    {
        this.GetText(UITextE.Bg_Text).text = $"+ {stat.DisplayName}";
        this.GetImage(UIImageE.Bg_icon).sprite = stat.Icon;
        this.GetImage(UIImageE.Bg_icon).SetNativeSize();

        _contentSizeRectTransform.SetFitHorizontal();
        pool.Play();
    }

    public enum UIImageE
    {
        Bg,
        Bg_Light,
        Bg_icon,
    }

    public enum UITextE
    {
        Bg_Text,
    }
}