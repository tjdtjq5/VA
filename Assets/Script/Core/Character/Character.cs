using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SpineAniController))]
[RequireComponent(typeof(CharacterMove))]
public class Character : MonoBehaviour
{
    SpineAniController _spineAniController;
    CharacterMove _characterMove;
    
    [SerializeField] SkeletonAnimation skeletonAnimation;

    private void Start()
    {
        _spineAniController = this.GetComponent<SpineAniController>();
        _spineAniController.Initialize(skeletonAnimation);
        
        _characterMove = this.GetComponent<CharacterMove>();
        _characterMove?.Initialize(this, _spineAniController);

        Test();
    }

    void Test()
    {
        CameraController cc = FindObjectOfType<CameraController>();
        cc.Initialize(this.transform);
    }
}
