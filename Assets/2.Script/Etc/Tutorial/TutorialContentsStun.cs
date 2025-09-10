using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TutorialContentsStun : TutorialContents
{
    [SerializeField] SkeletonGraphic _skeletonGraphic;
    [SerializeField] Week _week;
    [SerializeField] ParticleSystem _stunEffect;

    private SpineAniController _spineAniController;

    private readonly string _idleAnimationName = "idle";
    private readonly string _hitAnimationName = "hit";
    private readonly string _faintAnimationName = "Fainting";

    public override void Initialize()
    {
        _spineAniController = _skeletonGraphic.Initialize();
    }

    public override void Set(int index)
    {
        base.Set(index);
        Clear();

        switch (Index)
        {
            case 0:
                _spineAniController.Play(_faintAnimationName, true);
                _stunEffect.Play();
                _week.Faint();
                break;
        }
    }

    public override void Clear()
    {
        _stunEffect.Stop();
    }
}
