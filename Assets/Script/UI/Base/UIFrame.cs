using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFrame : UIBase
{
    List<Type> _ignoreTypeList = new List<Type>() { typeof(UIPopup), typeof(UICard) };

    [Button]
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
    [Button]
    public void BindEnumCreate()
    {
        if (IsIgnoreType)
        {
            UnityHelper.LogError_H($"Ignore Type");
            return;
        }

        Dictionary<string, List<string>> enumDics = new Dictionary<string, List<string>>();
        Dictionary<Type, string> bindDics = new Dictionary<Type, string>();

        List<Transform> childs = UnityHelper.IgnoreFindChilds<UIFrame>(this.transform);
        for (int i = 0; i < childs.Count; i++)
        {
            Transform child = childs[i];
            string value = child.name.Trim().Replace(" ", "");

            if (child.GetComponent<UIImage>())
            {
                string key = nameof(UIImage) + "E";
                if (!bindDics.ContainsKey(typeof(Image)))
                    bindDics.Add(typeof(Image), key);

                if (enumDics.ContainsKey(key))
                {
                    enumDics[key].Add(value);
                }
                else
                {
                    enumDics.Add(key, new List<string>() { value });
                }
            }
            if (child.GetComponent<UIText>())
            {
                string key = nameof(UIText) + "E";
                if (!bindDics.ContainsKey(typeof(Text)))
                    bindDics.Add(typeof(Text), key);

                if (enumDics.ContainsKey(key))
                {
                    enumDics[key].Add(value);
                }
                else
                {
                    enumDics.Add(key, new List<string>() { value });
                }
            }
            if (child.GetComponent<UIButton>())
            {
                string key = nameof(UIButton) + "E";
                if (!bindDics.ContainsKey(typeof(Button)))
                    bindDics.Add(typeof(Button), key);

                if (enumDics.ContainsKey(key))
                {
                    enumDics[key].Add(value);
                }
                else
                {
                    enumDics.Add(key, new List<string>() { value });
                }
            }
            if (child.GetComponent<UISlider>())
            {
                string key = nameof(UISlider) + "E";
                if (!bindDics.ContainsKey(typeof(Slider)))
                    bindDics.Add(typeof(Slider), key);

                if (enumDics.ContainsKey(key))
                {
                    enumDics[key].Add(value);
                }
                else
                {
                    enumDics.Add(key, new List<string>() { value });
                }
            }
            if (child.GetComponent<UIToggle>())
            {
                string key = nameof(UIToggle) + "E";
                if (!bindDics.ContainsKey(typeof(Toggle)))
                    bindDics.Add(typeof(Toggle), key);

                if (enumDics.ContainsKey(key))
                {
                    enumDics[key].Add(value);
                }
                else
                {
                    enumDics.Add(key, new List<string>() { value });
                }
            }
            if (child.GetComponent<UIScrollbar>())
            {
                string key = nameof(UIScrollbar) + "E";
                if (!bindDics.ContainsKey(typeof(Scrollbar)))
                    bindDics.Add(typeof(Scrollbar), key);

                if (enumDics.ContainsKey(key))
                {
                    enumDics[key].Add(value);
                }
                else
                {
                    enumDics.Add(key, new List<string>() { value });
                }
            }
        }

        foreach (var enumData in enumDics)
        {
            InnerEmumFormat.Set(this.GetType(), enumData.Key, enumData.Value.ToArray());
        }

        UIFrameInitFormat.Set(this.GetType(), bindDics);
    }

    bool IsIgnoreType
    {
        get
        {
            return _ignoreTypeList.Contains(this.GetType());
        }
    }
}
