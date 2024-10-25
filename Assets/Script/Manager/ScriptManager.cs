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
    public string Get(Tribe tribe)
    {
        switch (tribe)
        {
            case Tribe.Cat:
                return "고양이";
            case Tribe.Dragon:
                return "용";
            case Tribe.Druid:
                return "드루이드";
            case Tribe.Pirate:
                return "해적";
            case Tribe.Robot:
                return "로봇";
            case Tribe.Thief:
                return "도적";
            default:
                return string.Empty;
        }
    }
    public string Get(CharacterJob job)
    {
        switch (job)
        {
            case CharacterJob.Dealer:
                return "딜러";
            case CharacterJob.SubDealer:
                return "보조 딜러";
            case CharacterJob.Supporter:
                return "서포터";
            default:
                return string.Empty;
        }
    }
}
