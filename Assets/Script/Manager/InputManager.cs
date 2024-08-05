using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    Dictionary<KeyCode, Action> _keyActions = new Dictionary<KeyCode, Action>();
    Action<MouseEvent> _mouseAction;

    Dictionary<float, Action> _mousePressedActions = new Dictionary<float, Action>();
    Dictionary<Action, bool> _mousePressedActionFlags = new Dictionary<Action, bool>();

    bool _isPressed;
    float _pressedTimer;

    public void OnUpdate()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && _keyActions.Count > 0)
        {
            foreach (var keyAction in _keyActions) 
            {
                if (Input.GetKeyDown(keyAction.Key))
                {
                    keyAction.Value.Invoke();
                }
            }
        }

        if (_mouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                _mouseAction.Invoke(MouseEvent.Press);
                _isPressed = true;
            }
            else
            {
                if (_isPressed)
                    _mouseAction.Invoke(MouseEvent.Click);
                _isPressed = false;
            }
        }

        

        if (_mousePressedActions.Count > 0)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_isPressed)
                {
                    _isPressed = true;
                    _mousePressedActionFlags.Clear();
                    _pressedTimer = 0;
                }
            }
            else
            {
                _isPressed = false;
            }

            if (_isPressed)
            {
                _pressedTimer += Managers.Time.DeltaTime;

                foreach (var mousePressedAction in _mousePressedActions)
                {
                    if (mousePressedAction.Key < _pressedTimer)
                    {
                        if (_mousePressedActionFlags.TryAdd(mousePressedAction.Value, true))
                        {
                            mousePressedAction.Value.Invoke();
                        }
                    }
                }
            }
        }
    }
    public void AddKeyAction(KeyCode keyCode, Action action)
    {
        if (_keyActions.ContainsKey(keyCode))
        {
            _keyActions[keyCode] -= action;
            _keyActions[keyCode] += action;
        }
        else
        {
            _keyActions.Add(keyCode, action);
        }
    }
    public void AddMouseAction(Action<MouseEvent> action)
    {
        _mouseAction -= action;
        _mouseAction += action;
    }
    public void AddMousePressedTimeAction(float pressedTime, Action action)
    {
        if (_mousePressedActions.ContainsKey(pressedTime))
        {
            _mousePressedActions[pressedTime] -= action;
            _mousePressedActions[pressedTime] += action;
        }
        else
        {
            _mousePressedActions.Add(pressedTime, action);
        }
    }
    public void Clear()
    {
        _keyActions.Clear();
        _mouseAction = null;
        _mousePressedActions.Clear();
    }
}
