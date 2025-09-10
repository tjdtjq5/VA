using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleItem : UIFrame
{
    protected override void Initialize()
    {
        base.Initialize();

        _animator = GetComponent<Animator>();
        _aniController = _animator.Initialize();

        _aniController.SetEndFunc(_close, (clipName) => Clear());
    }
    
    [SerializeField] Skill skill;

    private Animator _animator;
    private AniController _aniController;

    private readonly string _open = "Open";
    private readonly string _close = "Close";
    private int _openHash = Animator.StringToHash("Open");
    private int _closeHash = Animator.StringToHash("Close");

    public Skill Skill => skill;

    public void Open()
    {
        _aniController.SetTrigger(_openHash);
    }

    public void Use()
    {
      	Managers.Observer.Player.CharacterSkill.PushSkill((Skill)skill.Clone());
        _aniController.SetTrigger(_closeHash);
    }

    public void Clear()
    {
	      Managers.Resources.Destroy(this.gameObject);
    }
}