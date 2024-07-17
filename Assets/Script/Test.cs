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
        Debug.Log(FileHelper.SelectFilePath("C:\\workspace"));
    }

    [ContextMenu("Create")]
    public void Create()
    {
        ServerProgramPacket.AddSingleton("absc");
        ServerProgramPacket.AddScoped("abscc");
    }

    [ContextMenu("Delete")]
    public void Delete()
    {
        ServerProgramPacket.RemoveSingleton("absc");
        ServerProgramPacket.RemoveScoped("abscc");
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
