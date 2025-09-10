using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISlider : UIFrame
{
    public Slider Slider
    {
        get
        {
            return GetComponent<Slider>();
        }
    }
    public float value
    {
        get => Slider.value;
        set
        {
            Slider.value = value;
        }
    }
    
    #if UNITY_EDITOR
    [Button]
    public void InspectorSetting()
    {
        UnityHelper.Log_H($"Handle Slide Area 수동 제거 필요");
        
        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(this.gameObject, true);
        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];
            string n = child.gameObject.name;
            child.gameObject.name = n.Replace(" ", "").Replace("_","").Replace("(Legacy)", "");
        }
        
        RectTransform fillArea = this.gameObject.FindChildByPath<RectTransform>("FillArea");
        fillArea.sizeDelta = Vector2.zero;
        fillArea.localPosition = Vector2.zero;
        
        RectTransform fill = this.gameObject.FindChildByPath<RectTransform>("FillArea/Fill");
        fill.sizeDelta = Vector2.zero;
        
        Slider.transition = Selectable.Transition.None; 
        Slider.interactable = false;
             
#if UNITY_EDITOR
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
#endif
        
    } 
    #endif
}
