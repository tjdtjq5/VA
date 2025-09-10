using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountView : UIFrame
{
    public Action<int> OnCountChanged;

    public int Count { get => _count; set => _count = value; }
    private int _count;
    private int _maxCount;
    private int _minCount;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.RightArrow).AddPressedEvent((ped) => OnRightArrow());
        GetButton(UIButtonE.LeftArrow).AddPressedEvent((ped) => OnLeftArrow());

        base.Initialize();
    }

    public void UISet(int count, int minCount, int maxCount)
    {
        _minCount = minCount;
        _maxCount = maxCount;

        _count = Mathf.Clamp(count, _minCount, _maxCount);

        _count = count;
        GetTextPro(UITextProE.Count_Text).text = $"{_count}";
    }

    private void OnRightArrow()
    {
        _count++;
        _count = Mathf.Clamp(_count, _minCount, _maxCount);

        GetTextPro(UITextProE.Count_Text).text = $"{_count}";

        OnCountChanged?.Invoke(_count);
    }
    private void OnLeftArrow()
    {
        _count--;
        _count = Mathf.Clamp(_count, _minCount, _maxCount);

        GetTextPro(UITextProE.Count_Text).text = $"{_count}";

        OnCountChanged?.Invoke(_count);
    }

	public enum UIImageE
    {
		Count,
    }
	public enum UITextProE
    {
		Count_Text,
    }
	public enum UIButtonE
    {
		RightArrow,
		LeftArrow,
    }
}