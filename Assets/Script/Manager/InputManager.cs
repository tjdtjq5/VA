using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    Action _keyAction;
    Action<MouseEvent> _mouseAction;

    bool _isPressed;

    public void OnUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && _keyAction != null)
        {
            _keyAction.Invoke();
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

        if (_keyAction != null)
            _keyAction.Invoke();
    }
    public void AddKeyAction(Action action)
    {
        _keyAction -= action;
        _keyAction += action;
    }
    public void AddMouseAction(Action<MouseEvent> action)
    {
        _mouseAction -= action;
        _mouseAction += action;
    }
    public void Clear()
    {
        _keyAction = null;
        _mouseAction = null;
    }
}
