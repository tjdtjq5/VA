using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Player;
using Sirenix.OdinInspector;
using UnityEngine;

public class ServerTest : MonoBehaviour
{
    [Button]
    public void Gacha()
    {
        PlayerEquipGachaRequest req = new PlayerEquipGachaRequest();
        req.Count = 10;

        Managers.Web.SendPostRequest<PlayerEquipGachaResponse>("player/gacha/picup", req, (res) =>
        {
            Managers.PlayerData.DbUpdate(res.Datas);
            UnityHelper.Log_H(CSharpHelper.SerializeObject(res));
        });
    }
}
