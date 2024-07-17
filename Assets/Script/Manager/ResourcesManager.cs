using System;
using UnityEngine;

public class ResourcesManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(path);

        if (prefab == null)
        {
            UnityHelper.LogError_H($"ResourcesManager Instantiate Null Error\npath : {path}");
            return null;
        }

        return UnityEngine.Object.Instantiate(prefab, parent);
    }
    public void Destroy(GameObject go) 
    {
        if (go == null)
            return;

        UnityEngine.Object.Destroy(go);
    }
}
