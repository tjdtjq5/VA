using System.Collections;
using System.Collections.Generic;
using Best.HTTP.Shared.Logger;
using System.Collections.ObjectModel;
using EasyButtons;
using Unity.VisualScripting;
using UnityEngine;
using System.Reflection;

public class Test : MonoBehaviour
{
    [Button]
    public void TT()
    {
        ItemTableData a = new ItemTableData();
        a.itemCode = "code_002";
        a.itemType = 1;
        a.tipName = "tipName";

        List<ItemTableData> datas = new List<ItemTableData>();
        datas.Add(a);

        UnityHelper.LogSerialize(datas);

        ItemTableData b = new ItemTableData();
        b.itemCode = "code_002";
        b.itemType = 2;
        b.tipName = "tipName2";

        datas.ForceAdd_H(b, b.itemCode);

        UnityHelper.LogSerialize(datas);
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