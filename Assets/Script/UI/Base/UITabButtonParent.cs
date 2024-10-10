using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabButtonParent : MonoBehaviour
{
    List<UITabButton> tabs = new();

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
            uITabButton.AddClickEvent((ped) => { SwitchTab(index); });
            tabs.Add(uITabButton);
        }
    }
    void SwitchTab(int index)
    {
        for (int i = 0; i < tabs.Count; i++)
            tabs[i].SwitchTab(i == index);
    }
}
