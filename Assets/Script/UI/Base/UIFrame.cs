using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFrame : UIBase
{
    Dictionary<Type, string> bindDics = new Dictionary<Type, string>();
    Dictionary<string, List<string>> enumDics = new Dictionary<string, List<string>>();

    [Button]
    public virtual void BindEnumCreate()
    {
        ChildCheckAndAddUIBaseComponent();
        SetBindDics();

        if (enumDics.Count <= 0 || bindDics.Count <= 0)
            return;

        foreach (var enumData in enumDics)
        {
            InnerEmumFormat.Set(this.GetType(), enumData.Key, enumData.Value.ToArray());
        }

        UIFrameInitFormat.Set(this.GetType(), bindDics);
    }
    public void ChildCheckAndAddUIBaseComponent()
    {
        List<RectTransform> childs = UnityHelper.FlindChilds<RectTransform>(this.gameObject, true);
        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];

            if (child.GetComponent<Image>())
            {
                UnityHelper.GetOrAddComponent<UIImage>(child.gameObject);
            }
            if (child.GetComponent<Text>())
            {
                UnityHelper.GetOrAddComponent<UIText>(child.gameObject);
            }
            if (child.GetComponent<Button>())
            {
                UnityHelper.GetOrAddComponent<UIButton>(child.gameObject);
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
    }
    public void SetBindDics()
    {
        enumDics.Clear();
        AddChildPath(null, this.transform);
        // AddRootPath();
    }
    void AddChildPath(string parentsName, Transform parents)
    {
        List<RectTransform> childs = UnityHelper.FlindChilds<RectTransform>(parents.gameObject);

        foreach (RectTransform child in childs)
        {
            string current = $"{parentsName}/{child.name}";
            if (string.IsNullOrEmpty(parentsName))
                current = $"{child.name}";
            else
                current = $"{parentsName}/{child.name}";

            if (child.GetComponent<UIImage>())
            {
                string key = $"{nameof(UIImage) + "E"}";
                BindDicsAdd(typeof(UIImage), key);
                EnumDicsAdd(key, current);
            }
            if (child.GetComponent<UIText>())
            {
                string key = $"{nameof(UIText) + "E"}";
                BindDicsAdd(typeof(UIText), key);
                EnumDicsAdd(key, current);
            }
            if (child.GetComponent<UISlider>())
            {
                string key = $"{nameof(UISlider) + "E"}";
                BindDicsAdd(typeof(UISlider), key);
                EnumDicsAdd(key, current);
            }
            if (child.GetComponent<UIToggle>())
            {
                string key = $"{nameof(UIToggle) + "E"}";
                BindDicsAdd(typeof(UIToggle), key);
                EnumDicsAdd(key, current);
            }
            if (child.GetComponent<UIScrollbar>())
            {
                string key = $"{nameof(UIScrollbar) + "E"}";
                BindDicsAdd(typeof(UIScrollbar), key);
                EnumDicsAdd(key, current);
            }
            if (child.GetComponent<UIButton>())
            {
                string key = $"{nameof(UIButton) + "E"}";
                BindDicsAdd(typeof(UIButton), key);
                EnumDicsAdd(key, current);
            }
            if (child.GetComponent<UIScrollView>())
            {
                string key = $"{nameof(UIScrollView) + "E"}";
                BindDicsAdd(typeof(UIScrollView), key);
                EnumDicsAdd(key, current);
            }

            if (child.GetComponent<UIFrame>())
            {
                continue;
            }

            AddChildPath(current, child);
        }
    }
    void AddRootPath()
    {
        if (this.GetComponent<UIImage>())
        {
            string key = $"{nameof(UIImage) + "E"}";
            BindDicsAdd(typeof(UIImage), key);
            EnumDicsAdd(key, nameof(UIImage));
        }
        if (this.GetComponent<UIText>())
        {
            string key = $"{nameof(UIText) + "E"}";
            BindDicsAdd(typeof(UIText), key);
            EnumDicsAdd(key, nameof(UIText));
        }
        if (this.GetComponent<UISlider>())
        {
            string key = $"{nameof(UISlider) + "E"}";
            BindDicsAdd(typeof(UISlider), key);
            EnumDicsAdd(key, nameof(UISlider));
        }
        if (this.GetComponent<UIToggle>())
        {
            string key = $"{nameof(UIToggle) + "E"}";
            BindDicsAdd(typeof(UIToggle), key);
            EnumDicsAdd(key, nameof(UIToggle));
        }
        if (this.GetComponent<UIScrollbar>())
        {
            string key = $"{nameof(UIScrollbar) + "E"}";
            BindDicsAdd(typeof(UIScrollbar), key);
            EnumDicsAdd(key, nameof(UIScrollbar));
        }
        if (this.GetComponent<UIScrollView>())
        {
            string key = $"{nameof(UIScrollView) + "E"}";
            BindDicsAdd(typeof(UIScrollView), key);
            EnumDicsAdd(key, nameof(UIScrollView));
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
}
