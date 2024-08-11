using System;
using UnityEngine;
using ImaginationOverflow.UniversalDeepLinking;

public class ProcessDeepLinkManager
{
    public void AddAction(LinkActivationHandler s)
    {
        DeepLinkManager.Instance.LinkActivated += s;
    }
}
