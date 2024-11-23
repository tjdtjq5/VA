using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SubclassSelector, SerializeReference] public Move move;

    private bool _isInitialized = false;
    
    public void Initialize(Character character, SpineAniController spineAniController)
    {
        move.Initialize(this.transform, spineAniController);

        _isInitialized = true;
    }
    
    private void FixedUpdate()
    {
        if (_isInitialized)
        {
            move.FixedUpdate();
        }
    }
}
