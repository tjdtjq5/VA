using UnityEngine;

public static class DefineColor
{
    public static Color GetGrade(Grade grade)
    {
        return Color.white;
    }
    public static Color GetTribe(Tribe tribe)
    {
        switch (tribe)
        {
            case Tribe.Cat:
                return new Color(1, 0.35f, 1, 1);
            case Tribe.Dragon:
                return new Color(1, 0.28f, 0.28f, 1);
            case Tribe.Druid:
                return new Color(0, 0.85f, 0, 1);
            case Tribe.Pirate:
                return new Color(0.65f, 0.34f, 0.98f, 1);
            case Tribe.Robot:
                return new Color(0.85f, 0.85f, 0.85f, 1);
            case Tribe.Thief:
                return new Color(0, 0.77f, 1, 1);
            default:
                return Color.white;
        }
    }
}
