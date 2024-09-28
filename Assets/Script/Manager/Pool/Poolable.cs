using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PoolObjectType
{
    None,
    Time,
    Particle,
    Animator,
    SpineAni,
}

public class Poolable : MonoBehaviour
{
    public bool IsUsing;

    public PoolObjectType poolObjectType;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Time)]
    float time;

    ParticleSystem particle;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Animator)]
    Animator animator;
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Animator)]
    string endAniName;
    AniController aniController;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.SpineAni)]
    SkeletonAnimation spineAnimation;
    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.SpineAni)]
    string spineEndAniName;
    SpineAniController spineAniController;

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
                aniController = animator.Initialize();
                aniController.SetEndFunc(endAniName, OnResourcesDestroy);
                break;
            case PoolObjectType.Time:
                destoryTime = 0;
                destoryTimer = 0;
                break;
            case PoolObjectType.SpineAni:
                spineAniController = spineAnimation.Initialize();
                spineAniController.SetEndFunc(spineEndAniName, OnResourcesDestroy);
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

    public void OnResourcesDestroy(string aniName)
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
