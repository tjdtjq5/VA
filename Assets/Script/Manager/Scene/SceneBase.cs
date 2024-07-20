using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SceneBase : MonoBehaviour
{
    SceneType _sceneType = SceneType.Unknown;
    public SceneType SceneType { get; protected set; }

    private void Start()
    {
        Initialize();
    }
    protected virtual void Initialize() 
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        if (obj == null)
            Managers.Resources.Instantiate("Prefab/UI/EventSystem").name = "@EventSystem";
    }
    public abstract void Clear();
}
