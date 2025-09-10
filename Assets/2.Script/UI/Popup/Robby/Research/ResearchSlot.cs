using System;
using Shared.DTOs.Player;
using Shared.Enums;
using UnityEngine;

public class ResearchSlot : UIFrame
{
    [SerializeField] private TimeFlow _timeFlow;

    private ResearchNode _node;
    private PlayerResearchDto _playerData;

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
    }

    public void UISet(ResearchNode node, PlayerResearchDto playerData)
    {
        _node = node;
        _playerData = playerData;

        int playerLevel = playerData != null ? playerData.Level : 0;
        GetImage(UIImageE.Icon).sprite = node.Research.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();
        GetTextPro(UITextProE.Count).text = $"{playerLevel} / {node.Research.MaxLevel}";

        FillSet();
        SetColor(_node.Type);

        if (_timeFlow != null)
        {
            _timeFlow.OnTimeFlow -= FillSet;
            _timeFlow.OnTimeFlow += FillSet;

            _timeFlow.OnTimeEnd -= TimeFlowEnd;
            _timeFlow.OnTimeEnd += TimeFlowEnd;
        }
    }

    private void FillSet()
    {
        if (_playerData == null || _node == null)
        {
            return;
        }

        int playerLevel = _playerData.Level;
        bool isProgress = _playerData.IsProgress;
        GetImage(UIImageE.FillAmount).FillAmount = 0f;
        if (isProgress)
        {
            float fillAmount = _playerData.ProgressValue(Managers.Time.Current);

            GetImage(UIImageE.FillAmount).FillAmount = fillAmount;
        }
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

    private void TimeFlowEnd()
    {
        _playerData.IsProgress = false;
    }


	public enum UIImageE
    {
		FillAmount,
		BG,
		BG_Outline,
		BG_InOutline,
		Light,
		Icon,
    }
	public enum UITextProE
    {
		Count,
    }
}