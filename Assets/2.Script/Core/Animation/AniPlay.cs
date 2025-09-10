using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniPlay : UIFrame
{
    public Action OnPlayEnded;
    public AniController AniController { get; private set; }

    [SerializeField] private Animator _animator;
    private readonly string _playAniName = "play";
    private readonly int _nullAniHash = Animator.StringToHash("null");
    private readonly int _playAniHash = Animator.StringToHash("play");

    private void Start()
    {
        Initialize();
    }
    protected override void Initialize()
    {
        base.Initialize();
        
        AniController = _animator.Initialize();
        AniController.SetEndFunc(_playAniName, PlayAniEnded);
    }

    public void Play()
    {
        AniController.SetTrigger(_playAniHash);
    }
    public void Null()
    {
        AniController.SetTrigger(_nullAniHash);
    }

    public float GetPlayAniLength() => AniController.GetClipLength(_playAniName);

    void PlayAniEnded(string aniName)
    {
        OnPlayEnded?.Invoke();
    }
}
