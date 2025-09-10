using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIForkRoadWood : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    public void Initialize(PuzzleBattleStateMachine puzzleBattleStateMachine)
    {
        this._puzzleBattleStateMachine = puzzleBattleStateMachine;
        
        _animator = this.GetComponent<Animator>();
        _aniController = _animator.Initialize();

        for (int i = 0; i < this.transform.childCount; i++)
            this.transform.GetChild(i).gameObject.SetActive(false);
    }

    public Action<UIForkRoadWood> OnOpen;
    public Action<UIForkRoadWood> OnClick;

    public List<ForkRoadWoodButton> GetButtons => _buttons;

    [SerializeField] private Transform contents;
    [SerializeField] private Sprite _wood_1;
    [SerializeField] private Sprite _wood_2;
    
    private Animator _animator;
    private AniController _aniController;
    private PuzzleBattleStateMachine _puzzleBattleStateMachine;
    private List<ForkRoadWoodButton> _buttons = new();
    private bool _isOpen = false;

    private readonly string _openTrigger = "Open";
    private readonly string _closeTrigger = "Close";
    private readonly string _buttonPrefabPath = "Prefab/UI/Button/InGame/ForkRoadWoodButton";
    private readonly float _positionX = 0f;
    private readonly float _positionY = -5.4f;
    private readonly Vector3 _leftPosition = new Vector3(-156f, 161f, 0);
    private readonly Vector3 _middlePosition = new Vector3(16f, 161f, 0);
    private readonly Vector3 _rightPosition = new Vector3(217f, 161f, 0);

    public void Open(int stage, int index)
    {
        _isOpen = true;
        
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }

        this.transform.position = new Vector3(_puzzleBattleStateMachine.StageDistance + _positionX, _positionY, 0);
        
        List<DungeonNode> nexts = _puzzleBattleStateMachine.NextNodes;

        if (nexts.Count == 1)
        {
            this.GetImage(UIImageE.Main_WoodLine).sprite = _wood_1;
            this.GetImage(UIImageE.Main_WoodLine).SetNativeSize();
        }
        else
        {
            this.GetImage(UIImageE.Main_WoodLine).sprite = _wood_2;
            this.GetImage(UIImageE.Main_WoodLine).SetNativeSize();
        }
	    
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < nexts.Count; i++)
        {
            DungeonNode next = nexts[i];
            
            ForkRoadWoodButton woodButton = null;
            if (i < _buttons.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                woodButton = _buttons[i];
            }
            else
            {
                woodButton = Managers.Resources.Instantiate<ForkRoadWoodButton>(_buttonPrefabPath, contents);
                _buttons.Add(woodButton);
            }
		    
            Direction direction = nexts.Count > 1 ? (i == 0 ? Direction.Left : Direction.Right) : Direction.Middle;
		    
            woodButton.RemoveEvent(UIEvent.Trigger);
            woodButton.AddClickAniEvent((ped)=> Click(next));
            woodButton.UISet(next.DungeonRoom.Icon, direction);
        }

        _aniController.SetTrigger(_openTrigger);
        
        OnOpen?.Invoke(this);
    }
    
    private void Click(DungeonNode dungeonNode)
    {
        if (!_isOpen)
            return;

        _isOpen = false;
        
        _puzzleBattleStateMachine.StageNext(dungeonNode.Stage, dungeonNode.Index);

        _aniController.SetTrigger(_closeTrigger);
        
        OnClick?.Invoke(this);
    }

    public void Close()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
	public enum UIImageE
    {
		Main_Shadow,
		Main_WoodLine,
    }
}