using System.Collections.Generic;
public class PlayerQuestClearRequest
{
    public string Code { get; set; }
}
public class PlayerQuestClearResponse
{
    public string Code { get; set; }
    public List<PlayerItemData> RewardItems { get; set; } = new List<PlayerItemData>();
}