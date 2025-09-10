using System;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.CSharp;
using Shared.DTOs.Player;

public class PlayerDataManager
{
    public Dictionary<string, Action<PlayerGetsData<object>>> OnDbUpdate = new Dictionary<string, Action<PlayerGetsData<object>>>();

    public List<PlayerGetsData<object>> Datas => _datas;
    private List<PlayerGetsData<object>> _datas = new List<PlayerGetsData<object>>();

    public void Initialize()
    {

    }
    public void DbGets(Type type, Action callback = null)
    {
        List<Type> types = new List<Type> { type };
        DbGets(types, callback);
    }

    public void DbGets(List<Type> types, Action callback = null)
    {
        if (callback != null)
            callback.Invoke();
        return;
        
        List<string> typeNames = new List<string>();
        foreach (var type in types)
        {
            if (!_datas.Exists(x => x.Name == type.Name))
                typeNames.Add(type.Name);
        }

        if (typeNames.Count == 0)
        {
            if (callback != null)
                callback.Invoke();
                
            return;
        }

        PlayerGetsRequest req = new PlayerGetsRequest();
        req.TypeNames = typeNames;

        Managers.Web.SendPostRequest<PlayerGetsResponse>("player/gets", req, (_result) =>
        {
            string json = _result.Datas.SerializeObject_H();

            var newDatas = GetPlayerDatas(json);
            DbUpdate(newDatas);

            if (callback != null)
                callback.Invoke();
        });
    }

    public void RedisGets(List<Type> types, Action callback = null)
    {
        List<string> typeNames = new List<string>();
        foreach (var type in types)
        {
            if (!_datas.Exists(x => x.Name == type.Name))
                typeNames.Add(type.Name);
        }

        if (typeNames.Count == 0)
        {
            if (callback != null)
                callback.Invoke();
                
            return;
        }

        PlayerGetsRequest req = new PlayerGetsRequest();
        req.TypeNames = typeNames;

        Managers.Web.SendPostRequest<PlayerGetsResponse>("player/gets/redis", req, (_result) =>
        {
            string json = _result.Datas.SerializeObject_H();

            var newDatas = GetPlayerDatas(json);
            DbUpdate(newDatas);

            if (callback != null)
                callback.Invoke();
        });
    }
    public void DbUpdate(List<PlayerGetsData<object>> datas)
    {
        foreach (var data in datas)
        {
            DbUpdate(data);
        }
    }
    public void DbUpdate(PlayerGetsData<object> data)
    {
        if (OnDbUpdate.TryGetValue(data.Name, out var callback))
        {
            callback?.Invoke(data);
        }

        var existingData = _datas.FindIndex(x => x.Name == data.Name);
        if (existingData >= 0)
            _datas[existingData] = data;
        else
            _datas.Add(data);
    }
    private List<PlayerGetsData<object>> GetPlayerDatas(string json)
    {
        return CSharpHelper.DeserializeObject<List<PlayerGetsData<object>>>(json);
    }
    public T GetPlayerData<T>()
    {
        string typeName = typeof(T).IsGenericType ? typeof(T).GetGenericArguments()[0].Name : typeof(T).Name;
        PlayerGetsData<object> loadData = _datas.Find(data => data.Name == typeName);

        if (loadData == null)
            return default(T);

        string json = CSharpHelper.SerializeObject(loadData.Datas);

        return CSharpHelper.DeserializeObject<T>(json);
    }

    public T GetPlayerData<T>(PlayerGetsData<object> data)
    {
        string json = CSharpHelper.SerializeObject(data.Datas);
        return CSharpHelper.DeserializeObject<T>(json);
    }

    #region Getter
    public BBNumber GetPlayerItemCount(string itemCode)
    {
        List<PlayerItemDto> playerItemDatas = GetPlayerData<List<PlayerItemDto>>();

        if (playerItemDatas == null)
            return 0;

        foreach (var item in playerItemDatas)
        {
            if (item.ItemCode == itemCode)
                return item.ItemCount;
        }

        return 0;
    }
    public void AddPlayerItem(string itemCode, BBNumber count)
    {
        List<PlayerItemDto> playerItemDatas = GetPlayerData<List<PlayerItemDto>>();

        if (playerItemDatas == null)
            return;

        foreach (var item in playerItemDatas)
        {
            if (item.ItemCode == itemCode)
            {
                item.ItemCount += count;
            }
        }

        List<PlayerGetsData<object>> datas = new List<PlayerGetsData<object>>()
        {
            new PlayerGetsData<object>()
            {
                Name = nameof(PlayerItemDto),
                Datas = playerItemDatas
            }
        };
        DbUpdate(datas);
    }

    public bool UsePlayerItem(string itemCode, BBNumber count)
    {
        List<PlayerItemDto> playerItemDatas = GetPlayerData<List<PlayerItemDto>>();

        if (playerItemDatas == null)
            return false;

        foreach (var item in playerItemDatas)
        {
            if (item.ItemCode == itemCode)
            {
                if (item.ItemCount < count)
                    return false;

                item.ItemCount -= count;
            }
        }

        List<PlayerGetsData<object>> datas = new List<PlayerGetsData<object>>()
        {
            new PlayerGetsData<object>()
            {
                Name = nameof(PlayerItemDto),
                Datas = playerItemDatas
            }
        };
        DbUpdate(datas);

        return true;
    }
    #endregion
}