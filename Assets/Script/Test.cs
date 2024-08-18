using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void T(BBNumber bBNumber)
    {
        UnityHelper.Log_H($"s = {bBNumber.significand} e : {bBNumber.exponent} alphabet : {bBNumber.Alphabet()}");
    }
}
