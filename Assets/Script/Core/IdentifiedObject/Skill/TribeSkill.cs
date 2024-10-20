using System.Collections.Generic;
using UnityEngine;

public class TribeSkill : MonoBehaviour
{
    Tribe tribe;
    Entity owner;
    Entity target;

    string EffectObjectPathFormat = "Effect/EFFECT_Tribe_{0}";
    string effectObjectPath;
    Effect mainEffect;
    Effect otherEffect;

    float otherRadius = 3f;

    public void Set(Tribe job, Entity owner)
    {
        this.tribe = job;
        this.owner = owner;

        effectObjectPath = CSharpHelper.Format_H(EffectObjectPathFormat, job);
    }

    public void Apply(Entity target)
    {
        if (mainEffect)
            mainEffect.Release();
     
        this.target = target;

        ApplyEffect();
    }

    void OnApply(Effect effect, int currentApplyCount, int prevApplyCount)
    {
        OtherApply(effect);
    }
    void OnRelease(Effect effect)
    {
        Destroy(mainEffect);
        mainEffect = null;
    }
    void ApplyEffect()
    {
        mainEffect = Managers.Resources.Load<Effect>(effectObjectPath).Clone() as Effect;
        mainEffect.Setup(owner, owner, 1);

        mainEffect.onApplied += OnApply;
        mainEffect.onReleased += OnRelease;

        mainEffect.SetTarget(target);
        mainEffect.Apply();
    }
    void OtherApply(Effect effect)
    {
        if (effect.MaxLevel < 2)
            return;

        List<Entity> otherTargets = GameFunction.SearchTargets(target, otherRadius, true);
        for (int i = 0; i < otherTargets.Count; i++)
        {
            OtherApplyEffect(otherTargets[i]);
        }
    }
    void OtherApplyEffect(Entity otherTarget)
    {
        otherEffect = Managers.Resources.Load<Effect>(effectObjectPath).Clone() as Effect;
        otherEffect.Setup(owner, owner, 2);

        otherEffect.onApplied += OnOtherApply;
        otherEffect.onReleased += OnOtherRelease;

        otherEffect.SetTarget(otherTarget);
        otherEffect.Apply();
    }
    void OnOtherApply(Effect effect, int currentApplyCount, int prevApplyCount)
    {
    }
    void OnOtherRelease(Effect effect)
    {
        Destroy(otherEffect);
        otherEffect = null;
    }
    private void FixedUpdate()
    {
        if (mainEffect && !mainEffect.IsReleased)
        {
            mainEffect.FixedUpdate();
        }
        if (otherEffect && !otherEffect.IsReleased)
        {
            otherEffect.FixedUpdate();
        }
    }

    private void OnDisable()
    {
        if (mainEffect)
            mainEffect.Release();

        if (otherEffect)
            otherEffect.Release();
    }
}
