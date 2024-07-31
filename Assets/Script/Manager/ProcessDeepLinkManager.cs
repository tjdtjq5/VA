using System;
using UnityEngine;

public class ProcessDeepLinkManager
{
    public void AddAction(System.Action<string> addEvent)
    {
        Application.deepLinkActivated -= addEvent;
        Application.deepLinkActivated += addEvent;

        if (!String.IsNullOrEmpty(Application.absoluteURL))
        {
            addEvent(Application.absoluteURL);
        }
    }
}
