using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Player;
using Shared.Enums;
using UnityEngine;

public class ResearchTreeSlot : UIButton
{
    private UIRobbyGrowResearch _growResearch;
    private ResearchNode _researchNode;
    private PlayerResearchDto _playerData;

    private readonly string _researchNodePrefabPath = "Robby/Research/UINodeResearch";
    private readonly string _researchAlreadyPrefabPath = "Robby/Research/UINodeResearchAlready";
    private readonly string _redColor = "FF2F31";
    private readonly string _blueColor = "2F47FF";
    private readonly string _greenColor = "19CA00";
    private readonly string _yellowColor = "FFAD00";
    private readonly string _masterColor = "FFFFFF";
    private readonly string _redLightColor = "FFBABB";
    private readonly string _blueLightColor = "69C8FF";
    private readonly string _greenLightColor = "CEFF6B";
    private readonly string _yellowLightColor = "F4FF55";
    private readonly string _masterLightColor = "FFFFFF";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();

        AddClickEvent(ped => OnClick());
    }

    public void UISet(UIRobbyGrowResearch growResearch, ResearchNode node, PlayerResearchDto playerData)
    {
        _growResearch = growResearch;
        _researchNode = node;
        _playerData = playerData;

        int playerLevel = playerData != null ? playerData.Level : 0;
        GetImage(UIImageE.Icon).sprite = node.Research.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();
        GetTextPro(UITextProE.Count).text = $"{playerLevel} / {node.Research.MaxLevel}";

        SetColor(node.Type);
    }

    private void SetColor(PlayerGrowResearch type)
    {
        switch (type)
        {
            case PlayerGrowResearch.RedBook:
                GetImage(UIImageE.BG).SetColor(_redColor);
                GetImage(UIImageE.Light).SetColor(_redLightColor);
                break;
            case PlayerGrowResearch.BlueBook:
                GetImage(UIImageE.BG).SetColor(_blueColor);
                GetImage(UIImageE.Light).SetColor(_blueLightColor);
                break;
            case PlayerGrowResearch.GreenBook:
                GetImage(UIImageE.BG).SetColor(_greenColor);
                GetImage(UIImageE.Light).SetColor(_greenLightColor);
                break;
            case PlayerGrowResearch.YellowBook:
                GetImage(UIImageE.BG).SetColor(_yellowColor);
                GetImage(UIImageE.Light).SetColor(_yellowLightColor);
                break;
            case PlayerGrowResearch.MasterBook:
                GetImage(UIImageE.BG).SetColor(_masterColor);
                GetImage(UIImageE.Light).SetColor(_masterLightColor);
                break;
        }
    }

    private void OnClick()
    {
        if (_researchNode == null || _growResearch == null)
            return;

        List<PlayerResearchDto> playerDatas = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
        PlayerResearchDto progressData = playerDatas.Find(x => x.IsProgress);

        bool isAlreadyProgress = progressData != null;

        if (isAlreadyProgress)
        {
            UINodeResearchAlready nodeUI = Managers.UI.ShopPopupUI<UINodeResearchAlready>(_researchAlreadyPrefabPath, CanvasOrderType.Middle);
            nodeUI.UISet(_researchNode);
        }
        else
        {
            UINodeResearch nodeUI = Managers.UI.ShopPopupUI<UINodeResearch>(_researchNodePrefabPath, CanvasOrderType.Middle);
            nodeUI.UISet(_growResearch, _researchNode);

            nodeUI.OnClose += () =>
            {
                _growResearch.Set();
            };
        }
    }

	public enum UIImageE
    {
		BG,
		BG_Outline,
		Light,
		Icon,
    }
	public enum UITextProE
    {
		Count,
    }
}