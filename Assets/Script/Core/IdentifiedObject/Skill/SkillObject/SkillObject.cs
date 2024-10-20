using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkillObject : MonoBehaviour
{
    [SerializeField]
    private bool isDelayFirstApplyByCycle;
    [SerializeField]
    private bool isDelayDestroyByCycle;

    private float currentDuration;
    private float currentApplyCycle;
    private int currentApplyCount;

    private TargetSearcher targetSearcher;

    public Entity Owner { get; private set; }
    public Skill Spawner { get; private set; }
    public TargetSearcher TargetSearcher => targetSearcher;
    public Vector3 ObjectScale { get; private set; }
    public float Duration { get; private set; }
    public int ApplyCount { get; private set; }
    public float ApplyCycle { get; private set; }
    public float DestroyTime { get; private set; }

    private bool IsApplicable => (ApplyCount == 0 || currentApplyCount < ApplyCount) &&
        currentApplyCycle >= ApplyCycle;

    public void Setup(Skill spawner, TargetSearcher targetSearcher, float duration, int applyCount, Vector3 objectScale)
    {
        Spawner = spawner.Clone() as Skill;
        Owner = spawner.Owner;
        this.targetSearcher = new TargetSearcher(targetSearcher);
        ApplyCount = applyCount;
        Duration = duration;
        ObjectScale = objectScale;
        ApplyCycle = CalculateApplyCycle(duration, applyCount);
        DestroyTime = Duration + (isDelayDestroyByCycle ? ApplyCycle : 0f);

        currentDuration = 0;
        currentApplyCycle = 0;

        foreach (var component in GetComponents<ISkillObjectComponent>())
            component.OnSetupSkillObject(this);

        if (!isDelayFirstApplyByCycle)
            Apply();
    }

    private void OnDestroy()
    {
        Managers.Resources.Destroy(Spawner.GameObject());
    }

    private void FixedUpdate()
    {
        currentDuration += Managers.Time.FixedDeltaTime;
        currentApplyCycle += Managers.Time.FixedDeltaTime;

        if (IsApplicable)
            Apply();

        if (currentDuration >= DestroyTime)
        {
            Managers.Resources.Destroy(gameObject);
        }
    }

    public float CalculateApplyCycle(float duration, int applyCount)
    {
        if (applyCount == 1)
            return 0f;
        else
            return isDelayFirstApplyByCycle ? (duration / applyCount) : (duration / (applyCount - 1));
    }

    private void Apply()
    {
        targetSearcher.SelectImmediate(Owner, gameObject, transform.position);
        var result = targetSearcher.SearchTargets(Owner, gameObject);

        foreach (var target in result.targets)
        {
            target.GetComponent<SkillSystem>().Apply(Spawner);
            JobAction(Owner, target.GetComponent<Entity>());
        }

        currentApplyCount++;
        currentApplyCycle %= ApplyCycle;
    }
    void JobAction(Entity owner, Entity target)
    {
        if (target == null)
            return;

        PlayerController player = owner.GetComponent<PlayerController>();
        if (!player)
            return;

        player.TribeSkillAction(target);
    }
}