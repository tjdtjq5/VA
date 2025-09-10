using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UIFrame : UIBase
{
    private Dictionary<Type, string> _bindDics = new Dictionary<Type, string>();
    private Dictionary<string, List<string>> _enumDics = new Dictionary<string, List<string>>();

    [Button]
    public virtual void BindEnumCreate()
    {
        ChildNameSetting();
        ChildCheckAndAddUIBaseComponent(this.transform);
        SetBindDics();

        if (_enumDics.Count <= 0 || _bindDics.Count <= 0)
            return;
         
        foreach (var enumData in _enumDics)
        {
            InnerEmumFormat.Set(this.GetType(), enumData.Key, enumData.Value.ToArray());
        }

        UIFrameInitFormat.Set(this.GetType(), _bindDics);
    }

    public void ChildNameSetting()
    {
        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(this.gameObject, true);
        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];
            string n = child.gameObject.name;
            child.gameObject.name = n.Replace(" ", "").Replace("_","").Replace("(Legacy)", "");
        }
        
        #if UNITY_EDITOR
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
        #endif
    }

    public void ChildCheckAndAddUIBaseComponent(Transform parents)
    {
        if (parents == null)
            return;

        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(parents.gameObject);

        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];
            if (child == null)
                continue;

            if (!child.GetComponent<UIFrame>())
            {
                if (child.GetComponent<Image>())
                {
                    UnityHelper.GetOrAddComponent<UIImage>(child.gameObject);
                }
                if (child.GetComponent<Text>())
                {
                    UnityHelper.GetOrAddComponent<UIText>(child.gameObject);
                }
                if (child.GetComponent<TextMeshProUGUI>())
                {
                    UnityHelper.GetOrAddComponent<UITextPro>(child.gameObject);
                }
                if (child.GetComponent<Slider>())
                {
                    UnityHelper.GetOrAddComponent<UISlider>(child.gameObject);
                }
                if (child.GetComponent<Toggle>())
                {
                    UnityHelper.GetOrAddComponent<UIToggle>(child.gameObject);
                }
                if (child.GetComponent<Scrollbar>())
                {
                    UnityHelper.GetOrAddComponent<UIScrollbar>(child.gameObject);
                }
            }

            ChildCheckAndAddUIBaseComponent(child);
        }
    }

    public void SetBindDics()
    {
        _enumDics.Clear();
        _bindDics.Clear();
        AddChildPath(null, this.transform);
    }

    private void AddChildPath(string parentsName, Transform parents)
    {
        if (parents == null)
            return;

        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(parents.gameObject);

        foreach (RectTransform child in childs)
        {
            if (child == null)
                continue;

            string childName = child.name;
            string current = string.IsNullOrEmpty(parentsName) ? childName : $"{parentsName}/{childName}";

            Component components = child.GetComponent(typeof(UIBase));

            if (components != null)
            {
                string key = $"{components.GetType()}E";
                BindDicsAdd(components.GetType(), key);
                EnumDicsAdd(key, current);
            }

            if (!child.GetComponent(typeof(UIFrame)))
                AddChildPath(current, child);
        }
    }

    private void AddRootPath()
    {
        Component components = this.GetComponent(typeof(UIBase));

        if (components != null)
        {
            string key = $"{components.GetType()}E";
            BindDicsAdd(components.GetType(), key);
            EnumDicsAdd(key, components.GetType().Name);
        }
    }

    private void EnumDicsAdd(string key, string value)
    {
        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
            return;

        value = value.Trim().Replace('/', '_');

        if (_enumDics.ContainsKey(key))
        {
            _enumDics[key].Add(value);
        }
        else
        {
            _enumDics.Add(key, new List<string>() { value });
        }
    }

    private void BindDicsAdd(Type key, string value)
    {
        if (key == null || string.IsNullOrEmpty(value))
            return;

        if (!_bindDics.ContainsKey(key))
            _bindDics.Add(key, value);
    }
}
