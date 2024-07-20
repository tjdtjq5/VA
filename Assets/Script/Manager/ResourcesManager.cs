using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string n = path;
            int index = n.LastIndexOf('/');
            if (index >= 0)
                n= n.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(n);
            if (go != null)
                return go as T;
        }

        T resourcesLoad = Resources.Load<T>(path);
        return resourcesLoad;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>(path);

        if (original == null)
        {
            UnityHelper.LogError_H($"ResourcesManager Instantiate Null Error\npath : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(original, parent);
        go.name = go.name.Replace("(Clone)", "");

        return go;
    }
    public void Destroy(GameObject go) 
    {
        if (go == null)
            return;

        Poolable poolable = go.GetComponent<Poolable>();

        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        UnityEngine.Object.Destroy(go);
    }
    public void Clear()
    {

    }
}
