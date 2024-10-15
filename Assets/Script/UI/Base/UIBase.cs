using EasyButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{
    Dictionary<Type, UIBase[]> objectDics = new Dictionary<Type, UIBase[]>();

    private void Awake()
    {
        Initialize();
    }
    protected virtual void Initialize() { }

    private void Start()
    {
        UISet();
    }
    protected virtual void UISet() { }
    protected void Bind<T>(Type type) where T : UIBase
    {
        string[] names = type.GetEnumNames();

        UIBase[] objs = new UIBase[names.Length];

        if (!objectDics.ContainsKey(typeof(T)))
        {
            objectDics.Add(typeof(T), objs);
        }
        else
        {
            var tempL = objectDics[typeof(T)].ToList();
            tempL.AddRange(objs);
            objectDics[typeof(T)] = tempL.ToArray();
        }

        for (int i = 0; i < names.Length; i++)
        {
            string path = names[i].Replace('_', '/');

            switch (path)
            {
                case nameof(UIImage):
                    objs[i] = gameObject.GetComponent<UIImage>();
                    continue;
                case nameof(UIText):
                    objs[i] = gameObject.GetComponent<UIText>();
                    continue;
                case nameof(UISlider):
                    objs[i] = gameObject.GetComponent<UISlider>();
                    continue;
                case nameof(UIToggle):
                    objs[i] = gameObject.GetComponent<UIToggle>();
                    continue;
                case nameof(UIScrollbar):
                    objs[i] = gameObject.GetComponent<UIScrollbar>();
                    continue;
                case nameof(UIScrollView):
                    objs[i] = gameObject.GetComponent<UIScrollView>();
                    continue;
                case nameof(UIButton):
                    objs[i] = gameObject.GetComponent<UIButton>();
                    continue;
                case nameof(UITabButton):
                    objs[i] = gameObject.GetComponent<UITabButton>();
                    continue;
                case nameof(UITabButtonParent):
                    objs[i] = gameObject.GetComponent<UITabButtonParent>();
                    continue;
                case nameof(UITabSlider):
                    objs[i] = gameObject.GetComponent<UITabSlider>();
                    continue;
                case nameof(UIInputField):
                    objs[i] = gameObject.GetComponent<UIInputField>();
                    continue;
                case nameof(UICheck):
                    objs[i] = gameObject.GetComponent<UICheck>();
                    continue;
            }

            if (typeof(T) == typeof(GameObject))
                objs[i] = UnityHelper.FindChildByPath<T>(gameObject, path);
            else
                objs[i] = UnityHelper.FindChildByPath<T>(gameObject, path);
        }
    }
    protected T Get<T>(Enum _enumValue) where T : UIBase
    {
        UIBase[] objs = null;

        if (objectDics.TryGetValue(typeof(T), out objs) == false)
        {
            UnityHelper.Error_H($"UIBase Get Error\nT : {typeof(T)}");
            return null;
        }

        return objs[_enumValue.GetHashCode()] as T;
    }
    protected UIText GetText(Enum _enumValue)
    {
        return Get<UIText>(_enumValue);
    }
    protected UIImage GetImage(Enum _enumValue)
    {
        return Get<UIImage>(_enumValue);
    }
    protected UISlider GetSlider(Enum _enumValue)
    {
        return Get<UISlider>(_enumValue);
    }
    protected UIToggle GetToggle(Enum _enumValue)
    {
        return Get<UIToggle>(_enumValue);
    }
    protected UIScrollbar GetScrollbar(Enum _enumValue)
    {
        return Get<UIScrollbar>(_enumValue);
    }
    protected UIScrollView GetScrollView(Enum _enumValue)
    {
        return Get<UIScrollView>(_enumValue);
    }
    protected UIButton GetButton(Enum _enumValue)
    {
        return Get<UIButton>(_enumValue);
    }
    protected UITabButton GetTabButton(Enum _enumValue)
    {
        return Get<UITabButton>(_enumValue);
    }
    protected UITabSlider GetTabSlider(Enum _enumValue)
    {
        return Get<UITabSlider>(_enumValue);
    }
    protected UIInputField GetInputField(Enum _enumValue)
    {
        return Get<UIInputField>(_enumValue);
    }
    protected UITabButtonParent GetTabButtonParent(Enum _enumValue)
    {
        return Get<UITabButtonParent>(_enumValue);
    }
    protected UICheck GetCheck(Enum _enumValue)
    {
        return Get<UICheck>(_enumValue);
    }


    protected void BindEvent(GameObject _go, Action<PointerEventData> _action, UIEvent eventType = UIEvent.Click)
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
    protected void UnBindEvent(GameObject _go, Action<PointerEventData> _action, UIEvent eventType = UIEvent.Click)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);

        switch (eventType)
        {
            case UIEvent.Click:
                evt.eventHandlerClick -= _action;
                break;
            case UIEvent.Drag:
                evt.eventHandlerOnDrag -= _action;
                break;
            case UIEvent.PointDown:
                evt.eventHandlerPointDown -= _action;
                break;
            case UIEvent.PointUp:
                evt.eventHandlerPointUp -= _action;
                break;
        }
    }
    protected Action<PointerEventData> GetEvent(GameObject _go, UIEvent eventType = UIEvent.Click)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);

        switch (eventType)
        {
            case UIEvent.Click:
                return evt.eventHandlerClick;
            case UIEvent.Drag:
                return evt.eventHandlerOnDrag;
            case UIEvent.PointDown:
                return evt.eventHandlerPointDown;
            case UIEvent.PointUp:
                return evt.eventHandlerPointUp;
        }

        return null;
    }

    public RectTransform RectTransform
    {
        get
        {
            return UnityHelper.GetOrAddComponent<RectTransform>(this.gameObject);
        }
    }

    [Button]
    public void Test()
    {
        foreach (var t in this.objectDics)
        {
            UnityHelper.Log_H($"====={t.Key.Name}=====");
            for (var i = 0; i < t.Value.Length; i++)
            {
                UnityHelper.Log_H($"{t.Value[i].GetType().Name}");
            }
        }
    }

}
