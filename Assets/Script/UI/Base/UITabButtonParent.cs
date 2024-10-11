using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UITabButtonParent : UIFrame
{
    List<UITabButton> tabs = new();

    Dictionary<int, Action<int>> SwitchOnHandler { get; set; } = new();
    Dictionary<int, Action<int>> SwitchOffHandler { get; set; } = new();

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
    void SwitchOn(int index)
    {
        for(int i = 0;i < tabs.Count; i++)
        {
            if (tabs[i].Index != index)
                tabs[i].Switch(false);
        }

        if(SwitchOnHandler.ContainsKey(index))
            SwitchOnHandler[index]?.Invoke(index);
    }
    void SwitchOff(int index)
    {
        if (SwitchOffHandler.ContainsKey(index))
            SwitchOffHandler[index]?.Invoke(index);
    }
}
