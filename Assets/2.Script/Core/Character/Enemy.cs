using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Character
{
    public override CharacterTeam Team => CharacterTeam.Enemy;

    protected readonly string DeathAnimationName_D = "Die_D";
    protected readonly string DeathAnimationName_G = "Die_G";
    protected readonly string DeathAnimationName_H = "Die_H";
    protected readonly string DeathAnimationName_M = "Die_M";
    public readonly string HitFaintingAnimationName = "Hit_Fainting";

    private PuzzleType _takeDamagePuzzleType;

    public override void Initialize(DungeonTree dungeonTree)
    {
        _characterAnimation =  this.gameObject.FindChildByPath<SkeletonAnimation>("Character");
        
        _spineMaterialBlink = _characterAnimation.gameObject.GetOrAddComponent<SpineMaterialBlink>();
        
        _characterSpineAniControllers.Add(_characterAnimation.GetOrAddComponent<SpineAniController>());
        
        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            _characterSpineAniControllers[i].Clear();

            _characterSpineAniControllers[i].SetEndFunc(DeathAnimationName, DeadEnd);
            _characterSpineAniControllers[i].SetEndFunc(DeathAnimationName_D, DeadEnd);
            _characterSpineAniControllers[i].SetEndFunc(DeathAnimationName_G, DeadEnd);
            _characterSpineAniControllers[i].SetEndFunc(DeathAnimationName_H, DeadEnd);
            _characterSpineAniControllers[i].SetEndFunc(DeathAnimationName_M, DeadEnd);

            _characterSpineAniControllers[i].SetEndFunc(HitAnimationName, HitEnd);
            _characterSpineAniControllers[i].SetEndFunc(HitFaintingAnimationName, HitEnd);
        }

        _characterSpineAniControllers[0].Initialize(_characterAnimation);
        
        _spineString = _characterAnimation.GetOrAddComponent<SpineString>();
        _spineString.Clear();
        _spineString.Initialize(_characterAnimation);
        
        _spineString.OnEffect -= EffectSpawn;
        _spineString.OnEffect += EffectSpawn;


        base.Initialize(dungeonTree);

        // Week
        if (!_weekBar)
            _weekBar = Managers.Resources.Instantiate<WeekBar>(_weekBarPath, this.transform);
        else
            _weekBar.gameObject.SetActive(true);
        _weekBar.transform.localPosition = new Vector3(0, BoxPosY + BoxHeight * 0.5f + 0.2f, 0);
        _weekBar.Initialize(weekCount);

        _weekBar.OnAllWeekCrash -= OnAllWeekCrash;
        _weekBar.OnAllWeekCrash += OnAllWeekCrash;
        _isWeekInitWhenTurnEnd = false;

        GameObject hitBoneObj = UnityHelper.FindChild(this.gameObject, HitBone, true);
        Transform hitBoneTr = hitBoneObj ? hitBoneObj.transform : this.transform;
        _boneTrDics[HitBone] = hitBoneTr;
        
        GameObject bodyBoneObj = UnityHelper.FindChild(this.gameObject, BodyBone, true);
        Transform bodyBoneTr = bodyBoneObj ? bodyBoneObj.transform : this.transform;
        _boneTrDics[BodyBone] = bodyBoneTr;
        
        GameObject handBoneObj = UnityHelper.FindChild(this.gameObject, HandBone, true);
        Transform handBoneTr = handBoneObj ? handBoneObj.transform : this.transform;
        _boneTrDics[HandBone] = handBoneTr;
        
        GameObject rootBoneObj = UnityHelper.FindChild(this.gameObject, RootBone, true);
        Transform rootBoneTr = rootBoneObj ? rootBoneObj.transform : this.transform;
        _boneTrDics[RootBone] = rootBoneTr;
        
        GameObject atBoneObj = UnityHelper.FindChild(this.gameObject, AtBone, true);
        Transform atBoneTr = atBoneObj ? atBoneObj.transform : this.transform;
        _boneTrDics[AtBone] = atBoneTr;
    }

    public override void SetHit()
    {
        base.SetHit();

        if (!IsDead)
        {
            SetAnimation(IsCC ? HitFaintingAnimationName : HitAnimationName, false);
        }
    }
    private void HitEnd()
    {
        if (IsCC)
        {
            CharacterCC.SetAniFainting();
        }
        else
        {
            SetIdle();
        }
    }

    public override void TakeDamage(Character applyOwner, object cause, BBNumber damage, DamageType damageType, CriticalType criticalType)
    {
        if (cause != null)
        {
            // Puzzle Data
            if (cause.GetType() == typeof(PuzzleAttackData))
            {
                PuzzleAttackData puzzleAttackData = (PuzzleAttackData)cause;
                _takeDamagePuzzleType = puzzleAttackData.data.puzzleType;
            }
            else
            {
                _takeDamagePuzzleType = PuzzleType.None;
            }
        }
        else
        {
            _takeDamagePuzzleType = PuzzleType.None;
        }

        base.TakeDamage(applyOwner, cause, damage, damageType, criticalType);
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

        switch (_takeDamagePuzzleType)
        {
            case PuzzleType.Blue:
                SetAnimation(DeathAnimationName_D, false);
                break;
            case PuzzleType.Red:
                SetAnimation(DeathAnimationName_G, false);
                break;
            case PuzzleType.Green:
                SetAnimation(DeathAnimationName_H, false);
                break;
            case PuzzleType.Yellow:
                SetAnimation(DeathAnimationName_M, false);
                break;
            default:
                SetAnimation(DeathAnimationName, false);
                break;
        }

        OnDead?.Invoke(this);
        OnDead = null;
    }
}
