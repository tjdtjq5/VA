using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProjectileAction : SkillAction
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private string spawnPointSocketName;
    [SerializeField]
    private float speed;

    public override void Apply(Skill skill)
    {
        var socket = skill.Owner.GetTransformSocket(spawnPointSocketName);
        var projectile = Managers.Resources.Instantiate(projectilePrefab);
        projectile.transform.position = socket.position;
        Vector3 direction = (skill.Targets[0].transform.position - skill.Owner.transform.position).normalized;
        projectile.GetComponent<Projectile>().Setup(skill.Owner, speed, direction, skill);
    }

    public override object Clone()
    {
        return new SpawnProjectileAction()
        {
            projectilePrefab = projectilePrefab,
            spawnPointSocketName = spawnPointSocketName,
            speed = speed
        };
    }
}
