using System.Collections;
using System.Collections.Generic;
using Shared.Enums;
using UnityEngine;

public static class DefineScript
{
    public static string GetGradeText(Grade grade)
    {
        switch (grade)
        {
            case Grade.D:
                return "일반";
            case Grade.C:
                return "고급";
            case Grade.B:
                return "희귀";
            case Grade.A:
                return "영웅";
            case Grade.S:
                return "전설";
            case Grade.SS:
                return "신화";
            case Grade.SSS:
                return "초월";
            default:
                return string.Empty;
        }
    }
}
