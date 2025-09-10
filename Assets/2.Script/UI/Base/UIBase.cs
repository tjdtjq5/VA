using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBase : MonoBehaviour
{
    Dictionary<Type, UIBase[]> objectDics = new Dictionary<Type, UIBase[]>();
    protected bool IsInitialized = false;

    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        IsInitialized = true; 
    }

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
            objectDics[typeof(T)] = null;
            objectDics[typeof(T)] = objs;
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
                case nameof(UITextPro):
                    objs[i] = gameObject.GetComponent<UITextPro>();
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

            if(objs[i] == null)
            {
                UnityHelper.Error_H($"UIBase Bind Error\nT : {typeof(T)}\nPath : {path}");
            }
        }
    }
    protected T Get<T>(Enum _enumValue) where T : UIBase
    {
        UIBase[] objs = null;

        if (objectDics.TryGetValue(typeof(T), out objs) == false)
        {
            UnityHelper.Error_H($"UIBase Get Error\nT : {typeof(T)} : {_enumValue}");
            return null;
        }

        if(objs[_enumValue.GetHashCode()] == null)
        {
            UnityHelper.Error_H($"UIBase Get Error\nT : {typeof(T)} : {_enumValue} : {_enumValue.GetHashCode()}");
            return null;
        }

        return objs[_enumValue.GetHashCode()] as T;
    }
    protected UIText GetText(Enum _enumValue)
    {
        return Get<UIText>(_enumValue);
    }
    protected UITextPro GetTextPro(Enum _enumValue)
    {
        return Get<UITextPro>(_enumValue);
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
                evt.EventHandlerClick -= _action;
                evt.EventHandlerClick += _action;
                break;
            case UIEvent.PointDown:
                evt.EventHandlerPointDown -= _action;
                evt.EventHandlerPointDown += _action;
                break;
            case UIEvent.PointUp:
                evt.EventHandlerPointUp -= _action;
                evt.EventHandlerPointUp += _action;
                break;
            case UIEvent.Trigger:
                evt.EventHandlerTrigger -= _action;
                evt.EventHandlerTrigger += _action;
                break;
            case UIEvent.Enter:
                evt.EventHandlerEnter -= _action;
                evt.EventHandlerEnter += _action;
                break;
            case UIEvent.Exit:
                evt.EventHandlerExit -= _action;
                evt.EventHandlerExit += _action;
                break;
        }
    }
    protected void UnBindEvent(GameObject _go, Action<PointerEventData> _action, UIEvent eventType = UIEvent.Click)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);

        switch (eventType)
        {
            case UIEvent.Click:
                evt.EventHandlerClick -= _action;
                break;
            case UIEvent.PointDown:
                evt.EventHandlerPointDown -= _action;
                break;
            case UIEvent.PointUp:
                evt.EventHandlerPointUp -= _action;
                break;
            case UIEvent.Trigger:
                evt.EventHandlerTrigger -= _action;
                break;
            case UIEvent.Enter:
                evt.EventHandlerEnter -= _action;
                break;
            case UIEvent.Exit:
                evt.EventHandlerExit -= _action;
                break;
        }
    }
    protected void UnBindEvent(GameObject _go, UIEvent eventType = UIEvent.Click)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);

        switch (eventType)
        {
            case UIEvent.Click:
                evt.EventHandlerClick = null;
                break;
            case UIEvent.PointDown:
                evt.EventHandlerPointDown = null;
                break;
            case UIEvent.PointUp:
                evt.EventHandlerPointUp = null;
                break;
            case UIEvent.Trigger:
                evt.EventHandlerTrigger = null;
                break;
            case UIEvent.Enter:
                evt.EventHandlerEnter = null;
                break;
            case UIEvent.Exit:
                evt.EventHandlerExit = null;
                break;
        }
    }
    protected void UnBindEventAll(GameObject _go)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);
        evt.EventHandlerClick = null;
        evt.EventHandlerPointDown = null;
        evt.EventHandlerPointUp = null;
        evt.EventHandlerTrigger = null;
        evt.EventHandlerEnter = null;
        evt.EventHandlerExit = null;
    }
    protected Action<PointerEventData> GetEvent(GameObject _go, UIEvent eventType = UIEvent.Click)
    {
        UIEventHandler evt = UnityHelper.GetOrAddComponent<UIEventHandler>(_go);

        switch (eventType)
        {
            case UIEvent.Click:
                return evt.EventHandlerClick;
            case UIEvent.PointDown:
                return evt.EventHandlerPointDown;
            case UIEvent.PointUp:
                return evt.EventHandlerPointUp;
            case UIEvent.Trigger:
                return evt.EventHandlerTrigger;
            case UIEvent.Enter:
                return evt.EventHandlerEnter;
            case UIEvent.Exit:
                return evt.EventHandlerTrigger;
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

    public float OffsetMaxX
    {
        get
        {
            return RectTransform.offsetMax.x;
        }
        set
        {
            RectTransform.offsetMax = new Vector2(-value, RectTransform.offsetMax.y);
        }
    } 
    public float OffsetMaxY
    {
        get
        {
            return RectTransform.offsetMax.y;
        }
        set
        {
            RectTransform.offsetMax = new Vector2(RectTransform.offsetMax.x, value);
        }
    } 
    public float OffsetMinX
    {
        get
        {
            return RectTransform.offsetMin.x;
        }
        set
        {
            RectTransform.offsetMin = new Vector2(value, RectTransform.offsetMin.y);
        }
    } 
    public float OffsetMinY
    {
        get
        {
            return RectTransform.offsetMin.y;
        }
        set
        {
            RectTransform.offsetMin = new Vector2(RectTransform.offsetMin.x, -value);
        }
    } 
}
