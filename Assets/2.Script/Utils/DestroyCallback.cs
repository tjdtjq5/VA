using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCallback : MonoBehaviour
{
    Action _callback;
    public void DestroyCallbackSetting(Action callback)
    {
        _callback = callback;
    }

    private void OnDestroy()
    {
        if ( _callback != null)
        {
            _callback.Invoke();
            _callback = null;
        }
    }
}
