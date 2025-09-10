using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.BBNumber;
using UnityEngine;

public class CharacterBuff : MonoBehaviour
{
    private Character _character;
    private List<Buff> _buffs = new();
    private List<TriggerPassiveBuff> _triggerPassiveBuffs = new();
    private BuffBar _buffBar;

    public BuffBar BuffBar => _buffBar;
    public List<Buff> Buffs => _buffs;
    
    private readonly string _barPrefabPath = "Prefab/Character/BuffBar";
    
    public void Initialize(Character character)
    {
        if (!_buffBar || _buffBar.gameObject.activeSelf == false)
        {
            _buffBar = Managers.Resources.Instantiate<BuffBar>(_barPrefabPath, character.transform);
        }

        _buffBar.transform.localPosition = new Vector3(0, character.BoxPosY - character.BoxHeight * 0.5f -1f, 0);
        
        this._character = character;

        character.OnTurnStart -= TurnStart;
        character.OnTurnEnd -= TurnEnd;
        character.OnCharacterActionStart -= MyTurnStart;
        character.OnCharacterActionEnd -= MyTurnEnd;
        character.OnTakeDamage -= TakeDamage;
        character.OnHpIncrease -= HpIncrease;
        character.OnHpDecrease -= HpDecrease;
        character.OnDead -= (Character)=> Clear();

        character.OnTurnStart += TurnStart;
        character.OnTurnEnd += TurnEnd;
        character.OnCharacterActionStart += MyTurnStart;
        character.OnCharacterActionEnd += MyTurnEnd;
        character.OnTakeDamage += TakeDamage;
        character.OnHpIncrease += HpIncrease;
        character.OnHpDecrease += HpDecrease;
        character.OnDead += (Character)=> Clear();
    }
    public Buff PushBuff(Character useCharacter, Buff buff)
    {
        List<Buff> buffs = _buffs.ToList();
        
        for (int i = 0; i < buffs.Count; i++)
        {
            if (buffs[i].CodeName.Equals(buff.CodeName))
            {
                buffs[i].Initialize(useCharacter, this._character);
                _buffBar.Push(buffs[i]);
                return buffs[i];
            }
        }
        
        Buff buffClone = (Buff)buff.Clone();
        buffClone.Initialize(useCharacter, this._character);
        _buffBar.Push(buffClone);
        
        _buffs.Add(buffClone);
        return buffClone;
    }
    public bool CheckBuff(string codeName)
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            if (_buffs[i].CodeName == codeName)
            {
                return true;
            }
        }

        return false;
    }
    public int GetBuffCount(string codeName)
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            if (_buffs[i].CodeName == codeName)
            {
                return _buffs[i].Count;
            }
        }

        return 0;
    }
    public void RemoveBuff(Buff buff)
    {
        BuffBar.Destroy(buff);
        buff.ForceEndBuff();
        buff.OnRemove?.Invoke(buff);
        _buffs.Remove(buff);
    }
    public void RemoveBuff(string codeName)
    {
        BuffBar.Destroy(codeName);
        
        Buff removeBuff = _buffs.Find(b => b.CodeName == codeName);

        if (removeBuff != null)
            RemoveBuff(removeBuff);
    }
    public void RemoveBuff(BuffBehaviour behaviour)
    {
        List<Buff> removeBuffs = new List<Buff>();
        for (int i = 0; i < _buffs.Count; i++)
        {
            if (_buffs[i].behaviour.GetType() == behaviour.GetType())
            {
                removeBuffs.Add(_buffs[i]);
            }
        }

        for (int i = 0; i < removeBuffs.Count; i++)
        {
            RemoveBuff(removeBuffs[i]);
        }
    }
    public int BuffCount(BuffBehaviour behaviour)
    {
        List<Buff> tempBuffs = _buffs.ToList();
        int total = 0;
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].behaviour.GetType() == behaviour.GetType())
            {
                total += tempBuffs[i].Count;
            }
        }
        
        return total;
    }
    private void TurnStart(int turn)
    {
        List<Buff> tempBuffs = _buffs.ToList();
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].StartTiming == BuffStartEndTiming.BattleTurn)
            {
                tempBuffs[i].StartBuff(turn);
            }
        }
    }

    private void TurnEnd(int turn)
    {
        List<Buff> tempBuffs = _buffs.ToList();

        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].EndTiming == BuffStartEndTiming.BattleTurn)
            {
                tempBuffs[i].EndBuff(turn);
            }
        }
    }

    private void MyTurnStart()
    {
        List<Buff> tempBuffs = _buffs.ToList();
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].StartTiming == BuffStartEndTiming.BattleCharacterTurn)
            {
                tempBuffs[i].StartBuff();
            }
        }
    }

    private void MyTurnEnd()
    {
        List<Buff> tempBuffs = _buffs.ToList();
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].EndTiming == BuffStartEndTiming.BattleCharacterTurn)
            {
                tempBuffs[i].EndBuff();
            }
        }
    }
    
    private void TakeDamage(BBNumber damage, object cause)
    {
        List<Buff> tempBuffs = _buffs.ToList();
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].StartTiming == BuffStartEndTiming.TakeDamage)
                tempBuffs[i].StartBuff(damage);
        }
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].EndTiming == BuffStartEndTiming.TakeDamage)
                tempBuffs[i].EndBuff(damage);
        }
    }
    
    private void HpIncrease(float value)
    {
        List<Buff> tempBuffs = _buffs.ToList();
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].StartTiming == BuffStartEndTiming.HpIncrease)
            {
                tempBuffs[i].StartBuff(value);
            }
        }
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].EndTiming == BuffStartEndTiming.HpIncrease)
            {
                tempBuffs[i].EndBuff(value);
            }
        }
    }
    private void HpDecrease(float value)
    {
        List<Buff> tempBuffs = _buffs.ToList();
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].StartTiming == BuffStartEndTiming.HpDecrease)
            {
                tempBuffs[i].StartBuff(value);
            }
        }
        
        for (int i = 0; i < tempBuffs.Count; i++)
        {
            if (tempBuffs[i].EndTiming == BuffStartEndTiming.HpDecrease)
            {
                tempBuffs[i].EndBuff(value);
            }
        }
    }
    
    public void PushTriggerPassiveBuff(TriggerPassiveBuff triggerPassiveBuff)
    {
        if (!_triggerPassiveBuffs.Contains(triggerPassiveBuff))
            _triggerPassiveBuffs.Add(triggerPassiveBuff);
    }
    public void RemoveTriggerPassiveBuff(TriggerPassiveBuff triggerPassiveBuff)
    {
        _triggerPassiveBuffs.Remove(triggerPassiveBuff);
    }
    public bool IsOnTriggerPassiveBuff(TriggerPassiveBuff triggerPassiveBuff) => _triggerPassiveBuffs.Contains(triggerPassiveBuff);

    private void BuffAllClear()
    {
        for (int i = 0; i < _buffs.Count; i++)
        {
            _buffBar?.Destroy(_buffs[i]);
            _buffs[i].Clear();
        }
    }
    public void Clear()
    {
        BuffAllClear();
    }
}

