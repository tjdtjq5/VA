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

    public void Push(Poolable poolable)
    {

    }
    public Poolable Pop(GameObject original, Transform parent = null)
    {
        return null;
    }
    public GameObject GetOriginal(string name)
    {
        return null;
    }

    public void Clear()
    {

    }
}
