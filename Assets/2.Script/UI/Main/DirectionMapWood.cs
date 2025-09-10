using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMapWood : UIFrame
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Animator _buttonAnimator;

    private AniController _aniController;
    private AniController _buttonAniController;

    private readonly string _directionMapPath = "InGame/DirectionMap";
    private readonly string _openTrigger = "Open";
    private readonly string _closeTrigger = "Close";

    private PuzzleBattleStateMachine _puzzleBattleStateMachine;
    private DungeonTree _dungeonTree;
    private DungeonNode _currentNode;
    private readonly float _positionX = 3f;
    private readonly float _positionY = -5.06f;
    private bool _isOpen = false;


    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.Main_Button).AddClickEvent((ped) => OnClick());

        _aniController = _animator.Initialize();
        _aniController.SetEndFunc(_closeTrigger, (clipName) => CloseEnd());

        _buttonAniController = _buttonAnimator.Initialize();

        base.Initialize();
    }

    public void Initialize(PuzzleBattleStateMachine puzzleBattleStateMachine)
    {
        this._puzzleBattleStateMachine = puzzleBattleStateMachine;
        this._dungeonTree = puzzleBattleStateMachine.DungeonTree;
        
        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Open(DungeonNode currentNode)
    {
        _isOpen = true;
        
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }

        _aniController.SetTrigger(_openTrigger);
        _buttonAniController.SetTrigger(_openTrigger);

        _currentNode = currentNode;

        this.transform.position = new Vector3(_puzzleBattleStateMachine.StageDistance + _positionX, _positionY, 0);

      for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(true);
    }

    public void Close()
    {
        if(!_isOpen)
            return;

        _isOpen = false;

        _aniController.SetTrigger(_closeTrigger);
        _buttonAniController.SetTrigger(_closeTrigger);
    }

    private void CloseEnd()
    {
        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if(!_isOpen)
            return;
    }
    
	public enum UIButtonE
    {
		Main_Button,
    }
}