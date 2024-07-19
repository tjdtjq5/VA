using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Button]
    public void T()
    {
        
    }

    [Button]
    public void Create()
    {
        Dictionary<Type, string> bindDics = new Dictionary<Type, string>();
        bindDics.Add(typeof(string), "str");
        bindDics.Add(typeof(Image), "img");
        bindDics.Add(typeof(Text), "text");

        UIFrameInitFormat.Set(this.GetType(), bindDics);
    }

    [Button]
    public void Delete()
    {
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
