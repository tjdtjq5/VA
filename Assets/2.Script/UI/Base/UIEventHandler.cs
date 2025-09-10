using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventHandler : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action<PointerEventData> EventHandlerClick;
    public Action<PointerEventData> EventHandlerPointDown;
    public Action<PointerEventData> EventHandlerPointUp;
    public Action<PointerEventData> EventHandlerTrigger;
    public Action<PointerEventData> EventHandlerEnter;
    public Action<PointerEventData> EventHandlerExit;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (EventHandlerClick != null)
            EventHandlerClick.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (EventHandlerPointDown != null)
            EventHandlerPointDown.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (EventHandlerPointUp != null)
            EventHandlerPointUp.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventHandlerEnter != null)
            EventHandlerEnter.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (EventHandlerExit != null)
            EventHandlerExit.Invoke(eventData);
    }
}
