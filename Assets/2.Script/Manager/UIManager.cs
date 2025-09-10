using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    const string rootName = "======UI======";
    Dictionary<CanvasOrderType, int> _order = new();
    int orderLayer = 1000;

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

    public void SetPopupCanvas(GameObject go, CanvasOrderType orderType)
    {
        Canvas canvas = UnityHelper.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if(_order.TryGetValue(orderType, out int value))
        {
            int next = value + 1;
            _order[orderType] = next;
            canvas.sortingOrder = next;
        }
        else
        {
            int startOrder = (int)orderType * orderLayer;
            _order.Add(orderType, startOrder);
            canvas.sortingOrder = startOrder;
        }
    }
    public T ShopPopupUI<T>(string name, CanvasOrderType orderType, Transform root = null) where T : UIPopup
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
        popup.OpenUISet(orderType);

        go.SetActive(true);
        
        if (root == null)
            go.transform.SetParent(RootGo.transform);
        else
            go.transform.SetParent(root);

        return popup;
    }
    public T ShopPopupUI<T>(UIPopup prepfab, CanvasOrderType orderType, Transform root = null) where T : UIPopup
    {
        GameObject go = null;
        if (_popupDics.ContainsKey(prepfab.name))
            go = _popupDics[prepfab.name];
        else
        {
            go = Managers.Resources.Instantiate(prepfab.gameObject);
            _popupDics.Add(prepfab.name, go);
        }

        _popupNameStack.Add(prepfab.name);

        T popup = UnityHelper.GetOrAddComponent<T>(go);
        popup.OpenUISet(orderType);

        if (root == null)
            go.transform.SetParent(RootGo.transform);
        else
            go.transform.SetParent(root);

        return popup;
    }
    public void ClosePopupUI()
    {
        if (_popupDics.Count == 0)
            return;

        string popupName = _popupNameStack[LastIndex];
        ClosePopupUI(popupName);
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
public enum CanvasOrderType
{
    Underground = -1,
    Bottom,
    Middle,
    Top
}
