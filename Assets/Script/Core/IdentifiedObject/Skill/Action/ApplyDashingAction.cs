using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ApplyDashingAction : SkillAction
{
    public DashActionType dashActionType;

    public float distance;

    [SerializeField, Min(0.1f)]
    float speed;

    [SerializeField]
    private string clipName;

    [SerializeField] private GameObject effectObj;
    [SerializeField] private SpawnTarget spawnTarget;
    [SerializeField] private bool isRotation;
    [SerializeField] private bool isOneEffect;

    [SerializeField]
    private bool isTargetDuplicateAllow;

    List<Transform> dashTargetList = new List<Transform>();

    private bool flag;
    public override void Start(Skill skill)
    {
        dashTargetList.Clear();
        flag = false;
    }

    public override void Apply(Skill skill)
    {
        skill.TargetSearcher.SelectTarget(skill.Owner, skill.Owner.gameObject, (targetSearcher, result) =>
        {
            if (result.resultMessage == SearchResultMessage.FindTarget)
            {
                if (!isTargetDuplicateAllow && dashTargetList.Contains(result.selectedTarget.transform))
                {
                    skill.Cancel();
                    return;
                }

                dashTargetList.Add(result.selectedTarget.transform);

                float dist = dashActionType == DashActionType.Distance ? distance : skill.Owner.transform.position.GetDistance(result.selectedTarget.transform.position);

                skill.Owner.Movement.Dash(dist, skill.Owner.transform.position.GetDirection(result.selectedTarget.transform.position), speed, clipName, ()=> { DashCallback(skill); });
            }
            else
                skill.Cancel();
        });
    }
    public override object Clone() => new ApplyDashingAction();

    void DashCallback(Skill skill)
    {
        skill.SearchTargets();
        foreach (var target in skill.Targets)
        {
            target.SkillSystem.Apply(skill);
            Transform st = spawnTarget == SpawnTarget.Target ? target.transform : skill.Owner.transform;
            EffectSpawn(st, target.transform);
        }

        if (skill.ApplyCount > skill.CurrentApplyCount)
            skill.Apply();
    }
    void EffectSpawn(Transform spawnTarget, Transform target)
    {
        if (isOneEffect && flag)
            return;

        flag = true;

        if (effectObj)
        {
            Transform spTr = Managers.Resources.Instantiate(effectObj).transform;
            spTr.position = spawnTarget.transform.position;

            if (isRotation)
                spTr.LookAt_H(target);
        }
    }
}
public enum DashActionType
{
    Distance,
    Target,
}
