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
    public void Push()
    {
       
    }
    [Button]
    public void Flush()
    {
        jobSerializer.Flush();
    }

    void DebugTime(string data)
    {
        UnityHelper.Log_H(data);
    }

    void DebugA()
    {
        UnityHelper.Log_H("A");
    }
    void DebugB()
    {
        UnityHelper.Log_H("B");
    }
    void DebugC()
    {
        UnityHelper.Log_H("C");
    }

    void Debug(int _data)
    {
        UnityHelper.Log_H(_data);
    }
    void Debug(float _data)
    {
        UnityHelper.Log_H(_data);
    }
    void Debug(string _data)
    {
        UnityHelper.Log_H(_data);
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
