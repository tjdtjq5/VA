using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class UIGoods : UIFrame
{
	public Action OnIncrease;
	public Item Item => _item;
	public RectTransform IconTr { get; set; }

	[SerializeField] private Item _item;

	private BBNumber _value;
	private Animator _animator;
	private readonly int _plusPlayAnim = Animator.StringToHash("plus");
	private readonly int _scalePlayAnim = Animator.StringToHash("scale");

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

		GetImage(UIImageE.Icon).sprite = Item.Icon;
		GetImage(UIImageE.Icon).SetNativeSize();
		UISet(0);

        base.Initialize();
    }
    protected override void UISet()
    {
	    base.UISet();
	    
	    _animator = GetComponent<Animator>();
	    IconTr = GetImage(UIImageE.Icon).RectTransform;
    }

    public void UISet(BBNumber count)
    {
		bool isAlphabet = _item.IsAlphabet;

	    _value = count;
		GetText(UITextE.Count).text = isAlphabet ? count.Alphabet() : count.ToInt().ToString();
    }
    public void PlusPlay(BBNumber newValue)
    {
		bool isAlphabet = _item.IsAlphabet;

		if(!isAlphabet)
		{
	    	Managers.Tween.TweenTextIncreaseValue(GetText(UITextE.Count), _value.ToInt(), newValue.ToInt(), 0.6f).SetOnComplete(()=> OnIncrease?.Invoke());
	    	_value = newValue;
		}
		else
		{
			UISet(newValue);
		}
    }
    public void ScalePlay()
    {
	    _animator.SetTrigger(_scalePlayAnim);
    }
	public enum UIImageE
    {
		Bg,
		Icon,
    }
	public enum UITextE
    {
		Count,
    }
}