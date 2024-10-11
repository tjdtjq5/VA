using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabButtonParent : MonoBehaviour
{
    List<UITabButton> tabs = new();

    Dictionary<int, Action<int>> SwitchOnHandler { get; set; }
    Dictionary<int, Action<int>> SwitchOffHandler { get; set; }

    [SerializeField] bool isAllOff;

    private void Awake()
    {
        Initialize();
    }
    void Initialize()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            int index = i;
            UITabButton uITabButton = this.transform.GetChild(i).GetComponent<UITabButton>();
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
    }
    void SwitchOff(int index)
    {
    }
}
