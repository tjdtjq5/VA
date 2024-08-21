using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public AniController AniController { get; private set; }
    private Entity entity;

    public readonly string deadClipName = "Dead";
    public readonly int kRunHash = UnityEngine.Animator.StringToHash("isRun");
    public readonly int kDeadHash = UnityEngine.Animator.StringToHash("isDead");
    public readonly int kDashHash = UnityEngine.Animator.StringToHash("isDash");
    public readonly int kIsStunningHash = UnityEngine.Animator.StringToHash("isStunning");
    public readonly int kIsSleepingHash = UnityEngine.Animator.StringToHash("isSleeping");


    public void Setup(Entity entity)
    {
        AniController = _animator.Initialize();

        this.entity = entity;

        AniController.SetEndFunc(deadClipName, OnDead);
    }

    public void OnDead(string aniName)
    {
        if (aniName.Equals(deadClipName))
            entity.Destroy();
    }
}
