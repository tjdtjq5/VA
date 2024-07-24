using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [Button]
    public void Gpgs()
    {
        Managers.Web.SendGetRequest<string>("", (res) => { UnityHelper.Log_H(res); });
    }

    Stack<GameObject> testCharacterList = new Stack<GameObject>();    
    [Button]
    public void Create()
    {
        testCharacterList.Push(Managers.Resources.Instantiate("Prepab/Character/TestCharacter"));
    }

    [Button]
    public void Delete()
    {
        Managers.Resources.Destroy(testCharacterList.Pop());
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
