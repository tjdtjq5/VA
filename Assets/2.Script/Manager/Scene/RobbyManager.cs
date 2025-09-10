using System;
using System.Collections;
using System.Collections.Generic;
using Shared.CSharp;
using Shared.DTOs.Player;
using Sirenix.OdinInspector;
using UnityEngine;

public class RobbyManager : SceneBase
{
    public Action<UIRobbyBackTabType> OnBackTab;

    [SerializeField] private UIGoodsController _goodsController;

    private Player _player;
    private UIRobbyTab _rTab;
    private CameraController _cameraController;
    // ParentName, PopupName, Popup
    private Dictionary<string, Dictionary<string, UIRobby>> _popupDic = new();
    private Stack<string> _popupHistory = new Stack<string>();

    private readonly string _playerPrefabPath = "Prefab/Character/RobbyPlayer";

    [Button]
    public void GetPlayerDataRedis()
    {
        List<Type> types = new List<Type> { typeof(PlayerGrowDto), typeof(PlayerResearchDto), typeof(PlayerItemDto) };
        Managers.PlayerData.RedisGets(types, () =>
        {
            List<PlayerItemDto> playerItemDatas = Managers.PlayerData.GetPlayerData<List<PlayerItemDto>>();
            UnityHelper.Log_H(playerItemDatas.SerializeObject_H());
        });
    }

    protected override void Initialize()
    {
        base.Initialize();

        Managers.Sound.Play("BGM/RobbyBGM", Sound.Bgm);

        SceneType = SceneType.Robby;
        Managers.Observer.RobbyManager = this;

        _player = PlayerSpawn();
        _player.Initialize(null);
        _player.OnChangeForm += OnChangeForm;

        _rTab = FindObjectOfType<UIRobbyTab>();
        _rTab.OnClickTab(UIRobbyTab.UIButtonE.Tabs_Main);
        _rTab.OnClickTabHandler -= OnClickTab;
        _rTab.OnClickTabHandler += OnClickTab;

        _cameraController = FindObjectOfType<CameraController>();
        _cameraController.Initialize();
        _cameraController.SetTarget(_player.transform, 2f);
        _cameraController.Speed = 1f;

        Managers.Observer.UIGoodsController = _goodsController;

        Map map = FindObjectOfType<Map>();
        map.Initialize(_player.transform);

        Managers.PlayerData.DbGets(typeof(PlayerItemDto), () =>
        {
            List<PlayerItemDto> playerItemDatas = Managers.PlayerData.GetPlayerData<List<PlayerItemDto>>();

            if (playerItemDatas == null)
                playerItemDatas = new List<PlayerItemDto>();

            foreach (var item in playerItemDatas)
            {
                _goodsController.UISet(item.ItemCode, item.ItemCount);
            }
        });

        PlayerAction();
    }

    public Player PlayerSpawn() => Managers.Resources.Instantiate<Player>(_playerPrefabPath);
    private void PlayerAction()
    {
        Vector3 destPos = _player.transform.position + new Vector3(12f, 0, 0);
        _player.CharacterMove.SetMoveDontStopMotion(destPos, ()=>
        {
            PuzzleType cType = _player.CurrentForm;
            PuzzleType nType = (PuzzleType)CSharpHelper.EnumNext<PuzzleType>((int)cType, false);
            _player.ChangeForm(nType);
        });
    }

    private void OnChangeForm(PuzzleType puzzleType)
    {
        PlayerAction();
    }

    public void ShopUI(string popupPrefabName, string parentPrefabName = "")
    {
        ShopUI<UIRobby>(popupPrefabName, parentPrefabName);
    }
    public T ShopUI<T>(string popupPrefabName, string parentPrefabName = "")
    {
        _popupHistory.Push(popupPrefabName);

        Dictionary<string, UIRobby> popupList = _popupDic.TryGetValue(parentPrefabName, out popupList) ? popupList : new Dictionary<string, UIRobby>();

        CloseOtherPopup(popupPrefabName, popupList);

        UIRobby popup = null;
        if (popupList.ContainsKey(popupPrefabName))
        {
            popup = popupList[popupPrefabName];
            Managers.UI.ShopPopupUI<UIRobby>(popup, CanvasOrderType.Bottom);
        }
        else
        {
            popup = Managers.UI.ShopPopupUI<UIRobby>(popupPrefabName, CanvasOrderType.Bottom);
            if (popup != null)
                popupList.Add(popupPrefabName, popup);
        }

        if (!_popupDic.ContainsKey(parentPrefabName))
            _popupDic.Add(parentPrefabName, new Dictionary<string, UIRobby>());

        _popupDic[parentPrefabName] = popupList;

        OnBackTab?.Invoke(popup.BackTabType);

        return popup.GetComponent<T>();
    }
    private void CloseOtherPopup(string popupPrefabName, Dictionary<string, UIRobby> popupList)
    {
        var keysToClose = new List<string>();

        foreach (var pop in popupList)
        {
            // 현재 팝업은 닫지 않고 스킵
            if (pop.Key == popupPrefabName)
                continue;

            // 닫을 대상으로 저장
            keysToClose.Add(pop.Key);
        }

        foreach (var key in keysToClose)
        {
            if (popupList.TryGetValue(key, out var popupToClose))
            {
                popupToClose?.ClosePopupUI();

                if (_popupDic.TryGetValue(key, out var nestedPopupList))
                {
                    CloseOtherPopup(key, nestedPopupList);
                }
            }
        }
    }

    private void OnClickTab(UIRobbyTab.UIButtonE button)
    {
        if (button == UIRobbyTab.UIButtonE.Tabs_Main)
        {
            Managers.Observer.UIGoodsController = _goodsController;
            _goodsController.Set();
        }
    }
    [Button]
    public void GoBack()
    {
        if (_popupHistory.Count > 1)
        {
            // 현재 팝업을 스택에서 제거합니다.
            string currentPopupName = _popupHistory.Pop();
            string parentName = "";

            foreach (var parentEntry in _popupDic)
            {
                Dictionary<string, UIRobby> popupList = parentEntry.Value;
                if (popupList.ContainsKey(currentPopupName))
                {
                    parentName = parentEntry.Key;
                    // 팝업 리스트를 찾았으니, CloseOtherPopup을 호출하여 닫습니다.
                    CloseOtherPopup(parentName, popupList);
                    // parentName = parentEntry.Key;
                    break; // 찾았으니 루프를 종료합니다.
                }
            }

            string beforePopupName = _popupHistory.Pop();

            if (!string.IsNullOrEmpty(parentName) && _popupDic.ContainsKey(parentName) && beforePopupName != parentName)
            {
                Dictionary<string, UIRobby> popupList = _popupDic[parentName];
                ShopUI(beforePopupName, parentName);
            }
        }
    }

    public override void Clear()
    {

    }
}
