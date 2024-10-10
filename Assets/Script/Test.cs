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
        List<string> jobs = new List<string>() { "딜러", "보조 딜러", "딜러", "보조 딜러", "서포터" };
        List<string> distances = new List<string>() { "근거리", "원거리", "근거리", "근거리", "원거리" };
        List<string> ranges = new List<string>() { "단일딜", "범위딜" };
        Dictionary<Tribe, string> totalDics = new Dictionary<Tribe, string>();
        int tribeCount = 0;

        for (int i = 0; i < jobs.Count; i++)
        {
            for (int j = 0; j < distances.Count; j++) 
            {
                for (int k = 0; k < ranges.Count; k++) 
                {
                    string r = $"[{jobs[i]}] [{distances[j]}] [{ranges[k]}]";
                    Tribe tribe = (Tribe)CSharpHelper.EnumInRemain<Tribe>(tribeCount, false);

                    if (totalDics.ContainsKey(tribe))
                    {
                        totalDics[tribe] = $"{totalDics[tribe]}\n{r}";
                    }
                    else
                    {
                        totalDics.Add(tribe, r);
                    }
                    tribeCount++;
                }
            }
        }

        int len = CSharpHelper.GetEnumLength<Tribe>();
        for (int i = 0; i < len; i++)
        {
            UnityHelper.Log_H((Tribe)i);
            string value = totalDics[(Tribe)i];
            List<string> values = value.Split('\n').ToList();
            UnityHelper.Log_H(values.Count);
            values.Shuffle();

            value = "";
            for (int j = 0;j < values.Count; j++)
            {
                value += $"{values[j]}\n";
            }
            value = value.Substring(0, value.Length - 1);

            totalDics[(Tribe)i] = value;
            UnityHelper.Log_H(totalDics[(Tribe)i]);
        }
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