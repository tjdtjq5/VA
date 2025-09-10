using Shared.Enums;
using UnityEngine;

public static class DefineColor
{
    public static Color GetGrade(Grade grade)
    {
        switch (grade)
        {
            case Grade.D:
                return new Color(0.33f, 0.33f, 0.33f, 1);
            case Grade.C:
                return new Color(0.36f, 0.74f, 0.36f, 1);
            case Grade.B:
                return new Color(0.33f, 0.4f, 0.96f, 1);
            case Grade.A:
                return new Color(0.67f, 0.4f, 0.71f, 1);
            case Grade.S:
                return new Color(1, 0.68f, 0.32f, 1);
            case Grade.SS:
                return new Color(1, 0.4f, 0.4f, 1);
            case Grade.SSS:
                return new Color(0.99f, 0.67f, 1, 1);
            default:
                return Color.white;
        }
    }
}
