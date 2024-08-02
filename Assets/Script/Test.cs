using EasyButtons;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Button]
    public void Gpgs()
    {
    }
    [Button]
    public async void Gpgs2()
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
