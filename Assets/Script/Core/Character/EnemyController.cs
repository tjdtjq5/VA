using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(EntityMovement))]
[RequireComponent(typeof(EntityAnimator))]
[RequireComponent(typeof(EntityStateMachine))]
[RequireComponent(typeof(SkillSystem))]
public class EnemyController : MonoBehaviour
{
    private Entity _entity;
    private void Start()
    {
        _entity = GetComponent<Entity>();
    }
}
