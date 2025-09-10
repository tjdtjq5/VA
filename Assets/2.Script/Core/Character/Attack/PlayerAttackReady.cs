using System;
using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Spine.Unity;
using UnityEngine;

public class PlayerAttackReady : MonoBehaviour
{
    public AttackGrade CurrentGrade { get => _currentGrade; set => _currentGrade = value; }

    private Player _player;

    private readonly string _effectPrefab = "Prefab/Effect/Skill/Weapon/Ready_{0}_{1}";
    private readonly List<string> _readyAniNames = new List<string>()
        { "Idle_1", "Idle_2", "Idle_3", "Idle_4" };
    
    private AttackGrade _currentGrade = AttackGrade.Oui;
    private Poolable _effectPool;
    
    public void Initialize(Player player)
    {
        this._player = player;
    }

    public void SetReady(PuzzleType puzzleType, AttackGrade attackGrade)
    {
        this._currentGrade = attackGrade;

        _player.SetAnimation(_readyAniNames[(int)attackGrade], true);
        
        RemoveEffect();

        if (attackGrade == AttackGrade.Focus || attackGrade == AttackGrade.Fatal || attackGrade == AttackGrade.Oui)
        {
            _effectPool = Managers.Resources.Instantiate<Poolable>(CSharpHelper.Format_H(_effectPrefab, puzzleType.ToString(), attackGrade.ToString()));
        }

        if (_effectPool != null)
            _effectPool.transform.position = _player.transform.position;
    }
    public void SetReady()
    {
        this._currentGrade = AttackGrade.Basic;
        _player.SetAnimation(_readyAniNames[(int)_currentGrade], true);
        RemoveEffect();
    }
    public void End()
    {
        RemoveEffect();
    }

    private void RemoveEffect()
    {
        if (_effectPool)
            Managers.Resources.Destroy(_effectPool.gameObject);
    }

    public void Clear()
    {
        
    }
}
