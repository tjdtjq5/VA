using EasyButtons;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Test : MonoBehaviour
{
    [Button]
    public void Gpgs()
    {
    }

    [Button]
    public async void Gpgs2()
    {
        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.GooglePlayGames,
            NetworkIdOrCode = "To",
        };

        AccountLoginRequest a = await WebTaskCall.Post<AccountLoginRequest>(true,"account/login", req);
        UnityHelper.LogSerialize(a);
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
