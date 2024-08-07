using System.Collections.Generic;
public class ChatSendRequest
{
    public ChatChannel Channel { get; set; }
    public ChatData ChatData { get; set; }
}
public class ChatGetRequest
{
    public ChatChannel Channel { get; set; }
}
public class ChatResponse
{
    public List<ChatData> ChatDatas { get; set; }
}
public class ChatData
{
    public int AccountId { get; set; }
    public string Message { get; set; }
}
