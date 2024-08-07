using EasyButtons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [Button]
    public void AdminResponse()
    {
        Managers.Web.SendGetRequest<string>("Test/AdminResponse", (res) => 
        {
            UnityHelper.Log_H(res);
        });
    }

    [Button]
    public void LoginResponse()
    {
        Managers.Web.SendGetRequest<string>("Test/LoginCheck", (res) =>
        {
            UnityHelper.Log_H(res);
        });
    }

    [Button]
    public void ChatConnect()
    {
        Managers.Chat.ConnectChanel(ChatChannel.All, ListenAction);
        Managers.Chat.SetChanelCycle(ChatChannel.All, ChatFrequencyCycle.Often);
    }

    [Button]
    public void ChatSend(string msg)
    {
        Managers.Chat.ChatSend(ChatChannel.All, msg);
    }

    void ListenAction(List<ChatData> chatDatas)
    {
        UnityHelper.LogSerialize(chatDatas);
    }
}
