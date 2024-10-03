using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject impactPrefab;

    private Entity owner;
    private Rigidbody rigidBody;
    private BoxCollider boxCollider;
    private float speed;
    private Skill skill;
    private Vector3 direction;

    public void Setup(Entity owner, float speed, Vector3 direction, Skill skill)
    {
        this.owner = owner;
        this.speed = speed;
        this.direction = direction;
        // 현재 Skill의 Level 정보를 저장하기 위해 Clone을 보관
        this.skill = skill.Clone() as Skill;
    }

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.useGravity = false;

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        this.gameObject.layer = 1;
    }

    private void OnDestroy()
    {
        Managers.Resources.Destroy(skill.GameObject());
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = direction * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Entity>() == owner)
            return;

        if (impactPrefab)
        {
            var impact = Managers.Resources.Instantiate(impactPrefab);
            impact.transform.position = transform.position;
        }

        var entity = other.GetComponent<Entity>();

        if (!entity)
            return;

        if (entity.IsDead)
            return;

        var hasCategory = owner.Categories.Any(x => entity.HasCategory(x));
        if (hasCategory)
            return;

        if (entity)
        {
            entity.SkillSystem.Apply(skill);
            Managers.Resources.Destroy(gameObject);
        }
    }
}
