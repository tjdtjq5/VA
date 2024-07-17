using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Action<PointerEventData> eventHandlerClick;
    public Action<PointerEventData> eventHandlerOnDrag;
    public Action<PointerEventData> eventHandlerPointDown;
    public Action<PointerEventData> eventHandlerPointUp;

    public void OnDrag(PointerEventData eventData)
    {
        if (eventHandlerOnDrag != null)
            eventHandlerOnDrag.Invoke(eventData);
    }

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
