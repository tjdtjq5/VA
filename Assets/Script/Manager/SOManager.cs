public class SOManager
{
    public Stat GetStat(string code)
    {
        string path = DefinePath.StatSOResourcesPath(code);
        return (Stat)Managers.Resources.Load<Stat>(path).Clone();
    }
    public CharacterSO GetCharacter(string code)
    {
        string path = DefinePath.StatSOResourcesPath(code);
        return (CharacterSO)Managers.Resources.Load<CharacterSO>(path).Clone();
    }
}
