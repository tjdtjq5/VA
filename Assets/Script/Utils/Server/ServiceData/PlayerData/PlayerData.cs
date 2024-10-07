using System;
using System.Collections.Generic;

public interface IPlayerData
{

}

[Serializable]
public class CharacterPlayerData : IPlayerData
{
    public string Code { get; set; }
    public int Level { get; set; } = 1;
    public int Awake { get; set; } = 1;
    public List<string> PotentialKeys { get; set; } = new List<string>();
    public int SpecialWeaponLevel { get; set; } = 1;
}