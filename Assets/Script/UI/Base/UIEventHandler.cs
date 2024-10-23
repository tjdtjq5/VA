using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEventHandler : UIBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<PointerEventData> eventHandlerClick;
    public Action<PointerEventData> eventHandlerPointDown;
    public Action<PointerEventData> eventHandlerPointUp;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventHandlerClick != null)
            eventHandlerClick.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventHandlerPointDown != null)
            eventHandlerPointDown.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventHandlerPointUp != null)
            eventHandlerPointUp.Invoke(eventData);
    }
}
