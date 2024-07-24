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
        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.GooglePlayGames,
            Token = "To",
        };

        Managers.Web.SendPostRequest<AccountLoginRequest>("account/login", req, (res) =>
        {
            UnityHelper.LogSerialize(res);
        });
    }

    [Button]
    public async void Gpgs2()
    {
        AccountLoginRequest req = new AccountLoginRequest()
        {
            ProviderType = ProviderType.GooglePlayGames,
            Token = "To",
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
