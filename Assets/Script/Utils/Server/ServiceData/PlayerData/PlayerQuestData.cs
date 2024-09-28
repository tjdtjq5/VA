using System;

[Serializable]
public class PlayerQuestData
{
    public string Code { get; set; }
    public int Level { get; set; }
    public DateTime ClearDate { get; set; }
}