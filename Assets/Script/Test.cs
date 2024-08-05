using Best.ServerSentEvents;
using EasyButtons;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    EventSource _sse;

    [Button]
    public void Json()
    {
        SseMessageResponse response = new SseMessageResponse();
        response.Id = $"ididid";
        response.Data = "message 11111";
        response.Event = "Evet 11111";

        UnityHelper.LogSerialize(response);
    }

    [Button]
    public void SseOpen()
    {
        _sse = new EventSource(new Uri($"{GameOptionManager.GetCurrentServerUrl}/Sse/Connect"));
        _sse.Open();

        _sse.OnOpen += OnEventSourceOpened;

        _sse.OnMessage += OnEventSourceMessage;

        _sse.OnError += OnEventSourceError;

        _sse.OnRetry += OnEventSourceRetry;
    }

    [Button]
    public void SseClose()
    {
        _sse.Close();
    }

    [Button]
    public void SendMessage()
    {
        SseMessageResponse message = new SseMessageResponse()
        {
             Id = "1123123213213",
             Data = "message 11111",
             Event = "Evet 11111",
        };

        Managers.Web.SendPostRequest<string>("Sse/Message", message, (res) => 
        {
        });
    }

    void OnEventSourceOpened(EventSource source)
    {
        UnityHelper.Log_H("Connection established!");
    }
    void OnEventSourceMessage(EventSource source, Message msg)
    {
        UnityHelper.Log_H($"Received LastEventId: {source.LastEventId}");
        UnityHelper.Log_H($"Received Data: {msg.Data}");
        UnityHelper.Log_H($"Received Id: {msg.Id}");
        UnityHelper.Log_H($"Received Event: {msg.Event}");
    }
    void OnEventSourceError(EventSource source, string error)
    {
        UnityHelper.Log_H($"Error encountered: {error}");
    }
    bool OnEventSourceRetry(EventSource source)
    {
        UnityHelper.Log_H("Attempting reconnection...");
        return true;  // Allow reconnection. Returning false will prevent retry.
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
