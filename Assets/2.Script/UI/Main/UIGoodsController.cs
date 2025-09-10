using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.CSharp;
using Shared.DTOs.Player;
using UnityEngine;

public class UIGoodsController : UIFrame
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform content;

    public Action OnEffectCollected;
    public UIGoods GetUIGoods(string itemCode) => _goods.Find(g => g.Item.CodeName == itemCode);
    
    private readonly List<UIGoods> _goods = new();
    private int _collectCount = 0;
    private int _collectedCount = 0;
    private string _plusItem;
    private BBNumber _plusNewValue;

    private readonly string _goodsSpawnPath = "Prefab/InGame/GoodsSpawn";
    private readonly string _collectEffectPath = "Prefab/Effect/InGame/GoodsCollect";

    protected override void Initialize()
    {
        base.Initialize();

        for (int i = 0; i < content.childCount; i++)
            _goods.Add(content.GetChild(i).GetComponent<UIGoods>());

        Managers.PlayerData.OnDbUpdate.TryAdd_H(typeof(PlayerItemDto).Name, OnChangeItem, true);
    }

    protected override void UISet()
    {
        base.UISet();
    }

    public void UISet(string item, BBNumber count)
    {
        UIGoods uiGoods = GetUIGoods(item);
        uiGoods?.UISet(count);
    }

    public void Set()
    {
        for (int i = 0; i < _goods.Count; i++)
        {
            string itemCode = _goods[i].Item.CodeName;
            BBNumber count = Managers.PlayerData.GetPlayerItemCount(itemCode);
            UISet(itemCode, count);
        }
    }

    private void OnChangeItem(PlayerGetsData<object> data)
    {
        List<PlayerItemDto> playerItemDatas = Managers.PlayerData.GetPlayerData<List<PlayerItemDto>>(data);

        for (int i = 0; i < playerItemDatas.Count; i++)
        {
            UISet(playerItemDatas[i].ItemCode, playerItemDatas[i].ItemCount);
        }
    }

    private void PlusSet()
    {
        UIGoods uiGoods = GetUIGoods(_plusItem);

        if (uiGoods)
        {
            uiGoods.OnIncrease -= OnEffectCollectedAction;
            uiGoods.OnIncrease += OnEffectCollectedAction;
            uiGoods.PlusPlay(_plusNewValue);
        }
    }

    public void PlusSet(string item, BBNumber newValue, Vector3 point, int count)
    {
        this._plusItem = item;
        this._plusNewValue = newValue;
        
        SpawnGoodsEffect(item, point, count);
    }

    public void SpawnGoodsEffect(string item, Vector3 point, int count)
    {
        UIGoods uiGoods = GetUIGoods(item);

        if (uiGoods == null)
            return;
        
        this._collectCount = count;
        this._collectedCount = 0;

        for (int i = 0; i < count; i++)
        {
            GoodsCollectEffect goodsCollectEffect = Managers.Resources.Instantiate<GoodsCollectEffect>(_collectEffectPath);
            goodsCollectEffect.transform.position = point;
            goodsCollectEffect.UISet(this, item, uiGoods.IconTr);
        }        
    }

    public void OnGoodsEffectCollected()
    {
        _collectedCount++;
        if (_collectedCount < _collectCount)
            return;

        PlusSet();
    }

    void OnEffectCollectedAction()
    {
        OnEffectCollected?.Invoke();
    }

    public void CanvasOrderSwitch(bool isOn)
    {
        _canvas.sortingOrder = isOn ? 10000 : 0;
    }
}