using UnityEngine;

[System.Serializable]
public class InstantApplyEffectSpawnAction : SkillAction
{
    [SerializeField] private GameObject effectObj;
    [SerializeField] private SpawnTarget spawnTarget;
    [SerializeField] private bool isRotation;
    [SerializeField] private bool isOneEffect;
    private bool flag;

    public override void Start(Skill skill)
    {
        base.Release(skill);

        flag = false;
    }

    public override void Apply(Skill skill)
    {
        skill.TargetSearcher.SelectTarget(skill.Owner, skill.Owner.gameObject, (targetSearcher, result) =>
        {
            if (result.resultMessage == SearchResultMessage.FindTarget)
            {
                skill.SearchTargets();
                foreach (var target in skill.Targets)
                {
                    target.SkillSystem.Apply(skill);
                    JobAction(skill.Owner, target);
                    Transform st = spawnTarget == SpawnTarget.Target ? target.transform : skill.Owner.transform;
                    EffectSpawn(st, target.transform);
                }
            }
            else
                skill.Cancel();
        });
    }
   
    public override object Clone() => new InstantApplyEffectSpawnAction();

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
public enum SpawnTarget
{
    Owner,
    Target
}