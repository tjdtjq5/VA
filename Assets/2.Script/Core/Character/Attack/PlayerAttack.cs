using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class PlayerAttack : Attack
{
    private Player _player;
    private Character _target;
    private PuzzleData _puzzleData;
    private int _puzzleCombo;
    private int _forceCount;

    private AttackGrade _attackGrade;
    private int _gradeIndex;
    private int _daggerTargetIndex;
    
    private readonly string _moveEvent = "move";
    
    private readonly List<string> _attackAniNames = new List<string>()
        { "At_1", "At_2", "At_3", "At_4" };

    private readonly string _moveAniName = "Move";
    private readonly string _reMoveAniName = "Re";
    private readonly string _greatswordHitPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Greatsword";
    private readonly string _daggerHitPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Dagger";
    private readonly string _bowHitPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Bow";
    private readonly string _staffHitPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Staff";
    private readonly string _greatswordHitOuiPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Greatsword_Oui";
    private readonly string _daggerHitOuiPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Dagger_Oui";
    private readonly string _bowHitOuiPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Bow_Oui";
    private readonly string _staffHitOuiPrefabPath = "Prefab/Effect/Skill/Weapon/Hit_Staff_Oui";

    private readonly string _greatswordPrefabPath_Fatal = "Prefab/Effect/Skill/Weapon/Hit_Greatsword_Fatal_And_Oui";
    private readonly string _greatswordHitPrefabPath_Oui = "Prefab/Effect/Skill/Weapon/Hit_Greatsword_Fatal_And_Oui";
    
    public override void Initialize(Character character, Transform transform)
    {
        base.Initialize(character, transform);
        _player = character as Player;
        
        for (int i = 0; i < Character.SpineSpineAniControllers.Count; i++)
        {
            for (int j = 0; j < _attackAniNames.Count; j++)
            {
                Character.SpineSpineAniControllers[i].SetEndFunc(_attackAniNames[j], AttackAniEnd);
                Character.SpineSpineAniControllers[i].SetEventFunc(_attackAniNames[j], _moveEvent , MovePos);
                Character.SpineSpineAniControllers[i].SetEventFunc(_attackAniNames[j], FxEvent , AttackEffect);
                Character.SpineSpineAniControllers[i].SetEventFunc(_attackAniNames[j], ActionEvent , AttackAniAction);
                Character.SpineSpineAniControllers[i].SetEventFunc(_attackAniNames[j], ActionEvent , AttackEffect);
            }
            Character.SpineSpineAniControllers[i].SetEndFunc(_reMoveAniName , OnAttackEnd);
        }
    }
    public override void SetAttack(List<Character> targets, object cause, bool isSequence)
    {
        base.SetAttack(targets, cause, isSequence);

        Character.CharacterAttackReady.End();
        
        this.Targets = targets;
        Vector3 pos = GetTargetPosition(targets[0], false);
        
        PuzzleAttackData pad = (PuzzleAttackData)cause;
        _puzzleData = pad.data;
        _puzzleCombo = pad.combo;
        _forceCount = pad.forceCount;

        _attackGrade = GameDefine.GetAttackGrade(_puzzleCombo);
        _gradeIndex = (int)_attackGrade;

        switch (_puzzleData.puzzleType)
        {
            case PuzzleType.Red: // Greatsword
                pos.x -= 0.5f;
                switch (_attackGrade)
                {
                    case AttackGrade.Focus:
                        StartAction(targets[0], cause);
                        break;
                    case AttackGrade.Fatal:
                        StartAction(targets[0], cause);
                        break;
                    case AttackGrade.Oui:
                        StartAction(targets[0], cause);
                        break;
                    default:
                        MoveAttack(targets[0], pos, _moveAniName, true, 1.6f, cause);
                        break;
                }
                break;
            case PuzzleType.Blue: // Dagger
                switch (_attackGrade)
                {
                    case AttackGrade.Fatal:
                        StandAttack();
                        StartAction(targets[0], cause);
                        break;
                    case AttackGrade.Oui:
                        StandAttack();
                        StartAction(targets[0], cause);
                        break;
                    default:
                        MoveAttack(targets[0], pos, _moveAniName, true, 2f, cause);
                        break;
                }
                _daggerTargetIndex = 0;
                break;
            case PuzzleType.Green: // Bow
                StartAction(targets[0], cause);
                break;
            case PuzzleType.Yellow: // Staff
                StartAction(targets[0], cause);
                break;
        }
    }
    protected override void StartAction(Character target, object cause)
    {
        this._target = target;
        this.Cause = cause;
        
        SetAnimation(_attackAniNames[_gradeIndex], false);
    }
    private void AttackAniAction()
    {
        Attack();
        
        switch (_puzzleData.puzzleType)
        {
            case PuzzleType.Red:
                switch (_attackGrade)
                {
                    case AttackGrade.Fatal:
                        GameObject fatalEffect = Managers.Resources.Instantiate(_greatswordPrefabPath_Fatal);
                        if(fatalEffect)
                            fatalEffect.transform.position = Targets[0].RootBoneTr.position;
                        break;
                    case AttackGrade.Oui:
                        GameObject ouiEffect = Managers.Resources.Instantiate(_greatswordHitPrefabPath_Oui);
                        if(ouiEffect)
                             ouiEffect.transform.position = Targets[0].RootBoneTr.position;
                        break;
                }
                break;
        }

    }
    private void AttackAniEnd()
    {
        switch (_puzzleData.puzzleType)
        {
            case PuzzleType.Red:
                AttackEnd(_moveAniName, true, 1.5f);
                break;
            case PuzzleType.Blue:
                switch (_attackGrade)
                {
                    case AttackGrade.Basic:
                        AttackEndRe(_reMoveAniName, 0.45f);
                        break;
                    case AttackGrade.Focus:
                        AttackEndRe(_reMoveAniName, 0.45f);
                        break;
                    default:
                        AttackEnd(_moveAniName, true, 1.8f);
                        break;
                }
                break;
            case PuzzleType.Green:
                AttackEnd(null, true);
                break;
            case PuzzleType.Yellow:
                AttackEnd(null, true);
                break;
        }
    }

    private void MovePos()
    {
        switch (_puzzleData.puzzleType)
        {
            case PuzzleType.Red: // Greatsword
                switch (_attackGrade)
                {
                    case AttackGrade.Focus:
                        TimeMove(null, GetTargetPosition(Targets[0], false), 0.45f);
                        break;
                    case AttackGrade.Fatal:
                        TimeMove(null, GetTargetPosition(Targets[0], false), 0.45f);
                        break;
                    case AttackGrade.Oui:
                        TimeMove(null, GetTargetPosition(Targets[0], false), 0.65f);
                        break;
                }
                break;
            case PuzzleType.Blue: // Dagger
                switch (_attackGrade)
                {
                    case AttackGrade.Fatal:
                        _daggerTargetIndex++;
                        Vector3 fatalFirstPos = GetTargetPosition(Targets[0], false);
                        Vector3 fatalLastPos = GetTargetPosition(Targets[Targets.Count - 1], true);
                        fatalLastPos.x -= this.Character.BoxWeidth * 0.5f;
                        Vector3 fatalPos = _daggerTargetIndex <= 1 ? fatalLastPos : fatalFirstPos;
                        this.Character.SetBack(_daggerTargetIndex == 2);
                        NotRecordMove(fatalPos, null, true, 8f);
                        break;
                    case AttackGrade.Oui:
                        _daggerTargetIndex++;
                        Vector3 ouiFirstPos = GetTargetPosition(Targets[0], false);
                        Vector3 ouiLastPos = GetTargetPosition(Targets[Targets.Count - 1], true);
                        ouiLastPos.x -= this.Character.BoxWeidth * 0.5f;
                        Vector3 ouiPos = _daggerTargetIndex % 2 == 0 ? ouiFirstPos : ouiLastPos;
                        this.Character.SetBack(_daggerTargetIndex % 2 == 0);
                        NotRecordMove(ouiPos, null, true, 8f);
                        break;
                }
                break;
            case PuzzleType.Green: // Bow
                break;
            case PuzzleType.Yellow: // Staff
                break;
        }
    }
    private void AttackEffect()
    {
        string effectPath = "";
        switch (_puzzleData.puzzleType)
        {
            case PuzzleType.Red:
                effectPath = _attackGrade == AttackGrade.Oui ? _greatswordHitOuiPrefabPath : _greatswordHitPrefabPath;
                break;
            case PuzzleType.Blue:
                effectPath = _attackGrade == AttackGrade.Oui ? _daggerHitOuiPrefabPath : _daggerHitPrefabPath;
                break;
            case PuzzleType.Green:
                effectPath = _attackGrade == AttackGrade.Oui ? _bowHitOuiPrefabPath : _bowHitPrefabPath;
                break;
            case PuzzleType.Yellow:
                effectPath = _attackGrade == AttackGrade.Oui ? _staffHitOuiPrefabPath : _staffHitPrefabPath;
                break;
        }
        
        switch (_attackGrade)
        {
            case AttackGrade.Fatal:
                for (int i = 0; i < Targets.Count; i++)
                {
                    Managers.Resources.Instantiate(effectPath).transform.position = Targets[i].BodyBoneTr.position;
                    Targets[i].SetHit();
                }
                break;
            case AttackGrade.Oui:
                for (int i = 0; i < Targets.Count; i++)
                {
                    Managers.Resources.Instantiate(effectPath).transform.position = Targets[i].BodyBoneTr.position;
                    Targets[i].SetHit();
                }
                break;
            default:
                Managers.Resources.Instantiate(effectPath).transform.position = Targets[0].BodyBoneTr.position;
                Targets[0].SetHit();
                break;
        }
    }
    void Attack()
    {
        if (Cause.GetType() == typeof(PuzzleAttackData))
        {
            PuzzleAttackData cause = Cause as PuzzleAttackData;
            cause.isSequence = IsSequence;
            Cause = cause;
        }

        float damageValue = GameDefine.ComboMultiplier(this.Character, _puzzleCombo, _forceCount);

        switch (_attackGrade)
        {
            case AttackGrade.Fatal:
                for (int i = 0; i < Targets.Count; i++)
                    OnAttackAction(Targets[i], Cause, damageValue);
                break;
            case AttackGrade.Oui:
                for (int i = 0; i < Targets.Count; i++)
                    OnAttackAction(Targets[i], Cause, damageValue);
                break;
            default:
                OnAttackAction(Targets[0], Cause, damageValue);
                break;
        }
    }
    protected override void OnAttackEnd()
    {
        Clear();

        this.Character.SetBack(false);
        this.Character.CharacterMove.SetSpeed();
        this.Character.SetOrderInLayer(this.Character.OrderInLayer);
        
        Targets = this.Targets.FindAll(t => !t.IsNotDetect);
        if (Targets.Count > 0 && this.Character.Stats.sequenceStat && this.Character.Stats.sequenceStat.IsMax)
        {
            this.Character.Stats.sequenceStat.DefaultValue = 0;
            IsAddSequence = false;
            SetAttack(this.Targets, this.Cause, true);
        }
        else
        {
            if (this.Character.IsOnTriggerPassiveBuff(TriggerPassiveBuff.Skill_AddSequenceAttack_Attack_Sequence) && IsSequence && !IsAddSequence)
            {
                bool isAddSequence = UnityHelper.Random_H(0, 100) >= 50;
                if (isAddSequence)
                {
                    IsAddSequence = true;
                    SetAttack(this.Targets, this.Cause, true);
                }
                else
                {
                    OnEnd?.Invoke();
                    OnEnd = null;

                    this.Character.CharacterAttackReady.CurrentGrade = AttackGrade.Basic;
                    this.Character.SetIdle();
                }
            }
            else
            {
                OnEnd?.Invoke();
                OnEnd = null;

                this.Character.CharacterAttackReady.CurrentGrade = AttackGrade.Basic;
                this.Character.SetIdle();
            }
        }
    }
    protected void AttackEndRe(string backMoveAniName, float time)
    {
        this.Character.CharacterMove.SetTimeMove(backMoveAniName, _moveStartPos, time, null);
    }
}