using System;
using System.Collections.Generic;
using Shared.BBNumber;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class Player : Character
{
    public Action<PuzzleType> OnChangeForm;

    public override CharacterTeam Team => CharacterTeam.Player;
    public bool IsChanging => _isChanging;
    public PuzzleType CurrentForm => _current;

    [SerializeField] private GameObject _basic;
    [SerializeField] private GameObject _greatsword;
    [SerializeField] private GameObject _dagger;
    [SerializeField] private GameObject _bow;
    [SerializeField] private GameObject _staff;

    private PuzzleType _current;
    private bool _isChanging = false;

    private SkeletonAnimation _basicSkeletonAnimation;
    private SkeletonAnimation _greatswordSkeletonAnimation;
    private SkeletonAnimation _daggerSkeletonAnimation;
    private SkeletonAnimation _bowSkeletonAnimation;
    private SkeletonAnimation _staffSkeletonAnimation;

    private SpineMaterialBlink _basicSpineMaterialBlink;
    private SpineMaterialBlink _greatswordSpineMaterialBlink;
    private SpineMaterialBlink _daggerSpineMaterialBlink;
    private SpineMaterialBlink _bowSpineMaterialBlink;
    private SpineMaterialBlink _staffSpineMaterialBlink;

    private SpineAniController _basicSpineAniController;
    private SpineAniController _greatswordSpineAniController;
    private SpineAniController _daggerSpineAniController;
    private SpineAniController _bowSpineAniController;
    private SpineAniController _staffSpineAniController;

    private SpineString _basicSpineString;
    private SpineString _greatswordSpineString;
    private SpineString _daggerSpineString;
    private SpineString _bowSpineString;
    private SpineString _staffSpineString;

    private Transform _basicHitBoneTr;
    private Transform _greatswordHitBoneTr;
    private Transform _daggerHitBoneTr;
    private Transform _bowHitBoneTr;
    private Transform _staffHitBoneTr;

    private Transform _basicBodyBoneTr;
    private Transform _greatswordBodyBoneTr;
    private Transform _daggerBodyBoneTr;
    private Transform _bowBodyBoneTr;
    private Transform _staffBodyBoneTr;

    private Transform _basicHandBoneTr;
    private Transform _greatswordHandBoneTr;
    private Transform _daggerHandBoneTr;
    private Transform _bowHandBoneTr;
    private Transform _staffHandBoneTr;

    private Transform _basicRootBoneTr;
    private Transform _greatswordRootBoneTr;
    private Transform _daggerRootBoneTr;
    private Transform _bowRootBoneTr;
    private Transform _staffRootBoneTr;

    private Transform _basicAtBoneTr;
    private Transform _greatswordAtBoneTr;
    private Transform _daggerAtBoneTr;
    private Transform _bowAtBoneTr;
    private Transform _staffAtBoneTr;

    private Tween<float> _smokeTween;

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

    private readonly string _changeReturnAniName = "Tr";
    private readonly string _smokePrefabPath = "Prefab/Effect/Universal/TrSmok";

    public override void Initialize(DungeonTree dungeonTree)
    {
        _basicSkeletonAnimation =  _basic.GetComponent<SkeletonAnimation>();
        _greatswordSkeletonAnimation =  _greatsword.GetComponent<SkeletonAnimation>();
        _daggerSkeletonAnimation =  _dagger.GetComponent<SkeletonAnimation>();
        _bowSkeletonAnimation =  _bow.GetComponent<SkeletonAnimation>();
        _staffSkeletonAnimation =  _staff.GetComponent<SkeletonAnimation>();

        _basicSpineMaterialBlink = _basicSkeletonAnimation.gameObject.GetOrAddComponent<SpineMaterialBlink>();
        _greatswordSpineMaterialBlink = _greatswordSkeletonAnimation.gameObject.GetOrAddComponent<SpineMaterialBlink>();
        _daggerSpineMaterialBlink = _daggerSkeletonAnimation.gameObject.GetOrAddComponent<SpineMaterialBlink>();
        _bowSpineMaterialBlink = _bowSkeletonAnimation.gameObject.GetOrAddComponent<SpineMaterialBlink>();
        _staffSpineMaterialBlink = _staffSkeletonAnimation.gameObject.GetOrAddComponent<SpineMaterialBlink>();

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

        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            _characterSpineAniControllers[i].Clear();
        }

        _basicSpineAniController.Initialize(_basicSkeletonAnimation);
        _greatswordSpineAniController.Initialize(_greatswordSkeletonAnimation);
        _daggerSpineAniController.Initialize(_daggerSkeletonAnimation);
        _bowSpineAniController.Initialize(_bowSkeletonAnimation);
        _staffSpineAniController.Initialize(_staffSkeletonAnimation);

        _basicSpineString = _basic.GetOrAddComponent<SpineString>();
        _greatswordSpineString = _greatsword.GetOrAddComponent<SpineString>();
        _daggerSpineString = _dagger.GetOrAddComponent<SpineString>();
        _bowSpineString = _bow.GetOrAddComponent<SpineString>();
        _staffSpineString = _staff.GetOrAddComponent<SpineString>();

        _basicSpineString.Clear();
        _greatswordSpineString.Clear();
        _daggerSpineString.Clear();
        _bowSpineString.Clear();
        _staffSpineString.Clear();

        _basicSpineString.Initialize(_basicSkeletonAnimation);
        _greatswordSpineString.Initialize(_greatswordSkeletonAnimation);
        _daggerSpineString.Initialize(_daggerSkeletonAnimation);
        _bowSpineString.Initialize(_bowSkeletonAnimation);
        _staffSpineString.Initialize(_staffSkeletonAnimation);

        _basicSpineString.OnEffect -= EffectSpawn;
        _greatswordSpineString.OnEffect -= EffectSpawn;
        _daggerSpineString.OnEffect -= EffectSpawn;
        _bowSpineString.OnEffect -= EffectSpawn;
        _staffSpineString.OnEffect -= EffectSpawn;

        _basicSpineString.OnEffect += EffectSpawn;
        _greatswordSpineString.OnEffect += EffectSpawn;
        _daggerSpineString.OnEffect += EffectSpawn;
        _bowSpineString.OnEffect += EffectSpawn;
        _staffSpineString.OnEffect += EffectSpawn;

        _basicSpineAniController.SetEndFunc(DeathAnimationName, DeadEnd);

        #region Bone
        GameObject basicHitBoneObj = UnityHelper.FindChild(_basic, HitBone, true);
        _basicHitBoneTr = basicHitBoneObj ? basicHitBoneObj.transform : this.transform;
        _boneTrDics[HitBone] = _basicHitBoneTr;

        GameObject basicBodyBoneObj = UnityHelper.FindChild(_basic, BodyBone, true);
        _basicBodyBoneTr = basicBodyBoneObj ? basicBodyBoneObj.transform : this.transform;
        _boneTrDics[BodyBone] = _basicBodyBoneTr;

        GameObject basicHandBoneObj = UnityHelper.FindChild(_basic, HandBone, true);
        _basicHandBoneTr = basicHandBoneObj ? basicHandBoneObj.transform : this.transform;
        _boneTrDics[HandBone] = _basicHandBoneTr;

        GameObject basicRootBoneObj = UnityHelper.FindChild(_basic, RootBone, true);
        _basicRootBoneTr = basicRootBoneObj ? basicRootBoneObj.transform : this.transform;
        _boneTrDics[RootBone] = _basicRootBoneTr;

        GameObject basicAtBoneObj = UnityHelper.FindChild(_basic, AtBone, true);
        _basicAtBoneTr = basicAtBoneObj ? basicAtBoneObj.transform : this.transform;
        _boneTrDics[AtBone] = _basicAtBoneTr;



        GameObject greatswordHitBoneObj = UnityHelper.FindChild(_greatsword, HitBone, true);
        _greatswordHitBoneTr = greatswordHitBoneObj ? greatswordHitBoneObj.transform : this.transform;

        GameObject greatswordBodyBoneObj = UnityHelper.FindChild(_greatsword, BodyBone, true);
        _greatswordBodyBoneTr = greatswordBodyBoneObj ? greatswordBodyBoneObj.transform : this.transform;

        GameObject greatswordHandBoneObj = UnityHelper.FindChild(_greatsword, HandBone, true);
        _greatswordHandBoneTr = greatswordHandBoneObj ? greatswordHandBoneObj.transform : this.transform;

        GameObject greatswordRootBoneObj = UnityHelper.FindChild(_greatsword, RootBone, true);
        _greatswordRootBoneTr = greatswordRootBoneObj ? greatswordRootBoneObj.transform : this.transform;

        GameObject greatswordAtBoneObj = UnityHelper.FindChild(_greatsword, AtBone, true);
        _greatswordAtBoneTr = greatswordAtBoneObj ? greatswordAtBoneObj.transform : this.transform;


        GameObject daggerHitBoneObj = UnityHelper.FindChild(_dagger, HitBone, true);
        _daggerHitBoneTr = daggerHitBoneObj ? daggerHitBoneObj.transform : this.transform;

        GameObject daggerBodyBoneObj = UnityHelper.FindChild(_dagger, BodyBone, true);
        _daggerBodyBoneTr = daggerBodyBoneObj ? daggerBodyBoneObj.transform : this.transform;

        GameObject daggerHandBoneObj = UnityHelper.FindChild(_dagger, HandBone, true);
        _daggerHandBoneTr = daggerHandBoneObj ? daggerHandBoneObj.transform : this.transform;

        GameObject daggerRootBoneObj = UnityHelper.FindChild(_dagger, RootBone, true);
        _daggerRootBoneTr = daggerRootBoneObj ? daggerRootBoneObj.transform : this.transform;

        GameObject daggerAtBoneObj = UnityHelper.FindChild(_dagger, AtBone, true);
        _daggerAtBoneTr = daggerAtBoneObj ? daggerAtBoneObj.transform : this.transform;


        GameObject bowHitBoneObj = UnityHelper.FindChild(_bow, HitBone, true);
        _bowHitBoneTr = bowHitBoneObj ? bowHitBoneObj.transform : this.transform;

        GameObject bowBodyBoneObj = UnityHelper.FindChild(_bow, BodyBone, true);
        _bowBodyBoneTr = bowBodyBoneObj ? bowBodyBoneObj.transform : this.transform;

        GameObject bowHandBoneObj = UnityHelper.FindChild(_bow, HandBone, true);
        _bowHandBoneTr = bowHandBoneObj ? bowHandBoneObj.transform : this.transform;

        GameObject bowRootBoneObj = UnityHelper.FindChild(_bow, RootBone, true);
        _bowRootBoneTr = bowRootBoneObj ? bowRootBoneObj.transform : this.transform;

        GameObject bowAtBoneObj = UnityHelper.FindChild(_bow, AtBone, true);
        _bowAtBoneTr = bowAtBoneObj ? bowAtBoneObj.transform : this.transform;

        GameObject staffHitBoneObj = UnityHelper.FindChild(_staff, HitBone, true);
        _staffHitBoneTr = staffHitBoneObj ? staffHitBoneObj.transform : this.transform;

        GameObject staffBodyBoneObj = UnityHelper.FindChild(_staff, BodyBone, true);
        _staffBodyBoneTr = staffBodyBoneObj ? staffBodyBoneObj.transform : this.transform;

        GameObject staffHandBoneObj = UnityHelper.FindChild(_staff, HandBone, true);
        _staffHandBoneTr = staffHandBoneObj ? staffHandBoneObj.transform : this.transform;

        GameObject staffRootBoneObj = UnityHelper.FindChild(_staff, RootBone, true);
        _staffRootBoneTr = staffRootBoneObj ? staffRootBoneObj.transform : this.transform;

        GameObject staffAtBoneObj = UnityHelper.FindChild(_staff, AtBone, true);
        _staffAtBoneTr = staffAtBoneObj ? staffAtBoneObj.transform : this.transform;

        #endregion

        SetForm(PuzzleType.None);

        base.Initialize(dungeonTree);

        _playerAttackReady = this.gameObject.GetOrAddComponent<PlayerAttackReady>();
        _playerAttackReady?.Clear();
        _playerAttackReady?.Initialize(this as Player);

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

        if(_smokeTween != null)
        {
            _smokeTween.FullKill();
        }

        _current = puzzleType;

        if (puzzleType == PuzzleType.None)
        {
            GameObject smoke = Managers.Resources.Instantiate(_smokePrefabPath);
            smoke.transform.position = this.BodyBoneTr.position;

            SetForm(puzzleType);
            _isChanging = false;
            SetIdle();

            OnChangeForm?.Invoke(puzzleType);
        }
        else
        {
            SetAnimation(_changeAniNames[puzzleType], false);
            _isChanging = true;
            PlayVoice();
        }
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
                _spineMaterialBlink = _basicSpineMaterialBlink;
                _spineString = _basicSpineString;

                _boneTrDics[HitBone] = _basicHitBoneTr;
                _boneTrDics[BodyBone] = _basicBodyBoneTr;
                _boneTrDics[HandBone] = _basicHandBoneTr;
                _boneTrDics[RootBone] = _basicRootBoneTr;
                _boneTrDics[AtBone] = _basicAtBoneTr;
                break;
            case PuzzleType.Red:
                _characterAnimation = _greatswordSkeletonAnimation;
                _spineMaterialBlink = _greatswordSpineMaterialBlink;
                _spineString = _greatswordSpineString;

                _boneTrDics[HitBone] = _greatswordHitBoneTr;
                _boneTrDics[BodyBone] = _greatswordBodyBoneTr;
                _boneTrDics[HandBone] = _greatswordHandBoneTr;
                _boneTrDics[RootBone] = _greatswordRootBoneTr;
                _boneTrDics[AtBone] = _greatswordAtBoneTr;
                break;
            case PuzzleType.Blue:
                _characterAnimation = _daggerSkeletonAnimation;
                _spineMaterialBlink = _daggerSpineMaterialBlink;
                _spineString = _daggerSpineString;

                _boneTrDics[HitBone] = _daggerHitBoneTr;
                _boneTrDics[BodyBone] = _daggerBodyBoneTr;
                _boneTrDics[HandBone] = _daggerHandBoneTr;
                _boneTrDics[RootBone] = _daggerRootBoneTr;
                _boneTrDics[AtBone] = _daggerAtBoneTr;
                break;
            case PuzzleType.Green:
                _characterAnimation = _bowSkeletonAnimation;
                _spineMaterialBlink = _bowSpineMaterialBlink;
                _spineString = _bowSpineString;

                _boneTrDics[HitBone] = _bowHitBoneTr;
                _boneTrDics[BodyBone] = _bowBodyBoneTr;
                _boneTrDics[HandBone] = _bowHandBoneTr;
                _boneTrDics[RootBone] = _bowRootBoneTr;
                _boneTrDics[AtBone] = _bowAtBoneTr;
                break;
            case PuzzleType.Yellow:
                _characterAnimation = _staffSkeletonAnimation;
                _spineMaterialBlink = _staffSpineMaterialBlink;
                _spineString = _staffSpineString;

                _boneTrDics[HitBone] = _staffHitBoneTr;
                _boneTrDics[BodyBone] = _staffBodyBoneTr;
                _boneTrDics[HandBone] = _staffHandBoneTr;
                _boneTrDics[RootBone] = _staffRootBoneTr;
                _boneTrDics[AtBone] = _staffAtBoneTr;
                break;
        }

        ClearAnimationState();

        _basicSpineString.Initialize(_basicSkeletonAnimation);
        _greatswordSpineString.Initialize(_greatswordSkeletonAnimation);
        _daggerSpineString.Initialize(_daggerSkeletonAnimation);
        _bowSpineString.Initialize(_bowSkeletonAnimation);
        _staffSpineString.Initialize(_staffSkeletonAnimation);
    }

    public void PlayVoice()
    {
        int rIndex = (int)UnityHelper.Random_H(0, _voiceSoundNames.Count);
        Managers.Sound.Play(_voiceSoundNames[rIndex], Sound.InGmae);
    }

    private void OnChangeAniEnd()
    {
        SetForm(_current);

        SetAnimation(_changeReturnAniName, false);
    }
    private void OnChangeReturnAniEnd()
    {
        _isChanging = false;
        SetIdle();
        OnChangeForm?.Invoke(_current);
    }

    public override void SetIdle()
    {
        if (IsDead)
            return;

        if (!IsCC)
        {
            if(_current == PuzzleType.None)
            {
                SetAnimation(IdleAnimationName, true);
            }
            else
            {
                _playerAttackReady.SetReady(_current, _playerAttackReady.CurrentGrade);
            }
        }
    }

    protected override void BattleEnd()
    {
        ChangeForm(PuzzleType.None);
    }

    public override void Dead()
    {
        for (int i = 0; i < _attachPools.Count; i++)
        {
            if (_attachPools[i] && _attachPools[i].gameObject.activeSelf)
                Managers.Resources.Destroy(_attachPools[i].gameObject);
        }
        
        CharacterBuff.BuffBar.gameObject.SetActive(false);
        _hpbar.gameObject.SetActive(false);

        ChangeForm(PuzzleType.None);
        SetAnimation(DeathAnimationName, false);

        OnDead?.Invoke(this);
        OnDead = null;
    }
}
