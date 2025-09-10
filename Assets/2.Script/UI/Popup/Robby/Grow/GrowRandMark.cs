using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.DTOs.Player;
using Shared.DTOs.Table;
using Shared.Enums;
using Shared.Fomula;
using UnityEngine;

public class GrowRandMark : UIFrame
{
    [SerializeField] private UIRobbyGrowGrow _uiRobbyGrowGrow;
    [SerializeField] private RectTransform _sectorRoot;
    [SerializeField] private GrowLevelSpeech _levelSpeech;

    private List<GrowSector> _sectors = new List<GrowSector>();
    private List<GrowRewardSpeech> _rewardSpeeches = new List<GrowRewardSpeech>();
    private int _growLevel = 0;
    private int _totalLevel = 0;
    private Tween<float> _sliderTween;

    private readonly string _sectorPrefabPath = "Prefab/UI/Card/Robby/Grow/Sector";
    private readonly string _rewardSpeechPrefabPath = "Prefab/UI/Card/Robby/Grow/GrowRewardSpeech";
    private readonly string _growViewPrefabPath = "Robby/UIGrowView";
    private readonly float _sliderMin = 0.063f;
    private readonly float _sliderMax = 0.905f;

    protected override void Initialize()
    {
        Bind<UIImage>(typeof(UIImageE));
        Bind<UITextPro>(typeof(UITextProE));
        Bind<UISlider>(typeof(UISliderE));
        Bind<UIButton>(typeof(UIButtonE));
        Bind<UIText>(typeof(UITextE));

        GetButton(UIButtonE.InfoButton).AddClickEvent((ped) => OnClickInfo());

        base.Initialize();
    }

    public void Set()
    {
        // Table Load
        List<TableGrowRewardDto> tableDatas = Managers.Table.GetTableData<List<TableGrowRewardDto>>();

        // Player Data Load
        List<Type> types = new List<Type> { typeof(PlayerGrowDto) };
        Managers.PlayerData.DbGets(types, () =>
        {
            if (_sliderTween != null)
            {
                _sliderTween.FullKill();
                _sliderTween = null;
            }

            List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();

            _growLevel = _uiRobbyGrowGrow.GetGrowLevel(playerGrowDatas);
            _totalLevel = _uiRobbyGrowGrow.GetTotalLevel(playerGrowDatas);
            int startLevel = GrowFomula.DestLevelToGrowLevel(_growLevel);
            int endLevel = GrowFomula.DestLevelToGrowLevel(_growLevel + 1);

            GrowFlagSet(_growLevel);
            NextStarSet(_growLevel + 1);
            SliderSet(_growLevel, _totalLevel);
            SectorSet(_growLevel, startLevel, endLevel, tableDatas);
            TotalLevelSpeechSet(_totalLevel, startLevel, endLevel);
            RewardSpeechSet(_growLevel, tableDatas);
        });
    }

    public void InitSet()
    {
        GrowFlagSet(0);
        NextStarSet(0);
        SliderSet(0, 0);
        SectorSet(0, 0, 0, null);
        TotalLevelSpeechSet(0, 0, 0);
        RewardSpeechSet(0, null);
    }

    public void UpgradeSet(int plusLevel)
    {
        int newTotalLevel = _totalLevel + plusLevel;
        // int newGrowLevel = _uiRobbyGrowGrow.GetGrowLevel(newTotalLevel);

        GrowFlagSet(_growLevel);
        NextStarSet(_growLevel + 1);
        SliderAnimation(_growLevel, newTotalLevel);
        SectorSet(_growLevel, _growLevel, _growLevel + 1, null);
        TotalLevelSpeechSet(newTotalLevel, _growLevel, _growLevel + 1);
        RewardSpeechSet(_growLevel, null);
    }

    private void GrowFlagSet(int growLevel)
    {
        GetTextPro(UITextProE.GrowLevelFlag_Level).text = $"{growLevel}";
    }

    private void NextStarSet(int nextGrowLevel)
    {
        GetText(UITextE.Next_Text).text = $"{nextGrowLevel}";
    }

    private void SliderSet(int growLevel, int totalLevel)
    {
        int startTotalLevel = GrowFomula.DestLevelToGrowLevel(growLevel);
        int endTotalLevel = GrowFomula.DestLevelToGrowLevel(growLevel + 1);

        float sliderValue = Mathf.Lerp(_sliderMin, _sliderMax, (float)(totalLevel - startTotalLevel) / (endTotalLevel - startTotalLevel));

        UISlider slider = Get<UISlider>(UISliderE.Slider);
        slider.value = sliderValue;
    }

    private void SliderAnimation(int growLevel, int totalLevel)
    {
        int startTotalLevel = GrowFomula.DestLevelToGrowLevel(growLevel);
        int endTotalLevel = GrowFomula.DestLevelToGrowLevel(growLevel + 1);

        float sliderValue = Mathf.Lerp(_sliderMin, _sliderMax, (float)(totalLevel - startTotalLevel) / (endTotalLevel - startTotalLevel));
        UISlider slider = Get<UISlider>(UISliderE.Slider);

        // if (_sliderTween != null)
        // {
        //     _sliderTween.FullKill();
        //     _sliderTween = null;
        // }

        _sliderTween = Managers.Tween.TweenSlider(slider, slider.value, sliderValue, 0.15f);
    }

    private void SectorSet(int growLevel, int startLevel, int endLevel, List<TableGrowRewardDto> tableDatas)
    {
        _sectors.ForEach(x => x.gameObject.SetActive(false));

        if (tableDatas == null || tableDatas.Count == 0)
        {
            return;
        }

        List<TableGrowRewardDto> sectorDatas = tableDatas.Where(x => GrowFomula.CurrentGrowLevel(x.Level) == growLevel).ToList();

        for (int i = 0; i < sectorDatas.Count; i++)
        {
            GrowSector sector = null;
            if (i >= _sectors.Count)
            {
                sector = Managers.Resources.Instantiate<GrowSector>(_sectorPrefabPath, _sectorRoot);
                _sectors.Add(sector);
            }
            else
            {
                sector = _sectors[i];
            }

            sector.gameObject.SetActive(true);
            sector.UISet(sectorDatas[i]);

            SectorPositionSet(sector.transform, sectorDatas[i].Level, startLevel, endLevel);
        }
    }

    private void TotalLevelSpeechSet(int totalLevel, int startLevel, int endLevel)
    {
        _levelSpeech.UISet(totalLevel);
    }

    private void RewardSpeechSet(int growLevel, List<TableGrowRewardDto> tableDatas)
    {
        _rewardSpeeches.ForEach(x => x.gameObject.SetActive(false));

        if (tableDatas == null || tableDatas.Count == 0)
        {
            return;
        }

        List<TableGrowRewardDto> rewardDatas = tableDatas.Where(x => GrowFomula.CurrentGrowLevel(x.Level) == growLevel).ToList();
        int startLevel = GrowFomula.DestLevelToGrowLevel(growLevel);
        int endLevel = GrowFomula.DestLevelToGrowLevel(growLevel + 1);

        for (int i = 0; i < rewardDatas.Count; i++)
        {
            GrowRewardSpeech rewardSpeech = null;
            if (i >= _rewardSpeeches.Count)
            {
                rewardSpeech = Managers.Resources.Instantiate<GrowRewardSpeech>(_rewardSpeechPrefabPath, _sectorRoot);
                _rewardSpeeches.Add(rewardSpeech);
            }
            else
            {
                rewardSpeech = _rewardSpeeches[i];
                rewardSpeech.gameObject.SetActive(true);
            }

            rewardSpeech.UISet(new ItemValue(Managers.SO.GetItem(rewardDatas[i].ItemCode), rewardDatas[i].ItemCount));
            SectorPositionSet(rewardSpeech.transform, rewardDatas[i].Level, startLevel, endLevel);
        }
    }

    private void SectorPositionSet(Transform sector, int level, int startLevel, int endLevel)
    {
        float normalizedPosition = (float)(level - startLevel) / (endLevel - startLevel);

        RectTransform rectTransform = sector.GetComponent<RectTransform>();
        float xPos = Mathf.Lerp(_sliderMin * _sectorRoot.rect.width, _sliderMax * _sectorRoot.rect.width, normalizedPosition) - (_sectorRoot.rect.width * 0.5f);
        rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
    }

    private void OnClickInfo()
    {
        UIGrowView growView = Managers.UI.ShopPopupUI<UIGrowView>(_growViewPrefabPath, CanvasOrderType.Middle);
        growView.UISet(_uiRobbyGrowGrow);
    }


    public enum UIImageE
    {
        GrowLevelFlag,
        Next,
    }

    public enum UITextProE
    {
        GrowLevelFlag_Level,
    }

    public enum UISliderE
    {
        Slider,
    }

    public enum UIButtonE
    {
        InfoButton,
    }

    public enum UITextE
    {
        Next_Text,
    }
}