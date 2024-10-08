using UnityEngine;

public class JobSkill : MonoBehaviour
{
    Entity owner;
    Entity target;

    [SerializeField] int ApplyCount;
    [SerializeField] float ApplyCycle;
    [SerializeField] Poolable EffectPrefab;
    [SerializeField] Effect effect;

    int applyCount = 0;
    float cooldownTimer = 0;
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
    public void Set(Entity owner, Entity target)
    {
        if (ApplyCount <= 0)
        {
            Release();
            return;
        }

        this.owner = owner;
        this.target = target;
        isApply = true;

        EffectPrefabSpawn();
        StartEffect();
        Apply();
    }
    void EffectPrefabSpawn()
    {
        if (ApplyCount <= 0 || ApplyCycle <= 0)
            return;

        if (target == null)
            return;

        UnityHelper.Log_H(123213);

        Poolable effectPoolable = Managers.Resources.Instantiate(EffectPrefab, target.transform);
        effectPoolable.transform.localPosition = Vector3.zero;
        float time = ApplyCount * ApplyCycle;
        effectPoolable.SetTime(time);
    }
    void FixedUpdate()
    {
        if (!isApply)
            return;

        InApply();
    }
    void Apply()
    {
        ApplyEffect();

        applyCount++;

        if (applyCount >= ApplyCount)
            Release();
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
        runningEffect = effect.Clone() as Effect;
        runningEffect.Setup(owner, owner, 1);
        runningEffect.SetTarget(target);
        runningEffect.Start();
    }
    void ApplyEffect()
    {
        if (runningEffect.IsApplicable)
            runningEffect.Apply();
    }
    void ReleaseEffect()
    {
        if (runningEffect.IsFinished)
        {
            runningEffect.Release();
            Destroy(runningEffect);
        }
    }
}
