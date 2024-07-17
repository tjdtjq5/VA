using System;
using UnityEngine;

public class InputManager
{
    Action _keyAction;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
            return;

        if (_keyAction != null)
            _keyAction.Invoke();
    }
    public void AddAction(Action keyAction)
    {
        _keyAction -= keyAction;
        _keyAction += keyAction;
    }
}
