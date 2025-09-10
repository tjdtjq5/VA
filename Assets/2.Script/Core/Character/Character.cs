using System;
using System.Collections.Generic;
using System.Linq;
using Shared.BBNumber;
using Shared.CSharp;
using Sirenix.OdinInspector;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterMove))]
[RequireComponent(typeof(CharacterAttack))]
[RequireComponent(typeof(CharacterSkill))]
[RequireComponent(typeof(CharacterBuff))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour
{
    public virtual CharacterTeam Team { get; protected set; }
    [ShowIf(nameof(Team), CharacterTeam.Enemy)]
    public EnemyType enemyType;
    [ShowIf(nameof(Team), CharacterTeam.Enemy)]
    [SerializeField, Min(0)] protected int weekCount;
    public bool IsPlayer => Team == CharacterTeam.Player;
    public int WeekCount => weekCount;
    [SerializeField] protected List<Stat> stats = new();
    [SerializeField] protected List<StatOverride> _statOverrides = new();
    
    protected SkeletonAnimation _characterAnimation;
    protected List<SpineAniController> _characterSpineAniControllers = new();
    protected SpineMaterialBlink _spineMaterialBlink;
    protected SpineString _spineString;
    protected CharacterMove _characterMove;
    protected CharacterAttack _characterAttack;
    protected CharacterSkill _characterSkill;
    protected CharacterBuff _characterBuff;
    protected CharacterCC _characterCC;
    protected PlayerAttackReady _playerAttackReady;
    protected Stats _stats;
    protected Rigidbody2D _rigidbody2D;
    protected BoxCollider2D _boxCollider2D;
    protected HpBar _hpbar;
    protected WeekBar _weekBar;
    
    public Action<Character> OnDead;
    public Action<Character> OnDeadEnd;
    public Action<Character> OnDestroy;
    public Action OnStageStart;
    public Action OnStageEnd;
    public Action OnBattleStart;
    public Action OnBattleEnd;
    public Action<int> OnTurnStart;
    public Action<int> OnTurnEnd;
    public Action OnCharacterActionStart;
    public Action OnCharacterActionEnd;
    public Action<BBNumber, object> OnTakeDamage;
    public Action OnStunSuccess;
    public Action OnSequenceAttack;
    public Action<int> OnSlashAttack;
    public Action<int> OnLightningAttack;
    public Action<int> OnHellFireAttack;
    public Action<int> OnGasAttack;
    public Action<int> OnIceThornAttack;
    public Action<int> OnBurnAttack;
    public Action<int> OnPoisonAttack;
    public Action<PuzzleAttackData> OnPuzzleAttackStart;
    public Action<PuzzleAttackData> OnPuzzleAttackEnd;
    public Action<CharacterApplyAttack> OnApplyAttack;
    public Action OnPerfect;
    public Action<float> OnHpIncrease;
    public Action<float> OnHpDecrease;

    public bool IsNotDetect => IsDead;
    public bool IsCC => _characterCC.IsStun;
    protected bool IsDead { get; set; } =  false;
    public bool IsLeft
    {
        get => _isLeft;
        set
        {
            for (int i = 0; i < SpineSpineAniControllers.Count; i++)
            {
                SpineSpineAniControllers[i].SkeletonAnimation.transform.localScale = value ? _leftVector : _rightVector;
            }
            _isLeft = value;
        } 
    }
    public HpBar HpBar => _hpbar;
    public WeekBar WeekBar => _weekBar;

    protected bool _isLeft = false;
    
    public CharacterMove CharacterMove => _characterMove;
    public CharacterAttack CharacterAttack => _characterAttack;
    public CharacterSkill CharacterSkill => _characterSkill;
    public CharacterBuff CharacterBuff => _characterBuff;
    public CharacterCC CharacterCC => _characterCC;
    public SpineMaterialBlink CharacterBlink => _spineMaterialBlink;
    public PlayerAttackReady CharacterAttackReady => _playerAttackReady;
    public Stats Stats => _stats;
    public List<SpineAniController> SpineSpineAniControllers => _characterSpineAniControllers;
    public float BoxWeidth => _boxCollider2D != null ? _boxCollider2D.bounds.size.x : this.GetComponent<BoxCollider2D>().bounds.size.x;
    public float BoxHeight => _boxCollider2D != null ? _boxCollider2D.bounds.size.y : this.GetComponent<BoxCollider2D>().bounds.size.y;
    public float BoxPosX => _boxCollider2D != null ? _boxCollider2D.offset.x : this.GetComponent<BoxCollider2D>().offset.x;
    public float BoxPosY => _boxCollider2D != null ? _boxCollider2D.offset.y : this.GetComponent<BoxCollider2D>().offset.y;
    public float HpPercent => (Stats.hpStat.Value / Stats.MaxStatValue(Stats.hpStat)).ToFloat();
    public int OrderInLayer => _orderInLayer;
    public bool IsOnTriggerPassiveBuff(TriggerPassiveBuff triggerPassiveBuff) =>
        CharacterBuff.IsOnTriggerPassiveBuff(triggerPassiveBuff);
    public int GetPageApplyCount(SkillApplyDamageType damageType) => _pageApplyCount.ContainsKey(damageType) ? _pageApplyCount[damageType] : 0;
    public void AddPageApplyCount(SkillApplyDamageType damageType)
    {
        if (_pageApplyCount.ContainsKey(damageType))
        {
            _pageApplyCount[damageType]++;
        }
        else
        {
            _pageApplyCount.Add(damageType, 1);
        }
    }
    
    protected Dictionary<string, Transform> _boneTrDics = new();
    public Transform HitBoneTr => _boneTrDics.TryGet_H(HitBone, this.transform);
    public Transform BodyBoneTr => _boneTrDics.TryGet_H(BodyBone, this.transform);
    public Transform HandBoneTr => _boneTrDics.TryGet_H(HandBone, this.transform);
    public Transform RootBoneTr => _boneTrDics.TryGet_H(RootBone, this.transform);
    public Transform AtBoneTr => _boneTrDics.TryGet_H(AtBone, this.transform);
    
    public readonly string DeathAnimationName = "Die";
    public readonly string IdleAnimationName = "Idle";
    public readonly string SummonAnimationName = "Summon";
    public readonly string HitAnimationName = "Hit";
    protected readonly string HitBone = "hit";
    protected readonly string BodyBone = "fx_body";
    protected readonly string HandBone = "fx_hand";
    protected readonly string RootBone = "fx_root";
    protected readonly string AtBone = "fx_at";
    protected readonly string _hpBarPath = "Prefab/Character/HpBar";
    protected readonly string _weekBarPath = "Prefab/Character/WeekBar";
    protected readonly string _shieldPath = "Prefab/Effect/Buff/Buff_Shield";
    protected readonly string _buffStatTextPath = "Prefab/InGame/BuffText";
    protected readonly Vector3 _leftVector = new Vector3(-1, 1, 1);
    protected readonly Vector3 _rightVector = new Vector3(1, 1, 1);
    protected readonly int _orderInLayer = 100;
    private readonly string _hpRecoveryEffectPath = "Prefab/Effect/Buff/Buff_HpRecovery";

    protected readonly List<Poolable> _attachPools = new();
    protected Poolable _shieldPool = new();
    protected bool _isWeekInitWhenTurnEnd = false;
    protected Dictionary<SkillApplyDamageType, int> _pageApplyCount = new();
    protected JobSerializer _buffStatTextSpawnJob = new();
    protected bool _isBuffStatTextSpawn = false;
    private Poolable _hpRecoveryEffectPool;

    public virtual void Initialize(DungeonTree dungeonTree)
    {
        IsDead = false;

        OnDead = null;
        OnDestroy = null;
        OnStageStart = null;
        OnStageEnd = null;
        OnBattleStart = null;
        OnBattleEnd = null;
        OnTurnStart = null;
        OnTurnEnd = null;
        OnCharacterActionStart = null;
        OnCharacterActionEnd = null;

                // Stage
        OnStageStart += StageStart;
        OnStageEnd += StageEnd;
        
        // Battle
        OnBattleStart += BattleStart;
        OnBattleEnd += BattleEnd;
        
        // Page
        OnTurnStart += TurnStart;
        OnTurnEnd += TurnEnd;
        
        // Turn
        OnCharacterActionStart += CharacterActionStart;
        OnCharacterActionEnd += CharacterActionEnd;
        
        _rigidbody2D = this.GetComponent<Rigidbody2D>();
        _boxCollider2D = this.GetComponent<BoxCollider2D>();
        Setting();
            
        _stats = this.GetComponent<Stats>();
        _stats.Clear();
        _stats.Initialize(stats);
        
        if (_stats.hpStat != null)
        {
            _stats.hpStat.onValueChanged -= OnHpValueChagned;
            _stats.hpStat.onValueChanged += OnHpValueChagned;
            _stats.hpStat.onBonusValueChanged -= OnHpMaxChanged;
            _stats.hpStat.onBonusValueChanged += OnHpMaxChanged;
            _stats.hpStat.onDecreaseValueChanged -= OnHpMaxChanged;
            _stats.hpStat.onDecreaseValueChanged += OnHpMaxChanged;
            _stats.shieldStat.onValueChanged -= OnShieldValueChagned;
            _stats.shieldStat.onValueChanged += OnShieldValueChagned;
        }


        if (_stats.sequenceStat)
        {
            _stats.sequenceStat.onValueChanged -= OnSequenceValueChagned;
            _stats.sequenceStat.onValueChanged += OnSequenceValueChagned;
        }
        
        _characterAttack = this.GetComponent<CharacterAttack>();
        _characterAttack?.Clear();
        _characterAttack?.Initialize(this);
        
        _characterBuff = this.GetComponent<CharacterBuff>();
        _characterBuff?.Clear();
        _characterBuff?.Initialize(this);
        
        _characterSkill = this.GetComponent<CharacterSkill>();
        _characterSkill?.Clear();
        _characterSkill?.Initialize(this);
        
        _characterMove = this.GetComponent<CharacterMove>();
        _characterMove?.Clear();
        _characterMove?.Initialize(this);
        
        _characterCC = this.GetOrAddComponent<CharacterCC>();
        _characterCC?.Clear();
        _characterCC?.Initialize(this);

        if (_stats.hpStat != null)
        {
            if (!_hpbar)
                _hpbar = Managers.Resources.Instantiate<HpBar>(_hpBarPath, this.transform);
            else
                _hpbar.gameObject.SetActive(true);
            _hpbar.transform.localPosition = new Vector3(0, BoxPosY - BoxHeight * 0.5f - 0.4f, 0);
        }
        
        for (int i = 0; i < _statOverrides.Count; i++)
            _statOverrides[i].ChangeStats(this);

        if (_stats.hpStat != null)
        {
            _hpbar.Initialize(_stats.MaxStatValue(_stats.hpStat), Stats.sequenceStat ? Stats.sequenceStat.MaxValue : 0);
            _hpbar.Shield = _stats.shieldStat.Value;
        }

        if (_stats.sequenceStat != null)
        {
            _stats.sequenceStat.DefaultValue = 0;
        }
        
        IsLeft = !IsPlayer;

        if (_hpbar != null)
            _hpbar.gameObject.SetActive(true);

        SetIdle();

        SetOrderInLayer(_orderInLayer);

        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            _characterSpineAniControllers[i].SetEndFunc(SummonAnimationName, SetIdle);
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
    
    public void SetAnimation(string aniName, bool isLoop)
    {
        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            if(_characterSpineAniControllers[i].gameObject.activeSelf)
                _characterSpineAniControllers[i].Play(aniName, isLoop, true);
        }
    }
    public void SetSummon()
    {
        if (IsDead)
            return;

        SetAnimation(SummonAnimationName, false);
    }
    public virtual void SetIdle()
    {
        if (IsDead)
            return;

        if (!IsCC)
        {
            SetAnimation(IdleAnimationName, true);
        }
    }
    public virtual void SetHit()
    {
        if (IsDead)
            return;
        
        this.CharacterBlink.Blink();
    }

    protected virtual void StageStart()
    {
        
    }
    protected virtual void StageEnd()
    {
        
    }
    protected virtual void BattleStart()
    {
        
    }
    protected virtual void BattleEnd()
    {
        
    }
    protected virtual void TurnStart(int page)
    {
        _pageApplyCount.Clear();
        
        if (_characterCC.IsStun)
        {
            if (_characterCC.AddStunCount())
            {
                _characterCC.ClearStun();
                _isWeekInitWhenTurnEnd = true;
            }
        }
    }
    protected virtual void TurnEnd(int page)
    {
        if (_isWeekInitWhenTurnEnd)
        {
            _isWeekInitWhenTurnEnd = false;
            _weekBar.Initialize(weekCount);
        }
    }
    protected virtual void CharacterActionStart()
    {
        
    }
    protected virtual void CharacterActionEnd()
    {
        
    }

    public void Perfect()
    {
        OnPerfect?.Invoke();
    }
    public void AddSequence()
    {
        if (Stats.sequenceStat)
        {
            int value = 100;
            Stats.sequenceStat.DefaultValue += value;
        }
    }
    protected void OnAllWeekCrash()
    {
        _characterCC.SetStun();
    }
    public void WeekConquer(Character takeOwner, PuzzleType puzzleType)
    {
        if (IsDead || _characterCC.IsStun)
            return;

        _weekBar.Conquer(takeOwner, puzzleType);
    }
    public void WeekConquer(Character takeOwner)
    {
        if (IsDead || _characterCC.IsStun)
            return;
        
        _weekBar.Conquer(takeOwner);
    }
    public void SetBack(bool flag)
    {
        bool origin = !IsPlayer;
        origin = flag ? !origin : origin;
        IsLeft = origin;
    }
    public void SetOrderInLayer(int orderInLayer)
    {
        for (int i = 0; i < _characterSpineAniControllers.Count; i++)
        {
            _characterSpineAniControllers[i].MeshRenderer.sortingOrder = orderInLayer;
        }
    }

    public void ApplyAttack(Character takeOwner, object cause, float damageValue, float criPercent, DamageType damageType, SkillApplyDamageType skillApplyDamageType)
    {
        if (takeOwner.IsDead)
            return;
        
        BBNumber damage = this.Stats.PreventedDamage * damageValue;

        // if (cause.GetType() == typeof(SkillHellFire))

        if (takeOwner.CharacterBuff.CheckBuff("DebuffFire"))
        {
            damage *= Stats.GetValue("BurnTakeDamage") * 0.01f + 1;
        }
        if (takeOwner.CharacterBuff.CheckBuff("DebuffPoison"))
        {
            damage *= Stats.GetValue("PoisonTakeDamage") * 0.01f + 1;
        }

        damage *= Stats.GetValue("AllDamage") * 0.01f + 1;
        
        CriticalType criticalType = UnityHelper.IsApplyPercent(criPercent) ? CriticalType.Critical : CriticalType.None;
        if (criticalType != CriticalType.None)
        {
            damage *= Stats.GetValue("CriDamage") * 0.01f + 1;

            // 슈퍼 치명타 판가름
            if (skillApplyDamageType == SkillApplyDamageType.Missile)
            {
                float superCriticalPercent = takeOwner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.Missile_P) ? 100f : 50f;
                criticalType = UnityHelper.IsApplyPercent(criPercent) ? CriticalType.SuperCritical : CriticalType.Critical;

                if (criticalType == CriticalType.SuperCritical)
                {
                    damage *= Stats.GetValue("SuperCriDamage") * 0.01f + 1;
                }
            }
        }

        OnApplyAttack?.Invoke(new CharacterApplyAttack() { takeOwner = takeOwner, damageType = skillApplyDamageType, cause = cause, damageValue = damageValue });
        
        takeOwner.TakeDamage(this, cause, damage, damageType, criticalType);
    }
    public virtual void TakeDamage(Character applyOwner, object cause, BBNumber damage, DamageType damageType, CriticalType criticalType)
    {
        if (IsDead)
            return;
        
        // Evas
        float evas = Stats.GetValue("Evas").ToFloat();
        if (UnityHelper.IsApplyPercent(evas))
        {
            Managers.FloatingText.Miss(this);
            return;
        }
        
        if (_characterCC.IsStun)
        {
            if(applyOwner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.StunDamageEnhancement_P))
            {
                damage *= 4f;
            }
            else if(applyOwner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.StunDamageEnhancement))
            {
                damage *= 3f;
            }
            else
            {
                damage *= 2f;
            }
        }
        
        // Cause
        if (cause != null)
        {
            // Puzzle Data
            if (cause.GetType() == typeof(PuzzleAttackData))
            {
                PuzzleAttackData puzzleAttackData = (PuzzleAttackData)cause;
                WeekConquer(applyOwner, puzzleAttackData.data.puzzleType);
            }
        }
        
        // Damage Reduction : Def
        damage -= Stats.DamageReduction * damage;
        
        // Round
        damage = BBNumber.Round(damage);
        
        BBNumber shield = _stats.shieldStat.Value;

        if (shield >= damage)
        {
            _stats.shieldStat.DefaultValue -= damage;
        }
        else
        {
            if (_stats.shieldStat.Value > 0)
            {
                _stats.shieldStat.DefaultValue -= shield;
                _stats.hpStat.DefaultValue -= (damage - shield);
            }
            else
            {
                _stats.hpStat.DefaultValue -= damage;
            }
        }

        int submersionCount = CharacterBuff.GetBuffCount("Submersion");

        if (cause != null && cause.ToString() != "Submersion")
        {
            if (submersionCount > 0)
            {
                BBNumber submersionDamage = damage * 0.1f * submersionCount;
                this.TakeDamage(applyOwner, "Submersion", submersionDamage, DamageType.Add, criticalType);
            }
        }
        
        Managers.FloatingText.Damage(this, damage, damageType, criticalType);
        
        OnTakeDamage?.Invoke(damage, cause);
    }
    public void HpRecovery(float value)
    {
        BBNumber maxValue = _stats.MaxStatValue(_stats.hpStat);
        BBNumber recovery = maxValue * value;
        BBNumber currentValue = _stats.hpStat.Value;

        if (maxValue > currentValue + recovery)
        {
            Stats.hpStat.DefaultValue += recovery;
            Managers.FloatingText.Heal(this, recovery);
        }
        else
        {
            BBNumber healValue = maxValue - currentValue;
            Stats.hpStat.DefaultValue += healValue;
            Managers.FloatingText.Heal(this, healValue);
        }

        if (_hpRecoveryEffectPool == null || !_hpRecoveryEffectPool.gameObject.activeSelf)
        {
            _hpRecoveryEffectPool = Managers.Resources.Instantiate<Poolable>(_hpRecoveryEffectPath);
            Attach(_hpRecoveryEffectPool, BodyBoneTr);
            _hpRecoveryEffectPool.transform.localPosition = Vector3.zero;
        }
    }
    public void HpDecrease(float value)
    {
        BBNumber maxValue = _stats.MaxStatValue(_stats.hpStat);
        BBNumber decrease = maxValue * value;

        Stats.hpStat.DefaultValue -= decrease;
    }
    public void StartSkillOrBuff(Character useCharacter, IdentifiedObject skillOrBuff)
    {
        Skill skill = skillOrBuff as Skill;
        Buff buff = skillOrBuff as Buff;
        
        if (skill)
            CharacterSkill.PushSkill(skill);
        else if (buff)
            CharacterBuff.PushBuff(useCharacter, buff);
        else
            throw new InvalidCastException($"SkillOrBuff is invalid");
    }
    protected void OnHpValueChagned(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        _hpbar.Hp = stat.Value;
        
        if (!IsDead && currentValue < prevValue)
            OnHpDecrease?.Invoke(HpPercent);
        
        if (!IsDead && currentValue > prevValue)
            OnHpIncrease?.Invoke(HpPercent);

        IsDead = currentValue <= 0;

        if (IsDead)
            Dead();
    }
    protected void OnHpMaxChanged(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        _hpbar?.Initialize(Stats.MaxStatValue(stat), Stats.sequenceStat ? Stats.sequenceStat.MaxValue : 0);
    }
    protected void OnShieldValueChagned(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        _hpbar.Shield = currentValue;

        if (currentValue > 0)
        {
            if (!_shieldPool)
            {
                _shieldPool = Managers.Resources.Instantiate<Poolable>(_shieldPath);
                Attach(_shieldPool, BodyBoneTr);
                _shieldPool.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            if (_shieldPool)
                _shieldPool.Destroy();

            _shieldPool = null;
        }
    }
    protected void OnSequenceValueChagned(Stat stat, BBNumber currentValue, BBNumber prevValue)
    {
        _hpbar.Sequence = currentValue;
    }
    protected void EffectSpawn(string prefabPath, string boneName, SpineStringPositionType positionStr)
    {
        if (string.IsNullOrEmpty(prefabPath))
            return;
        
        Transform boneTr = _boneTrDics[boneName];
        Poolable prefabPool = null;

        switch (positionStr)
        {
            case SpineStringPositionType.nr:
                prefabPool = Managers.Resources.Instantiate<Poolable>(prefabPath);
                prefabPool.transform.localRotation = boneTr.localRotation;
                break;
            case SpineStringPositionType.p:
                prefabPool = Managers.Resources.Instantiate<Poolable>(prefabPath);
                if (prefabPool)
                {
                    AttachPosition attachPosition = prefabPool.gameObject.GetOrAddComponent<AttachPosition>();
                    attachPosition.Initialize(boneTr);
                }
                break;
            case SpineStringPositionType.pr:
                prefabPool = Managers.Resources.Instantiate<Poolable>(prefabPath, boneTr);
                break;
            default:
                prefabPool = Managers.Resources.Instantiate<Poolable>(prefabPath);
                prefabPool.transform.position = boneTr.position;
                break;
        }
        
        if(prefabPool)
            prefabPool.transform.position = boneTr.position;
        else
            UnityHelper.Error_H($"EffectSpawn Null Error prefabPath : {prefabPath}\nGameObject : {this.gameObject}");
    }
    public void Attach(Poolable pool, Transform parent)
    {
        if (pool == null)
        {
            UnityHelper.Error_H($"Attach Null Error pool : {pool}\nGameObject : {this.gameObject}");
            return;
        }

        if (!_attachPools.Contains(pool))
        {
            _attachPools.Add(pool);
            pool.OnDestroyAction += () => _attachPools.Remove(pool);
        }

        if (parent)
            pool.transform.SetParent(parent);
    }
    public void PushBuffStatTextSpawn(Stat stat)
    {
        if(_isBuffStatTextSpawn)
        {
            _buffStatTextSpawnJob.Push<Stat>(BuffStatTextSpawn, stat);
        }
        else
        {
            _isBuffStatTextSpawn = true;
            BuffStatTextSpawn(stat);
        }
    }
    private void BuffStatTextSpawn(Stat stat)
    {
        BuffText buffText = Managers.Resources.Instantiate<BuffText>(_buffStatTextPath);
        buffText.UISet(stat);
        
        Vector3 buffTextPos = transform.position;
        buffTextPos.y += BoxHeight;
        
        buffText.transform.position = buffTextPos;

        Managers.Tween.TweenInvoke(0.9f).SetOnComplete(() => {
            if (_buffStatTextSpawnJob.Count <= 0)
                _isBuffStatTextSpawn = false;
            else
                _buffStatTextSpawnJob.Pop().Execute();
        });
    }
    public virtual void Dead()
    {
        for (int i = 0; i < _attachPools.Count; i++)
        {
            if (_attachPools[i] && _attachPools[i].gameObject.activeSelf)
                Managers.Resources.Destroy(_attachPools[i].gameObject);
        }
        
        CharacterBuff.BuffBar.gameObject.SetActive(false);
        _hpbar.gameObject.SetActive(false);
        SetAnimation(DeathAnimationName, false);

        OnDead?.Invoke(this);
        OnDead = null;
    }
    protected void DeadEnd()
    {
        OnDeadEnd?.Invoke(this);
        OnDeadEnd = null;
        Destroy();
    }
    public void Destroy()
    {
        OnDestroy?.Invoke(this);
        OnDestroy = null;
        Managers.Resources.Destroy(this.gameObject);
    }
    void Setting()
    {
        _rigidbody2D.gravityScale = 0;
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        _boxCollider2D.isTrigger = true;
    }

    #if UNITY_EDITOR
    [Button]
    public void SetStat()
    {
        this.stats = Resources.LoadAll<Stat>(DefinePath.StatSOResourcesPath()).ToList();
        
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
    }
    [Button]
    public void PlusAttackSpeed()
    {
        Stats.GetStat("AttackSpeed").DefaultValue += 10;
    }
    [Button]
    public void PlusMoveSpeed()
    {
        Stats.GetStat("MoveSpeed").DefaultValue += 10;
    }
    #endif
}

public enum CharacterTeam
{
    Player,
    Enemy,
}

public enum EnemyType
{
    Normal,
    Elite,
    Boss
}

public struct CharacterApplyAttack
{
    public Character takeOwner;
    public SkillApplyDamageType damageType;
    public object cause;
    public float damageValue;
}

public enum CriticalType
{
    None,
    Critical,
    SuperCritical,
}