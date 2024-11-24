using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Character _character;
    private SpineAniController _spineAniController;
    private Attack _attack;

    public float AttackRadius() => _attack.AttackRadius();
    
    public bool IsAttack => _attack.IsAttack;
    public void AttackAction(bool isLeft) => _attack.AttackAction(isLeft);
    public void EndAction() => _attack.EndAction();
    
    private bool _isInitialized = false;
    public void Initialize(Character character, SpineAniController spineAniController)
    {
        this._character = character;
        this._spineAniController = spineAniController;
        
        _attack = null;

        if (character.team.Equals(CharacterTeam.Player))
        {
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
        else if (character.team.Equals(CharacterTeam.Enemy))
        {
            _attack = new EnemyAttackNomal();
        }
        
        _attack?.Initialize(character, this.transform, spineAniController);
        
        _isInitialized = true;
    }

    private void FixedUpdate()
    {
        if (_isInitialized)
            _attack?.FixedUpdate();
    }
}