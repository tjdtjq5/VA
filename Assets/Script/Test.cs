using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void T()
    {
        // 65 ~ 90
        char c = System.Convert.ToChar(90);
        UnityHelper.Log_H(c);
    }
}
