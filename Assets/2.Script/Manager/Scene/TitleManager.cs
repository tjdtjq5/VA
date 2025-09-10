using System;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Player;
using Shared.DTOs.Table;
using Sirenix.OdinInspector;

public class TitleManager : SceneBase
{
    protected override void Initialize()
    {
        base.Initialize();

        SceneType = SceneType.Title;

        LoginService.AtLogin(() => 
        {
            UnityHelper.Log_H("Auto Login Success");
            AfterLogin();
        }, () => 
        {
            UILoginTest UI_Login = Managers.UI.ShopPopupUI<UILoginTest>("UILoginTest", CanvasOrderType.Middle);

            UI_Login.LoginAfterJob(() =>
            {
                UnityHelper.Log_H("Login Success");
                UI_Login.ClosePopupUI();
                AfterLogin();
            });
        });
    }

    public override void Clear()
    {

    }

    void SetMonitor()
    {
        if (!GameOptionManager.IsRelease)
        {
            Managers.Resources.Instantiate("Prefab/UI/Popup/Monitor/Monitor");
        }
    }
    void AfterLogin()
    {
        GetTable(() =>
        {
            Managers.Scene.LoadScene(SceneType.Robby);
        });
    }

    [Button]
    public void GetPlayerDataRedis()
    {
        List<Type> types = new List<Type> { typeof(PlayerGrowDto), typeof(PlayerResearchDto), typeof(PlayerItemDto) };

        List<string> typeNames = new List<string>();
        foreach (var type in types)
        {
            typeNames.Add(type.Name);
        }

        PlayerGetsRequest req = new PlayerGetsRequest();
        req.TypeNames = typeNames;

        Managers.Web.SendPostRequest<PlayerGetsResponse>("admin/redis/gets", req, (_result) =>
        {
            UnityHelper.Log_H(_result.Datas.SerializeObject_H());
        });
    }

    [Button]
    public void RemoveAllCache()
    {
        Managers.Web.SendGetRequest<string>("admin/cache/remove", (res) =>
        {
            UnityHelper.Log_H(res);
        });
    }


    [Button]
    public void GetTable(Action callback = null)
    {
        Managers.Table.DbGets(() =>
        {
            UnityHelper.Log_H("GetTable Success");

            List<TableGrowRewardDto> rewardDatas = Managers.Table.GetTableData<List<TableGrowRewardDto>>();
            // UnityHelper.Log_H(rewardDatas.SerializeObject_H());

            callback?.Invoke();
        });
    }

    [Button]
    public void GetPlayerData(Action callback = null)
    {
        List<Type> types = new List<Type> { typeof(PlayerGrowDto), typeof(PlayerResearchDto), typeof(PlayerItemDto) };
        Managers.PlayerData.DbGets(types, () =>
        {
            UnityHelper.Log_H("GetPlayerData Success");

            callback?.Invoke();
        });
    }

    [Button]
    public void GetPlayerItems()
    {
        List<Type> types = new List<Type> { typeof(PlayerItemDto) };
        Managers.PlayerData.DbGets(types, () =>
        {
            List<PlayerItemDto> playerItemDatas = Managers.PlayerData.GetPlayerData<List<PlayerItemDto>>();
            UnityHelper.Log_H(playerItemDatas.SerializeObject_H());
        });
    }

    [Button]
    public void GetPlayerItems2()
    {
        Managers.Web.SendGetRequest<PlayerItemResponse>("player/item/gets", (_result) =>
        {
            List<PlayerItemDto> playerItemDatas = _result.Items;
            UnityHelper.Log_H(playerItemDatas.SerializeObject_H());
        });
    }
}
