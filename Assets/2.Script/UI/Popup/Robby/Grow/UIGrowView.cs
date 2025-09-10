using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using Shared.DTOs.Player;
using UnityEngine;
using System.Linq;
using Shared.Fomula;

public class UIGrowView : UIPopup
{
    [SerializeField] private RectTransform _sectorRoot;

    private UIRobbyGrowGrow _uiRobbyGrowGrow;

    private List<GrowSector> _sectors = new List<GrowSector>();
    private List<GrowRewardSpeech> _rewardSpeeches = new List<GrowRewardSpeech>();
    private GrowLevelSpeech _levelSpeech;
    private readonly string _sectorPrefabPath = "Prefab/UI/Card/Robby/Grow/GrowViewSector";
    private readonly string _rewardSpeechPrefabPath = "Prefab/UI/Card/Robby/Grow/GrowViewRewardSpeech";
    private readonly string _levelSpeechPrefabPath = "Prefab/UI/Card/Robby/Grow/GrowViewLevelSpeech";

    private readonly float _sliderMin = 0.185f;
    private readonly float _sliderMax = 0.826f;
    private readonly string _cardPrefabPath = "Robby/Research/GrowViewLevelCard";

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<UISlider>(typeof(UISliderE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));


        GetButton(UIButtonE.CloseButton).AddClickEvent((ped) => ClosePopupUIPlayAni());

        base.Initialize();
    }

    public void UISet(UIRobbyGrowGrow uiRobbyGrowGrow)
    {
        _uiRobbyGrowGrow = uiRobbyGrowGrow;
        
        List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();
        int playerGrowLevel = _uiRobbyGrowGrow.GetGrowLevel(playerGrowDatas);

        UISet(playerGrowLevel);
    }

    private void UISet(int growLevel)
    {
        List<TableGrowRewardDto> tableDatas = Managers.Table.GetTableData<List<TableGrowRewardDto>>();
        List<PlayerGrowDto> playerGrowDatas = Managers.PlayerData.GetPlayerData<List<PlayerGrowDto>>();

        int playerTotalLevel = _uiRobbyGrowGrow.GetTotalLevel(playerGrowDatas);
        int playerGrowLevel = _uiRobbyGrowGrow.GetGrowLevel(playerGrowDatas);

        int startLevel = GrowFomula.DestLevelToGrowLevel(growLevel);
        int endLevel = GrowFomula.DestLevelToGrowLevel(growLevel + 1);

        ScrollViewSet(tableDatas, growLevel);
        StarSet(growLevel);
        SliderSet(growLevel, playerTotalLevel);
        SectorSet(growLevel, startLevel, endLevel, tableDatas);
        TotalLevelSpeechSet(playerTotalLevel, startLevel, endLevel, playerGrowLevel == growLevel);
        RewardSpeechSet(growLevel, tableDatas);
    }

    private void ScrollViewSet(List<TableGrowRewardDto> tableDatas, int playerGrowLevel)
    {
        List<ICardData> cardDatas = new List<ICardData>();

        int maxLevel = tableDatas.Max(x => x.Level);
        int maxGrowLevel = GrowFomula.CurrentGrowLevel(maxLevel);
        int selectIndex = maxGrowLevel;

        for (int i = maxGrowLevel; i >= 1; i--)
        {
            cardDatas.Add(new GrowViewLevelCardData { GrowLevel = i });

            if (playerGrowLevel == i)
            {
                selectIndex = i;
            }
        }

        GetScrollView(UIScrollViewE.Main_GrowLevels).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPrefabPath, cardDatas, selectIndex, 1, UIScrollViewLayoutStartCorner.Middle, 0, 0);

        for (int i = 0; i < GetScrollView(UIScrollViewE.Main_GrowLevels).CardCount; i++)
        {
            GrowViewLevelCard growViewLevelCard = GetScrollView(UIScrollViewE.Main_GrowLevels).GetCard(i) as GrowViewLevelCard;
            growViewLevelCard.OnClickAction -= OnClickCard;
            growViewLevelCard.OnClickAction += OnClickCard;
        }
    }

    private void StarSet(int currentGrowLevel)
    {
        GetText(UITextE.Main_Next_Text).text = $"{currentGrowLevel + 1}";

        GetText(UITextE.Main_Before_Text).text = $"{currentGrowLevel}";
    }

    private void SliderSet(int growLevel, int totalLevel)
    {
        int startTotalLevel = GrowFomula.DestLevelToGrowLevel(growLevel);
        int endTotalLevel = GrowFomula.DestLevelToGrowLevel(growLevel + 1);

        float sliderValue = Mathf.Lerp(_sliderMin, _sliderMax, (float)(totalLevel - startTotalLevel) / (endTotalLevel - startTotalLevel));

        UISlider slider = Get<UISlider>(UISliderE.Main_Slider);
        slider.value = sliderValue;
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
    private void TotalLevelSpeechSet(int totalLevel, int startLevel, int endLevel, bool isOn)
    {
        if (_levelSpeech == null)
        {
            _levelSpeech = Managers.Resources.Instantiate<GrowLevelSpeech>(_levelSpeechPrefabPath, _sectorRoot);
        }

        _levelSpeech.gameObject.SetActive(isOn);

        if (!isOn)
            return;

        _levelSpeech.UISet(totalLevel);

        SectorPositionSet(_levelSpeech.transform, totalLevel, startLevel, endLevel);
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
            SectorPositionSet(rewardSpeech.transform, rewardDatas[i].Level, startLevel, endLevel, -186f);
        }
    }
    private void SectorPositionSet(Transform sector, int level, int startLevel, int endLevel, float addPosX = 0)
    {
        float normalizedPosition = (float)(level - startLevel) / (endLevel - startLevel);

        RectTransform rectTransform = sector.GetComponent<RectTransform>();
        float yPos = Mathf.Lerp(_sliderMin * _sectorRoot.rect.height, _sliderMax * _sectorRoot.rect.height, normalizedPosition) - (_sectorRoot.rect.height * 0.5f);
        rectTransform.anchoredPosition = new Vector2(addPosX, yPos);
    }

    private void OnClickCard(int growLevel)
    {
        UISet(growLevel);
    }


	public enum UIImageE
    {
		Black,
		Main,
		Main_Next,
		Main_Before,
		Image,
    }
	public enum UIScrollViewE
    {
		Main_GrowLevels,
    }
	public enum UISliderE
    {
		Main_Slider,
    }
	public enum UITextE
    {
		Main_Next_Text,
		Main_Before_Text,
    }
	public enum UIButtonE
    {
		CloseButton,
    }
}