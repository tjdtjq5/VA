using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    private Move _move;
    private bool _isInitialized = false;
    
    public void Initialize(Character character, SpineAniController spineAniController)
    {
        _move = null;
        switch (character.control)
        {
            case CharacterControllType.Input:
                _move = new InputMove();
                break;
            case CharacterControllType.UI:
                _move = new UIMove();
                break;
            case CharacterControllType.AI:
                _move = new AIMove();
                break;
        }
        
        _move.Initialize(character, this.transform, spineAniController);

        _isInitialized = true;
    }
    
    private void FixedUpdate()
    {
        if (_isInitialized)
        {
            _move.FixedUpdate();
        }
    }
}
