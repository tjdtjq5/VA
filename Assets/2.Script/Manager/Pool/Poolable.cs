using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public enum PoolObjectType
{
    None,
    Time,
    Particle,
    Animator,
    SpineAni,
    SpineAniGraphic,
}

public class Poolable : MonoBehaviour, ITime
{
    public bool IsUsing { get; set; }
    public Action OnDestroyAction;

    [SerializeField] private bool isUnTime;
    
    public ParticleSystem Particle => _particle;
    public AniController AniController => _aniController;
    public SpineAniController SpineAniController => _spineAniController;
    public SpineAniController GraphicAniController => _graphicAniController;

    public PoolObjectType poolObjectType;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Time)]
    float time;

    private ParticleSystem _particle;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Animator)]
    protected Animator animator;
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Animator)]
    string endAniName;

    private AniController _aniController;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.SpineAni)]
    SkeletonAnimation spineAnimation;
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.SpineAni)]
    string spineEndAniName;

    private SpineAniController _spineAniController;
    
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.SpineAniGraphic)]
    SkeletonGraphic graphicAnimation;
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.SpineAniGraphic)]
    string graphicEndAniName;

    private SpineAniController _graphicAniController;

    private bool _isDestroy = false;
    private float _destoryTime;
    private float _destoryTimer;

    public void SetTime(float time)
    {
        if (poolObjectType != PoolObjectType.Time)
            return;

        this.time = time;

        Init();
        TimeDestroy();
    }

    private void OnEnable()
    {
        Init();
        
        switch (poolObjectType)
        {
            case PoolObjectType.Time:
                TimeDestroy();
                break;
            case PoolObjectType.Particle:
                ParticleDestory();
                break;
        }
        
        if(!isUnTime)
            Managers.Time.TimeAdd(this);
    }
    private void OnDisable()
    {
        if(!isUnTime)
            Managers.Time.TimeRemove(this);
    }
    
    void Init()
    {
        switch (poolObjectType)
        {
            case PoolObjectType.Particle:
                _particle = GetComponent<ParticleSystem>();
                ParticleLifeTimeCheck();
                break;
            case PoolObjectType.Animator:
                _aniController = animator.Initialize();
                _aniController.SetEndFunc(endAniName, OnResourcesDestroyAnimation);
                break;
            case PoolObjectType.Time:
                _destoryTime = 0;
                _destoryTimer = 0;
                break;
            case PoolObjectType.SpineAni:
                _spineAniController = spineAnimation.Initialize();
                _spineAniController.SetEndFunc(spineEndAniName, OnResourcesDestroySpineAnimation);
                break;
            case PoolObjectType.SpineAniGraphic:
                _graphicAniController = graphicAnimation.Initialize();
                _graphicAniController.SetEndFunc(graphicEndAniName, OnResourcesDestroySpineAnimation);
                break;
        }

        _isDestroy = false;
    }

    public void Play()
    {
        switch (poolObjectType)
        {
            case PoolObjectType.Particle:
                _particle.Play();
                break;
            case PoolObjectType.Animator:
                _aniController.SetTrigger(endAniName);
                break;
            case PoolObjectType.SpineAni:
                _spineAniController.Play(spineEndAniName, false, true);
                break;
            case PoolObjectType.SpineAniGraphic:
                _graphicAniController.Play(graphicEndAniName, false, true);
                break;
        }
    }

    void TimeDestroy()
    {
        _destoryTime = time;
        _destoryTimer = 0;
        _isDestroy = true;
    }
    void ParticleDestory()
    {
        float totalDuration = _particle.main.duration + _particle.main.startLifetimeMultiplier;
        
        _destoryTime = totalDuration;
        _destoryTimer = 0;
        _isDestroy = true;
    }
    void OnResourcesDestroyAnimation(string aniName)
    {
        if (aniName.Equals(endAniName))
            Destroy();
    }
    void OnResourcesDestroySpineAnimation()
    {
        Destroy();
    }
    private void FixedUpdate()
    {
        if (_isDestroy)
        {
            _destoryTimer += isUnTime ? Managers.Time.UnscaledTime : Managers.Time.FixedDeltaTime;

            if (_destoryTimer > _destoryTime)
            {
                _destoryTimer = 0;
                _isDestroy = false;

                Destroy();
            }
        }
    }
    public void Destroy()
    {
        OnDestroyAction?.Invoke();
        OnDestroyAction = null;
        
        Managers.Resources.Destroy(this.gameObject);
    }
    void ParticleLifeTimeCheck()
    {
        // 현재 오브젝트의 파티클 시스템을 가져옵니다
        _particle = GetComponent<ParticleSystem>();
        if (!_particle)
            return;

        float totalDuration = 0;
        float totalStartLifetimeMultiplier = 0;
        
        // 자식 오브젝트들의 파티클 시스템을 검사하여 가장 긴 지속시간을 찾습니다
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Transform child = this.transform.GetChild(i);
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            if (ps)
            {
                // 각 파티클의 지속시간과 수명 중 최대값을 저장
                totalDuration = Mathf.Max(ps.main.duration, totalDuration);
                totalStartLifetimeMultiplier = Mathf.Max(ps.main.startLifetimeMultiplier, totalStartLifetimeMultiplier);
            }
        }
        
        // 메인 파티클 시스템을 일시 정지
        if (Application.isPlaying)
            _particle.Stop(); 
        
        var main = _particle.main;
        
        // 찾은 최대 지속시간과 수명을 메인 파티클에 적용
        main.duration = totalDuration;
        main.startLifetimeMultiplier = totalStartLifetimeMultiplier;

        // 파티클 시스템 재시작
        _particle.Play(); 
    }

    public void TimeScale(float value)
    {
        switch (poolObjectType)
        {
            case PoolObjectType.Time:
                if (value <= 0)
                    _destoryTime = Mathf.Infinity;
                else
                    _destoryTime = time / value;
                break;
            case PoolObjectType.Particle:
                float totalDuration = _particle.main.duration + _particle.main.startLifetimeMultiplier;
                if (value <= 0)
                    _destoryTime = Mathf.Infinity;
                else
                    _destoryTime = totalDuration / value;
                _particle.time = value;
                break;
        }
    }
    public void TimeUnScale()
    {
        Managers.Time.TimeRemove(this);
        
        switch (poolObjectType)
        {
            case PoolObjectType.Time:
                _destoryTime = time;
                break;
            case PoolObjectType.Particle:
                float totalDuration = _particle.main.duration + _particle.main.startLifetimeMultiplier;
                _destoryTime = totalDuration;
                _particle.time = 1;
                break;
        }
    }
    
    #if UNITY_EDITOR
    [Button]
    public void DebugDestroyTime()
    {
        UnityHelper.Log_H($"PoolObjectType : {poolObjectType}");
        float totalDuration = 0;

        switch (poolObjectType)
        {
            case PoolObjectType.Time:
                totalDuration = time;
                break;
            case PoolObjectType.Particle:
                ParticleLifeTimeCheck();
                totalDuration = _particle.main.duration + _particle.main.startLifetimeMultiplier;
                break;
            case PoolObjectType.Animator:
                break;
            case PoolObjectType.SpineAni:
                break;
            case PoolObjectType.SpineAniGraphic:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        UnityHelper.Log_H($"DestroyTime : {totalDuration} Seconds");
    }
    #endif
}
