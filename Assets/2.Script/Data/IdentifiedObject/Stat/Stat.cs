using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public class Stat : IdentifiedObject
{
    public delegate void ValueChangedHandler(Stat stat, BBNumber currentValue, BBNumber prevValue);

    public bool IsPercent => isPercentType;
    public BonusFormulaType GetBonusFormulaType => bonusFormulaType;

    [SerializeField]
    private bool isPercentType;
    [SerializeField]
    private BBNumber maxValue;
    [SerializeField]
    private BBNumber minValue;
    [SerializeField]
    private BBNumber defaultValue;
    [SerializeField]
    private BonusFormulaType bonusFormulaType;

    public enum BonusFormulaType
    {
        AllAdd,
        MainMultiple,
        AllMultiple,
    }

    private readonly Dictionary<object, Dictionary<object, BBNumber>> _bonusValuesByKey = new();
    private readonly Dictionary<object, Dictionary<object, BBNumber>> _decreaseValuesByKey = new();
    public Dictionary<object, Dictionary<object, BBNumber>> BonusValuesByKey => _bonusValuesByKey;
    public Dictionary<object, Dictionary<object, BBNumber>> DecreaseValuesByKey => _decreaseValuesByKey;

    public bool IsPercentType => isPercentType;
    public BBNumber MaxValue
    {
        get => maxValue;
        set => maxValue = value;
    }
    public BBNumber MinValue
    {
        get => minValue;
        set => minValue = value;
    }

    public BBNumber DefaultValue
    {
        get => defaultValue;
        set
        {
            BBNumber prevValue = Value;
            
            defaultValue = value;

            TryInvokeValueChangedEvent(Value, prevValue);
        }
    }
    public BBNumber BonusValue { get; private set; }
    public BBNumber DecreaseValue { get; private set; }
    public BBNumber Value
    {
        get
        {
            if (MaxValue <= 0)
                return BBNumber.Max(defaultValue + BonusValue - DecreaseValue, MinValue);
            else
                return BBNumber.Clamp(defaultValue + BonusValue - DecreaseValue, MinValue, MaxValue);
        }
    }
    public bool IsMax => BBNumber.Approximately(Value, maxValue);
    public bool IsMin => BBNumber.Approximately(Value, minValue);
    public int BonusCount => _bonusValuesByKey.Count;
    public int DecreaseCount => _decreaseValuesByKey.Count;

    public event ValueChangedHandler onValueChanged;
    public event ValueChangedHandler onBonusValueChanged;
    public event ValueChangedHandler onDecreaseValueChanged;
    public event ValueChangedHandler onValueMax;
    public event ValueChangedHandler onValueMin;

    private void TryInvokeValueChangedEvent(BBNumber currentValue, BBNumber prevValue)
    {
        if (!BBNumber.Approximately(currentValue, prevValue))
        {
            onValueChanged?.Invoke(this, currentValue, prevValue);
            if (BBNumber.Approximately(currentValue, MaxValue))
                onValueMax?.Invoke(this, MaxValue, prevValue);
            else if (BBNumber.Approximately(currentValue, MinValue))
                onValueMin?.Invoke(this, MinValue, prevValue);
        }
    }

    #region Bonus

    public void SetBonusValue(object key, object subKey, BBNumber value)
    {
        if (!_bonusValuesByKey.ContainsKey(key))
            _bonusValuesByKey[key] = new Dictionary<object, BBNumber>();
        else
        {
            if (_bonusValuesByKey[key].ContainsKey(subKey))
                _bonusValuesByKey[key][subKey] = 0;
        }

        BonusValue = CurrentBonusValue;
        BBNumber prevValue = Value;
        _bonusValuesByKey[key][subKey] = value;
        BonusValue = CurrentBonusValue;
        BBNumber currentValue = Value;

        if (codeName.Equals("HP"))
        {
            onBonusValueChanged?.Invoke(this, currentValue - defaultValue, prevValue - defaultValue);
            TryInvokeValueChangedEvent(currentValue - defaultValue, prevValue - defaultValue);
        }
        else
        {
            onBonusValueChanged?.Invoke(this, currentValue, prevValue);
            TryInvokeValueChangedEvent(currentValue, prevValue);
        }
    }

    public void SetBonusValue(object key, BBNumber value)
        => SetBonusValue(key, string.Empty, value);

    BBNumber CurrentBonusValue
    {
        get
        {
            BBNumber result = 0;

            switch (bonusFormulaType)
            {
                case BonusFormulaType.MainMultiple:
                    result = 1;
                    break;
                case BonusFormulaType.AllMultiple:
                    result = 1;
                    break;
            }

            foreach (var mainKey in _bonusValuesByKey)
            {
                object key = mainKey.Key;

                switch (bonusFormulaType)
                {
                    case BonusFormulaType.AllAdd:
                        result += GetBonusValue(key);
                        break;
                    case BonusFormulaType.MainMultiple:
                        result *= GetBonusValue(key);
                        break;
                    case BonusFormulaType.AllMultiple:
                        result *= GetBonusValue(key);
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
    public BBNumber GetBonusValue(object key)
    {
        BBNumber result = 0;

        switch (bonusFormulaType)
        {
            case BonusFormulaType.AllAdd:
                if (_bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkeyAA))
                {
                    foreach (var sub in bonusValuesBySubkeyAA)
                    {
                        result += sub.Value;
                    }
                }
                break;
            case BonusFormulaType.MainMultiple:
                if (_bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkeyMM))
                {
                    foreach (var sub in bonusValuesBySubkeyMM)
                    {
                        result += sub.Value;
                    }
                }
                break;
            case BonusFormulaType.AllMultiple:
                result = 1;
                if (_bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkeyAM))
                {
                    foreach (var sub in bonusValuesBySubkeyAM)
                    {
                        result *= sub.Value;
                    }
                }
                break;
            default:
                break;
        }

        return result;
    }
    public BBNumber GetBonusValue(object key, object subKey)
    {
        if (_bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            if (bonusValuesBySubkey.TryGetValue(subKey, out var value))
                return value;
        }
        return 0f;
    }
    public bool RemoveBonusValue(object key)
    {
        if (_bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            BBNumber prevValue = Value;

            _bonusValuesByKey.Remove(key);
            BonusValue = CurrentBonusValue;

            onBonusValueChanged?.Invoke(this, Value, prevValue);
            TryInvokeValueChangedEvent(Value, prevValue);
            return true;
        }
        return false;
    }
    public bool RemoveBonusValue(object key, object subKey)
    {
        if (_bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            if (bonusValuesBySubkey.Remove(subKey, out var value))
            {
                var prevValue = Value;
                BonusValue = CurrentBonusValue;
                
                onBonusValueChanged?.Invoke(this, Value, prevValue);
                TryInvokeValueChangedEvent(Value, prevValue);
                return true;
            }
        }
        return false;
    }

    public bool ContainsBonusValue(object key)
        => _bonusValuesByKey.ContainsKey(key);

    public bool ContainsBonusValue(object key, object subKey)
        => _bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubKey) ? bonusValuesBySubKey.ContainsKey(subKey) : false;

    #endregion
    
    #region Decrease
    
    public void SetDecreaseValue(object key, object subKey, BBNumber value)
    {
        if (!_decreaseValuesByKey.ContainsKey(key))
            _decreaseValuesByKey[key] = new Dictionary<object, BBNumber>();
        else
        {
            if (_decreaseValuesByKey[key].ContainsKey(subKey))
                _decreaseValuesByKey[key][subKey] = 0;
        }

        DecreaseValue = CurrentDecreaseValue;
        BBNumber prevValue = Value;
        _decreaseValuesByKey[key][subKey] = value;
        DecreaseValue = CurrentDecreaseValue;

        onDecreaseValueChanged?.Invoke(this, Value, prevValue);
        TryInvokeValueChangedEvent(Value, prevValue);
    }

    public void SetDecreaseValue(object key, BBNumber value)
        => SetDecreaseValue(key, string.Empty, value);

    BBNumber CurrentDecreaseValue
    {
        get
        {
            BBNumber result = 0;

            foreach (var mainKey in _decreaseValuesByKey)
            {
                object key = mainKey.Key;
                result += GetDecreaseValue(key);
            }

            return result;
        }
    }
    public BBNumber GetDecreaseValue(object key)
    {
        BBNumber result = 0;

        if (_decreaseValuesByKey.TryGetValue(key, out var decreaseValuesBySubkeyAA))
        {
            foreach (var sub in decreaseValuesBySubkeyAA)
            {
                result += sub.Value;
            }
        }

        return result;
    }
    public BBNumber GetDecreaseValue(object key, object subKey)
    {
        if (_decreaseValuesByKey.TryGetValue(key, out var decreaseValuesBySubkey))
        {
            if (decreaseValuesBySubkey.TryGetValue(subKey, out var value))
                return value;
        }
        return 0f;
    }
    public bool RemoveDecreaseValue(object key)
    {
        if (_decreaseValuesByKey.TryGetValue(key, out var decreaseValuesBySubkey))
        {
            BBNumber prevValue = Value;

            _decreaseValuesByKey.Remove(key);
            DecreaseValue = CurrentDecreaseValue;

            onDecreaseValueChanged?.Invoke(this, Value, prevValue);
            TryInvokeValueChangedEvent(Value, prevValue);
            return true;
        }
        return false;
    }
    public bool RemoveDecreaseValue(object key, object subKey)
    {
        if (_decreaseValuesByKey.TryGetValue(key, out var decreaseValuesBySubkey))
        {
            if (decreaseValuesBySubkey.Remove(subKey, out var value))
            {
                var prevValue = Value;
                DecreaseValue = CurrentDecreaseValue;
                
                onDecreaseValueChanged?.Invoke(this, Value, prevValue);
                TryInvokeValueChangedEvent(Value, prevValue);
                return true;
            }
        }
        return false;
    }

    public bool ContainsDecreaseValue(object key)
        => _decreaseValuesByKey.ContainsKey(key);

    public bool ContainsDecreaseValue(object key, object subKey)
        => _decreaseValuesByKey.TryGetValue(key, out var decreaseValuesBySubKey) ? decreaseValuesBySubKey.ContainsKey(subKey) : false;
    
    #endregion

    public void Load(StatSaveData saveData)
    {
        this.codeName = saveData.CodeName;
        this.DefaultValue = saveData.DefaultValue;

        if (saveData.BonusValuesByKey != null)
        {
            foreach (var bonusValues in saveData.BonusValuesByKey)
            {
                Dictionary<object, BBNumber> valueDics = bonusValues.Value;

                foreach (var value in valueDics)
                {
                    SetBonusValue(bonusValues.Key, value.Key, value.Value);
                }            
            }
        }


        if (saveData.DecreaseValuesByKey != null)
        {
            foreach (var decreaseValues in saveData.DecreaseValuesByKey)
            {
                Dictionary<object, BBNumber> valueDics = decreaseValues.Value;

                foreach (var value in valueDics)
                {
                    SetDecreaseValue(decreaseValues.Key, value.Key, value.Value);
                }            
            }
        }
    }
}

[System.Serializable]
public class StatSaveData
{
    public string CodeName;
    public BBNumber DefaultValue;
    public Dictionary<object, Dictionary<object, BBNumber>> BonusValuesByKey = new();
    public Dictionary<object, Dictionary<object, BBNumber>> DecreaseValuesByKey = new();

    public StatSaveData(Stat stat)
    {
        if (stat == null)
        {
            this.CodeName = string.Empty;
            this.DefaultValue = 0;
            BonusValuesByKey.Clear();
            DecreaseValuesByKey.Clear();
            return;
        }

        this.CodeName = stat.CodeName;
        this.DefaultValue = stat.DefaultValue;
        this.BonusValuesByKey = stat.BonusValuesByKey;
        this.DecreaseValuesByKey = stat.DecreaseValuesByKey;
    }
}