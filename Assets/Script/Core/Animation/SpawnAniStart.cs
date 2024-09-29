using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAniStart : MonoBehaviour
{
    [SerializeField] string startAniName;
    [SerializeField] bool isLoop;

    private void OnEnable()
    {
        AniStart();
    }

    void AniStart()
    {
        Animator animator = GetComponent<Animator>();
        SkeletonAnimation sa = GetComponent<SkeletonAnimation>();

        if (animator)
        {
            AniController aniController = animator.Initialize();
            aniController.anim.SetTrigger(startAniName);
        }
        else if (sa)
        {
            SpineAniController spineAniController = sa.Initialize();
            spineAniController.Play(startAniName,isLoop);
        }
    }
}
