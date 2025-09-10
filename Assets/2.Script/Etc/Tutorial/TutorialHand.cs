using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private readonly string _clickAniName = "Click";
    private readonly string _pointDownAniName = "PointDown";
    private readonly string _pointUpAniName = "PointUp";

    private AniController _aniController;

    void Awake()
    {
        _aniController = animator.Initialize();
    }

    public void Click() => _aniController.SetTrigger(_clickAniName);
    public void PointDown() => _aniController.SetTrigger(_pointDownAniName);
    public void PointUp() => _aniController.SetTrigger(_pointUpAniName);
}
