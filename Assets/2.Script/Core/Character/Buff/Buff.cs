using System;
using System.Collections.Generic;
using Shared.Enums;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "IdentifiedObject/Buff")]
public class Buff : IdentifiedObject
{
    public Action<Buff> OnClear; // Buff Remove
    public Action<Buff> OnStart; // Battle Page or Turn Start
    public Action<Buff> OnRemove;
    public Action<int> OnCountChange;

    public bool IsUsed { get; set; }
    public bool IsStart { get; set; }
    public Grade buffGrade;
    public int Count
    {
        get => _count;
        set { _count = value; OnCountChange?.Invoke(value); }
    }
    public BuffStartEndTiming StartTiming => startTiming;
    public BuffStartEndTiming EndTiming => endTiming;

    public BuffCountType BuffCountType => buffCountType;
    public Sprite BuffIcon => buffIcon;
    
    private Character _useCharacter;
    private Character _takeCharacter;
    private object _cause;

    private int _count;
    
    [OdinSerialize, SerializeReference]
    public BuffBehaviour behaviour;

    [SerializeField] private Sprite buffIcon;
    [SerializeField] BuffCountType buffCountType;
    [SerializeField] private int addCount = 1;
    [SerializeField] private bool isNotRemove;
    [SerializeField] private bool isBattleEndClear = false;
    [SerializeField] private BuffStartEndTiming startTiming = BuffStartEndTiming.Instance;
    [SerializeField] BuffStartEndTiming endTiming = BuffStartEndTiming.None;

    [ShowIf("buffCountType", BuffCountType.Charge)] public int maxChargeCount = 0;
    [ShowIf("startTiming", BuffStartEndTiming.BattleTurn)] public int turnStart = 0;
    [ShowIf("endTiming", BuffStartEndTiming.BattleTurn)] public int turnEnd = 0;
    [ShowIf("startTiming", BuffStartEndTiming.StackInstance)] public int stackInstanceCountStart = 1;
    [ShowIf("endTiming", BuffStartEndTiming.StackInstance)] public int stackInstanceCountEnd = 1;
    [ShowIf("startTiming", BuffStartEndTiming.HpIncrease)] public float hpIncreaseValueStart;
    [ShowIf("startTiming", BuffStartEndTiming.HpDecrease)] public float hpDecreaseValueStart;
    [ShowIf("endTiming", BuffStartEndTiming.HpIncrease)] public float hpIncreaseValueEnd;
    [ShowIf("endTiming", BuffStartEndTiming.HpDecrease)] public float hpDecreaseValueEnd;

    public void Initialize(Character useCharacter, Character takeCharacter)
    {
        this.behaviour.Initialize(this);
        this._useCharacter = useCharacter;
        this._takeCharacter = takeCharacter;

        if (isBattleEndClear)
        {
            Managers.Observer.PuzzleBattleStateMachine.OnBattleEnd -= Clear;
            Managers.Observer.PuzzleBattleStateMachine.OnBattleEnd += Clear;
        }

        int ac = addCount;
        
        if (buffCountType == BuffCountType.Reset)
        {
            Count = ac;
        }
        
        if (buffCountType == BuffCountType.NotReset)
        {
            if (IsUsed)
                return;

            Count = ac;
        }
        
        if (buffCountType == BuffCountType.Charge)
        {
            if (IsUsed)
            {
                if (maxChargeCount > 0)
                {
                    Count = Mathf.Min(Count + ac, maxChargeCount);
                }
                else
                {
                    Count += ac;
                }
            }
            else
            {
                Count = ac;
            }
        }

        if (startTiming == BuffStartEndTiming.Instance)
        {
            StartBuff();
        }
        if (endTiming == BuffStartEndTiming.Instance)
        {
            EndBuff();
        }

        int sics = stackInstanceCountStart;
        int sice = stackInstanceCountEnd;

        if (behaviour.GetType() == typeof(DebuffFire) && useCharacter.IsOnTriggerPassiveBuff(TriggerPassiveBuff.InstantCombustion))
        {
            sics--;
            sice--;
        }
        
        if (behaviour.GetType() == typeof(DebuffFire) && useCharacter.IsOnTriggerPassiveBuff(TriggerPassiveBuff.InstantCombustion_P))
        {
            sics -= 2;
            sice -= 2;
        }
        
        if (startTiming == BuffStartEndTiming.StackInstance && Count >= sics)
        {
            StartBuff();
        }
        if (endTiming == BuffStartEndTiming.StackInstance && Count >= sice)
        {
            EndBuff();
        }
        
        IsUsed = true;
    }

    public void StartBuff(object cause = null)
    {
        if (IsStart)
            return;

        if (!IsTimingConditionCheck(cause, true))
            return;

        _cause = cause;
        
        if (Count > 0)
        {
            IsStart = true;
            behaviour.OnStart(_useCharacter, _takeCharacter, _cause);
            OnStart?.Invoke(this);
        }
    }
    public void EndBuff(object cause = null)
    {
        if (!IsTimingConditionCheck(cause, false))
            return;

        _cause = cause;

        Count--;

        if (Count <= 0)
        {
            if (isNotRemove)
            {
                Count = addCount;
            }
            else
            {
                IsStart = false;
                behaviour.OnEnd(_useCharacter, _takeCharacter, _cause);
                Clear();
            }
        }
    }

    public void ForceEndBuff()
    {
        if (!IsUsed)
            return;
        
        IsUsed = false;
        
        Count = 0;
        IsStart = false;
        behaviour.OnEnd(_useCharacter, _takeCharacter, _cause);
        Clear();
    }

    public bool IsTimingConditionCheck(object cause, bool isTimingStart)
    {
        if (isTimingStart)
        {
            if (startTiming == BuffStartEndTiming.BattleTurn)
            {
                int turn = (int)cause;
                
                return turnStart <= 0 || turnStart == turn;
            }

            if (startTiming == BuffStartEndTiming.HpIncrease)
            {
                float value = (float)cause;

                return hpIncreaseValueStart <= value * 100f;
            }

            if (startTiming == BuffStartEndTiming.HpDecrease)
            {
                float value = (float)cause;

                return hpDecreaseValueStart >= value * 100f;
            }
        }
        else
        {
            if (endTiming == BuffStartEndTiming.BattleTurn)
            {
                int turn = (int)cause;
                
                return turnEnd <= 0 || turnEnd == turn;
            }

            if (endTiming == BuffStartEndTiming.HpIncrease)
            {
                float value = (float)cause;

                return hpIncreaseValueEnd <= value * 100f;
            }

            if (endTiming == BuffStartEndTiming.HpDecrease)
            {
                float value = (float)cause;

                return hpDecreaseValueEnd >= value * 100f;
            }
        }

        return true;
    }

    public void Clear()
    {
        OnClear?.Invoke(this);
        
        IsUsed = false;
        
        OnClear = null;
        OnStart = null;
        OnCountChange = null;
        Count = 0;
        
        _useCharacter.CharacterBuff.RemoveBuff(this);
    }

    public Dictionary<string, string> StringsByKeyword(string preface)
    {
        var stringsByKeyword = new Dictionary<string, string>()
        {
            { $"{preface}AddCount", addCount.ToString() },
        };

        switch (startTiming)
        {
            case BuffStartEndTiming.Instance:
                break;
            case BuffStartEndTiming.BattleTurn:
                stringsByKeyword.Add($"{preface}TurnStart", turnStart.ToString());
                break;
            case BuffStartEndTiming.BattleCharacterTurn:
                break;
            case BuffStartEndTiming.TakeDamage:
                break;
            case BuffStartEndTiming.StackInstance:
                stringsByKeyword.Add($"{preface}StackInstanceCountStart", stackInstanceCountStart.ToString());
                break;
            case BuffStartEndTiming.HpIncrease:
                stringsByKeyword.Add($"{preface}HpIncreaseValueStart", hpIncreaseValueStart.ToString("###.#"));
                break;
            case BuffStartEndTiming.HpDecrease:
                stringsByKeyword.Add($"{preface}HpDecreaseValueStart", hpDecreaseValueStart.ToString("###.#"));
                break;
        }

        switch (endTiming)
        {
            case BuffStartEndTiming.Instance:
                break;
            case BuffStartEndTiming.BattleTurn:
                stringsByKeyword.Add($"{preface}TurnEnd", turnEnd.ToString());
                break;
            case BuffStartEndTiming.BattleCharacterTurn:
                break;
            case BuffStartEndTiming.TakeDamage:
                break;
            case BuffStartEndTiming.StackInstance:
                stringsByKeyword.Add($"{preface}StackInstanceCountEnd", stackInstanceCountEnd.ToString());
                break;
            case BuffStartEndTiming.HpIncrease:
                stringsByKeyword.Add($"{preface}HpIncreaseValueEnd", hpIncreaseValueEnd.ToString("###.#"));
                break;
            case BuffStartEndTiming.HpDecrease:
                stringsByKeyword.Add($"{preface}HpDecreaseValueEnd", hpDecreaseValueEnd.ToString("###.#"));
                break;
        }
        
        stringsByKeyword.AddRange(behaviour.StringsByKeyword($"{preface}b."));

        return stringsByKeyword;
    }
    public override string Description
    {
        get
        {
            string description = base.Description;

            var stringsByKeyword = new Dictionary<string, string>();
            stringsByKeyword.AddRange(StringsByKeyword(""));
            
            description = TextReplacer.Replace(description, stringsByKeyword);
            description = description.Replace("\\n", "\n");

            return description;
        }
    }

#if UNITY_EDITOR
    [Button]
    public void LogDescription()
    {
        var stringsByKeyword = StringsByKeyword("");

        foreach (var UPPER in stringsByKeyword)
        {
            UnityHelper.Log_H($"Key : {UPPER.Key}   Value : {UPPER.Value}");
        }
        
        UnityHelper.Log_H(Description);
    }
#endif
    
    public void Load(BuffSaveData saveData)
    {
        this.codeName = saveData.CodeName;
        this.IsUsed = saveData.IsUsed;
        this._count = saveData.Count;
    }
}

public enum BuffCountType
{
    Reset,
    NotReset,
    Charge,
}

public enum BuffStartEndTiming
{
    None,
    Instance,
    BattleTurn,
    BattleCharacterTurn,
    TakeDamage,
    StackInstance,
    HpIncrease,
    HpDecrease,
}

[System.Serializable]
public class BuffSaveData
{
    public string CodeName;
    public bool IsUsed;
    public int Count;

    public BuffSaveData(Buff buff)
    {
        if (buff == null)
        {
            this.CodeName = string.Empty;
            this.IsUsed = false;
            this.Count = 0;
            return;
        }
        
        this.CodeName = buff.CodeName;
        this.IsUsed = buff.IsUsed;
        this.Count = buff.Count;
    }
}
