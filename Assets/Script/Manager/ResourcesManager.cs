using Unity.VisualScripting;
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
                n = n.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(n);
            if (go != null)
                return go as T;
        }

        T resourcesLoad = Resources.Load<T>(path);


        return resourcesLoad;
    }
    public T Instantiate<T>(string path, Transform parent = null) where T : UnityEngine.Object
    {
        T original = Load<T>(path);

        if (original == null)
        {
            UnityHelper.Error_H($"ResourcesManager Instantiate Null Error\npath : {path}");
            return null;
        }

        return Instantiate(original, parent);
    }
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>(path);

        if (original == null)
        {
            UnityHelper.Error_H($"ResourcesManager Instantiate Null Error\npath : {path}");
            return null;
        }

        return Instantiate(original, parent);
    }
    public T Instantiate<T>(T obj, Transform parent = null) where T : UnityEngine.Object
    {
        if (obj.GetComponent<Poolable>() != null)
        {
            var p = Managers.Pool.Pop(obj.GameObject(), parent);
            return p.GetComponent<T>();
        }

        T go = UnityEngine.Object.Instantiate(obj, parent);

        return go;
    }
    public GameObject Instantiate(GameObject obj, Transform parent = null)
    {
        if (obj.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(obj, parent).gameObject;

        GameObject go = UnityEngine.Object.Instantiate(obj, parent);
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
