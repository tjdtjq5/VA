using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleAutoButton : UIButton
{
    public Action OnClickEvent;

    [SerializeField] private UIPuzzle _puzzle;
    [SerializeField] private Sprite noneSprite;
    [SerializeField] private Sprite adSprite;
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));


        
        _count = _initCount;
        AddClickEvent((ped) => OnClick());

        SetSwitch(false);
        SetCount(_count);

        base.Initialize();
    }

    private bool _isOn = false;
    private int _count = 0;
    private int _initCount = 10;

    private void OnClick()
    {
        if (_count <= 0)
        {
            _count = _initCount;
            SetCount(_count);
            return;
        }

        SetCount(_count);
        OnClickEvent?.Invoke();
        SetSwitch(false);
    }

    public bool Use()
    {
        if (!_isOn)
            return false;

        if (_count <= 0)
        {
            _isOn = false;
            SetCount(_count);
            return false;
        }

        _count--;
        SetCount(_count);
        return true;
    }

    public void SetSwitch(bool isOn)
    {
        _isOn = isOn;
        // GetImage(UIImageE.Flow_Icon).sprite = isOn ? playSprite : stopSprite;
        // GetImage(UIImageE.Flow_Icon).SetNativeSize();

        // UISet(isOn ? ButtonSprite.Button_Green : ButtonSprite.Button_Gray);
    }
    private void SetCount(int count)
    {
        _count = count;
        GetTextPro(UITextProE.Count_Text).text = count <= 0 ? "" : $"{count} / {_initCount}";
        GetImage(UIImageE.Count_Ad).gameObject.SetActive(_count <= 0);
    }

	public enum UIImageE
    {
		Count,
		Count_Ad,
    }
	public enum UITextProE
    {
		Count_Text,
    }
}