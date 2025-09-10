using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillBehaviourTime : SkillBehaviour
{
    [SerializeField] private float _time;

    protected float GetTime => _time * _timeMultiplier;
    protected float _timeMultiplier = 1f;
    protected Dictionary<float, Action> _actions = new Dictionary<float, Action>();
    
    private bool _isOn = false;
    private float _timer;
    private Character _owner;
    private object _cause;
    
    public override void Start(Character owner, object cause)
    {
        _owner = owner;
        _cause = cause;
        
        _timer = 0;
        _isOn = true;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (_isOn)
        {
            _timer += Managers.Time.FixedDeltaTime;

            float timeValue = _timer / GetTime;

            List<float> removeKeys = new List<float>();
            foreach (var action in _actions)
            {
                if (timeValue >= action.Key)
                {
                    action.Value?.Invoke();
                    removeKeys.Add(action.Key);
                }
            }

            foreach (var key in removeKeys)
            {
                _actions.Remove(key);
            }
            
            if (_timer >= GetTime)
            {
                End(_owner, _cause);
            }
        }
    }

    public override void End(Character owner, object cause)
    {
        _actions.Clear();

        _isOn = false;
        OnEnd?.Invoke(this, owner, cause);
    }
}
