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
    protected Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    protected AniController AniController;

    private void Start()
    {
        Image.raycastTarget = true;
    }

    protected override void Initialize()
    {
        if (Animator)
            AniController = Animator.Initialize();
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
    public void AddDragEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.Drag);
    }
}
