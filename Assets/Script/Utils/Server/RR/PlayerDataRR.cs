using System.Collections.Generic;
public class PCDLevelUpRequest
{
    public string Code { get; set; }
    public int PlusLevel { get; set; }
}
public class PCDLevelUpResponse
{
    public PlayerCharacterData LevelUpCharacterData { get; set; }
    public PlayerItemData RemainItems { get; set; }
}
public class PCDAwakeUpRequest
{
    public string Code { get; set; }
}
public class PCDAwakeUpResponse
{
    public PlayerCharacterData LevelUpCharacterData { get; set; }
    public PlayerItemData RemainItem { get; set; }
}
public class PCDAwakeAllRequest
{
    // None
}
public class PCDAwakeAllResponse
{
    public List<PlayerCharacterData> LevelUpCharacterDatas { get; set; } = new();
    public List<PlayerItemData> RemainItems { get; set; } = new();
}

public class PCDPotentialChangeRequest
{
    public string Code { get; set; }
    public int LockCount { get; set; }
}
public class PCDPotentialChangeResponse
{
    public List<string> PotentialKeys { get; set; } = new();
    public PlayerItemData RemainItem { get; set; }
}
public class PCDSpecialWeaponLevelUpRequest
{
    public string Code { get; set; }
    public int PlusLevel { get; set; }
}
public class PCDSpecialWeaponLevelUpResponse
{
    public PlayerCharacterData LevelUpCharacterData { get; set; }
    public PlayerItemData RemainItems { get; set; }
}