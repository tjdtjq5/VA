using System;
using System.Collections;
using System.Collections.Generic;
using Shared.BBNumber;
using Shared.DTOs.Player;
using Shared.Fomula;
using UnityEngine;

public class UIResearchShort : UIPopup
{
    [SerializeField] private Item _scrollSkip;

    private ResearchNode _researchNode;
    private PlayerResearchDto _playerData;
    private BBNumber _scrollSkipCount;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<ResearchTreeSlot>(typeof(ResearchTreeSlotE));
		Bind<TimeFlow>(typeof(TimeFlowE));
		Bind<UIText>(typeof(UITextE));
		Bind<CountView>(typeof(CountViewE));
		Bind<NeedItemView>(typeof(NeedItemViewE));
		Bind<UIButton>(typeof(UIButtonE));


        GetButton(UIButtonE.Main_OkBtn).AddClickEvent(ped => OnResearchShort());
        GetButton(UIButtonE.Main_CloseButton).AddClickEvent((ped) => ClosePopupUIPlayAni());

        Get<CountView>(CountViewE.Main_CountView).OnCountChanged += OnChagedCount;
        Get<TimeFlow>(TimeFlowE.Main_TimeFlow).OnTimeFlow += OnChagedTime;

        base.Initialize();
    }

    public void UISet(ResearchNode researchNode)
    {
        _researchNode = researchNode;
        List<PlayerResearchDto> playerDatas = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
        _playerData = playerDatas.Find(x => x.Floor == researchNode.Floor && x.Index == researchNode.Index);
        _scrollSkipCount = Managers.PlayerData.GetPlayerItemCount(_scrollSkip.CodeName);

        Get<CountView>(CountViewE.Main_CountView).Count = 1;

        TitleSet(researchNode.Research);
        TreeSlotSet(researchNode, _playerData);
        TimeSet(researchNode, _playerData);
        CountSet(researchNode, _playerData, _scrollSkipCount);
    }

    private void TitleSet(Research research)
    {
        GetTextPro(UITextProE.Main_Title_Text).text = $"{research.DisplayName}";
    }

    private void TreeSlotSet(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        Get<ResearchTreeSlot>(ResearchTreeSlotE.Main_Slot).UISet(null, researchNode, playerData);
    }

    private void TimeSet(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        int playerLevel = playerData != null ? playerData.Level : 0;
        TimeSpan remainTime = playerData.RemainTime(Managers.Time.Current);
        Get<TimeFlow>(TimeFlowE.Main_TimeFlow).UISet(remainTime, true);
    }

    private void CountSet(ResearchNode researchNode, PlayerResearchDto playerData, BBNumber scrollSkipCount)
    {
        int playerLevel = playerData != null ? playerData.Level : 0;
        TimeSpan remainTime = playerData.RemainTime(Managers.Time.Current);
        int scrollSkipSecValue = ItemFomula.ScrollSkipSecValue;
        int skipCount = (int)(remainTime.TotalSeconds / scrollSkipSecValue);
        int countViewMax = Mathf.Min(scrollSkipCount.ToInt(), skipCount);
        int count = Get<CountView>(CountViewE.Main_CountView).Count;

        Get<CountView>(CountViewE.Main_CountView).UISet(count, 1, countViewMax);
        NeedItemSet(count);
        ShortTimeSet(count);
    }

    private void NeedItemSet(int count)
    {
        List<ItemValue> itemValues = new List<ItemValue>();
        itemValues.Add(new ItemValue(_scrollSkip, count));

        Get<NeedItemView>(NeedItemViewE.Main_NeedItemView).UISet(itemValues.ToArray());
    }

    private void ShortTimeSet(int count)
    {
        int minute = ItemFomula.ScrollSkipSecValue * count / 60;
        GetText(UITextE.Main_TimeShort).text = $"{minute}분 단축";
    }

    private void OnChagedCount(int count)
    {
        NeedItemSet(count);
        ShortTimeSet(count);
    }
    private void OnChagedTime()
    {
        if (_researchNode == null || _playerData == null)
            return;

        CountSet(_researchNode, _playerData, _scrollSkipCount);
    }
    private void OnResearchShort()
    {
        return;
        
        if (_researchNode == null || _playerData == null)
            return;

        int needItemCount = Get<CountView>(CountViewE.Main_CountView).Count;
        if (Managers.PlayerData.GetPlayerItemCount(_scrollSkip.CodeName) < needItemCount)
        {
            UnityHelper.Log_H("스크롤 스킵 아이템이 부족합니다.");
            return;
        }

        PlayerResearchShortRequest request = new PlayerResearchShortRequest()
        {
            Type = _researchNode.Type,
            Floor = _researchNode.Floor,
            Index = _researchNode.Index,
            Count = Get<CountView>(CountViewE.Main_CountView).Count
        };

        Managers.Web.ShowWebLoading();
        Managers.Web.SendPostRequest<PlayerResearchShortResponse>("player/grow/research/short", request, (response) =>
        {
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
    }
	public enum UITextProE
    {
		Main_Title_Text,
    }
	public enum ResearchTreeSlotE
    {
		Main_Slot,
    }
	public enum TimeFlowE
    {
		Main_TimeFlow,
    }
	public enum UITextE
    {
		Main_TimeShort,
		Main_Ex,
    }
	public enum CountViewE
    {
		Main_CountView,
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