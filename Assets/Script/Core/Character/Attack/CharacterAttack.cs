using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    private Character _character;
    private SpineAniController _spineAniController;
    public Attack Attack;

    public float AttackRadius() => Attack.AttackRadius();
    
    public bool IsAttack => Attack.IsAttack;
    public void AttackAction(bool isLeft) => Attack.AttackAction(isLeft);
    public void Clear() => Attack.Clear();
    
    private bool _isInitialized = false;
    public void Initialize(Character character, SpineAniController spineAniController)
    {
        this._character = character;
        this._spineAniController = spineAniController;
        
        Attack = null;

        if (character.team.Equals(CharacterTeam.Player))
        {
            switch (character.control)
            {
                case CharacterControllType.Input:
                    Attack = new InputAttack();
                    break;
                case CharacterControllType.UI:
                    Attack = new UIAttack();
                    break;
                case CharacterControllType.AI:
                    Attack = new AIAttack();
                    break;
            }
        }
        else if (character.team.Equals(CharacterTeam.Enemy))
        {
            Attack = new EnemyAttackNomal();
        }
        
        Attack?.Initialize(character, this.transform, spineAniController);
        
        _isInitialized = true;
    }

    private void FixedUpdate()
    {
        if (_isInitialized)
            Attack?.FixedUpdate();
    }
}