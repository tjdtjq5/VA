using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Spine.Unity;
using UnityEngine;

public class UIInGameResult : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));

		_titleSpine = titleGraphic.Initialize();
		_characterSpine = characterGraphic.Initialize();
		
		_titleSpine.SetEndFunc(_winStartAni, TitleAniEnd);
		_titleSpine.SetEndFunc(_loseStartAni, TitleAniEnd);
		_characterSpine.SetEndFunc(_characterWinAni, CharacterAniEnd);
		_characterSpine.SetEndFunc(_characterLoseAni, CharacterAniEnd);

		GetButton(UIButtonE.Main_Button).AddClickEvent((ped) => ReStart());

        base.Initialize();
    }
    
    [SerializeField] SkeletonGraphic titleGraphic;
    [SerializeField] SkeletonGraphic characterGraphic;
    [SerializeField] Transform rewardTransform;

    private readonly string _winStartAni = "0";
    private readonly string _winLoopAni = "1";
    private readonly string _loseStartAni = "2";
    private readonly string _loseLoopAni = "3";
    private readonly string _characterWinAni = "Idle";
    private readonly string _characterLoseAni = "Die_Loop";
    
    private UIInGameResultType _resultType;
    private Dictionary<string, BBNumber> _rewardDics = new Dictionary<string, BBNumber>();
	private bool _isReStartOk = false;
	private bool _isRewardOk = false;

    private SpineAniController _titleSpine;
    private SpineAniController _characterSpine;
    private IEnumerator _rewardCoroutine;
    private List<InGameRewardCard> _rewardCards = new List<InGameRewardCard>();

    public void UISet(UIInGameResultType resultType, int stage ,Dictionary<string, BBNumber> rewardDics)
    {
	    _resultType = resultType;
	    _rewardDics = rewardDics;
		_isReStartOk = false;
		_isRewardOk = false;

	    bool isWin = resultType == UIInGameResultType.Win;
	
	    _titleSpine.Play(isWin? _winStartAni : _loseStartAni, false);
	    GetTextPro(UITextProE.Main_Title_Text).text = isWin? "승리" : "패배";
		GetTextPro(UITextProE.Main_Title_Text).SetColor(isWin ? "FFD94AFF" : "FFFFFFFF");
	    GetText(UITextE.Main_Stage).text = $"{stage} 스테이지 달성";
	    GetText(UITextE.Main_Stage).Fade(0);
	    GetImage(UIImageE.Main_Title_Light).SetColor(isWin ? "FF9B0107" : "FFFFFF07");
	    _characterSpine.gameObject.SetActive(false);
	    for (int i = 0; i < _rewardCards.Count; i++)
	    {
		    _rewardCards[i].gameObject.SetActive(false);
	    }
    }

    void TitleAniEnd()
    {
	    bool isWin = _resultType == UIInGameResultType.Win;
	    
	    _titleSpine.Play(isWin? _winLoopAni : _loseLoopAni, true);
	    Managers.Tween.TweenAlpha(GetText(UITextE.Main_Stage), 0, 1, 0.5f);
	    _characterSpine.gameObject.SetActive(true);

	    switch (_resultType)
	    {
		    case UIInGameResultType.Win:
			    _characterSpine.Play(_characterWinAni, true);
			    break;
		    case UIInGameResultType.Lose:
			    _characterSpine.Play(_characterLoseAni, true);
			    break;
		    case UIInGameResultType.TimeOver:
			    _characterSpine.Play(_characterLoseAni, true);
			    break;
	    }

		_isReStartOk = true;
    }

    void CharacterAniEnd()
    {
		_isReStartOk = true;
	    SetReward();
    }

    void SetReward()
    {
		if (_isRewardOk)
			return;

		_isRewardOk = true;
	    List<string> itemCodes = new List<string>();
	    List<BBNumber> itemCounts = new List<BBNumber>();

	    foreach (var rewardItem in _rewardDics)
	    {
		    itemCodes.Add(rewardItem.Key);
		    itemCounts.Add(rewardItem.Value);
	    }
	    
	    for (int i = 0; i < _rewardCards.Count; i++)
	    {
		    _rewardCards[i].gameObject.SetActive(false);
	    }

	    if (_rewardCoroutine != null)
	    {
		    StopCoroutine(_rewardCoroutine);
	    }

	    _rewardCoroutine = RewardCoroutine(itemCodes, itemCounts);
	    StartCoroutine(_rewardCoroutine);
    }

    IEnumerator RewardCoroutine(List<string> itemCodes, List<BBNumber> itemCounts)
    {
	    for (int i = 0; i < itemCodes.Count; i++)
	    {
		    if (i < _rewardCards.Count)
		    {
			    _rewardCards[i].gameObject.SetActive(true);
			    _rewardCards[i].Setting(itemCodes[i], itemCounts[i], false, true, 1.6f, 250f);
			    _rewardCards[i].Play();
		    }
		    else
		    {
			    InGameRewardCard card =
				    Managers.Resources.Instantiate<InGameRewardCard>("Prefab/UI/Card/InGame/InGameRewardCard", rewardTransform);
			    card.Setting(itemCodes[i], itemCounts[i], false, true, 1.6f, 250f);
			    card.Play();
			    _rewardCards.Add(card);
		    }
		    
		    yield return new WaitForSeconds(0.5f);
	    }
    }

	void ReStart()
	{
		if (_isReStartOk)
		{
			Managers.Scene.LoadScene(SceneType.InGame);
		}
	}

	public enum UIImageE
    {
		BlackPannel,
		Main_Title_Light,
    }
	public enum UITextProE
    {
		Main_Title_Text,
    }
	public enum UITextE
    {
		Main_Stage,
		Main_ExText,
    }
	public enum UIButtonE
    {
		Main_Button,
    }
}

public enum UIInGameResultType
{
	Win,
	Lose,
	TimeOver,
}