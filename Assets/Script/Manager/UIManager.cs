using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    const string rootName = "======UI======";
    int _order = 100;

    GameObject rootGo = null;

    Dictionary<string, GameObject> _popupDics = new Dictionary<string, GameObject>();
    List<string> _popupNameStack = new List<string>();
    GameObject RootGo
    {
        get
        {
            if (rootGo == null)
            {
                rootGo = GameObject.Find(rootName);

                if (rootGo == null)
                {
                    rootGo = new GameObject { name = rootName };
                }
            }

            return rootGo;
        }
    } 
    int Count => _popupDics.Count;
    int LastIndex => Count - 1;

    public void SetPopupCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = UnityHelper.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T ShopPopupUI<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;
        }

        GameObject go = null;
        if (_popupDics.ContainsKey(name))
            go = _popupDics[name];
        else
        {
            go = Managers.Resources.Instantiate($"Prefab/UI/Popup/{name}");
            _popupDics.Add(name, go);
        }

        _popupNameStack.Add(name);

        T popup = UnityHelper.GetOrAddComponent<T>(go);

        go.transform.SetParent(RootGo.transform);

        return popup;
    }
    public void ClosePopupUI()
    {
        if (_popupDics.Count == 0)
            return;

        string popupName = _popupNameStack[LastIndex];
        ClosePopupUI(popupName);
        _popupNameStack.RemoveAt(LastIndex);
    }
    public void ClosePopupUI(string name)
    {
        if (_popupDics.ContainsKey(name))
        {
            Managers.Resources.Destroy(_popupDics[name]);
            _popupDics.Remove(name);
        }

        if (_popupNameStack.Contains(name))
            _popupNameStack.Remove(name);
    }
    public void ClosePopupUI(UIPopup popup)
    {
        foreach (var go in _popupDics) 
        {
            if (go.Value.gameObject == popup.gameObject)
            {
                ClosePopupUI(go.Key);
                break;
            }
        }
    }
    public void CloseAllPopupUI()
    {
        while (_popupDics.Count > 0) 
        {
            ClosePopupUI();
        }
    }
    public void Clear()
    {
        CloseAllPopupUI();
        rootGo = null;
    }
}
