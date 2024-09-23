using System.Collections;
using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class Test : MonoBehaviour
{
    [Button]
    public void MasterGets()
    {
        Managers.Table.DbGets();
    }
    [Button]
    public void ItemGets()
    {
    
    }

    public class TestA
    {
        public int x;
        public int y;
    }
    public class TestB
    {
        public int x;
        public int y;
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