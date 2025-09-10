using System;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.CSharp;
using Sirenix.OdinInspector;
using UnityEngine;

public class InGameManager : SceneBase
{
    public int Chapter { get; private set; }
    public DungeonNode CurrentNode { get; private set; }
    public DungeonTree DungeonTree => _dungeonTree;
    public Player Player => _player;
    public UIPuzzle Puzzle => _puzzle;
    public UIBattle UIBattle => _uiBattle;

    [SerializeField] DungeonTree _dungeonTree;
    [SerializeField] private PuzzleBattleStateMachine _puzzleBattleStateMachine;
    [SerializeField] private bool _isBoos;

    private Sequencer _sequencer;
    private UIBattle _uiBattle;
    private UIGoodsController _uiGoodsController;
    private CameraController _cameraController;
    private UIPuzzle _puzzle;
    private UIPuzzleBase _puzzleBase;
    private Player _player;
    private readonly string _playerPrefabPath = "Prefab/Character/Player";
    private readonly string _chapterDungeonCode = "InGame_Chapter{0}";
    private readonly string _briefMapPrefabPath = "InGame/BriefMap";

    [Button]
    public void TimeScale(float value)
    {
        Managers.Time.TimeScale(value);
    }

    protected override void Initialize()
    {
        base.Initialize();

        Managers.Sound.Play("BGM/InGameBGM", Sound.Bgm);

        SceneType = SceneType.InGame;
        Chapter = 1;

        _sequencer = GetComponent<Sequencer>();

        _puzzleBattleStateMachine.Initialize(this, _dungeonTree);

        _puzzleBattleStateMachine.OnEnemyDead += OnEnemyDead;
        Managers.Observer.PuzzleBattleStateMachine = _puzzleBattleStateMachine;
        
        // Player
        _player = PlayerSpawn();
    
        // Goods
        _uiGoodsController = FindObjectOfType<UIGoodsController>();
        _uiGoodsController.UISet("Gesso", 0);
        Managers.Observer.UIGoodsController = _uiGoodsController;
        Managers.Observer.OnGessoChanged += (BBNumber value) => _uiGoodsController.UISet("Gesso", value);
        
        // Battle
        _uiBattle = FindObjectOfType<UIBattle>();

        // Puzzle
        _puzzle = FindObjectOfType<UIPuzzle>();
        Managers.Observer.UIPuzzle = _puzzle;
        _puzzle.OnPerfect += _player.Perfect;
        _puzzle.Close();
        
        // Player
        Managers.Observer.Player = _player;
        _player.Initialize(_dungeonTree);
        _player.OnDeadEnd += (Character character) => _puzzleBattleStateMachine.Failed();
        _player.CharacterSkill.OnSkillPlay += _uiBattle.SkillPlay;
        
        // PuzzleBase
        _puzzleBase = FindObjectOfType<UIPuzzleBase>();
        _puzzleBase.Open();
        
        _cameraController = FindObjectOfType<CameraController>();
        _cameraController.Initialize();
        Managers.Observer.CameraController = _cameraController;
        Managers.Observer.Camera = Camera.main;
        
        Map map = FindObjectOfType<Map>();
        map.Initialize(_player.transform);
        
        _sequencer.OnSequencerEnd = SequencerEnd;
        _sequencer.Excute();
        
        if (!PlayerPrefsHelper.GetBool_H(PlayerPrefsKey.tutorial_puzzle))
        {
            _puzzle.OnOpen += PuzzleTutorialReady;
            _puzzle.OnAniOpen += PuzzleTutorialStart;
            _puzzle.OnPuzzlePointUp += PuzzleTutorialEnd;
        }
        
        if (!PlayerPrefsHelper.GetBool_H(PlayerPrefsKey.tutorial_week))
        {
            _puzzle.OnAniOpen += WeekTutorial;
        }
        
        if (!PlayerPrefsHelper.GetBool_H(PlayerPrefsKey.tutorial_stun))
        {
            _player.OnStunSuccess += StunTutorial;
        }
    }

    public void BriefMapOpen(int stage, int index)
    {
        BriefMap briefMap = Managers.UI.ShopPopupUI<BriefMap>(_briefMapPrefabPath, CanvasOrderType.Bottom);

        if (!briefMap.IsInit)
        {
            briefMap.Initialize(_puzzleBattleStateMachine);

            // Tutorial
            if (!PlayerPrefsHelper.GetBool_H(PlayerPrefsKey.tutorial_forkroad))
            {
                briefMap.OnOpen += ForkRoadTutorialStart;
                briefMap.OnClick += ForkRoadTutorialEnd;
            }
        }

        briefMap.Open(stage, index);
    }

    public override void Clear()
    {
        _dungeonTree = null;
    }
    public Player PlayerSpawn() => Managers.Resources.Instantiate<Player>(_playerPrefabPath);
    
    void SequencerEnd()
    {
        DungeonStart();
    }
    public void DungeonStart()
    {
        _puzzleBattleStateMachine.StageStart();
    }
    void OnEnemyDead(Character character)
    {
        int value = 100;
        int count = 3;

        switch (character.enemyType)
        {
            case EnemyType.Normal:
                count = 3;
                break;
            case EnemyType.Elite:
                count = 7;
                break;
            case EnemyType.Boss:
                count = 10;
                break;
            default:
                count = 3;
                break;
        }
        
        Managers.Observer._gesso += value;
        
        _uiGoodsController.PlusSet("Gesso", Managers.Observer.Gesso, character.BodyBoneTr.position, count);
    }
    
    [Button]
    public void DebufPuzzleBattleState()
    {
        UnityHelper.Log_H($"{_puzzleBattleStateMachine.currentState.State.GetType().Name}");
    }

    #region Tutorial

    private TutorialHand _forkRoadTutorialHand;
    private TutorialHand _puzzleTutorialHand;
    
    private readonly Vector2Int[] _puzzleTutorialNum = new [] { new Vector2Int(1, 1), new Vector2Int(2, 2), new Vector2Int(3, 3), new Vector2Int(4, 4), new Vector2Int(5, 5) };
    private int _puzzleTutorialNumIndex = 0;
    private Tween<Vector3> _puzzleTutorialTween;

    private void ForkRoadTutorialStart(BriefMap briefMap)
    {
        string tutorialPrefabPath = "Prefab/UI/Etc/Tutorial/ForkRoadHand";
        _forkRoadTutorialHand = Managers.Resources.Instantiate<TutorialHand>(tutorialPrefabPath);
        _forkRoadTutorialHand.Click();

        _forkRoadTutorialHand.transform.position = briefMap.GetButtons[0].transform.position;
    }
    private void ForkRoadTutorialEnd(BriefMap briefMap)
    {
        PlayerPrefsHelper.Set_H(PlayerPrefsKey.tutorial_forkroad, true);
        briefMap.OnOpen -= ForkRoadTutorialStart;
        briefMap.OnClick -= ForkRoadTutorialEnd;

        if (_forkRoadTutorialHand)
            Managers.Resources.Destroy(_forkRoadTutorialHand.gameObject);
    }

    private void PuzzleTutorialReady(UIPuzzle puzzle)
    {
        for (int i = 0; i < _puzzleTutorialNum.Length; i++)
            puzzle.Change(_puzzleTutorialNum[i], new PuzzleData() { puzzleType = PuzzleType.Red }, false);

        _puzzleBattleStateMachine.Enemies[0].WeekBar.ChangeWeek(0, PuzzleType.Red);
    }
    private void PuzzleTutorialStart(UIPuzzle puzzle)
    {
        string tutorialPrefabPath = "Prefab/UI/Etc/Tutorial/PuzzleHand";
        _puzzleTutorialHand = Managers.Resources.Instantiate<TutorialHand>(tutorialPrefabPath);
        
        _puzzleTutorialNumIndex = 0;

        Vector3 firstPos = puzzle.GetPuzzlePosition(_puzzleTutorialNum[_puzzleTutorialNumIndex]);
        _puzzleTutorialHand.transform.position = firstPos;
        _puzzleTutorialHand.PointDown();
        
        _puzzleTutorialTween = Managers.Tween.TweenPosition(_puzzleTutorialHand.transform, firstPos, firstPos, 1f)
            .SetOnComplete(PuzzleTutorialTween);
    }

    private void PuzzleTutorialTween()
    {
        _puzzleTutorialNumIndex++;

        if (_puzzleTutorialNumIndex >= _puzzleTutorialNum.Length)
        {
            _puzzleTutorialHand.gameObject.SetActive(false);
            _puzzleTutorialNumIndex = 0;
            _puzzleTutorialHand.transform.position = _puzzle.GetPuzzlePosition(_puzzleTutorialNum[_puzzleTutorialNumIndex]);
            _puzzleTutorialHand.gameObject.SetActive(true);
        }
        
        Vector3 current = _puzzleTutorialHand.transform.position;
        Vector3 next = _puzzle.GetPuzzlePosition(_puzzleTutorialNum[_puzzleTutorialNumIndex]);
        
        _puzzleTutorialTween = Managers.Tween.TweenPosition(_puzzleTutorialHand.transform, current, next, 0.3f)
            .SetOnComplete(PuzzleTutorialTween);
    }
    private void PuzzleTutorialEnd(PuzzleData data, int combo, int forceCount, int itemCount)
    {
        PlayerPrefsHelper.Set_H(PlayerPrefsKey.tutorial_puzzle, true);
        
        if (_puzzleTutorialHand)
            Managers.Resources.Destroy(_puzzleTutorialHand.gameObject);

        _puzzleTutorialTween?.FullKill();

        _puzzle.OnOpen -= PuzzleTutorialReady;
        _puzzle.OnAniOpen -= PuzzleTutorialStart;
        _puzzle.OnPuzzlePointUp -= PuzzleTutorialEnd;
    }
    
    private void WeekTutorial(UIPuzzle puzzle)
    {
        if (_puzzleBattleStateMachine.Enemies.Count <= 0 || _puzzleBattleStateMachine.Enemies[0].WeekCount <= 0)
            return;
        
        PlayerPrefsHelper.Set_H(PlayerPrefsKey.tutorial_week, true);
        _puzzle.OnAniOpen -= WeekTutorial;
        
        Managers.Time.TimeScale(0);
        
        string path = "InGame/UIShowTutorial";
        UIShowTutorial tutorialPopup = Managers.UI.ShopPopupUI<UIShowTutorial>(path, CanvasOrderType.Middle);
        tutorialPopup.UISet("WeekTutorial");
        tutorialPopup.OnClose = ()=> Managers.Time.TimeScale(1);
    }
    private void StunTutorial()
    {
        PlayerPrefsHelper.Set_H(PlayerPrefsKey.tutorial_stun, true);
        Managers.Observer.Player.OnStunSuccess -= StunTutorial;
        
        Managers.Time.TimeScale(0);
        
        string path = "InGame/UIShowTutorial";
        UIShowTutorial tutorialPopup = Managers.UI.ShopPopupUI<UIShowTutorial>(path, CanvasOrderType.Middle);
        tutorialPopup.UISet("StunTutorial");
        tutorialPopup.OnClose = ()=> Managers.Time.TimeScale(1);
    }

    #endregion

    [Button]
    public void Save()
    {
        UnityHelper.Log_H("Save");

        // Stat
        List<Stat> stats = Managers.Observer.Player.Stats.Gets;
        List<StatSaveData> statSaveDatas = new();
        for (int i = 0; i < stats.Count; i++)
        {
            StatSaveData saveData = new StatSaveData(stats[i]);
            statSaveDatas.Add(saveData);
        }

        // Skill
        List<Skill> skills = Managers.Observer.Player.CharacterSkill.Skills;
        List<SkillSaveData> skillSaveDatas = new();
        for (int i = 0; i < skills.Count; i++)
        {
            SkillSaveData saveData = new SkillSaveData(skills[i]);
            skillSaveDatas.Add(saveData);
        }
        
        // Buff
        List<Buff> buffs = Managers.Observer.Player.CharacterBuff.Buffs;
        List<BuffSaveData> buffSaveDatas = new();
        for (int i = 0; i < buffs.Count; i++)
        {
            BuffSaveData saveData = new BuffSaveData(buffs[i]);
            buffSaveDatas.Add(saveData);
        }
        
        // Position
        Vector3 playerPos = Managers.Observer.Player.transform.position;
        
        // SaveData
        InGameManagerSaveData gameSaveData = new InGameManagerSaveData();
        gameSaveData.Stats = statSaveDatas;
        gameSaveData.Skills = skillSaveDatas;
        gameSaveData.Buffs = buffSaveDatas;
        gameSaveData.PlayerPosX = playerPos.x;
        
        UnityHelper.Log_H(CSharpHelper.SerializeObject(gameSaveData));
        
        PlayerPrefsHelper.Set_H(PlayerPrefsKey.ingame_manager_data, CSharpHelper.SerializeObject(gameSaveData));
    }

    [Button]
    public void Load()
    {
        UnityHelper.Log_H("Load");

        if (!PlayerPrefsHelper.HasKey_H(PlayerPrefsKey.ingame_manager_data))
        {
            UnityHelper.Error_H($"Null PlayerPrefsKey HasKey_H ingame_manager_data");
            return;
        }

        string saveDataStr = PlayerPrefsHelper.GetString_H(PlayerPrefsKey.ingame_manager_data);
        InGameManagerSaveData saveData = CSharpHelper.DeserializeObject<InGameManagerSaveData>(saveDataStr);
        
        // Stat 
        List<StatSaveData> Stats = saveData.Stats;
        for (int i = 0; i < Stats.Count; i++)
        {
            Managers.Observer.Player.Stats.GetStat(Stats[i].CodeName).Load(Stats[i]);
        }
        
        // Skill
        List<SkillSaveData> Skills = saveData.Skills;
        Managers.Observer.Player.CharacterSkill.SkillAllRemove();
        for (int i = 0; i < Skills.Count; i++)
        {
            Skill skill = Managers.SO.GetSkill(Skills[i].CodeName);
            Managers.Observer.Player.CharacterSkill.PushSkill(skill);
        }
        
        // Buff
        List<BuffSaveData> Buffs = saveData.Buffs;
        Managers.Observer.Player.CharacterBuff.Buffs.Clear();
        for (int i = 0; i < Buffs.Count; i++)
        {
            Buff buff = Managers.SO.GetBuff(Buffs[i].CodeName);
            Managers.Observer.Player.CharacterBuff.PushBuff(Managers.Observer.Player, buff);
        }
        
        // PlayerPos
        float PlayerPosX = saveData.PlayerPosX;
        Vector3 playerPos = Managers.Observer.Player.transform.position;
        playerPos.x = PlayerPosX;
        Managers.Observer.Player.transform.position = playerPos;
        _cameraController.SetPosition(playerPos.x);
    }
}

[System.Serializable]
public class InGameManagerSaveData
{
    public List<StatSaveData> Stats = new();
    public List<SkillSaveData> Skills = new();
    public List<BuffSaveData> Buffs = new();
    public float PlayerPosX;
}