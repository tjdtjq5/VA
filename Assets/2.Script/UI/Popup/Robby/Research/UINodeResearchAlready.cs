using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Player;
using UnityEngine;

public class UINodeResearchAlready : UIPopup
{
    protected override void Initialize()
    {
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));
        Bind<ResearchTreeSlot>(typeof(ResearchTreeSlotE));
        Bind<TimeFlow>(typeof(TimeFlowE));
        Bind<UIText>(typeof(UITextE));
        Bind<UIButton>(typeof(UIButtonE));

        GetButton(UIButtonE.Main_CloseButton).AddClickEvent((ped) => ClosePopupUIPlayAni());

        base.Initialize();
    }

    public void UISet(ResearchNode researchNode)
    {
        List<PlayerResearchDto> playerDatas = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
        PlayerResearchDto playerData = playerDatas.Find(x => x.Floor == researchNode.Floor && x.Index == researchNode.Index);

        TitleSet(researchNode.Research);
        TreeSlotSet(researchNode, playerData);
        TimeSet(researchNode, playerData);
        ValueSet(researchNode, playerData);
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
        int researchTime = playerData != null ? playerData.ResearchTimeSec() : 0;
        Get<TimeFlow>(TimeFlowE.Main_TimeFlow).UISet(researchTime, false);
    }

    private void ValueSet(ResearchNode researchNode, PlayerResearchDto playerData)
    {
        int playerLevel = playerData != null ? playerData.Level : 0;
        StatValue currentValue = researchNode.Research.StatValue(playerLevel);

        GetTextPro(UITextProE.Main_CurrentValue).text = $"{currentValue.ToString()}";
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
        Main_CurrentValue,
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
        Main_Ex,
    }

    public enum UIButtonE
    {
        Main_CloseButton,
    }
}