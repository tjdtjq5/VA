using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabButtonParent : MonoBehaviour
{
    int currentIndex = 0;
    List<UITabButton> tabs = new();

    [SerializeField] bool isAllOff;

    private void Awake()
    {
        Initialize();

        currentIndex = isAllOff ? -1 : 0;
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
        if (isAllOff && index == currentIndex)
            index = -1;

        for (int i = 0; i < tabs.Count; i++)
            tabs[i].SwitchTab(i == index);

        currentIndex = index;
    }
}
