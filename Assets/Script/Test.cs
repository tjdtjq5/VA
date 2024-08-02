using EasyButtons;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    JobSerializer jobSerializer = new JobSerializer();

    [Button]
    public void SendDataTest(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AccountLoginRequest req = new AccountLoginRequest()
            {
                NetworkIdOrCode = $"data_{i}"
            };

            Managers.Web.SendPostRequest<AccountLoginResponce>("Test/Log", req, (res) =>
            {
                UnityHelper.Log_H(res.JwtAccessToken);
            });
        }
    }

    [Button]
    public void LoginCheck()
    {
        Managers.Web.SendPostRequest<bool>("Test/LoginCheck", null, (res) =>
        {
            UnityHelper.Log_H(res);
        });
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
