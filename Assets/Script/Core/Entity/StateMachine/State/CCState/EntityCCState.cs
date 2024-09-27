using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityCCState : State<Entity>
{
    public abstract string Description { get; }
    protected abstract string AnimationClipName { get; }

    public override void Enter()
    {
        Entity.Animator?.Play(AnimationClipName, true);
        Entity.Movement?.Stop();
        Entity.SkillSystem.CancelAll();

        var playerController = Entity.GetComponent<PlayerController>();
        if (playerController)
            playerController.enabled = false;
    }

    public override void Exit()
    {
        Entity.Animator?.Play(AnimationClipName, false);

        var playerController = Entity.GetComponent<PlayerController>();
        if (playerController)
            playerController.enabled = true;
    }
}
