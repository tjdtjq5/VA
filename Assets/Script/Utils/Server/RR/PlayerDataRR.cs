using System.Collections.Generic;
public class PlayerDataGetsResponse
{
    public List<CharacterPlayerData> Characters { get; set; }
}
public class CPDLevelUpRequest
{
    public string Code { get; set; }
    public int PlusLevel { get; set; }
}
public class CPDLevelUpResponse
{
    public CharacterPlayerData LevelUpCharacterData { get; set; }
    public PlayerItemData RemainItems { get; set; }
}
public class CPDAwakeUpRequest
{
    public string Code { get; set; }
}
public class CPDAwakeUpResponse
{
    public CharacterPlayerData LevelUpCharacterData { get; set; }
    public PlayerItemData RemainItem { get; set; }
}
public class PCDAwakeAllRequest
{
    // None
}
public class CPDAwakeAllResponse
{
    public List<CharacterPlayerData> LevelUpCharacterDatas { get; set; } = new();
    public List<PlayerItemData> RemainItems { get; set; } = new();
}

public class CPDPotentialChangeRequest
{
    public string Code { get; set; }
    public int LockCount { get; set; }
}
public class CPDPotentialChangeResponse
{
    public List<string> PotentialKeys { get; set; } = new();
    public PlayerItemData RemainItem { get; set; }
}
public class CPDSpecialWeaponLevelUpRequest
{
    public string Code { get; set; }
    public int PlusLevel { get; set; }
}
public class CPDSpecialWeaponLevelUpResponse
{
    public CharacterPlayerData LevelUpCharacterData { get; set; }
    public PlayerItemData RemainItems { get; set; }
}