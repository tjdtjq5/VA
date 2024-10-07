using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void TT()
    {
        UnityHelper.Log_H(PlayerDataCPacket.FilePathName("Character"));
    }
    [Button]
    public void MasterGets()
    {
        Managers.Table.DbGets(); 
    }
    [Button]
    public void CharacterGets()
    {
        UnityHelper.LogSerialize(Managers.PlayerData.Item.Gets());
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