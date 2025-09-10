using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using System;
using Sirenix.OdinInspector;

public class UIPlayer : UIFrame
{
    public Action<PuzzleType> OnChangeForm;

    public CharacterTeam Team => CharacterTeam.Player;
    public bool IsChanging => _isChanging;
    public PuzzleType CurrentForm => _current;

    [SerializeField] private GameObject _basic;
    [SerializeField] private GameObject _greatsword;
    [SerializeField] private GameObject _dagger;
    [SerializeField] private GameObject _bow;
    [SerializeField] private GameObject _staff;

    private PuzzleType _current;
    private bool _isChanging = false;
    private SkeletonGraphic _characterAnimation;
    private List<SpineAniController> _characterSpineAniControllers = new List<SpineAniController>();

    private SkeletonGraphic _basicSkeletonAnimation;
    private SkeletonGraphic _greatswordSkeletonAnimation;
    private SkeletonGraphic _daggerSkeletonAnimation;
    private SkeletonGraphic _bowSkeletonAnimation;
    private SkeletonGraphic _staffSkeletonAnimation;

    private SpineAniController _basicSpineAniController;
    private SpineAniController _greatswordSpineAniController;
    private SpineAniController _daggerSpineAniController;
    private SpineAniController _bowSpineAniController;
    private SpineAniController _staffSpineAniController;

    private readonly Dictionary<PuzzleType, string> _changeAniNames = new Dictionary<PuzzleType, string>()
    {
        { PuzzleType.Red, "Tr_G" },
        { PuzzleType.Blue, "Tr_D" },
        { PuzzleType.Green, "Tr_H" },
        { PuzzleType.Yellow, "Tr_M" }
    };
    public readonly List<string> _voiceSoundNames = new List<string>()
    {
        "Voice/PlayerVoice_1",
        "Voice/PlayerVoice_2",
        "Voice/PlayerVoice_3",
        "Voice/PlayerVoice_4"
    };

    private readonly List<string> _idleAniNames = new List<string>()
        { "Idle_1", "Idle_2", "Idle_3", "Idle_4" };

    private readonly string IdleAnimationName = "Idle";
    private readonly string _changeReturnAniName = "Tr";

    protected override void Initialize()
    {
        _basicSkeletonAnimation =  _basic.GetComponent<SkeletonGraphic>();
        _greatswordSkeletonAnimation =  _greatsword.GetComponent<SkeletonGraphic>();
        _daggerSkeletonAnimation =  _dagger.GetComponent<SkeletonGraphic>();
        _bowSkeletonAnimation =  _bow.GetComponent<SkeletonGraphic>();
        _staffSkeletonAnimation =  _staff.GetComponent<SkeletonGraphic>();

        _basicSpineAniController = _basic.GetOrAddComponent<SpineAniController>();
        _greatswordSpineAniController = _greatsword.GetOrAddComponent<SpineAniController>();
        _daggerSpineAniController = _dagger.GetOrAddComponent<SpineAniController>();
        _bowSpineAniController = _bow.GetOrAddComponent<SpineAniController>();
        _staffSpineAniController = _staff.GetOrAddComponent<SpineAniController>();

        _basicSpineAniController.Clear();
        _greatswordSpineAniController.Clear();
        _daggerSpineAniController.Clear();
        _bowSpineAniController.Clear();
        _staffSpineAniController.Clear();

        _characterSpineAniControllers.Clear();
        _characterSpineAniControllers.Add(_basicSpineAniController);
        _characterSpineAniControllers.Add(_greatswordSpineAniController);
        _characterSpineAniControllers.Add(_daggerSpineAniController);
        _characterSpineAniControllers.Add(_bowSpineAniController);
        _characterSpineAniControllers.Add(_staffSpineAniController);

        _basicSpineAniController.Initialize(_basicSkeletonAnimation);
        _greatswordSpineAniController.Initialize(_greatswordSkeletonAnimation);
        _daggerSpineAniController.Initialize(_daggerSkeletonAnimation);
        _bowSpineAniController.Initialize(_bowSkeletonAnimation);
        _staffSpineAniController.Initialize(_staffSkeletonAnimation);

        SetForm(PuzzleType.None);

        foreach (var item in _changeAniNames)
        {
            _basicSpineAniController.SetEndFunc(item.Value, OnChangeAniEnd);
            _greatswordSpineAniController.SetEndFunc(item.Value, OnChangeAniEnd);
            _daggerSpineAniController.SetEndFunc(item.Value, OnChangeAniEnd);
            _bowSpineAniController.SetEndFunc(item.Value, OnChangeAniEnd);
            _staffSpineAniController.SetEndFunc(item.Value, OnChangeAniEnd);
        }

        _greatswordSpineAniController.SetEndFunc(_changeReturnAniName, OnChangeReturnAniEnd);
        _daggerSpineAniController.SetEndFunc(_changeReturnAniName, OnChangeReturnAniEnd);
        _bowSpineAniController.SetEndFunc(_changeReturnAniName, OnChangeReturnAniEnd);
        _staffSpineAniController.SetEndFunc(_changeReturnAniName, OnChangeReturnAniEnd);
    }

    [Button]
    public void ChangeForm(PuzzleType puzzleType)
    {
        if (_current == puzzleType) return;

        _current = puzzleType;

        if (puzzleType == PuzzleType.None)
        {
            SetForm(puzzleType);
            _isChanging = false;
            SetIdle(AttackGrade.Basic);

            OnChangeForm?.Invoke(puzzleType);
        }
        else
        {
            SetAnimation(_changeAniNames[puzzleType], false);
            _isChanging = true;
        }

        PlayVoice();
    }

    public void SetForm(PuzzleType puzzleType)
    {
        _basic.SetActive(puzzleType == PuzzleType.None);
        _greatsword.SetActive(puzzleType == PuzzleType.Red);
        _dagger.SetActive(puzzleType == PuzzleType.Blue);
        _bow.SetActive(puzzleType == PuzzleType.Green);
        _staff.SetActive(puzzleType == PuzzleType.Yellow);

        switch (puzzleType)
        {
            case PuzzleType.None:
                _characterAnimation = _basicSkeletonAnimation;
                break;
            case PuzzleType.Red:
                _characterAnimation = _greatswordSkeletonAnimation;
                break;
            case PuzzleType.Blue:
                _characterAnimation = _daggerSkeletonAnimation;
                break;
            case PuzzleType.Green:
                _characterAnimation = _bowSkeletonAnimation;
                break;
            case PuzzleType.Yellow:
                _characterAnimation = _staffSkeletonAnimation;
                break;
        }

        ClearAnimationState();
    }
    public void PlayVoice()
    {
        int rIndex = (int)UnityHelper.Random_H(0, _voiceSoundNames.Count);
        Managers.Sound.Play(_voiceSoundNames[rIndex], Sound.InGmae);
    }
    public void SetIdle(AttackGrade _currentGrade)
    {
        if(_current == PuzzleType.None)
        {
            SetAnimation(IdleAnimationName, true);
        }
        else
        {
            SetAnimation(_idleAniNames[(int)_currentGrade], true);
        }
    }

    private void OnChangeAniEnd()
    {
        SetForm(_current);

        SetAnimation(_changeReturnAniName, false);
    }
    private void OnChangeReturnAniEnd()
    {
        _isChanging = false;
        SetIdle(AttackGrade.Basic);
        OnChangeForm?.Invoke(_current);
    }

    public void SetAnimation(string aniName, bool isLoop)
    {
        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            if(_characterSpineAniControllers[i].gameObject.activeSelf)
                _characterSpineAniControllers[i].Play(aniName, isLoop, true);
        }
    }
    public void ClearAnimationState()
    {
        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            if(_characterSpineAniControllers[i].gameObject.activeSelf)
            {
                _characterSpineAniControllers[i].ClearState();
            }
        }
    }
}
