using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttackMultipleAttack : EnemyAttack
{
    protected readonly List<string> _attackNames = new List<string>() { "At", "At_2"};

    public override void Initialize(Character character, Transform transform)
    {
        this.Character = character;
        this.Transform = transform;

        for (int i = 0; i < Character.SpineSpineAniControllers.Count; i++)
        {
            for(int j = 0; j < _attackNames.Count; j++)
            {
                string attackName = _attackNames[j];
                Character.SpineSpineAniControllers[i].SetEndFunc(attackName, AttackAniEnd);
                Character.SpineSpineAniControllers[i].SetEventFunc(attackName, ActionEvent , AttackEffect);
                Character.SpineSpineAniControllers[i].SetEventFunc(attackName, FxEvent , AttackEffect);
                Character.SpineSpineAniControllers[i].SetEventFunc(attackName, ActionEvent , AttackAniAction);
            }
        }
    }

    protected override void StartAction(Character target, object cause)
    {
        this._target = target;
        this.Cause = cause;

        int rIndex = (int)UnityHelper.Random_H(0, _attackNames.Count);
        string attackName = _attackNames[rIndex];

        SetAnimation(attackName, false);
    }
}
