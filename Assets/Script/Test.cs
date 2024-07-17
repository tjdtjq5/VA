using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour
{
    [ContextMenu("T")]  
    public void T()
    {
        Managers.UI.ShopPopupUI<UI_Login>("UI_Login");
    }

    [ContextMenu("Create")]
    public void Create()
    {
        Managers.UI.ShopPopupUI<UI_Login>("UI_Login");
    }

    [ContextMenu("Delete")]
    public void Delete()
    {
        Managers.UI.ClosePopupUI();
    }
}
[System.Serializable]
public class TestA
{
    public string itemCode { get; set; }
    public int itemType { get; set; }
    public string tipName { get; set; }
    public string tipName2 { get; set; }
}
