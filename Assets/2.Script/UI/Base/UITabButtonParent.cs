using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class UITabButtonParent : UIFrame
{
    [SerializeField] Transform tabContainer;
    [SerializeField] bool isAllOff;
    
    List<UITabButton> tabs = new();
    public int Index {  get; set; }
    public Action<int> SwitchOnHandler;
    public Action<int> SwitchOffHandler;

    protected virtual float TabWidthMin => 225f;

    protected override void Initialize()
    {
        for (int i = 0; i < tabContainer.childCount; i++)
        {
            int index = i;
            UITabButton uITabButton = tabContainer.GetChild(i).GetOrAddComponent<UITabButton>();
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
        if (index != Index)
            SwitchOff(Index);

        Index = index;

        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i].Index != index)
                tabs[i].Switch(false);
        }
        for (int i = 0; i < tabs.Count; i++)
        {
            if (tabs[i].Index == index)
                tabs[i].Switch(true);
        }

        if (SwitchOnHandler != null)
            SwitchOnHandler.Invoke(index);
    }
    void SwitchOff(int index)
    {
        if (SwitchOffHandler != null)
            SwitchOffHandler.Invoke(index);
    }

    #if UNITY_EDITOR
    [Button]
    public void InspectorSetting()
    {
        float totalWidth = this.transform.GetComponent<RectTransform>().sizeDelta.x;
        float width = Mathf.Clamp(totalWidth / tabContainer.childCount, TabWidthMin, totalWidth);
        
        for (int i = 0; i < tabContainer.childCount; i++)
        {
            Transform child = tabContainer.GetChild(i); 
            child.GetComponent<RectTransform>().sizeDelta = new Vector2(width, child.GetComponent<RectTransform>().sizeDelta.y);
        }
        
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
    }
    #endif
}
