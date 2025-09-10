using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BriefMap : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
        Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.Main_ViewDetailButton).AddClickEvent((ped) => OnClickDirectionMap());

        base.Initialize();

        AniController.SetEndFunc(_openMapStr, (aniClip) => ViewDirectionMap());
    }

    public Action<BriefMap> OnOpen;
    public Action<BriefMap> OnClick;

    public List<BriefMapIconButton> GetButtons => _buttons;
    public bool IsInit => _isInit;

    [SerializeField] Transform _iconParent;
    [SerializeField] Sprite _ground_1;
    [SerializeField] Sprite _ground_2;
    [SerializeField] DirectionMap _directionMap;
    [SerializeField] GameObject _directionObj;

    private PuzzleBattleStateMachine _puzzleBattleStateMachine;
    private bool _isOpen = false;
    private List<BriefMapIconButton> _buttons = new();
    private bool _isInit = false;
    private bool _isViewDirectionMap = false;

    private readonly string _buttonPrefabPath = "Prefab/UI/Button/InGame/BriefMapIconButton";
    private readonly Vector3 _leftPosition = new Vector3(-159f, 20f, 0);
    private readonly Vector3 _middlePosition = new Vector3(0f, 34.25f, 0);
    private readonly Vector3 _rightPosition = new Vector3(163f, 23f, 0);
    private readonly string _closeMapStr = "CloseMap";
    private readonly string _openMapStr = "OpenMap";
    int openMapHash = UnityEngine.Animator.StringToHash("OpenMap");
    int closeMapHash = UnityEngine.Animator.StringToHash("CloseMap");

    public void Initialize(PuzzleBattleStateMachine puzzleBattleStateMachine)
    {
        this._puzzleBattleStateMachine = puzzleBattleStateMachine;
        _isInit = true;
    }

    public void Open(int stage, int index)
    {
        _isOpen = true;
        _isViewDirectionMap = false;

        List<DungeonNode> nexts = _puzzleBattleStateMachine.NextNodes;

        this.GetImage(UIImageE.Main_Map_Mask_Ground).sprite = (nexts.Count == 1) ? _ground_1 : _ground_2;
	    
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < nexts.Count; i++)
        {
            DungeonNode next = nexts[i];
            
            BriefMapIconButton woodButton = null;
            if (i < _buttons.Count)
            {
                _buttons[i].gameObject.SetActive(true);
                woodButton = _buttons[i];
            }
            else
            {
                woodButton = Managers.Resources.Instantiate<BriefMapIconButton>(_buttonPrefabPath, _iconParent);
                _buttons.Add(woodButton);
            }
		    
            Direction direction = nexts.Count > 1 ? (i == 0 ? Direction.Left : Direction.Right) : Direction.Middle;

            switch (direction)
            {
                case Direction.Left:
                    woodButton.transform.localPosition = _leftPosition;
                    break;
                case Direction.Middle:
                    woodButton.transform.localPosition = _middlePosition;
                    break;
                case Direction.Right:
                    woodButton.transform.localPosition = _rightPosition;
                    break;
            }
		    
            woodButton.RemoveEvent(UIEvent.Trigger);
            woodButton.AddClickAniEvent((ped)=> Click(next));
            woodButton.UISet(next.DungeonRoom.Icon);
        }

        OnOpen?.Invoke(this);
    }

    private void Click(DungeonNode dungeonNode)
    {
        if (!_isOpen)
            return;

        _isOpen = false;
        
        _puzzleBattleStateMachine.StageNext(dungeonNode.Stage, dungeonNode.Index);

        OnClick?.Invoke(this);

        ClosePopupUIPlayAni();
    }
    
    private void OnClickDirectionMap()
    {
        _isViewDirectionMap = !_isViewDirectionMap;

        AniController.SetTrigger(_isViewDirectionMap ? openMapHash : closeMapHash);

        if(!_isViewDirectionMap)
            CloseDirectionMap();
    }
    private void ViewDirectionMap()
    {
        _directionObj.SetActive(true);
        _directionMap.UISet(_puzzleBattleStateMachine.DungeonTree, _puzzleBattleStateMachine.CurrentNode);
    }

    private void CloseDirectionMap()
    {
        _directionObj.SetActive(false);
    }

	public enum UIImageE
    {
		Main_BlackBG,
		Main_Map_Mask,
		Main_Map_Mask_Ground,
		Main_Map_Player,
		Main_Map_Under,
		Main_Map_Upper,
    }
    public enum UIButtonE
    {
        Main_ViewDetailButton,  
    }
}