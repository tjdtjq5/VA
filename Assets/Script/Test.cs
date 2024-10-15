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
        string a = "Test";

        UnityHelper.Log_H(CSharpHelper.GetTypeByString(a));
    }
    [Button]
    public void MasterGets()
    {
        Managers.Table.DbGets(); 
    }
    [Button]
    public void CharacterGets()
    {
        UnityHelper.SerializeL(Managers.PlayerData.Item.Gets());
    }
    [Button]
    public void SimpleFormatTest_Update()
    {
        PlayerDataCPacket.Create("Character");
    }

    [Button]
    public void SimpleFormatTest_Exist()
    {
        UnityHelper.Log_H(PlayerDataCPacket.Exist("Character"));
    }
 
    [Button]
    public void SimpleFormatTest_Remove()
    {
        PlayerDataCPacket.Remove("Character");
    }
}