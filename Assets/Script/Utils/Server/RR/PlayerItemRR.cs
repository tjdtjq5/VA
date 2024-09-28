using System.Collections.Generic;

public class PlayerItemGetResponse
{
    public List<PlayerItemData> Datas { get; set; } = new();
    public void Push(PlayerItemData item) => Datas.Add(item);
}
public class PlayerItemPushRequest
{
    public List<PlayerItemData> Datas { get; set; } = new();
    public void Push(PlayerItemData item) => Datas.Add(item);
}
public class PlayerItemPushResponse
{
    public List<PlayerItemData> Datas { get; set; } = new();
    public void Push(PlayerItemData item) => Datas.Add(item);
}