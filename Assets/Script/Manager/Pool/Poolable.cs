using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public enum PoolObjectType
{
    None,
    Time,
    Particle,
}

public class Poolable : MonoBehaviour
{
    public bool IsUsing;

    public PoolObjectType poolObjectType;

    [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Time)]
    float time;

    // [SerializeField, ShowWhen("poolObjectType", PoolObjectType.Particle)]
    ParticleSystem particle;

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
        particle = GetComponent<ParticleSystem>();

        isDestroy = false;
        destoryTime = 0;
        destoryTimer = 0;
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
