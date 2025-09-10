using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class UIPuzzle : UIFrame
{
    protected override void Initialize()
    {
		_backgroundAniController = backgroundAnimator.Initialize();
		_backgroundAniController.SetEndFunc(_openAniName, BackgroundOpen);
		_backgroundAniController.SetEndFunc(_closeAniName, BackgroundClose);
		
		_puzzleZoom = GetComponent<UIPuzzleZoom>();
		_puzzleItem = GetComponent<UIPuzzleItem>();
		_puzzleItem.Initialize(this);
		_puzzleBase = GetComponent<UIPuzzleBase>();
		
		_puzzleDataMatrices = new PuzzleData[_puzzleMatricesSize, _puzzleMatricesSize];
		_puzzleMatrices = new ButtonPuzzle[_puzzleMatricesSize, _puzzleMatricesSize];
		_puzzleInitPositions = new Vector3[_puzzleMatricesSize, _puzzleMatricesSize];

		Reset();
		Spawn();
		SetPosition();

		pointUpButton.AddPointUpEvent((ped) => OnPointerUp());
		pointUpButton.gameObject.SetActive(false);

		_leftLofController = _leftLof.Initialize();
		_rightLofController = _rightLof.Initialize();

		_puzzleAutoButton.OnClickEvent -= OnClickPuzzleAutoButton;
		_puzzleAutoButton.OnClickEvent += OnClickPuzzleAutoButton;

        base.Initialize();
    }

    public Action<UIPuzzle> OnOpen;
    public Action<UIPuzzle> OnAniOpen;
    public Action<PuzzleData, int, int, int> OnPuzzlePointUp;
    public Action<PuzzleType, int> OnPuzzleChain;
    public Action OnPerfect;
    
    public bool Switch { get => _switch; set { _switch = value; OnChagneSwitch(value); } }
    public Transform Root => backgroundAnimator.transform;
    public Vector3 GetPuzzlePosition(Vector2Int number) => _puzzleMatrices[number.x, number.y].transform.position;
    public ButtonPuzzle GetPuzzle(Vector2Int number) => _puzzleMatrices[number.x, number.y];
    public PuzzleData GetPuzzleData(Vector2Int number)
    {
	    if (number.x >= _puzzleMatricesSize || number.y >= _puzzleMatricesSize || number.x < 0 || number.y < 0)
		    return null;

	    return _puzzleDataMatrices[number.x, number.y];
    }
    
    [SerializeField] Transform puzzleRoot;
    [SerializeField] Transform chainRoot;
    [SerializeField] Transform comboRoot;
    [SerializeField] Transform multiplierRoot;
    [SerializeField] Animator backgroundAnimator;
    [SerializeField] Transform perfectRoot;
	[SerializeField] UIButton pointUpButton;
	[SerializeField] SkeletonGraphic _leftLof;
	[SerializeField] SkeletonGraphic _rightLof;
	[SerializeField] PuzzleAutoButton _puzzleAutoButton;
    
    //private PuzzeCamera _puzzleCamera;
    private UIPuzzleZoom _puzzleZoom;
    private ChainCombo _chainCombo;
    private AniController _backgroundAniController;
    private UIPuzzleItem _puzzleItem;
    private UIPuzzleBase _puzzleBase;
	private SpineAniController _leftLofController;
	private SpineAniController _rightLofController;
    
    private readonly string _puzzlePrefab = "Prefab/UI/Button/InGame/Puzzle";
    private readonly string _chainPrefab = "Prefab/UI/Etc/InGame/PuzzleChain";
	private readonly string _perfectLinePrefab = "Prefab/UI/Etc/InGame/PerfectLineChain";
	private readonly string _perfectTrailFollower = "Prefab/UI/Etc/InGame/PuzzlePerfectTrailFollower";
    private readonly string _comboPrefab = "Prefab/UI/Etc/InGame/ChainCombo";
    private readonly string _multiplierPrefab = "Prefab/UI/Etc/InGame/PuzzleComboMultiplier";
    private readonly string _perfectPrefab = "Prefab/UI/Etc/InGame/Perfect";
    private readonly string _openAniName = "Open";
    private readonly string _closeAniName = "Close";
	private readonly string _boom1AniName = "Boom1";
	private readonly string _boom2AniName = "Boom2";
	private readonly string _bigBoomAniName = "BigBoom";
	private readonly string _lofAniName = "1";
	private readonly string _lofBoomAniName = "2";
    private readonly int _puzzleMatricesSize = 7;
    private readonly float _puzzleCellSize = 130f;
    
    private Vector3[,] _puzzleInitPositions;
    private ButtonPuzzle[,] _puzzleMatrices;
    private PuzzleData[,] _puzzleDataMatrices;
    private readonly List<Vector2Int> _puzzleCellEnters = new();
    private readonly List<PuzzleChain> _chains = new();
    private readonly List<PuzzleComboMultiplier> _multipliers = new();
    private List<Vector2Int> _perfectLinks = new();
    private PathTrailFollower _pathTrailFollower;
	private bool _isChainGradeChange = false;
	private AttackGrade ChainGrade { get => _chainGrade; set {  _isChainGradeChange = _chainGrade != value; _chainGrade = value; } }
	private AttackGrade _chainGrade = AttackGrade.Basic;
    private bool _isOpen = false;
	private bool _isAutoPerfect = false;
	private IEnumerator _autoPerfectCoroutine;
	private bool _switch = false;
    public void Open()
    {
	    if (_isOpen)
		    return;

	    _isOpen = true;
	    // Switch = true;
	    _backgroundAniController.SetTrigger(_openAniName);

		pointUpButton.gameObject.SetActive(true);
	    
	    OnOpen?.Invoke(this);

		LofAni();
    }
    public void Close()
    {
	    if (!_isOpen)
		    return;
	    
	    _isOpen = false;
	    // Switch = false;
	    _backgroundAniController.SetTrigger(_closeAniName);

		pointUpButton.gameObject.SetActive(false);
    }

	private void OnChagneSwitch(bool isOn)
	{
		_puzzleAutoButton.SetSwitch(isOn);
	}
	private void OnClickPuzzleAutoButton()
	{
		if (_puzzleAutoButton.Use())
		{
			AutoPerfect(PuzzleType.None);
		}
	}
	private void OnClickWeekAutoButton()
	{
		if (Managers.Observer.PuzzleBattleStateMachine.Enemies.Count <= 0)
			return;

		bool isOk = false;
		for(int i = 0; i < Managers.Observer.PuzzleBattleStateMachine.Enemies.Count; i++)
		{
			if (!Managers.Observer.PuzzleBattleStateMachine.Enemies[i].WeekBar.IsAllWeekCrash)
			{
				isOk = true;
				break;
			}
		}

		if (!isOk)
			return;

		if (_puzzleAutoButton.Use())
		{
			for(int i = 0; i < Managers.Observer.PuzzleBattleStateMachine.Enemies.Count; i++)
			{
				if (!Managers.Observer.PuzzleBattleStateMachine.Enemies[i].WeekBar.IsAllWeekCrash)
				{
					AutoPerfect(Managers.Observer.PuzzleBattleStateMachine.Enemies[i].WeekBar.CurrentWeek);
					break;
				}
			}
		}
	}

    private void BackgroundOpen(string aniName)
    {
	    // Clear();
	    OnAniOpen?.Invoke(this);
    }
    private void BackgroundClose(string aniName)
    {
	    Clear();
	    
	    PuzzleCrashReset();
    }
    private void Reset()
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    Vector2Int puzzlePoint = new Vector2Int(j, i);
			    _puzzleDataMatrices[j, i] = GetRandomPuzzle();
			    _puzzleDataMatrices[j, i].number = puzzlePoint;
		    }
	    }
	    
	    _puzzleDataMatrices[0, 0].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[1, 0].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[0, 1].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[_puzzleMatricesSize - 1, 0].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[_puzzleMatricesSize - 2, 0].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[_puzzleMatricesSize - 1, 1].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[_puzzleMatricesSize - 1, _puzzleMatricesSize - 1].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[_puzzleMatricesSize - 1, _puzzleMatricesSize - 2].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[_puzzleMatricesSize - 2, _puzzleMatricesSize - 1].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[1, _puzzleMatricesSize - 1].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[0, _puzzleMatricesSize - 2].puzzleType = PuzzleType.None;
	    _puzzleDataMatrices[0, _puzzleMatricesSize - 1].puzzleType = PuzzleType.None;
    }
    private void Spawn()
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] == null)
			    {
				    _puzzleMatrices[j, i] = Managers.Resources.Instantiate<ButtonPuzzle>(_puzzlePrefab, puzzleRoot);
				    _puzzleMatrices[j, i].UISet(_puzzleDataMatrices[j, i]);
				    _puzzleMatrices[j, i].OnEnter += OnEnter;
				    _puzzleMatrices[j, i].OnExit += OnExit;
				    _puzzleMatrices[j, i].OnPointerUp += OnPointerUp;
			    }
		    }
	    }
    }
    private void SetPosition()
    {
	    float startX = _puzzleCellSize * 0.5f + -_puzzleCellSize * _puzzleMatricesSize * 0.5f;
	    float startY = -_puzzleCellSize * 0.5f + _puzzleCellSize * _puzzleMatricesSize * 0.5f;
	    Vector3 startPos = new Vector3(startX, startY, 0);

		Vector3 threeByThree = Vector3.zero;
		Vector3 twoByFour = Vector3.zero;
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null)
			    {
				    Vector3 pos = startPos;
				    pos.x += _puzzleCellSize * j;
				    pos.y -= _puzzleCellSize * i;
				    _puzzleMatrices[j, i].transform.localPosition = pos;
				    _puzzleInitPositions[j, i] = _puzzleMatrices[j, i].transform.position;

					if (j == 3 && i == 3)
					{
						threeByThree = _puzzleInitPositions[j, i];
					}
					else if (j == 2 && i == 4)
					{
						twoByFour = _puzzleInitPositions[j, i];
					}
			    }
		    }
	    }

		float cellSize = threeByThree.y - twoByFour.y;
		_puzzleZoom?.Set(cellSize, threeByThree.x, threeByThree.y);
    }
    private void Chain(Vector2Int number)
    {
		PuzzleType puzzleType = _puzzleDataMatrices[number.x, number.y].puzzleType;

	    if (_puzzleCellEnters.Count <= 0)
	    {
		    if (IsLimitedType(number))
			    return;

			Managers.Sound.Play("UI/Button/Puzzle");

		    _puzzleCellEnters.Add(number);
		    _puzzleZoom?.Zoom(_puzzleInitPositions[number.x, number.y]);
		    _puzzleMatrices[number.x, number.y].Switch(true);

			if(Managers.Observer.Player.CurrentForm != puzzleType)
			{
				Managers.Observer.Player.CharacterAttackReady.CurrentGrade = GameDefine.GetAttackGrade(_puzzleCellEnters.Count);
				Managers.Observer.Player.ChangeForm(puzzleType);
			}

			_isChainGradeChange = true;

		    return;
	    }
	    Vector2Int current = _puzzleCellEnters[_puzzleCellEnters.Count - 1];
	    if (!IsSide(current, number))
	    {
		    return;
	    }
	    if (UnChain(number))
	    {
		    return;
	    }
	    if (!IsChain(number))
		    return;
	    
	    _puzzleZoom?.Zoom(_puzzleInitPositions[number.x, number.y]);
	    
	    PuzzleChain chain = Managers.Resources.Instantiate<PuzzleChain>(_chainPrefab, chainRoot);
		chain.transform.localScale = Vector3.one;
	    chain.Chain(_puzzleMatrices[current.x, current.y].transform, _puzzleMatrices[number.x, number.y].transform);
	    _chains.Add(chain);
	    
	    _puzzleMatrices[number.x, number.y].Switch(true);
	    
	    _puzzleCellEnters.Add(number);
	    OnPuzzleChain?.Invoke(puzzleType, _puzzleCellEnters.Count);
		ChainGrade = GameDefine.GetAttackGrade(_puzzleCellEnters.Count);

		if (!Managers.Observer.Player.IsChanging && _isChainGradeChange)
		{
			Managers.Observer.Player.CharacterAttackReady.SetReady(puzzleType, ChainGrade);
		}
		else
			Managers.Observer.Player.CharacterAttackReady.CurrentGrade = ChainGrade;

		Managers.Sound.Play("UI/Button/Puzzle");
	    
	    ComboIn(number);
	    MultiplierIn(number);
    }
    private void ComboIn(Vector2Int number)
    {
	    if (_chainCombo == null)
	    {
		    _chainCombo = Managers.Resources.Instantiate<ChainCombo>(_comboPrefab, comboRoot);
	    }

	    int combo = _puzzleCellEnters.Count;

	    if (combo <= 1)
	    {
		    _chainCombo.Out();
		    return;
	    }

	    Vector3 pos = _puzzleMatrices[number.x, number.y].transform.position;
	    pos.x += 15;
	    pos.y += 6;
	    _chainCombo.transform.position = pos;
	    
	    _chainCombo.UISet(_puzzleDataMatrices[number.x, number.y].puzzleType, combo);
    }

    private void MultiplierIn(Vector2Int number)
    {
	    PuzzleComboMultiplier multiplier =  Managers.Resources.Instantiate<PuzzleComboMultiplier>(_multiplierPrefab, multiplierRoot);
	    multiplier.UISet(Managers.Observer.Player, _puzzleCellEnters.Count, GetForceCount(_puzzleCellEnters), _puzzleDataMatrices[number.x, number.y].isForce);

	    Vector3 pos = _puzzleMatrices[number.x, number.y].transform.position;
	    pos.y -= 6f;
	    multiplier.transform.position = pos;
	    _multipliers.Add(multiplier);
    }
    private void MultiplierRemove()
    {
	    if (_multipliers.Count < 1)
		    return;

	    Managers.Resources.Destroy(_multipliers[_multipliers.Count - 1].gameObject);
	    _multipliers.RemoveAt(_multipliers.Count - 1);
    }
    private void SetForce(Vector2Int num, bool isForce)
    {
	    _puzzleDataMatrices[num.x, num.y].isForce = isForce;
	    GetPuzzle(num).ForceSwitch(isForce);
    }
    public void Change(Vector2Int number, PuzzleData data, bool isEffect)
    {
	    _puzzleDataMatrices[number.x, number.y].puzzleType = data.puzzleType;
	    _puzzleMatrices[number.x, number.y].UISet(_puzzleDataMatrices[number.x, number.y]);
	    
	    if(isEffect)
		    _puzzleMatrices[number.x, number.y].ChangeEffect();
    }
    private void SetBlock(Vector2Int number, bool isBlock)
    {
	    _puzzleBase.Close(_puzzleMatrices[number.x, number.y].transform.position);
	    _puzzleDataMatrices[number.x, number.y].isBlock = isBlock;
	    _puzzleMatrices[number.x, number.y].UISet(_puzzleDataMatrices[number.x, number.y]);
	    _puzzleMatrices[number.x, number.y].ClashEffect();
    }
	private void LofAni()
	{
		_leftLofController.Play(_lofAniName, false);
		_rightLofController.Play(_lofAniName, false);
	}
	public void BoomAni(int index)
	{
		switch (index)
		{
			case 0:
				_backgroundAniController.SetTrigger(_boom1AniName);
				_leftLofController.Play(_lofBoomAniName, false, true);
				_rightLofController.Play(_lofBoomAniName, false, true);
				break;
			case 1:
				_backgroundAniController.SetTrigger(_boom2AniName);
				_leftLofController.Play(_lofBoomAniName, false, true);
				_rightLofController.Play(_lofBoomAniName, false, true);
				break;
			case 2:
				_backgroundAniController.SetTrigger(_boom1AniName);
				_leftLofController.Play(_lofBoomAniName, false, true);
				_rightLofController.Play(_lofBoomAniName, false, true);
				break;
			case 3:
				_backgroundAniController.SetTrigger(_boom2AniName);
				_leftLofController.Play(_lofBoomAniName, false, true);
				_rightLofController.Play(_lofBoomAniName, false, true);
				break;
		}

	}
	public void BigBoomAni()
	{
		_backgroundAniController.SetTrigger(_bigBoomAniName);
		_leftLofController.Play(_lofBoomAniName, false, true);
		_rightLofController.Play(_lofBoomAniName, false, true);
	}

    private void Clear()
    {
	    for (int i = 0; i < _chains.Count; i++)
		    Managers.Resources.Destroy(_chains[i].gameObject);
	    _chains.Clear();
	    
	    for (int i = 0; i < _multipliers.Count; i++)
		    Managers.Resources.Destroy(_multipliers[i].gameObject);
	    _multipliers.Clear();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleMatrices[j, i] != null)
			    {
				    _puzzleMatrices[j, i].Switch(false);
			    }
		    }
	    }
	    
	    _puzzleCellEnters.Clear();
    }

    private void OnEnter(Vector2Int number)
    {
	    if (!Switch || _isAutoPerfect)
		    return;

	    Chain(number);
    }
    private void OnExit(Vector2Int number)
    {
    }
    private void OnPointerUp()
    {
		if (!Switch)
		    return;
			
	    if (_puzzleCellEnters.Count > 1)
	    {
			PuzzleType puzzleType = _puzzleDataMatrices[_puzzleCellEnters[0].x, _puzzleCellEnters[0].y].puzzleType;
			Managers.Observer.Player.SetForm(puzzleType);

		    CheckPerfectPuzzle(PuzzleType.None);

			bool isPerfect = _puzzleCellEnters.Count >= _perfectLinks.Count;

			int itemCount = _puzzleItem.ItemCount(_puzzleCellEnters.ToList());
		    _puzzleItem.PointUp(_puzzleCellEnters.ToList());

			OnPuzzlePointUp?.Invoke((PuzzleData)_puzzleDataMatrices[_puzzleCellEnters[0].x, _puzzleCellEnters[0].y].Clone(), _puzzleCellEnters.Count, GetForceCount(_puzzleCellEnters), itemCount);
		    OnPuzzlePointUp = null;

		    for (int i = 0; i < _puzzleCellEnters.Count; i++)
		    {
			    Change(_puzzleCellEnters[i], GetRandomPuzzle(), true);
			    SetForce(_puzzleCellEnters[i], false);
		    }

			if (isPerfect)
		    {
			    Poolable perfectPool = Managers.Resources.Instantiate<Poolable>(_perfectPrefab, perfectRoot);
			    perfectPool.transform.localPosition = Vector3.zero;
			    perfectPool.transform.localScale = Vector3.one;
			    perfectPool.Play();
			    OnPerfect?.Invoke();
		    }
	    }
		else
		{
			Managers.Observer.Player.CharacterAttackReady.End();
		}

	    _puzzleZoom?.ZoomOut();
	    
	    _chainCombo?.Out();
	    
	    Clear();
    }

    private bool IsSide(Vector2Int num1, Vector2Int num2)
    {
	    int xDistance = num1.x > num2.x ? num1.x - num2.x : num2.x - num1.x;
	    int yDistance = num1.y > num2.y ? num1.y - num2.y : num2.y - num1.y;
	    
	    return xDistance <= 1 && yDistance <= 1;
    }
    private bool IsChain(Vector2Int num)
    {
	    Vector2Int current = _puzzleCellEnters[_puzzleCellEnters.Count - 1];
	    bool isType = !IsLimitedType(num);
	    bool isContain = _puzzleCellEnters.Contains(num);
	    bool isSameType = _puzzleDataMatrices[current.x, current.y].puzzleType == _puzzleDataMatrices[num.x, num.y].puzzleType;
	    return isType && !isContain && isSameType;
    }
    private bool IsLimitedType(Vector2Int num)
    {
	    return _puzzleDataMatrices[num.x, num.y].puzzleType == PuzzleType.None || _puzzleDataMatrices[num.x, num.y].isBlock;
    }
    private bool UnChain(Vector2Int num)
    {
	    if (_puzzleCellEnters.Count <= 1)
	    {
		    return false;
	    }
	    
	    Vector2Int before = _puzzleCellEnters[_puzzleCellEnters.Count - 2];
	    bool result = before == num;

	    if (result)
	    {
		    before = _puzzleCellEnters[_puzzleCellEnters.Count - 1];
		    _puzzleMatrices[before.x, before.y].Switch(false);
		    
		    Managers.Resources.Destroy(_chains[_chains.Count - 1].gameObject);
		    _chains.RemoveAt(_chains.Count - 1);
		    _puzzleCellEnters.RemoveAt(_puzzleCellEnters.Count - 1);
		    
		    OnPuzzleChain?.Invoke(_puzzleDataMatrices[num.x, num.y].puzzleType, _puzzleCellEnters.Count);
		    ComboIn(num);
		    MultiplierRemove();
		    _puzzleZoom?.Zoom(_puzzleInitPositions[num.x, num.y]);

			Managers.Sound.Play("UI/Button/Puzzle");

			PuzzleType puzzleType = _puzzleDataMatrices[num.x, num.y].puzzleType;
			ChainGrade = GameDefine.GetAttackGrade(_puzzleCellEnters.Count);
			if (!Managers.Observer.Player.IsChanging && _isChainGradeChange)
			{
				Managers.Observer.Player.CharacterAttackReady.SetReady(puzzleType, ChainGrade);
			}
			else
				Managers.Observer.Player.CharacterAttackReady.CurrentGrade = ChainGrade;

	    }
	    
	    return result;
    }
    private int GetForceCount(List<Vector2Int> points)
    {
	    int total = 0;

	    for (int i = 0; i < points.Count; i++)
	    {
		    if (_puzzleDataMatrices[points[i].x, points[i].y].isForce)
		    {
			    total++;
		    }
	    }
	    
	    return total;
    }
 
    private PuzzleData GetRandomPuzzle()
    {
	    int r = (int)UnityHelper.Random_H(1, 5);
	    PuzzleType puzzleType = (PuzzleType)r;
	    
	    return new PuzzleData()
	    {
		    puzzleType = puzzleType 
	    };
    }

	private void CheckPerfectPuzzle(PuzzleType selectType)
	{
		_perfectLinks.Clear();
		int maxLength = 0;

		// 퍼즐판이 전부 같은 색인지 체크
		PuzzleType? type = null;
		bool allSame = true;
		for (int x = 0; x < _puzzleMatricesSize; x++)
		{
			for (int y = 0; y < _puzzleMatricesSize; y++)
			{
				var cell = _puzzleDataMatrices[x, y];
				if (cell != null && !cell.isBlock)
				{
					if (type == null)
						type = cell.puzzleType;
					else if (type != cell.puzzleType)
					{
						allSame = false;
						break;
					}
				}
			}

			if (!allSame)
				break;
		}

		if (selectType == PuzzleType.None || type == selectType)
		{
			if (allSame && type != null)
			{
				// 그냥 모든 연결 가능한 칸을 다 넣어주기
				for (int x = 0; x < _puzzleMatricesSize; x++)
				{
					for (int y = 0; y < _puzzleMatricesSize; y++)
					{
						var cell = _puzzleDataMatrices[x, y];
						if (cell != null && cell.puzzleType == type && !cell.isBlock)
						{
							_perfectLinks.Add(new Vector2Int(x, y));
						}
					}
				}
				return;
			}
		}

		// 아니면, 랜덤 DFS 여러 번 돌리기
		int tryCount = 100; // 100회 시도
		for (int x = 0; x < _puzzleMatricesSize; x++)
		{
			for (int y = 0; y < _puzzleMatricesSize; y++)
			{
				var cell = _puzzleDataMatrices[x, y];

				if (selectType == PuzzleType.None ? cell != null && cell.puzzleType != PuzzleType.None && !cell.isBlock : cell != null && cell.puzzleType == selectType && !cell.isBlock)
				{
					for (int t = 0; t < tryCount; t++)
					{
						var path = FindLongRandomPath(new Vector2Int(x, y), cell.puzzleType, 15);
						if (path.Count > maxLength)
						{
							maxLength = path.Count;
							_perfectLinks = path;
						}
					}
				}
			}
		}
	}

	// “랜덤하게 한 번만 이어붙는” DFS
	private List<Vector2Int> FindLongRandomPath(Vector2Int start, PuzzleType type, int maxDepth)
	{
		var path = new List<Vector2Int> { start };
		var visited = new HashSet<Vector2Int> { start };

		Vector2Int[] directions = {
			new(-1, 1), new(0, 1), new(1, 1),
			new(1, 0), new(1, -1), new(0, -1),
			new(-1, -1), new(-1, 0)
		};

		System.Random rng = new System.Random();

		Vector2Int current = start;
		for (int depth = 1; depth < maxDepth; depth++)
		{
			var candidates = new List<Vector2Int>();
			foreach (var dir in directions)
			{
				var next = current + dir;
				if (IsValidPoint(next) &&
					!visited.Contains(next) &&
					_puzzleDataMatrices[next.x, next.y].puzzleType == type &&
					!_puzzleDataMatrices[next.x, next.y].isBlock)
				{
					candidates.Add(next);
				}
			}

			if (candidates.Count == 0)
				break;

			// 랜덤으로 한 방향만 선택
			var nextPick = candidates[rng.Next(candidates.Count)];
			path.Add(nextPick);
			visited.Add(nextPick);
			current = nextPick;
		}

		return path;
	}
	private bool IsValidPoint(Vector2Int point)
	{
		return point.x >= 0 && point.y >= 0 && point.x < _puzzleMatricesSize && point.y < _puzzleMatricesSize;
	}
	
	[Button]
	public void AutoPerfect(PuzzleType selectType)
	{
		if (!Switch || _isAutoPerfect)
		    return;

		Clear();

		_isAutoPerfect = true;

		CheckPerfectPuzzle(selectType);

		if (_autoPerfectCoroutine != null)
		{
			StopCoroutine(_autoPerfectCoroutine);
		}

		_autoPerfectCoroutine = AutoPerfectCoroutine();
		StartCoroutine(_autoPerfectCoroutine);	
	}
	[Button]
	public void StopAutoPerfect()
	{
		if (!_isAutoPerfect)
			return;

		if (_autoPerfectCoroutine != null)
		{
			StopCoroutine(_autoPerfectCoroutine);
		}

		_isAutoPerfect = false;

		if (!Managers.Observer.Player.IsChanging)
			Managers.Observer.Player.CharacterAttackReady.SetReady();

		_puzzleZoom?.ZoomOut();
	    _chainCombo?.Out();
	    Clear();
	}
	private IEnumerator AutoPerfectCoroutine()
	{
		int index = 0;

		while(index < _perfectLinks.Count)
		{
			Vector2Int current = _perfectLinks[index];

			Chain(current);

			yield return new WaitForSeconds(0.1f);

			index++;
		}

		yield return new WaitForSeconds(0.2f);

		OnPointerUp();

		_isAutoPerfect = false;
	}
	public void PerfectTrail()
	{
		CheckPerfectPuzzle(PuzzleType.None);
		List<Transform> pathPoints = new List<Transform>();
		for(int i = 0; i < _perfectLinks.Count; i++)
		{
			pathPoints.Add(_puzzleMatrices[_perfectLinks[i].x, _perfectLinks[i].y].transform);
		}

		if(_pathTrailFollower == null)
		{
			_pathTrailFollower = Managers.Resources.Instantiate<PathTrailFollower>(_perfectTrailFollower, perfectRoot);
			_pathTrailFollower.OnEnd -= PerfectTrail;
			_pathTrailFollower.OnEnd += PerfectTrail;
		}

		_pathTrailFollower.transform.position = pathPoints[0].position;
		_pathTrailFollower.StartPath(pathPoints.ToArray(), perfectRoot);
	}
	public void PerfectTrailEnd()
	{
		if(_pathTrailFollower != null)
		{
			Managers.Resources.Destroy(_pathTrailFollower.gameObject);
			_pathTrailFollower.OnEnd = null;
		}
	}
    // Change Puzzle Skill
    [Button]
    public void ChangeCrossPuzzle()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    PuzzleData resultPuzzleData = GetRandomPuzzle();
	    
	    int[] deltaX = { 0, 0, 1, -1, 0 };
	    int[] deltaY = { 1, -1, 0, 0, 0 };
	    
	    int rx = (int)UnityHelper.Random_H(0, _puzzleMatricesSize);
	    int ry = (int)UnityHelper.Random_H(0, _puzzleMatricesSize);
	    
	    for (int j = rx; j < _puzzleMatricesSize + rx; j++)
	    {
		    if (points.Count == deltaX.Length)
			    break;
		    
		    for (int i = ry; i < _puzzleMatricesSize + ry; i++)
		    {
			    if (points.Count == deltaX.Length)
				    break;
			    
			    int x = j % _puzzleMatricesSize;
			    int y = i % _puzzleMatricesSize;

			    if (_puzzleDataMatrices[x, y].puzzleType == resultPuzzleData.puzzleType)
				    continue;

			    points.Clear();
			    for (int k = 0; k < deltaX.Length; k++)
			    {
				    int dx = x + deltaX[k];
				    int dy = y + deltaY[k];

				    if (dx >= _puzzleMatricesSize || dx < 0 || dy >= _puzzleMatricesSize || dy < 0)
					    break;
				    
				    if (_puzzleDataMatrices[dx, dy].isBlock || _puzzleDataMatrices[dx, dy].puzzleType == PuzzleType.None)
					    break;
				    
				    points.Add(new Vector2Int(dx, dy));
			    }
		    }
	    }

	    for (int i = 0; i < points.Count; i++)
	    {
		    Change(points[i], resultPuzzleData, true);
	    }
    }
    [Button]
    public void ChangeRowLinePuzzle()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    PuzzleData resultPuzzleData = GetRandomPuzzle();
	    
	    int ry = (int)UnityHelper.Random_H(0, _puzzleMatricesSize);
	    
	    for (int x = 0; x < _puzzleMatricesSize; x++)
	    {
		    if (_puzzleDataMatrices[x, ry].isBlock || _puzzleDataMatrices[x, ry].puzzleType == PuzzleType.None)
			    continue;
			    
		    points.Add(new Vector2Int(x, ry));
	    }
	    
	    for (int i = 0; i < points.Count; i++)
	    {
		    Change(points[i], resultPuzzleData, true);
	    }
    }
    [Button]
    public void ChangeColumnLinePuzzle()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    PuzzleData resultPuzzleData = GetRandomPuzzle();
	    
	    int rx = (int)UnityHelper.Random_H(0, _puzzleMatricesSize);
	    
	    for (int y = 0; y < _puzzleMatricesSize; y++)
	    {
		    if (_puzzleDataMatrices[rx, y].isBlock || _puzzleDataMatrices[rx, y].puzzleType == PuzzleType.None)
			    continue;
			    
		    points.Add(new Vector2Int(rx, y));
	    }
	    
	    for (int i = 0; i < points.Count; i++)
	    {
		    Change(points[i], resultPuzzleData, true);
	    }
    }

    // PuzzleItemSpawn
    [Button]
    public void RandomLightningSpawn()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomLightningSpawn(points);
    }
    [Button]
    public void RandomHellFire()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomHellFire(points);
    }
    [Button]
    public void RandomGas()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomGas(points);
    }
    [Button]
    public void RandomSlash()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomSlash(points);
    }
    [Button]
    public void RandomIceThorn()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomIceThorn(points);
    }
    [Button]
	public void RandomBloodBlade()
	{
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomBloodBlade(points);
	}
	[Button]
	public void RandomMissile()
	{
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomMissile(points);
	}
	[Button]
	public void RandomPrism()
	{
		List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomPrism(points);
	}
	[Button]
	public void RandomWave()
	{
		List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomWave(points);
	}
	[Button]
	public void RandomWind()
	{
		List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomWind(points);
	}
	[Button]
	public void RandomShootingStar()
	{
		List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomShootingStar(points);
	}
	[Button]
    public void RandomHpRecovery()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomHpRecovery(points);
    }
    [Button]
    public void RandomAtk()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomAtk(points);
    }
    [Button]
    public void RandomShield()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomShield(points);
    }
    [Button]
    public void RandomForce()
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None || _puzzleDataMatrices[j, i].isForce)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    int rIndex = (int)UnityHelper.Random_H(0, points.Count);
	    Vector2Int point = points[rIndex];

	    SetForce(point, true);
    }
    [Button]
    public void RandomItem(PuzzleItem puzzleItem)
    {
	    List<Vector2Int> points = new List<Vector2Int>();
	    
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock || _puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    points.Add(_puzzleDataMatrices[j, i].number);
		    }
	    }
	    
	    _puzzleItem.RandomItem(points, puzzleItem);
    }
    
    [Button]
    public void AllChange(PuzzleType puzzleType)
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].puzzleType == PuzzleType.None)
			    {
				    continue;
			    }
			    
			    Change(new Vector2Int(j, i), new PuzzleData() { puzzleType = puzzleType }, true);
		    }
	    }
    }
    
    [Button]
    public void PuzzleCrash(int count)
    {
	    for (int i = 0; i < count; i++)
	    {
		    int whileCount = 0;

		    while (whileCount < 25)
		    {
			    int rx = (int)UnityHelper.Random_H(0, _puzzleMatricesSize);
			    int ry = (int)UnityHelper.Random_H(0, _puzzleMatricesSize);

			    if (_puzzleDataMatrices[rx, ry].puzzleType != PuzzleType.None &&
			        !_puzzleDataMatrices[rx, ry].isBlock)
			    {
				    SetBlock(new Vector2Int(rx, ry), true);
				
				    break;
			    }

			    whileCount++;
		    }
	    }
    }
    
    [Button]
    public void PuzzleCrashReset()
    {
	    for (int j = 0; j < _puzzleMatricesSize; j++)
	    {
		    for (int i = 0; i < _puzzleMatricesSize; i++)
		    {
			    if (_puzzleDataMatrices[j, i].isBlock)
				    SetBlock(new Vector2Int(j, i), false);
		    }
	    }
    }


}