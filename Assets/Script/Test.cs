using EasyButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void TT()
    {
        string statCode = "BasicAtk";

        Stat stat = Managers.SO.GetStat(statCode);

        stat.SetBonusValue("Test", "T", 111);
    }
}