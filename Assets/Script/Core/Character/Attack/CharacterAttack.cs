using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Character _character;
    private SpineAniController _spineAniController;
    private Attack _attack;

    [Range(0f, 10f)] public float AttackLength;
    [Range(0f, 10f)] public float MoveRadiusAttack1;
    [Range(0f, 10f)] public float MoveRadiusAttack2;
    [Range(0f, 10f)] public float MoveRadiusAttack3;
    [Range(0f, 10f)] public float MoveRadiusAttack4;


    public bool IsAttack => _attack.IsAttack;
    public void AttackAction(bool isLeft) => _attack.AttackAction(isLeft);
    
    public void Initialize(Character character, SpineAniController spineAniController)
    {
        this._character = character;
        this._spineAniController = spineAniController;
        
        _attack = null;
        switch (character.control)
        {
            case CharacterControllType.Input:
                _attack = new InputAttack();
                break;
            case CharacterControllType.UI:
                _attack = new UIAttack();
                break;
            case CharacterControllType.AI:
                _attack = new AIAttack();
                break;
        }
    }
}
