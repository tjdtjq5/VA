using EasyButtons;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void TT()
    {
        string expr = "(1050+6636)+1*(100-100)-5000+((35*53+555.5) + 0.5 + 5000) ^ 15";

        UnityHelper.Log_H(FomulaCompute.Compute(expr).ToCountString());
    }
    [Button]
    public void MasterGets()
    {
        Managers.Table.DbGets(); 
    }
    [Button]
    public void ItemGets()
    {
        Managers.Resources.Instantiate("Prefab/Effect/SkillStarEffect");
    }

    [Button]
    public void SimpleFormatTest_Exist()
    {
        UnityHelper.Log_H(MasterTableServicePacket.Exist("TestT"));
    }
 
    [Button]
    public void SimpleFormatTest_Remove()
    {
        MasterTableServicePacket.Remove("TestT");
    }
}