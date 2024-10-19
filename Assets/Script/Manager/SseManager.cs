using Best.ServerSentEvents;
using System;
using System.Collections.Generic;

public class SseManager
{
    EventSource _sse;
    Dictionary<SseEvent, Action<string>> _eventActions = new Dictionary<SseEvent, Action<string>>();

    public void Initialize()
    {
        SseOpen();
    }
    void SseOpen()
    {
        _sse = new EventSource(new Uri($"{GameOptionManager.GetCurrentServerUrl}/Sse/Connect"));
        _sse.Open();

        _sse.OnOpen += OnEventSourceOpened;

        _sse.OnMessage += OnEventSourceMessage;

        _sse.OnError += OnEventSourceError;

        _sse.OnRetry += OnEventSourceRetry;
    }

    void SseClose()
    {
        _sse.Close();
    }

    void OnEventSourceOpened(EventSource source)
    {
        UnityHelper.Log_H("Connection established!");
    }
    void OnEventSourceMessage(EventSource source, Message msg)
    {
        SseMessageResponse sseMessage = CSharpHelper.DeserializeObject<SseMessageResponse>(msg.Data);
        if (sseMessage == null)
        {
            UnityHelper.Error_H($"Wrong Data SseMessage\nmsgData : {msg.Data}");
            return;
        }

        if (_eventActions.TryGetValue(sseMessage.Event, out Action<string> eventAction))
        {
            eventAction.Invoke(sseMessage.Data);
        }
    }
    void OnEventSourceError(EventSource source, string error)
    {
        UnityHelper.Log_H($"Error encountered: {error}");
    }
    bool OnEventSourceRetry(EventSource source)
    {
        UnityHelper.Log_H("Attempting reconnection...");
        return true;  
    }

    public void AddEventListen(SseEvent sseEvent, Action<string> action)
    {
        if (_eventActions.ContainsKey(sseEvent))
        {
            _eventActions[sseEvent] -= action;
            _eventActions[sseEvent] += action;
        }
        else
        {
            _eventActions.Add(sseEvent, action);
        }
    }
    public void Clear()
    {
        _eventActions.Clear();
    }
}
