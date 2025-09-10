using System;
using System.Collections.Generic;
using Shared.CSharp;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInGameFlipOver : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIInGameFlipOverRewardCard>(typeof(UIInGameFlipOverRewardCardE));
		Bind<UIInGameFlipOverCard>(typeof(UIInGameFlipOverCardE));

        base.Initialize();
    }
	private readonly int _legendaryCount = 3;
	private readonly int _rareCount = 5;
    private readonly int _duplicatedCount = 3;
    private readonly int _basicGesso = 150;
    private readonly int _rareGesso = 450;
    private readonly int _legendaryGesso = 750;
    private readonly string _getPopupName = "InGame/UIInGameGet";
    int CardLen => CSharpHelper.GetEnumLength<UIInGameFlipOverCardE>();
    UIInGameFlipOverCard GetCard(int index) => Get<UIInGameFlipOverCard>((UIInGameFlipOverCardE)index);
    public bool IsFlipping { get; private set; }
    public bool IsEnded { get; private set; }

    private readonly List<int> _basicCardIndexes = new();
    private readonly List<int> _rareCardIndexes = new();
    private readonly List<int> _legendaryCardIndexes = new();
    private readonly List<FlipOverCardType> _resultTypes = new();

    protected override void UISet()
    {
	    base.UISet();
	    
	    Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Legendary).UISet(FlipOverCardType.Legendary);
	    Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Rare).UISet(FlipOverCardType.Rare);
	    Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Basic).UISet(FlipOverCardType.Basic);
	    
	    for (int i = 0; i < CardLen; i++)
	    {
		    UIInGameFlipOverCard card = GetCard(i);
		    card.Initialize(this);
		    card.OnFlipStart += OnFlipStart;
		    card.OnFlipEnd += OnFlipEnd;
	    }
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    this.IsFlipping = false;
	    this.IsEnded = false;
	    this._resultTypes.Clear();
	    CardIndexShuffle();
	    SetCard();
    }
    void CardIndexShuffle()
    {
	    _basicCardIndexes.Clear();
	    _rareCardIndexes.Clear();
	    _legendaryCardIndexes.Clear();
	    
	    List<int> totalIndexes = new();

	    for (int i = 0; i < CardLen; i++)
	    {
		    totalIndexes.Add(i);
	    }
	    
	    totalIndexes.Shuffle();

	    int startIndex = 0;
	    for (int i = startIndex; i < _legendaryCount; i++)
		    _legendaryCardIndexes.Add(totalIndexes[i]);

	    startIndex += _legendaryCount;
	    for (int i = startIndex; i < startIndex + _rareCount; i++)
		    _rareCardIndexes.Add(totalIndexes[i]);
	    
	    startIndex += _rareCount;
	    for (int i = startIndex; i < totalIndexes.Count; i++)
		    _basicCardIndexes.Add(totalIndexes[i]);
    }
    void SetCard()
    {
	    for (int i = 0; i < _basicCardIndexes.Count; i++)
	    {
		    UIInGameFlipOverCard card = GetCard(_basicCardIndexes[i]);
		    card.UISet(FlipOverCardType.Basic);
	    }
	    
	    for (int i = 0; i < _rareCardIndexes.Count; i++)
	    {
		    UIInGameFlipOverCard card = GetCard(_rareCardIndexes[i]);
		    card.UISet(FlipOverCardType.Rare);
	    }
	    
	    for (int i = 0; i < _legendaryCardIndexes.Count; i++)
	    {
		    UIInGameFlipOverCard card = GetCard(_legendaryCardIndexes[i]);
		    card.UISet(FlipOverCardType.Legendary);
	    }
    }
    void CheckSet(int count, FlipOverCardType type)
	{
		switch (type)
		{
			case FlipOverCardType.Basic:
				Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Basic).CheckSet(count);
				break;
			case FlipOverCardType.Rare:
				Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Rare).CheckSet(count);
				break;
			case FlipOverCardType.Legendary:
				Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Legendary).CheckSet(count);
				break;
		}
	}
    void OnFlipStart(FlipOverCardType type)
    {
	    IsFlipping = true;
    }
    void OnFlipEnd(FlipOverCardType type)
    {
	    IsFlipping = false;
	    
	    _resultTypes.Add(type);

	    if (!IsEnded)
	    {
		    int count = _resultTypes.FindAll(t => t == type).Count;
		    CheckSet(count, type);
		    if (count >= _duplicatedCount)
		    {
			    IsEnded = true;

			    CardLight(type);
			    RewardLight(type);
			    Managers.Tween.TweenInvoke(1.5f).SetOnPerceontCompletedFloat(1, ()=> Result(type)).SetIgnoreTimeScaleFloat();
		    }
	    }
    }

    void CardLight(FlipOverCardType type)
    {
	    for (int i = 0; i < CardLen; i++)
	    {
		    UIInGameFlipOverCard card = GetCard(i);
		    if (card.IsFlipped && card.Type == type)
		    {
			    card.Light();
		    }
	    }
    }
    void RewardLight(FlipOverCardType type)
    {
	    switch (type)
	    {
		    case FlipOverCardType.Basic:
			    Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Basic).Light();
			    break;
		    case FlipOverCardType.Rare:
			    Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Rare).Light();
			    break;
		    case FlipOverCardType.Legendary:
			    Get<UIInGameFlipOverRewardCard>(UIInGameFlipOverRewardCardE.Rewards_Legendary).Light();
			    break;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(type), type, null);
	    }
    }

    void Result(FlipOverCardType type)
    {
	    int resultCount = GetCoinCount(type);
	    
	    Managers.Observer.Gesso += resultCount;
	    
	    UIInGameGet uiInGameGet = Managers.UI.ShopPopupUI<UIInGameGet>(_getPopupName, CanvasOrderType.Middle);
	    uiInGameGet.UISetGesso(resultCount);

	    uiInGameGet.OnClose -= ClosePopupUI;
	    uiInGameGet.OnClose += ClosePopupUI;
    }
    
    int GetCoinCount(FlipOverCardType coinResult)
    {
	    switch (coinResult)
	    {
		    case FlipOverCardType.Basic:
			    return _basicGesso;
		    case FlipOverCardType.Rare:
			    return _rareGesso;
		    case FlipOverCardType.Legendary:
			    return _legendaryGesso;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(coinResult), coinResult, null);
	    }
    }

    public enum UIImageE
    {
		BlackPannel,
		Title,
    }
	public enum UITextE
    {
		Title_Text,
		Ex,
    }
	public enum UIInGameFlipOverRewardCardE
    {
		Rewards_Legendary,
		Rewards_Rare,
		Rewards_Basic,
    }
	public enum UIInGameFlipOverCardE
    {
		Cards_0,
		Cards_1,
		Cards_2,
		Cards_3,
		Cards_4,
		Cards_5,
		Cards_6,
		Cards_7,
		Cards_8,
		Cards_9,
		Cards_10,
		Cards_11,
    }
}	