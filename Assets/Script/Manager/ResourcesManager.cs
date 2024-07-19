using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    Dictionary<string, UnityEngine.Object> _loadDics = new Dictionary<string, UnityEngine.Object>();
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (_loadDics.TryGetValue(path, out UnityEngine.Object dicsLoad))
        {
            return (T)dicsLoad;
        }
        else
        {
            T resourcesLoad = Resources.Load<T>(path);
            _loadDics.Add(path, resourcesLoad);

            return resourcesLoad;
        }
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(path);

        if (prefab == null)
        {
            UnityHelper.LogError_H($"ResourcesManager Instantiate Null Error\npath : {path}");
            return null;
        }

        GameObject go = UnityEngine.Object.Instantiate(prefab, parent);
        go.name = go.name.Replace("(Clone)", "");

        return go;
    }
    public void Destroy(GameObject go) 
    {
        if (go == null)
            return;

        UnityEngine.Object.Destroy(go);
    }
    public void Clear()
    {
        _loadDics.Clear();
    }
}
