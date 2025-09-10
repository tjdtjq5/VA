using Shared.BBNumber;

public enum ServerUrlType
{
    DebugUrl,
    LocalhostUrl,
    DebugNgrok,
    DevUrl,
    ReleaseUrl
}
public enum SceneType
{
    Unknown,
    Title,
    Robby,
    InGame,
    Dungeon,
}
public enum UIEvent
{
    Click,
    PointDown,
    PointUp,
    Trigger,
    Enter,
    Exit,
}
public enum MouseEvent
{
    Press,
    Click,
}
public enum Sound
{
    Bgm,
    InGmae,
    UI,
}
public enum AnchorStyles
{
    Left,
    Middle,
    Right,
}
public enum UIScrollViewLayoutStartCorner
{
    Left,
    Middle,
    Right,
}
public enum UIScrollViewLayoutStartAxis
{
    Vertical,
    Horizontal,
}
public enum ChatFrequencyCycle
{
    Often,
    Sometimes,
    Close
}
public enum Direction
{
    Right,
    Middle,
    Left,
}

public enum AttackGrade
{
    Basic,
    Focus,
    Fatal,
    Oui
}

public enum DamageType
{
    None,
    Sequence,
    Skill,
    Burn,
    Poison,
    Add,
}
[System.Serializable]
public class ItemValue
{
    public Item item;
    public BBNumber value;

    public ItemValue(Item item, BBNumber value)
    {
        this.item = item;
        this.value = value;
    }

    public string ToValueString()
    {
        if (item.IsAlphabet)
        {
            return value.Alphabet();
        }
        else
        {
            return value.ToString();
        }
    }

    public override string ToString()
    {
        return $"{item.DisplayName} +{value.Alphabet()}";
    }
}
[System.Serializable]
public class StatValue
{
    public Stat stat;
    public BBNumber value;

    public StatValue(Stat stat, BBNumber value)
    {
        this.stat = stat;
        this.value = value;
    }

    public override string ToString()
    {
        if (stat.IsPercent)
        {
            return $"{stat.DisplayName} +{value}%";
        }
        else
        {
            return $"{stat.DisplayName} +{value.Alphabet()}";
        }
    }
}