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
                return "�����";
            case Tribe.Dragon:
                return "��";
            case Tribe.Druid:
                return "����̵�";
            case Tribe.Pirate:
                return "����";
            case Tribe.Robot:
                return "�κ�";
            case Tribe.Thief:
                return "����";
            default:
                return string.Empty;
        }
    }
    public string Get(CharacterJob job)
    {
        switch (job)
        {
            case CharacterJob.Dealer:
                return "����";
            case CharacterJob.SubDealer:
                return "���� ����";
            case CharacterJob.Supporter:
                return "������";
            default:
                return string.Empty;
        }
    }
}
