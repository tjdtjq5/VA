using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stat : IdentifiedObject
{
    public delegate void ValueChangedHandler(Stat stat, BBNumber currentValue, BBNumber prevValue);

    // % type�ΰ�? (ex, 1 => 100%, 0 => 0%)
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
        AllAdd, // ��� �� ���ϱ�
        MainMultiple, // ���γ��� ��� ��
        AllMultiple, // ��� ���ϱ�
    }

    // �⺻ stat ���� bonus stat�� �����ϴ� dictionary,
    // key ���� bonus stat�� �� ��� (ex. ��� bonus Stat�� �־��ٸ� �� ��� key���� ��)
    // value Dictionary�� key ���� SubKey
    // mainKey�� bonus stat�� ������ �� �� �� bonus ���� �����ϱ� ���� �뵵
    // subKey�� �ʿ���� ��� string.Empty�� subKey�� bonus�� ������
    private Dictionary<object, Dictionary<object, BBNumber>> bonusValuesByKey = new();
    public Dictionary<object, Dictionary<object, BBNumber>> BonusValuesByKey => bonusValuesByKey;

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

            // value�� ������ �� event�� �˸�
            TryInvokeValueChangedEvent(Value, prevValue);
        }
    }
    // �� Dictionary�� ����� bonus value�� ��
    public BBNumber BonusValue { get; private set; }
    // Default + Bonus, ���� �� ��ġ
    public BBNumber Value
    {
        get
        {
            if (MaxValue <= 0)
                return BBNumber.Max(defaultValue + BonusValue, MinValue);
            else
                return BBNumber.Clamp(defaultValue + BonusValue, MinValue, MaxValue);
        }
    }
    public bool IsMax => BBNumber.Approximately(Value, maxValue);
    public bool IsMin => BBNumber.Approximately(Value, minValue);

    public event ValueChangedHandler onValueChanged;
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

    public void SetBonusValue(object key, object subKey, BBNumber value)
    {
        if (!bonusValuesByKey.ContainsKey(key))
            bonusValuesByKey[key] = new Dictionary<object, BBNumber>();
        else
        {
            if (bonusValuesByKey[key].ContainsKey(subKey))
                bonusValuesByKey[key][subKey] = 0;
        }

        BonusValue = CurrentBonusValue;
        BBNumber prevValue = Value;
        bonusValuesByKey[key][subKey] = value;
        BonusValue = CurrentBonusValue;

        TryInvokeValueChangedEvent(Value, prevValue);
    }

    public void SetBonusValue(object key, BBNumber value)
        => SetBonusValue(key, string.Empty, value);

    public void SetBonusValue(Dictionary<object, Dictionary<object, BBNumber>> _bonusValuesByKey)
    {
        foreach (var bonusKey in _bonusValuesByKey)
        {
            object key = bonusKey.Key;

            foreach (var bonusValue in bonusKey.Value)
            {
                object subKey = bonusValue.Key;
                BBNumber value = bonusValue.Value;

                SetBonusValue(key, subKey, value);
            }
        }
    }

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

            foreach (var mainKey in bonusValuesByKey)
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
                if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkeyAA))
                {
                    foreach (var sub in bonusValuesBySubkeyAA)
                    {
                        result += sub.Value;
                    }
                }
                break;
            case BonusFormulaType.MainMultiple:
                if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkeyMM))
                {
                    foreach (var sub in bonusValuesBySubkeyMM)
                    {
                        result += sub.Value;
                    }
                }
                break;
            case BonusFormulaType.AllMultiple:
                result = 1;
                if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkeyAM))
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
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            if (bonusValuesBySubkey.TryGetValue(subKey, out var value))
                return value;
        }
        return 0f;
    }

    public bool RemoveBonusValue(object key)
    {
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            BBNumber prevValue = Value;
            BBNumber sumValue = 0;
            foreach (var bonusValue in bonusValuesBySubkey)
            {
                sumValue += bonusValue.Value;
            }

            BonusValue -= sumValue;
            bonusValuesByKey.Remove(key);

            TryInvokeValueChangedEvent(Value, prevValue);
            return true;
        }
        return false;
    }

    public bool RemoveBonusValue(object key, object subKey)
    {
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            if (bonusValuesBySubkey.Remove(subKey, out var value))
            {
                var prevValue = Value;
                BonusValue -= value;
                TryInvokeValueChangedEvent(Value, prevValue);
                return true;
            }
        }
        return false;
    }

    public void RemoveBonusValueAndExceptionSub(object key, object exceptSubKeys)
    {
        if (bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubkey))
        {
            List<object> removeSubKeys = new List<object>();

            foreach (var subBonus in bonusValuesBySubkey)
            {
                if (!exceptSubKeys.Equals(subBonus.Key))
                    removeSubKeys.Add(subBonus.Key);
            }

            for (int i = 0; i < removeSubKeys.Count; i++)
                RemoveBonusValue(key, removeSubKeys[i]);
        }
    }

    public bool ContainsBonusValue(object key)
        => bonusValuesByKey.ContainsKey(key);

    public bool ContainsBonusValue(object key, object subKey)
        => bonusValuesByKey.TryGetValue(key, out var bonusValuesBySubKey) ? bonusValuesBySubKey.ContainsKey(subKey) : false;
}
public enum OnlyCharacterStatKey // Sub -> CharacterCode
{
    CharacterLevel,
    CharacterPotential,
    CharacterSpecialWeaponLevel,
}
public enum ScreenStatKey
{
    Buff,
}