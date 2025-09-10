using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    Dictionary<KeyCode, Action> _keyDownActions = new Dictionary<KeyCode, Action>();
    Dictionary<KeyCode, Action> _keyUpActions = new Dictionary<KeyCode, Action>();


    Action _mouseDownAction;
    Action _mouseUpAction;
    Action<MouseEvent> _mouseAction;
    Action<Vector2> _mouseScreenPosAction;
    Dictionary<float, Action> _mousePressedActions = new Dictionary<float, Action>();
    Dictionary<Action, bool> _mousePressedActionFlags = new Dictionary<Action, bool>();

    bool _isPressed;
    float _pressedTimer;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

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

        if (_mouseDownAction != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _mouseDownAction.Invoke();
            }
        }

        if (_mouseUpAction != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _mouseUpAction.Invoke();
            }
        }

        if (_mouseScreenPosAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                _mouseScreenPosAction.Invoke(Input.mousePosition);
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

        if (_keyDownActions.Count > 0)
        {
            foreach (var keyAction in _keyDownActions)
            {
                if (Input.GetKeyDown(keyAction.Key))
                {
                    keyAction.Value.Invoke();
                }
            }
        }

        if (_keyUpActions.Count > 0)
        {
            foreach (var keyAction in _keyUpActions)
            {
                if (Input.GetKeyUp(keyAction.Key))
                {
                    keyAction.Value.Invoke();
                }
            }
        }
    }

    public void AddKeyDownAction(KeyCode keyCode, Action action)
    {
        if (_keyDownActions.ContainsKey(keyCode))
        {
            _keyDownActions[keyCode] -= action;
            _keyDownActions[keyCode] += action;
        }
        else
        {
            _keyDownActions.Add(keyCode, action);
        }
    }
    public void AddKeyUpAction(KeyCode keyCode, Action action)
    {
        if (_keyUpActions.ContainsKey(keyCode))
        {
            _keyUpActions[keyCode] -= action;
            _keyUpActions[keyCode] += action;
        }
        else
        {
            _keyUpActions.Add(keyCode, action);
        }
    }
    public void AddMouseDownAction(Action action)
    {
        _mouseDownAction -= action;
        _mouseDownAction += action;
    }
    public void AddMouseUpAction(Action action)
    {
        _mouseUpAction -= action;
        _mouseUpAction += action;
    }
    public void AddMouseAction(Action<MouseEvent> action)
    {
        _mouseAction -= action;
        _mouseAction += action;
    }
    public void AddMouseAction(Action<Vector2> action)
    {
        _mouseScreenPosAction -= action;
        _mouseScreenPosAction += action;
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

    public void SubKeyDownAction(KeyCode keyCode, Action action)
    {
        if (_keyDownActions.ContainsKey(keyCode))
        {
            _keyDownActions[keyCode] -= action;
        }
    }
    public void SubKeyUpAction(KeyCode keyCode, Action action)
    {
        if (_keyUpActions.ContainsKey(keyCode))
        {
            _keyUpActions[keyCode] -= action;
        }
    }
    public void SubMouseAction(Action<MouseEvent> action)
    {
        _mouseAction -= action;
    }
    public void SubMouseAction(Action<Vector2> action)
    {
        _mouseScreenPosAction -= action;
    }
    public void SubMousePressedTimeAction(float pressedTime, Action action)
    {
        if (_mousePressedActions.ContainsKey(pressedTime))
        {
            _mousePressedActions[pressedTime] -= action;
        }
    }

    public void Clear()
    {
        _keyDownActions.Clear();
        _keyUpActions.Clear();
        _mouseAction = null;
        _mouseDownAction = null;
        _mouseUpAction = null;
        _mousePressedActions.Clear();
    }
}
