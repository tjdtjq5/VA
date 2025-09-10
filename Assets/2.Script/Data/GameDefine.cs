using Shared.Enums;
using UnityEngine;

public static class GameDefine
{
    #region Combo
    private static readonly float _comboMultiplier = 0.1f;
    private static readonly float _forceMultiplier = 0.3f;
    private static readonly float _gradeMultiplier = 0.5f;
    private static readonly float _sctMultiplier = 0.1f;
    private static readonly float _sctpMultiplier = 0.2f;
    public static float ComboMultiplier(Character owner, int combo, int forceCount)
    {
        if (combo <= 1)
            return 1;

        float cm = _comboMultiplier;
        float fm = _forceMultiplier;

        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.ComboTranscendence))
            cm += _sctMultiplier;

        if (owner.IsOnTriggerPassiveBuff(TriggerPassiveBuff.ComboTranscendence_P))
            cm += _sctpMultiplier;
        
        return 1 + cm * combo + fm * forceCount + _gradeMultiplier * (int)GetAttackGrade(combo);
    }
    public static AttackGrade GetAttackGrade(int combo)
    {
        if (combo <= 4)
        {
            return AttackGrade.Basic;
        }
        else if (combo <= 6)
        {
            return AttackGrade.Focus;
        }
        else if (combo <= 8)
        {
            return AttackGrade.Fatal;
        }
        else
        {
            return AttackGrade.Oui;
        }
    }
    #endregion

    #region Color

    public static Color GetColor(Grade grade)
    {
        switch (grade)
        {
            case Grade.D:
                return new Color32(157, 157, 157, 255);
            case Grade.C:
                return new Color32(105, 255, 132, 255);
            case Grade.B:
                return new Color32(145, 226, 255, 255);
            case Grade.A:
                return new Color32(243, 151, 255, 255);
            case Grade.S:
                return new Color32(255, 216, 96, 255);
            case Grade.SS:
                return new Color32(255, 106, 106, 255);
            case Grade.SSS:
                return new Color32(154, 178, 255, 255);
            default:
                return new Color32(255, 255, 255, 255);
        }
    }

    #endregion
}
