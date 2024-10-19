using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour
{
    [SerializeField] SkeletonAnimation _animator;
    public SpineAniController AniController { get; private set; }
    private Entity entity;

    public readonly string attackClipName = "attack";
    public readonly string deadClipName = "dead";
    public readonly string skillClipName = "skill";
    public readonly string waitClipName = "wait";
    public readonly string walkClipName = "walk";
    public readonly string precedingClipName = "skill_casting";

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

    public void AniSpeed(float speed) => AniController.AniSpeed(speed);
    public void Play(string aniName, bool isLoop, bool isDupli = false, int index = 0)
    {
        if (entity.IsDead)
            return;

        AniController.Play(aniName, isLoop, isDupli, index);
    }
    public bool IsPlay(string aniName, int index = 0) => AniController.IsPlay(aniName, index);
}
