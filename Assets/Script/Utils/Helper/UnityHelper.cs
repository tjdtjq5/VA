using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityHelper
{
    #region Log
    public static void Log_H(object message)
    {
        Debug.Log(message);
    }
    public static void LogError_H(object message)
    {
        Debug.LogError(message);
    }
    public static void LogSerialize(object message)
    {
        Debug.Log(CSharpHelper.SerializeObject(message));
    }
    #endregion

    #region FindChild
    public static GameObject FindChild(GameObject _go, string _name, bool _recursive = false)
    {
        Transform tr = FindChild<Transform>(_go, _name, _recursive);
        if (tr != null)
            return tr.gameObject;

        return null;
    }
    public static T FindChild<T>(GameObject _go, string _name, bool _recursive = false) where T : UnityEngine.Object
    {
        if (_go == null)
        {
            return null;
        }

        if (!_recursive)
        {
            for (int i = 0; i < _go.transform.childCount; i++)
            {
                Transform childTr = _go.transform.GetChild(i);

                if (string.IsNullOrEmpty(_name) || childTr.name == _name)
                {
                    T component = childTr.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in _go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(_name) || component.name == _name)
                    return component;
            }
        }

        return null;
    }
    public static T GetOrAddComponent<T>(GameObject _go) where T : UnityEngine.Component
    {
        T component = _go.GetComponent<T>();
        if (component == null) component = _go.AddComponent<T>();
        return component;
    }
    #endregion
}
