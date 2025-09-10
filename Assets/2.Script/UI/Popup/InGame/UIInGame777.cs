using System;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Shared.CSharp;
using UnityEngine;
using Random = UnityEngine.Random;

public class UIInGame777 : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));


		_coinLeftAniCon = coinLeftAni.Initialize();
		_coinMiddleAniCon = coinMiddleAni.Initialize();
		_coinRightAniCon = coinRightAni.Initialize();
		_machineAniCon = machineAni.Initialize();
		
		GetButton(UIButtonE.Main_RollBtn).AddClickAniEvent((ped) => OnClickRoll());
		GetButton(UIButtonE.Main_StopBtn).AddClickAniEvent((ped) => OnClickStop());

        base.Initialize();
    }
    [SerializeField] private Animator machineAni;
    [SerializeField] private Animator coinLeftAni;
    [SerializeField] private Animator coinMiddleAni;
    [SerializeField] private Animator coinRightAni;
    [SerializeField] private ParticleImage coinLeftParticle;
    [SerializeField] private ParticleImage coinMiddleParticle;
    [SerializeField] private ParticleImage coinRightParticle;

    private readonly int _basicCount = 50;
    private readonly int _rareCount = 150;
    private readonly int _legendaryCount = 250;
    private readonly int _rollHash = Animator.StringToHash("Roll");
    private readonly int _basicHash = Animator.StringToHash("Set1");
    private readonly int _rareHash = Animator.StringToHash("Set2");
    private readonly int _legendaryHash = Animator.StringToHash("Set3");
    private readonly int _playHash = Animator.StringToHash("Play");
    private readonly int _noneHash = Animator.StringToHash("None");
    private readonly string _getPopupName = "InGame/UIInGameGet";

    private AniController _machineAniCon;
    private AniController _coinLeftAniCon;
    private AniController _coinMiddleAniCon;
    private AniController _coinRightAniCon;
    
    IEnumerator _rollCoroutine;
    IEnumerator _stopCoroutine;
    
    private InGame777Progress _progress;
    
    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    SetCoinLeft(GetRandomCoinResult);
	    SetCoinMiddle(GetRandomCoinResult);
	    SetCoinRight(GetRandomCoinResult);

	    _machineAniCon.SetTrigger(_noneHash);

	    SetProgress(InGame777Progress.Open);
    }

    void SetRoll()
    {
	    _machineAniCon.SetTrigger(_playHash);
	    _coinLeftAniCon.SetTrigger(_rollHash);
	    _coinMiddleAniCon.SetTrigger(_rollHash);
	    _coinRightAniCon.SetTrigger(_rollHash);
    }
    void SetCoinLeft(CoinResultType coinResult) => _coinLeftAniCon.SetTrigger(GetAniHash(coinResult));
    void SetCoinMiddle(CoinResultType coinResult) => _coinMiddleAniCon.SetTrigger(GetAniHash(coinResult));
    void SetCoinRight(CoinResultType coinResult) => _coinRightAniCon.SetTrigger(GetAniHash(coinResult));

    void SetProgress(InGame777Progress progress)
    {
	    this._progress = progress;
	    SetButton(progress);
    }
    void SetButton(InGame777Progress progress)
    {
	    switch (progress)
	    {
		    case InGame777Progress.Open:
			    GetButton(UIButtonE.Main_RollBtn).gameObject.SetActive(true);
			    GetButton(UIButtonE.Main_StopBtn).gameObject.SetActive(false);
			    break;
		    case InGame777Progress.RollStart:
			    GetButton(UIButtonE.Main_RollBtn).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_StopBtn).gameObject.SetActive(false);
			    break;
		    case InGame777Progress.Roll:
			    GetButton(UIButtonE.Main_RollBtn).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_StopBtn).gameObject.SetActive(true);
			    break;
		    case InGame777Progress.Stop:
			    GetButton(UIButtonE.Main_RollBtn).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_StopBtn).gameObject.SetActive(false);
			    break;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(progress), progress, null);
	    }
    }
    void OnClickRoll()
    {
	    if (_progress != InGame777Progress.Open)
		    return;
	    
	    SetProgress(InGame777Progress.RollStart);

	    SetRoll();
	    
	    if (_rollCoroutine != null)
		    StopCoroutine(_rollCoroutine);
	    
	    _rollCoroutine = RollCoroutine();
	    StartCoroutine(_rollCoroutine);
    }
    void OnClickStop()
    {
	    if (_progress != InGame777Progress.Roll)
		    return;
	    
	    SetProgress(InGame777Progress.Stop);
	    
	    if (_stopCoroutine != null)
		    StopCoroutine(_stopCoroutine);

	    _stopCoroutine = StopCoroutine();
	    StartCoroutine(_stopCoroutine);
    }

    IEnumerator RollCoroutine()
    {
	    //yield return new WaitForSeconds(1.5f);
	    yield return new WaitForSeconds(0.2f);
	    SetProgress(InGame777Progress.Roll);
    }
    IEnumerator StopCoroutine()
    {
	    CoinResultType leftResult = GetRandomCoinResult;
	    CoinResultType middleResult = GetRandomCoinResult;
	    CoinResultType rightResult = GetRandomCoinResult;
	    
	    SetCoinLeft(leftResult);
	    coinLeftParticle.Play();
	    
	    yield return new WaitForSeconds(0.5f);
	    
	    SetCoinMiddle(middleResult);
	    coinMiddleParticle.Play();
	    
	    yield return new WaitForSeconds(0.5f);
	    
	    SetCoinRight(rightResult);
	    coinRightParticle.Play();

		_machineAniCon.SetTrigger(_noneHash);
	    
	    yield return new WaitForSeconds(0.5f);

	    Result(leftResult, middleResult, rightResult);
    }

    void Result(CoinResultType leftResult, CoinResultType middleResult, CoinResultType rightResult)
    {
	    int resultCount = 0;
	    resultCount += GetCoinCount(leftResult);
	    resultCount += GetCoinCount(middleResult);
	    resultCount += GetCoinCount(rightResult);
	    
	    Managers.Observer.Gesso += resultCount;
	    
	    UIInGameGet uiInGameGet = Managers.UI.ShopPopupUI<UIInGameGet>(_getPopupName, CanvasOrderType.Middle);
	    uiInGameGet.UISetGesso(resultCount);

	    uiInGameGet.OnClose -= ClosePopupUI;
	    uiInGameGet.OnClose += ClosePopupUI;	
    }

    int GetAniHash(CoinResultType coinResult)
    {
	    switch (coinResult)
	    {
		    case CoinResultType.Basic:
			    return _basicHash;
		    case CoinResultType.Rare:
			    return _rareHash;
		    case CoinResultType.Legendary:
			    return _legendaryHash;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(coinResult), coinResult, null);
	    }
    }

    int GetCoinCount(CoinResultType coinResult)
    {
	    switch (coinResult)
	    {
		    case CoinResultType.Basic:
			    return _basicCount;
		    case CoinResultType.Rare:
			    return _rareCount;
		    case CoinResultType.Legendary:
			    return _legendaryCount;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(coinResult), coinResult, null);
	    }
    }
    CoinResultType GetRandomCoinResult => (CoinResultType)UnityHelper.Random_H(0, CSharpHelper.GetEnumLength<CoinResultType>());
    public enum CoinResultType
    {
	    Basic,
	    Rare,
	    Legendary,
    }

    public enum InGame777Progress
    {
	    Open,
	    RollStart,
	    Roll,
	    Stop,
    }
	public enum UIImageE
    {
		BlackPannel,
		Main_Title,
		Main_777_Bg,
		Main_777_Bg_Left_Main_coin1,
		Main_777_Bg_Left_Main_coin2,
		Main_777_Bg_Middle_Main_coin1,
		Main_777_Bg_Middle_Main_coin2,
		Main_777_Bg_Right_Main_coin1,
		Main_777_Bg_Right_Main_coin2,
		Main_777_Frame,
    }
	public enum UITextE
    {
		Main_Title_Text,
		Main_Ex,
    }
	public enum UIButtonE
    {
		Main_RollBtn,
		Main_StopBtn,
    }
}