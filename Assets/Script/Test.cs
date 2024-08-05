using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    Dictionary<int, int> dats = new Dictionary<int, int>();
    [Button]
    public void Credentials(bool flag)
    {
        Debug.Log(1111111);
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
