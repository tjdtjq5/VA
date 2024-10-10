using System;
using System.Collections.Generic;
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

        objectDics.Add(typeof(T), objs);

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
                case nameof(UIInputField):
                    objs[i] = gameObject.GetComponent<UIInputField>();
                    continue;
            }

            if (typeof(T) == typeof(GameObject))
                objs[i] = UnityHelper.FindChildPath<T>(gameObject, path);
            else
                objs[i] = UnityHelper.FindChildPath<T>(gameObject, path);
        }
    }
    protected T Get<T>(Enum _enumValue) where T : UIBase
    {
        UIBase[] objs = null;

        if (objectDics.TryGetValue(typeof(T), out objs) == false)
            return null;

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
    protected UIInputField GetInputField(Enum _enumValue)
    {
        return Get<UIInputField>(_enumValue);
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

    public RectTransform RectTransform
    {
        get
        {
            return UnityHelper.GetOrAddComponent<RectTransform>(this.gameObject);
        }
    }

}
