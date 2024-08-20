using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public AniController AniController { get; private set; }
    private Entity entity;
    private MoveController moveController;
    private EntityMovement entityMovement;

    private readonly static int kSpeedHash = UnityEngine.Animator.StringToHash("speed");
    private readonly static int kDeadHash = UnityEngine.Animator.StringToHash("isDead");
    private readonly static int kDashHash = UnityEngine.Animator.StringToHash("isDash");
    private readonly static int kIsStunningHash = UnityEngine.Animator.StringToHash("isStunning");
    private readonly static int kIsSleepingHash = UnityEngine.Animator.StringToHash("isSleeping");


    public void Setup(Entity entity)
    {
        AniController = _animator.Initialize();

        this.entity = entity;

        entityMovement = this.entity?.Movement;
        moveController = entityMovement?.MoveController;

        AniController.OnAnimationComplete = OnAniEnd;
        AniController.OnAnimationComplete += OnAniEnd;
    }
    void FixedUpdate()
    {
        if (AniController.anim)
        {
            AniController.SetBool(kDeadHash, entity.IsDead);

            if (!entity.IsDead)
            {
                if (entityMovement)
                    AniController.SetBool(kDashHash, entityMovement.IsDashing);

                if (moveController)
                    AniController.SetFloat(kSpeedHash, moveController.Weight);

                AniController.SetBool(kIsStunningHash, entity.IsInState<StunningState>());
                AniController.SetBool(kIsSleepingHash, entity.IsInState<SleepingState>());
            }
        }
    }

    public void OnAniEnd(string aniName)
    {
        if (aniName.Equals("Dead"))
            entity.Destroy();
    }
}
