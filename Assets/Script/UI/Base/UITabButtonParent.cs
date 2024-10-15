using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UITabButtonParent : UIFrame
{
    List<UITabButton> tabs = new();

    public Action<int> SwitchOnHandler;
    public Action<int> SwitchOffHandler;

    [SerializeField] bool isAllOff;

    protected override void Initialize()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            int index = i;
            UITabButton uITabButton = this.transform.GetChild(i).GetOrAddComponent<UITabButton>();
            uITabButton.Set(index, isAllOff);
            uITabButton.SwitchOnHandler += SwitchOn;
            uITabButton.SwitchOffHandler += SwitchOff;
            tabs.Add(uITabButton);
        }
    }
    public void UISet(int index)
    {
        SwitchOn(index);
    }
    void SwitchOn(int index)
    {
        for(int i = 0;i < tabs.Count; i++)
            tabs[i].Switch(tabs[i].Index == index);

        if (SwitchOnHandler != null)
            SwitchOnHandler.Invoke(index);
    }
    void SwitchOff(int index)
    {
        if (SwitchOffHandler != null)
            SwitchOffHandler.Invoke(index);
    }
}
