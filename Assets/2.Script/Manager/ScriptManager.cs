using Shared.Enums;

public class ScriptManager
{
    public string Get(Grade grade)
    {
        switch (grade)
        {
            case Grade.D:
                return "D";
            case Grade.C:
                return "C";
            case Grade.B:
                return "B";
            case Grade.A:
                return "A";
            case Grade.S:
                return "S";
            case Grade.SS:
                return "SS";
            case Grade.SSS:
                return "SSS";
            default:
                return string.Empty;
        }
    }
}
