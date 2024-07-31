using ImaginationOverflow.UniversalDeepLinking;
using System;
using UnityEngine;

public class ProcessDeepLinkManager
{
    public void AddAction(LinkActivationHandler s)
    {
        ImaginationOverflow.UniversalDeepLinking.DeepLinkManager.Instance.LinkActivated += s;
    }
}
