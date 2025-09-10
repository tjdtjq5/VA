using System;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.Define;
using Shared.DTOs.Chat;

public class ChatManager
{
    Dictionary<ChatChannel, Action<List<ChatData>>> _chatAction = new Dictionary<ChatChannel, Action<List<ChatData>>>();
    ChatFrequencyCycle _defaultFrequencyCycle = ChatFrequencyCycle.Close;
    Dictionary<ChatChannel, ChatFrequencyCycle> _chanelFrequencyCycle = new Dictionary<ChatChannel, ChatFrequencyCycle>();
    Dictionary<ChatFrequencyCycle, float> _frequencyCycleTimer = new Dictionary<ChatFrequencyCycle, float>();

    float _oftenSec = 3f;
    float _sometimesSec = 20f;

    public void Initialize()
    {
        _frequencyCycleTimer.Clear();
        int len = CSharpHelper.GetEnumLength<ChatFrequencyCycle>();
        for (int i = 0; i < len; i++) 
        {
            ChatFrequencyCycle cycle = (ChatFrequencyCycle)i;
            _frequencyCycleTimer.Add(cycle, 0);
        }
    }

    public void ConnectChanel(ChatChannel chatChannel, Action<List<ChatData>> action)
    {
        _chatAction.TryAdd(chatChannel, action);
        _chanelFrequencyCycle.TryAdd(chatChannel, _defaultFrequencyCycle);
    }
    public void SetChanelCycle(ChatChannel chatChannel, ChatFrequencyCycle frequencyCycle)
    {
        if (_chanelFrequencyCycle.ContainsKey(chatChannel))
        {
            _chanelFrequencyCycle[chatChannel] = frequencyCycle;
        }
        else
        {
            _chanelFrequencyCycle.Add(chatChannel, frequencyCycle);
        }
    }

    public void ChatSend(ChatChannel chatChannel, string message)
    {
        ChatSendRequest req = new ChatSendRequest()
        {
            Channel = chatChannel,
            ChatData = new ChatData()
            {
                AccountId = Managers.Web.AccountId,
                Message = message
            }
        };

        Managers.Web.SendPostRequest<ChatResponse>("Chat/Send", req, (res) =>
        {
            ChatBroadcast(chatChannel, res.ChatDatas);
        });
    }

    void ChatGet(ChatChannel channel)
    {
        ChatGetRequest req = new ChatGetRequest()
        {
            Channel = channel,
        };

        Managers.Web.SendPostRequest<ChatResponse>("Chat/Get", req, (res) =>
        {
            ChatBroadcast(channel, res.ChatDatas);
        });
    }

    void ChatBroadcast(ChatChannel channel, List<ChatData> chatDatas)
    {
        if (chatDatas == null || chatDatas.Count <= 0)
        {
            return;
        }

        if (_chatAction.ContainsKey(channel))
        {
            
            _chatAction[channel].Invoke(chatDatas);
        }
    }

    public void OnFixedUpdate()
    {
        foreach (var cfc in _chanelFrequencyCycle)
        {
            ChatChannel channel = cfc.Key;
            ChatFrequencyCycle cycle = cfc.Value;

            _frequencyCycleTimer[cycle] += Managers.Time.FixedDeltaTime;

            switch (cycle)
            {
                case ChatFrequencyCycle.Often:

                    if (_frequencyCycleTimer[cycle] > _oftenSec)
                    {
                        _frequencyCycleTimer[cycle] = 0;

                        ChatGet(channel);
                    }

                    break;
                case ChatFrequencyCycle.Sometimes:

                    if (_frequencyCycleTimer[cycle] > _sometimesSec)
                    {
                        _frequencyCycleTimer[cycle] = 0;

                        ChatGet(channel);
                    }

                    break;
            }
        }
    }

    public void Clear()
    {
        _chatAction.Clear();
    }

}
