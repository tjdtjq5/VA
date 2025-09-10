using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using UnityEngine;

public abstract class HpBar : UIFrame
{
    protected Action<BBNumber, BBNumber> OnHpChanged;
    protected Action<BBNumber, BBNumber> OnShieldChanged;
    protected Action<BBNumber, BBNumber> OnSequenceChanged;
    
    public BBNumber MaxHp { get; set; }
    public BBNumber MaxSequence { get; set; }

    private BBNumber _hp;
    private BBNumber _shield;
    private BBNumber _sequence;
    public BBNumber Hp
    {
        get { return _hp;}
        set
        {
            if (MaxHp < value)
                MaxHp = value;

            BBNumber oldHp = _hp;
            _hp = value;

            OnHpChanged?.Invoke(oldHp, value);
        } 
    }
    public BBNumber Shield
    {
        get { return _shield;}
        set
        {
            BBNumber oldShield = _shield;
            _shield = value;

            OnShieldChanged?.Invoke(oldShield, value);
        } 
    }
    public BBNumber Sequence
    {
        get { return _sequence;}
        set
        {
            BBNumber oldSequence = _sequence;
            _sequence = value;

            OnSequenceChanged?.Invoke(oldSequence, value);
        } 
    }
    public virtual void Initialize(BBNumber maxHp, BBNumber maxSequence)
    {
        OnHpChanged = null;
        OnShieldChanged = null;
        OnSequenceChanged = null;
        
        this.MaxHp = maxHp;
        this.MaxSequence = maxSequence;
        this.Sequence = maxSequence;
    }
}
