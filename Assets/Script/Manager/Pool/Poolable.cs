using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    None,
    Time,
    Particle,
    Animator,
}

public class Poolable : MonoBehaviour
{
    public bool IsUsing;

    public PoolObjectType poolObjectType;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Time)]
    float time;

    // [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Particle)]
    ParticleSystem particle;

    Animator animator;
    BaseLayerBehaviour _stateMachine;
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Animator)]
    string endAniName;

    bool isDestroy = false;
    float destoryTime;
    float destoryTimer;

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
            case PoolObjectType.Animator:
                AnimatorDestroy();
                break;
        }
    }

    void Init()
    {
        switch (poolObjectType)
        {
            case PoolObjectType.Particle:
                particle = GetComponent<ParticleSystem>();
                break;
            case PoolObjectType.Animator:
                animator = GetComponent<Animator>();
                _stateMachine = animator.GetBehaviour<BaseLayerBehaviour>();
                break;
            case PoolObjectType.Time:
                destoryTime = 0;
                destoryTimer = 0;
                break;
        }

        isDestroy = false;
    }

    void TimeDestroy()
    {
        destoryTime = time;
        destoryTimer = 0;
        isDestroy = true;
    }
    void ParticleDestory()
    {
        float totalDuration = particle.main.duration + particle.main.startLifetimeMultiplier;
        destoryTime = totalDuration;
        destoryTimer = 0;
        isDestroy = true;
    }
    void AnimatorDestroy()
    {
        _stateMachine.onStateExit -= OnStateEnd;
        _stateMachine.onStateExit += OnStateEnd;
    }
    public void OnStateEnd(string aniName)
    {
        if (aniName.Equals(endAniName))
            Managers.Resources.Destroy(this.gameObject);
    }
    private void FixedUpdate()
    {
        if (isDestroy)
        {
            destoryTimer += Managers.Time.FixedDeltaTime;

            if (destoryTimer > destoryTime)
            {
                destoryTimer = 0;
                isDestroy = false;

                Managers.Resources.Destroy(this.gameObject);
            }
        }
    }
}
