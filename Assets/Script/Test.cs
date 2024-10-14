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
        var datas = Managers.Table.CharacterTable.Gets();
        UnityHelper.LogSerialize(datas);
        UnityHelper.LogSerialize(Managers.Table.CharacterTable.GetTableSO(datas[0].characterCode).codeName);
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