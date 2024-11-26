using EasyButtons;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFrame : UIBase
{
    Dictionary<Type, string> bindDics = new Dictionary<Type, string>();
    Dictionary<string, List<string>> enumDics = new Dictionary<string, List<string>>();

    [Button]
    public virtual void BindEnumCreate()
    {
        ChildNameSetting();
        ChildCheckAndAddUIBaseComponent(this.transform);
        SetBindDics();

        if (enumDics.Count <= 0 || bindDics.Count <= 0)
            return;
         
        foreach (var enumData in enumDics)
        {
            InnerEmumFormat.Set(this.GetType(), enumData.Key, enumData.Value.ToArray());
        }

        UIFrameInitFormat.Set(this.GetType(), bindDics);
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
    }
    public void ChildCheckAndAddUIBaseComponent(Transform parents)
    {
        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(parents.gameObject);

        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];

            if (child != parents && child.GetComponentInChildren<UIFrame>())
            {
                continue;
            }

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

            ChildCheckAndAddUIBaseComponent(child);
        }
    }
    public void SetBindDics()
    {
        enumDics.Clear();
        AddChildPath(null, this.transform);
    }
    void AddChildPath(string parentsName, Transform parents)
    {
        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(parents.gameObject);

        foreach (RectTransform child in childs)
        {
            string childName = child.name;
            string current = $"{parentsName}/{child.name}";

            if (string.IsNullOrEmpty(parentsName))
                current = $"{childName}";
            else
                current = $"{parentsName}/{childName}";

            Component components = child.GetComponent(typeof(UIBase));

            if (components)
            {
                string key = $"{components.GetType() + "E"}";
                BindDicsAdd(components.GetType(), key);
                EnumDicsAdd(key, current);
            }

            if (!child.GetComponent(typeof(UIFrame)))
                AddChildPath(current, child);
        }
    }
    void AddRootPath()
    {
        Component components = this.GetComponent(typeof(UIBase));

        if (components)
        {
            string key = $"{components.GetType() + "E"}";
            BindDicsAdd(components.GetType(), key);
            EnumDicsAdd(key, components.GetType().Name);
        }
    }
    void EnumDicsAdd(string key, string value)
    {
        value = value.Trim().Replace('/', '_');

        if (enumDics.ContainsKey(key))
        {
            enumDics[key].Add(value);
        }
        else
        {
            enumDics.Add(key, new List<string>() { value });
        }
    }
    void BindDicsAdd(Type key, string value)
    {
        if (!bindDics.ContainsKey(key))
            bindDics.Add(key, value);
    }

    [Button]
    public void Test()
    {
        foreach (var t in this.enumDics)
        {
            UnityHelper.Log_H($"====={t.Key}=====");
            for (var i = 0; i < t.Value.Count; i++)
            {
                UnityHelper.Log_H($"{t.Value[i]}");
            }
        }
    }
}
