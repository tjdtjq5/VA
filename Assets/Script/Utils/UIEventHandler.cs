using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> eventHandlerClick;
    public Action<PointerEventData> eventHandlerOnDrag;

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
}
