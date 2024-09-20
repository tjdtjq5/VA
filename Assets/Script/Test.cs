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

    string tableName = "Item";

    [Button]
    public void SimpleFormatTest_Exist()
    {
        UnityHelper.Log_H(TableControllerPacket.Exist(tableName));
    }
    [Button]
    public void SimpleFormatTest_Modify()
    {
       // TableRRPacket.Modify(tableName);
    }
    [Button]
    public void SimpleFormatTest_Remove()
    {
        TableControllerPacket.Remove(tableName);
    }
    [Button]
    public void SimpleFormatTest_Create()
    {
        TableControllerPacket.Create(tableName);
    }
}
