using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Player;
using Shared.Fomula;
using UnityEngine;

public class UINodeResearch : UIPopup
{
    private ResearchNode _researchNode;
    private PlayerResearchDto _playerData;
    private bool _isResearching;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<ResearchTreeSlot>(typeof(ResearchTreeSlotE));
		Bind<TimeFlow>(typeof(TimeFlowE));
		Bind<NeedItemView>(typeof(NeedItemViewE));
		Bind<UIButton>(typeof(UIButtonE));


        GetButton(UIButtonE.Main_CloseButton).AddClickEvent((ped) => ClosePopupUIPlayAni());
        GetButton(UIButtonE.Main_OkBtn).AddClickEvent((ped) => OnResearch());

        base.Initialize();
    }

    public void UISet(UIRobbyGrowResearch growResearch, ResearchNode researchNode)
    {
        _researchNode = researchNode;
        _isResearching = false;
        
        List<PlayerResearchDto> playerDatas = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
        _playerData = playerDatas.Find(x => x.Type == researchNode.Type && x.Floor == researchNode.Floor && x.Index == researchNode.Index);
    
        TitleSet(researchNode.Research);
        TreeSlotSet(_playerData);
        TimeSet(researchNode, _playerData);
        ValueSet(researchNode, _playerData);
        NeedItemSet(researchNode);
        ResearchButtonSet(playerDatas);
    }

    private void TitleSet(Research research)
    {
        GetTextPro(UITextProE.Main_Title_Text).text = $"{research.DisplayName}";
    }
    private void TreeSlotSet(PlayerResearchDto playerData)
    {
        Get<ResearchTreeSlot>(ResearchTreeSlotE.Main_Slot).UISet(null, _researchNode, playerData);
    }
    private void TimeSet(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        int playerLevel = playerData != null ? playerData.Level : 0;
        int researchTime = ResearchFomula.GetResearchMaxTime(researchNode.Floor, playerLevel, ResearchFomula.IsSpecial(researchNode.Floor));
        Get<TimeFlow>(TimeFlowE.Main_TimeFlow).UISet(researchTime, false);
    }
    private void ValueSet(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        int playerLevel = playerData != null ? playerData.Level : 0;
        int nextLevel = playerLevel + 1;
        int maxLevel = researchNode.Research.MaxLevel;

        if(nextLevel <= maxLevel)
        {
            StatValue currentValue = researchNode.Research.StatValue(playerLevel);
            StatValue nextValue = researchNode.Research.StatValue(nextLevel);

            GetTextPro(UITextProE.Main_CurrentValue).text = $"{currentValue.ToString()}";
            GetTextPro(UITextProE.Main_NextValue_Text).text = $"{nextValue.ToString()}";
        }
        else
        {
            StatValue currentValue = researchNode.Research.StatValue(playerLevel);

            GetTextPro(UITextProE.Main_CurrentValue).text = $"{currentValue.ToString()}";
            GetTextPro(UITextProE.Main_NextValue_Text).text = $"{currentValue.ToString()}";
        }
    }

    private void NeedItemSet(ResearchNode researchNode)
    {
        int floor = researchNode.Floor;
        List<ItemValue> needItems = new List<ItemValue>();
        needItems.Add(new ItemValue(Managers.SO.GetItem(ResearchFomula.ResearchItemCode), ResearchFomula.GetNeedItemCount(floor)));

        if (ResearchFomula.IsSpecial(floor))  
        {
            needItems.Add(new ItemValue(Managers.SO.GetItem(ResearchFomula.SpeicalResearchItemCode), ResearchFomula.GetSpecialNeedItemCount(floor)));
        }

        Get<NeedItemView>(NeedItemViewE.Main_NeedItemView).UISet(needItems.ToArray());
    }

    private void ResearchButtonSet(List<PlayerResearchDto> playerDatas)
    {
        bool isProgress = playerDatas.Find(x => x.IsProgress) != null;
        bool isLevelMax = _playerData != null && _playerData.Level >= _researchNode.Research.MaxLevel;

        bool isOk = !isProgress && !isLevelMax;

        GetButton(UIButtonE.Main_OkBtn).UISet(isOk ? ButtonSprite.Button_Green : ButtonSprite.Button_Gray);
    }

    private void OnResearch()
    {
        return;
        
        if(_isResearching)
            return;

        _isResearching = true;

        List<PlayerResearchDto> playerDatas = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
        bool isProgress = playerDatas.Find(x => x.IsProgress) != null;

        if(isProgress)
        {
            UnityHelper.Log_H("이미 진행중인 연구가 있습니다.");
            return;
        }

        if(_playerData != null && _playerData.Level >= _researchNode.Research.MaxLevel)
        {
            UnityHelper.Log_H("최대 레벨에 도달했습니다.");
            return;
        }

        PlayerResearchRequest request = new PlayerResearchRequest()
        {
            Type = _researchNode.Type,
            Floor = _researchNode.Floor,
            Index = _researchNode.Index,
        };

        Managers.Web.ShowWebLoading();
        Managers.Web.SendPostRequest<PlayerResearchResponse>("player/grow/research", request, (response) =>
        {
            _isResearching = false;
            Managers.PlayerData.DbUpdate(response.Datas);

            ClosePopupUIPlayAni();
        });
    }

	public enum UIImageE
    {
		Black,
		Main,
		Main_InBG,
		Main_Title,
		Main_Arrow,
		Main_NextValue,
    }
	public enum UITextProE
    {
		Main_Title_Text,
		Main_CurrentValue,
		Main_NextValue_Text,
    }
	public enum ResearchTreeSlotE
    {
		Main_Slot,
    }
	public enum TimeFlowE
    {
		Main_TimeFlow,
    }
	public enum NeedItemViewE
    {
		Main_NeedItemView,
    }
	public enum UIButtonE
    {
		Main_OkBtn,
		Main_CloseButton,
    }
}