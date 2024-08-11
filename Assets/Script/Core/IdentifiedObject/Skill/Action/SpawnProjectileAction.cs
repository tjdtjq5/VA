using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnProjectileAction : SkillAction
{
    [SerializeField]
    private string projectilePrefabPath;
    [SerializeField]
    private string spawnPointSocketName;
    [SerializeField]
    private float speed;

    public override void Apply(Skill skill)
    {
        var socket = skill.Owner.GetTransformSocket(spawnPointSocketName);
        var projectile = Managers.Resources.Instantiate(projectilePrefabPath);
        projectile.transform.position = socket.position;
        projectile.GetComponent<Projectile>().Setup(skill.Owner, speed, socket.forward, skill);
    }

    public override object Clone()
    {
        return new SpawnProjectileAction()
        {
            projectilePrefabPath = projectilePrefabPath,
            spawnPointSocketName = spawnPointSocketName,
            speed = speed
        };
    }
}
