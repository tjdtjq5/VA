using UnityEngine;

public static class UnityHelperExtention
{
    #region Child Find
    public static T GetOrAddComponent<T>(this GameObject _go) where T : UnityEngine.Component
    {
        return UnityHelper.GetOrAddComponent<T>(_go);
    }
    #endregion
}
