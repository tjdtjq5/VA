using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> objectDics = new Dictionary<Type, UnityEngine.Object[]>();

    private void Awake()
    {
        Initialize();
    }
    public virtual void Initialize() { }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = type.GetEnumNames();

        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];

        objectDics.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objs[i] = UnityHelper.FindChild<T>(gameObject, names[i], true);
            else
                objs[i] = UnityHelper.FindChild<T>(gameObject, names[i], true);
        }
    }
    protected T Get<T>(Enum _enumValue) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;

        if (objectDics.TryGetValue(typeof(T), out objs) == false)
            return null;

        return objs[_enumValue.GetHashCode()] as T;
    }
    protected void AddUIEvent(GameObject _go, Action<PointerEventData> _action, UIEvent eventType = UIEvent.Click)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);

        switch (eventType)
        {
            case UIEvent.Click:
                evt.eventHandlerClick -= _action;
                evt.eventHandlerClick += _action;
                break;
            case UIEvent.Drag:
                evt.eventHandlerOnDrag -= _action;
                evt.eventHandlerOnDrag += _action;
                break;
            case UIEvent.PointDown:
                evt.eventHandlerPointDown -= _action;
                evt.eventHandlerPointDown += _action;
                break;
            case UIEvent.PointUp:
                evt.eventHandlerPointUp -= _action;
                evt.eventHandlerPointUp += _action;
                break;
        }
    }

    public RectTransform RectTransform
    {
        get
        {
            return UnityHelper.GetOrAddComponent<RectTransform>(this.gameObject);
        }
    }
}
