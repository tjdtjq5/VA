using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    const string rootName = "======UI======";
    int _order = 10;

    GameObject rootGo = null;

    Stack<UIPopup> _popupStack = new Stack<UIPopup>(); 
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

    public void SetCanvas(GameObject go, bool sort = true)
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

        GameObject go = Managers.Resources.Instantiate($"Prepab/UI/Popup/{name}");
        T popup = UnityHelper.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(RootGo.transform);

        return popup;
    }
    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UIPopup popup = _popupStack.Pop();

        Managers.Resources.Destroy(popup.gameObject);

        popup = null;

        _order--;
    }
    public void ClosePopupUI(UIPopup popup)
    {
        if (_popupStack.Count == 0)
            return;

        if (_popupStack.Peek() != popup)
        {
            UnityHelper.LogError_H($"UIManager ClosePopupUI Failed");
            return;
        }

        ClosePopupUI();
    }
    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0) 
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
