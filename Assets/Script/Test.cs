using EasyButtons;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void TT()
    {
        PlayerItemData item = new PlayerItemData()
        {
            ItemCode = "aa",
        };
        item.Set(1);

        item.Add(3);;

        UnityHelper.Log_H(item.Count());
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
    public void SimpleFormatTest_Update()
    {
        TableDefineCodePacket.Update("TestT", new List<string>() { "ABB", "A_B", "AFEFV" });
    }

    [Button]
    public void SimpleFormatTest_Exist()
    {
        UnityHelper.Log_H(CSharpHelper.ExistEnumData<DefineTableCodeType>("Item"));
        UnityHelper.Log_H(CSharpHelper.ExistEnumData<DefineTableCodeType>("Item2"));
    }
 
    [Button]
    public void SimpleFormatTest_Remove()
    {
        TableDefineCodePacket.Remove("TestT");
    }
}