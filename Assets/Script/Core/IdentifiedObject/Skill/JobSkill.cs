using System.Collections.Generic;
using UnityEngine;

public class JobSkill : MonoBehaviour
{
    CharacterJob job;
    Entity owner;
    Entity target;

    [SerializeField] int ApplyCount;
    [SerializeField] float ApplyCycle;
    [SerializeField] Poolable EffectPrefab;
    [SerializeField] Effect effect;

    int applyCount = 0;
    float cooldownTimer = 0;
    float otherRadius = 2.5f;
    Effect runningEffect;
    bool isApply = false;

    void Awake()
    {
        Initialize();
    }
    void Initialize() { }
    private void OnDisable()
    {
        this.owner = null;
        this.target = null;
        this.cooldownTimer = 0;
        isApply = false;
    }
    public void Set(CharacterJob job, Entity owner, Entity target)
    {
        if (ApplyCount <= 0)
        {
            Release();
            return;
        }

        this.job = job;
        this.owner = owner;
        this.target = target;
        isApply = true;

        EffectPrefabSpawn(target);
        StartEffect();
        Apply();
    }
    void EffectPrefabSpawn(Entity target)
    {
        if (ApplyCount <= 0 || ApplyCycle <= 0)
            return;

        if (target == null)
            return;

        Poolable effectPoolable = Managers.Resources.Instantiate(EffectPrefab);
        effectPoolable.transform.localPosition = target.transform.position;
    }
    void FixedUpdate()
    {
        if (!isApply)
            return;

        InApply();
    }
    void Apply()
    {
        ApplyEffect(target);
       // OtherApply();

        applyCount++;

        if (applyCount >= ApplyCount)
            Release();
    }
    void OtherApply()
    {
        if (job == CharacterJob.Dragon || job == CharacterJob.Robot)
        {
            List<Entity> otherTargets = GameFunction.SearchTargets(target, otherRadius);
            for (int i = 0; i < otherTargets.Count; i++)
            {
                ApplyEffect(otherTargets[i]);
                EffectPrefabSpawn(otherTargets[i]);
            }
        }
    }
    void InApply() 
    {
        cooldownTimer += Managers.Time.FixedDeltaTime;
        if (cooldownTimer > ApplyCycle)
        {
            Apply();
            cooldownTimer = 0;
        }
    }
    void Release()
    {
        isApply = false;
        ReleaseEffect();
        Managers.Resources.Destroy(this.gameObject);
    }

    void StartEffect()
    {
        if (runningEffect != null)
            ReleaseEffect();

        runningEffect = effect.Clone() as Effect;
        runningEffect.Setup(owner, owner, 1); 
    }
    void ApplyEffect(Entity target)
    {
        runningEffect.SetTarget(target);
     //   runningEffect.Start();

        if (runningEffect.IsApplicable)
            runningEffect.Apply();
    }
    void ReleaseEffect()
    {
        runningEffect.Release();
        Destroy(runningEffect);
    }
}
