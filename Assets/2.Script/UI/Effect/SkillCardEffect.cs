using System;
using System.Collections;
using System.Collections.Generic;
using Shared.Enums;
using UnityEngine;

public class SkillCardEffect : MonoBehaviour
{
    private void Awake()
    {
        _aniController = animator.Initialize();
        _rectTransform = GetComponent<RectTransform>();
    }

    [SerializeField] Animator animator;

    private AniController _aniController;
    private RectTransform _rectTransform;
    
    private readonly int _noneHash = Animator.StringToHash("None");
    private readonly int _dHash = Animator.StringToHash("D");
    private readonly int _cHash = Animator.StringToHash("C");
    private readonly int _bHash = Animator.StringToHash("B");
    private readonly int _aHash = Animator.StringToHash("A");
    private readonly int _sHash = Animator.StringToHash("S");
    private readonly int _ssHash = Animator.StringToHash("SS");
    private readonly int _sssHash = Animator.StringToHash("SSS");
    
    public void UISet(Grade grade, Vector2 sizeDelta)
    {
        switch (grade)
        {
            // case Grade.D:
            //     _aniController.SetTrigger(_dHash);
            //     break;
            case Grade.A:
                _aniController.SetTrigger(_aHash);
                break;
            case Grade.S:
                _aniController.SetTrigger(_sHash);
                break;
            case Grade.SS:
                _aniController.SetTrigger(_ssHash);
                break;
            case Grade.SSS:
                _aniController.SetTrigger(_sssHash);
                break;
            default:
                _aniController.SetTrigger(_noneHash);
                break;
        }

        _rectTransform.sizeDelta = sizeDelta;
    }
}
