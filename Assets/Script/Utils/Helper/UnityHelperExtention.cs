using System.Collections.Generic;
using UnityEngine;

public static class UnityHelperExtention
{
    #region Child Find
    public static T GetOrAddComponent<T>(this GameObject _go) where T : UnityEngine.Component
    {
        return UnityHelper.GetOrAddComponent<T>(_go);
    }
    public static T FindChild<T>(this GameObject _go, bool _recursive = false) where T : UnityEngine.Object
    {
        return UnityHelper.FindChild<T>(_go, _recursive);
    }
    public static T FindChild<T>(this GameObject _go, string name) where T : UnityEngine.Object
    {
        return UnityHelper.FindChild<T>(_go, name);
    }
    public static T FindChildByPath<T>(this GameObject _go, string path) where T : UnityEngine.Object
    {
        return UnityHelper.FindChildByPath<T>(_go, path);
    }
    public static List<T> FindChilds<T>(this GameObject _go, bool _recursive = false) where T : UnityEngine.Object
    {
        return UnityHelper.FindChilds<T>(_go, _recursive);
    }
    #endregion
}
