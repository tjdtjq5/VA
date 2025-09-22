using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFlowGachaPicup : TimeFlow
{
    protected override void TimeSet()
    {
        int days = _timeSpan.Days;
        int hour = _timeSpan.Hours;

        GetTextPro(UITextProE.Text).text = $"남은시간 : {days:D2}일 {hour:D2}시간";
    }
}
