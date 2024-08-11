using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TweenManager
{
    public void OnFixedUpdate()
    {

    }

    double EaseValue(Ease ease, float x)
    {
        switch (ease)
        {
            case Ease.InSine:
                return InSine(x);
            case Ease.OutSine:
                return OutSine(x);
            case Ease.InOutSine:
                return InOutSine(x);
            default:
                return x;
        }
    }

    double InSine(float x)
    {
        return 1 - Math.Cos((x * Math.PI) / 2);
    }
    double OutSine(float x)
    {
        return Math.Sin((x * Math.PI) / 2);
    }
    double InOutSine(float x)
    {
        return -(Math.Cos(Math.PI * x) - 1) / 2;
    }
}
public enum Ease
{
    Linear,
    InSine,
    OutSine,
    InOutSine
}