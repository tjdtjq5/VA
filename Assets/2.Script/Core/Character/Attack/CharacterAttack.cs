using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public Action OnStart
    {
        get => attack.OnStart;
        set => attack.OnStart = value;
    }
    public Action OnEnd
    {
        get => attack.OnEnd;
        set => attack.OnEnd = value;
    }
    
    [SerializeField, SerializeReference] private Attack attack;
    private Character _character;
    
    private bool _isInitialized = false;
    
    public void Initialize(Character character)
    {
        this._character = character;

        if (attack == null)
        {
            attack = character.IsPlayer ? new PlayerAttack() : new EnemyAttack();
        }
        
        attack?.Initialize(character, this.transform);
        
        _isInitialized = true;
    }
    private void FixedUpdate()
    {
        if (_isInitialized)
        {
            attack?.FixedUpdate();
        }
    }

    public bool IsAttack { get => attack.IsAttack; set => attack.IsAttack = value; }
    public void SetAttack(List<Character> targets, object cause)
    {
        // attack.SetAttack(targets, cause);
        
        if (_character.IsCC)
        {
            Invoke(nameof(AttackCancleInvoke), 0.2f);
        }
        else
        {
            this._character.AddSequence();
            attack.SetAttack(targets, cause, false);
        }
    }
    public void Clear() => attack?.Clear();

    void AttackCancleInvoke()
    {
        OnEnd?.Invoke();
        OnEnd = null;
    }
}

