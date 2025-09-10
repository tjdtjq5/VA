using System;
using UnityEngine;

public class ButtonPuzzle : UIButton
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

		_fxAniController = fxAnimator.Initialize();
		
		AddOnEnterEvent((ped) => OnEnterFunc());
		AddOnExitEvent((ped) => OnExitFunc());
		AddPointDownEvent((ped) => OnEnterFunc());
		AddPointUpEvent((ped) => OnExitFunc());
		AddPointUpEvent((ped) => OnPointerUpFunc());
		
        base.Initialize();
    }

    public Action<Vector2Int> OnEnter;
    public Action<Vector2Int> OnExit;
    public Action OnPointerUp;
    public Action<PuzzleData> OnUISet;
    
	public Transform PerfectLineTr;
    [SerializeField] private Animator fxAnimator;
    
    [SerializeField] private Sprite puzzleNone;
    [SerializeField] private Sprite puzzleRed;
    [SerializeField] private Sprite puzzleBlue;
    [SerializeField] private Sprite puzzleGreen;
    [SerializeField] private Sprite puzzleYellow;

    [SerializeField] private Sprite weaponRed;
    [SerializeField] private Sprite weaponBlue;
    [SerializeField] private Sprite weaponGreen;
    [SerializeField] private Sprite weaponYellow;
    
    private PuzzleData _puzzleData;
    private bool _isSelect = false;
    private bool _isForce = false;
    private bool _isOnEnter = false;
	private bool _isItem = false;
    private AniController _fxAniController;

    private readonly string _colorWhite = "FFFFFF";
    private readonly string _offColorRed = "7E2437";
    private readonly string _offColorBlue = "166FAB";
    private readonly string _offColorGreen = "1A7E0B";
    private readonly string _offColorYellow = "BE7E00";
    private readonly int _force_rHash = Animator.StringToHash("Force_Red");
    private readonly int _force_bHash = Animator.StringToHash("Force_Blue");
    private readonly int _force_gHash = Animator.StringToHash("Force_Green");
    private readonly int _force_yHash = Animator.StringToHash("Force_Yellow");
	private readonly int _noneHash = Animator.StringToHash("None");
    private readonly int _changeHash = Animator.StringToHash("Change");
    private readonly string _piecePrefabPath = "Prefab/Effect/InGame/PuzzleCrash";

    public void UISet(PuzzleData puzzleData)
    {
	    GetImage(UIImageE.Select).gameObject.SetActive(false);

	    this._puzzleData = puzzleData;
	    Switch(false);
	    SetColor(puzzleData);
	    SetWeapon(puzzleData);
	    
	    OnUISet?.Invoke(puzzleData);
    }

    public void Switch(bool isOn)
    {
	    this._isSelect = isOn;
	    
	    switch (_puzzleData.puzzleType)
	    {
		    case PuzzleType.Red:
			    GetImage(UIImageE.Weapon).SetColor(isOn ? _colorWhite : _offColorRed);
			    break;
		    case PuzzleType.Blue:
			    GetImage(UIImageE.Weapon).SetColor(isOn ? _colorWhite : _offColorBlue);
			    break;
		    case PuzzleType.Green:
			    GetImage(UIImageE.Weapon).SetColor(isOn ? _colorWhite : _offColorGreen);
			    break;
		    case PuzzleType.Yellow:
			    GetImage(UIImageE.Weapon).SetColor(isOn ? _colorWhite : _offColorYellow);
			    break;
	    }

	    SetOutLine(isOn);
    }
    
    public void ItemSwitch(bool isOn)
    {
		_isItem = isOn;
	    GetImage(UIImageE.Weapon).gameObject.SetActive(!isOn);
    }

    public void ForceSwitch(bool isOn)
    {
	    this._isForce = isOn;
	    SetOutLine(_isSelect);

		if (isOn)
		{
			switch (_puzzleData.puzzleType)
			{
				case PuzzleType.Red:
					_fxAniController.SetTrigger(_force_rHash);
					break;
				case PuzzleType.Blue:
					_fxAniController.SetTrigger(_force_bHash);
					break;
				case PuzzleType.Green:
					_fxAniController.SetTrigger(_force_gHash);
					break;
				case PuzzleType.Yellow:
					_fxAniController.SetTrigger(_force_yHash);
					break;
			}
		}
		else
		{
			_fxAniController.SetTrigger(_noneHash);
		}
    }

    public void ChangeEffect()
    {
	    _fxAniController.SetTrigger(_changeHash);
    }

    public void ClashEffect()
    {
	    PuzzleCrash piece = Managers.Resources.Instantiate<PuzzleCrash>(_piecePrefabPath, this.transform);
	    piece.transform.localPosition = Vector3.zero;
	    piece.transform.localScale = Vector3.one;
	    piece.Play(_puzzleData.puzzleType);
    }

    void SetColor(PuzzleData puzzleData)
    {
	    GetImage(UIImageE.Crash).gameObject.SetActive(puzzleData.isBlock);
	    Image.color = puzzleData.isBlock ? Color.clear : Color.white;
	    if (puzzleData.isBlock)
		    return;
	    
	    switch (puzzleData.puzzleType)
	    {
		    case PuzzleType.None:
			    Image.sprite = puzzleNone;
			    break;
		    case PuzzleType.Red:
			    Image.sprite = puzzleRed;
			    break;
		    case PuzzleType.Blue:
			    Image.sprite = puzzleBlue;
			    break;
		    case PuzzleType.Green:
			    Image.sprite = puzzleGreen;
			    break;
		    case PuzzleType.Yellow:
			    Image.sprite = puzzleYellow;
			    break;
	    }
    }

    void SetOutLine(bool isOn)
    {
	    GetImage(UIImageE.Select).gameObject.SetActive(isOn);
    }
    void SetWeapon(PuzzleData puzzleData)
    {
	    GetImage(UIImageE.Weapon).gameObject.SetActive(!puzzleData.isBlock && !_isItem);

	    switch (puzzleData.puzzleType)
	    {
		    case PuzzleType.None:
			    GetImage(UIImageE.Weapon).gameObject.SetActive(false);
			    break;
		    case PuzzleType.Red:
			    GetImage(UIImageE.Weapon).sprite = weaponRed;
			    GetImage(UIImageE.Weapon).SetNativeSize();
			    break;
		    case PuzzleType.Blue:
			    GetImage(UIImageE.Weapon).sprite = weaponBlue;
			    GetImage(UIImageE.Weapon).SetNativeSize();
			    break;
		    case PuzzleType.Green:
			    GetImage(UIImageE.Weapon).sprite = weaponGreen;
			    GetImage(UIImageE.Weapon).SetNativeSize();
			    break;
		    case PuzzleType.Yellow:
			    GetImage(UIImageE.Weapon).sprite = weaponYellow;
			    GetImage(UIImageE.Weapon).SetNativeSize();
			    break;
	    }
    }

    void OnEnterFunc()
    {
	    if (_isOnEnter)
		    return;
	    
	    if (!Input.GetMouseButton(0))
		    return;

	    _isOnEnter = true;
	    OnEnter?.Invoke(_puzzleData.number);
    }
    void OnExitFunc()
    {
	    if (!_isOnEnter)
		    return;
	    
	    _isOnEnter = false;
	    
	    if (!Input.GetMouseButton(0))
		    return;
	    
	    OnExit?.Invoke(_puzzleData.number);
    }

    void OnPointerUpFunc()
    {
	    OnPointerUp?.Invoke();
    }

	public enum UIImageE
    {
		Weapon,
		Select,
		Crash,
		Fx_ForceOutLine,
		Fx_ForceOutLine_Rotation,
		Fx_Common,
		Fx_Common_LightOutLine,
		Fx_Common_CircleFlow,
		Fx_Common_CircleFlowSmall,
		Fx_Common_CircleFrame,
		Fx_Common_Light,
		Fx_Common_BLight,
    }
}
[System.Serializable]
public class PuzzleData : ICloneable
{
	public PuzzleType puzzleType;
	public Vector2Int number;
	public bool isForce;
	public bool isBlock;
	
	public object Clone() { return this.MemberwiseClone(); }
}

public class PuzzleAttackData
{
	public PuzzleData data;
	public int combo;
	public int forceCount;
	public int itemCount;
	public bool isSequence;
}

public enum PuzzleType
{
	None,
	Red,
	Blue,
	Green,
	Yellow,
}