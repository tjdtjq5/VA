using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Player;

public class UIRobbyGrowResearch : UIRobby
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<ResearchSlot>(typeof(ResearchSlotE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<TimeFlow>(typeof(TimeFlowE));
		Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.SafeArea_UpgradeSlot_ShortBtn).AddClickEvent(ped => OnShort());
        GetButton(UIButtonE.SafeArea_UpgradeSlot_CompleteBtn).AddClickEvent(ped => OnComplete());

        Get<TimeFlow>(TimeFlowE.SafeArea_UpgradeSlot_Time).OnTimeFlow += OnTimeFlowComplete;

        base.Initialize();
    }

    private PlayerResearchDto _playerData;
    private ResearchNode _researchNode;
    private bool _isComplete;

    private readonly string _researchPrefabPath = "Robby/Research/RobbyGrowResearch";
    private readonly string _researchBookPrefabPath = "Robby/Research/RobbyGrowResearchBook";
    private readonly string _researchShortPrefabPath = "Robby/Research/UIResearchShort";

    public override void OpenUISet(CanvasOrderType orderType)
    {
        base.OpenUISet(orderType);

        RobbyGrowResearchBook book = Managers.Observer.RobbyManager.ShopUI<RobbyGrowResearchBook>(_researchBookPrefabPath, _researchPrefabPath);
        book.UISet(this);

        _isComplete = false;

        Set();
    }

    public void Set()
    {
        if (_playerData == null)
          InitSet();

        // Player Data Load
        List<Type> types = new List<Type> { typeof(PlayerResearchDto) };
        Managers.PlayerData.DbGets(types, () =>
        {
            List<PlayerResearchDto> playerDatas = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
            PlayerResearchDto progressData = playerDatas?.Find(x => x.IsProgress);

            SetResearchSlot(progressData);
        });
    }

    private void InitSet()
    {
        SetResearchSlot(null);
    }

    private void SetResearchSlot(PlayerResearchDto playerData)
    {
        _playerData = playerData;
        if (playerData == null)
        {
            _researchNode = null;

            GetImage(UIImageE.SafeArea_UpgradeSlot).gameObject.SetActive(false);
        }
        else
        {
            var researchTree = Managers.NodeTree.GetResearchTree(playerData.Type.ToString());
            var researchNode = researchTree.GetNodes().FirstOrDefault(x => x.Floor == playerData.Floor && x.Index == playerData.Index);
            _researchNode = researchNode;

            GetImage(UIImageE.SafeArea_UpgradeSlot).gameObject.SetActive(true);

            SetSlot(researchNode, playerData);
            SetValue(researchNode, playerData);
            SetTimeFlow(researchNode, playerData);
            SetButton(researchNode, playerData);
        }
    }

    private void SetSlot(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        Get<ResearchSlot>(ResearchSlotE.SafeArea_UpgradeSlot_Slot).UISet(researchNode, playerData);
    }
    private void SetValue(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        int level = playerData.Level;
        StatValue statValue = researchNode.Research.StatValue(level);
        GetTextPro(UITextProE.SafeArea_UpgradeSlot_Title).text = $"{researchNode.Research.DisplayName} LV. {level}";
        GetTextPro(UITextProE.SafeArea_UpgradeSlot_Script).text = $"{statValue.ToString()}";
    }
    private void SetTimeFlow(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        TimeSpan remainTime = playerData.RemainTime(Managers.Time.Current);
        UnityHelper.Log_H($"remainTime : {remainTime}");
        Get<TimeFlow>(TimeFlowE.SafeArea_UpgradeSlot_Time).UISet(remainTime);
    }
    private void SetButton(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        TimeSpan remainTime = playerData.RemainTime(Managers.Time.Current);
        if (remainTime.TotalSeconds > 0)
        {
            GetButton(UIButtonE.SafeArea_UpgradeSlot_ShortBtn).gameObject.SetActive(true);
            GetButton(UIButtonE.SafeArea_UpgradeSlot_CompleteBtn).gameObject.SetActive(false);
        }
        else
        {
            GetButton(UIButtonE.SafeArea_UpgradeSlot_ShortBtn).gameObject.SetActive(false);
            GetButton(UIButtonE.SafeArea_UpgradeSlot_CompleteBtn).gameObject.SetActive(true);
        }
    }

    private void OnTimeFlowComplete()
    {
        if (_researchNode == null || _playerData == null)
            return;

        SetButton(_researchNode, _playerData);
    }

    private void OnShort()
    {
        if (_researchNode == null || _playerData == null)
            return;

        UIResearchShort shortUI = Managers.UI.ShopPopupUI<UIResearchShort>(_researchShortPrefabPath, CanvasOrderType.Middle);
        shortUI.UISet(_researchNode);

        shortUI.OnClose += () =>
        {
            Set();
        };
    }
    private void OnComplete()
    {
        return;
        
        if (_researchNode == null || _playerData == null)
            return;

        TimeSpan remainTime = _playerData.RemainTime(Managers.Time.Current);
        if (remainTime.TotalSeconds > 0)
        {
            UnityHelper.Error_H("남은 시간이 있습니다.");
            return;
        }

        if (_isComplete)
            return;

        _isComplete = true;

        PlayerResearchCompleteRequest req = new PlayerResearchCompleteRequest()
        {
            Type = _playerData.Type,
            Floor = _playerData.Floor,
            Index = _playerData.Index,
        };

        Managers.Web.SendPostRequest<PlayerResearchCompleteResponse>("player/grow/research/complete", req, (res) =>
        {
            Managers.PlayerData.DbUpdate(res.Datas);
            _isComplete = false;
            Set();
        });
    }
    
	public enum UIImageE
    {
		SafeArea_UpgradeSlot,
    }
	public enum ResearchSlotE
    {
		SafeArea_UpgradeSlot_Slot,
    }
	public enum UITextProE
    {
		SafeArea_UpgradeSlot_Title,
		SafeArea_UpgradeSlot_Script,
    }
	public enum TimeFlowE
    {
		SafeArea_UpgradeSlot_Time,
    }
	public enum UIButtonE
    {
		SafeArea_UpgradeSlot_ShortBtn,
		SafeArea_UpgradeSlot_CompleteBtn,
    }
}