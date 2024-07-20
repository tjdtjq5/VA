using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : UIFrame
{
    Image Image
    {
        get
        {
            return GetComponent<Image>();
        }
    }

    private void Start()
    {
        Image.raycastTarget = true;
    }

    public void AddClickEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.Click);
    }
    public void AddPointDownEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.PointDown);
    }
    public void AddPointUpEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.PointUp);
    }
}
