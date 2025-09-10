using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TutorialContentsWeek : TutorialContents
{
    [SerializeField] SkeletonGraphic _skeletonGraphic;
    [SerializeField] Week _week;
    [SerializeField] ParticleSystem _stunEffect;

    private SpineAniController _spineAniController;

    private readonly string _idleAnimationName = "Idle";
    private readonly string _hitAnimationName = "Hit";
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
                _spineAniController.Play(_idleAnimationName, true);
                _week.UISet(PuzzleType.Red);
                break;
            case 1:
                _spineAniController.Play(_hitAnimationName, false);
                _week.Crash();
                break;
            case 2:
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
