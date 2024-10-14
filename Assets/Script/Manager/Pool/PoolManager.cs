using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original {  get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Initialize(GameObject original, int count = 5)
        {
            Original = original;

            string rootObjName = $"@{original.name}_Root";
            Root = new GameObject(rootObjName).transform;

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }
        public void Push(Poolable poolable)
        {
            if (poolable == null)
            {
                UnityHelper.LogError_H($"Pool poolable Null Error");
                return;
            }

            poolable.transform.SetParent(Root);
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            _poolStack.Push(poolable);
        }
        public Poolable Pop(Transform parent)
        {
            Poolable poolable = null;

            if (_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            if (parent == null)
                parent = Root;

            poolable.transform.SetParent(parent);
            poolable.gameObject.SetActive(true);
            poolable.IsUsing = true;

            return poolable;    
        }
    }
    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();

    Transform _root;

    public void Initialize()
    {
        if (_root == null)
        {
            string rootObjName = "@Pool";
            _root = new GameObject(rootObjName).transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    void Create(GameObject original, int count = 1)
    {
        Pool pool = new Pool();
        pool.Initialize(original, count);
        pool.Root.parent = _root;

        _pools.Add(original.name, pool);
    }
    public void Push(Poolable poolable)
    {
        string n = poolable.gameObject.name;

        if (!_pools.ContainsKey(n))
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pools[n].Push(poolable);
    }
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (original == null) 
        {
            UnityHelper.LogError_H($"PoolManager original Null Error");
            return null;
        }

        if (!_pools.ContainsKey(original.name))
            Create(original);

        return _pools[original.name].Pop(parent);
    }
    public GameObject GetOriginal(string name)
    {
        if (!_pools.ContainsKey(name))
            return null;

        return _pools[name].Original;
    }

    public void Clear()
    {
        if (_root)
        {
            foreach (Transform child in _root)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        _pools.Clear();
    }
}
