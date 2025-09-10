using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssetKits.ParticleImage;
using Shared.DTOs.Player;
using Shared.Enums;
using UnityEngine;

public class RobbyGrowResearchTree : UIRobby
{
    protected override void Initialize()
    {
		Bind<ResearchBGColor>(typeof(ResearchBGColorE));
		Bind<ResearchBookParticle>(typeof(ResearchBookParticleE));
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    [SerializeField] private Transform _slotParent;
    private List<ResearchTreeSlot> _researchSlots = new();

    private readonly float _paddingY = 250f;
    private readonly float _spacingX = 250f;
    private readonly float _spacingY = 250f;

    private readonly string _slotPrefabPath = "Prefab/UI/Card/Robby/Research/ResearchTreeSlot";

    public void UISet(UIRobbyGrowResearch growResearch, PlayerGrowResearch type)
    {
        InitSet();
        SetColor(type);

        Managers.PlayerData.DbGets(typeof(PlayerResearchDto), ()=>
        {
            List<PlayerResearchDto> playerResearches = Managers.PlayerData.GetPlayerData<List<PlayerResearchDto>>();
            SetResearchTree(growResearch, type, playerResearches);
        });
    }

    private void InitSet()
    {
        for (int i = 0; i < _researchSlots.Count; i++)
            _researchSlots[i].gameObject.SetActive(false);
    }

    private void SetResearchTree(UIRobbyGrowResearch growResearch, PlayerGrowResearch type, List<PlayerResearchDto> playerResearches)
    {
        if (playerResearches == null)
        {
            playerResearches = new List<PlayerResearchDto>();
        }

        var researchTree = Managers.NodeTree.GetResearchTree(type.ToString());
        var playerDatas = playerResearches.Where(r => r.Type == type).ToList();

        for (int i = 0; i < _researchSlots.Count; i++)
            _researchSlots[i].gameObject.SetActive(false);

        var nodes = researchTree.GetNodes();
        int maxFloor = nodes.Max(n => n.Floor);
        float totalHeight = (maxFloor * _spacingY) + (_paddingY * 2);

        RectTransform slotParentRect = _slotParent.GetComponent<RectTransform>();
        slotParentRect.sizeDelta = new Vector2(slotParentRect.sizeDelta.x, totalHeight);

        var floorGroups = nodes.GroupBy(n => n.Floor)
            .ToDictionary(g => g.Key, g => g.OrderBy(n => n.Index).ToArray());

        int slotIndex = 0;
        foreach (var floorPair in floorGroups)
        {
            int floor = floorPair.Key;
            var nodesOnFloor = floorPair.Value;
            int countOnFloor = nodesOnFloor.Length;

            for (int i = 0; i < countOnFloor; i++)
            {
                var node = nodesOnFloor[i];
                int index = node.Index;

                ResearchTreeSlot slot = null;
                if (slotIndex < _researchSlots.Count)
                    slot = _researchSlots[slotIndex];
                else
                {
                    slot = Managers.Resources.Instantiate<ResearchTreeSlot>(_slotPrefabPath, _slotParent);
                    _researchSlots.Add(slot);
                }

                PlayerResearchDto playerData = playerDatas.FirstOrDefault(r => r.Floor == floor && r.Index == index);

                slot.gameObject.SetActive(true);
                slot.UISet(growResearch, node, playerData);

                float x = (i - (countOnFloor - 1) / 2f) * _spacingX;
                float y = -floor * _spacingY - _paddingY;

                slot.transform.localPosition = new Vector3(x, y, 0);
                slotIndex++;
            }
        }

        for (int i = slotIndex; i < _researchSlots.Count; i++)
            _researchSlots[i].gameObject.SetActive(false);
    }

    private void SetColor(PlayerGrowResearch type)
    {
        Get<ResearchBGColor>(ResearchBGColorE.BG).UISet(type);
        Get<ResearchBookParticle>(ResearchBookParticleE.Particle).UISet(type);
    }
	public enum ResearchBGColorE
    {
		BG,
    }
	public enum ResearchBookParticleE
    {
		Particle,
    }
	public enum UIImageE
    {
		SafeArea_ScrollView,
		SafeArea_ScrollView_Viewport,
		SafeArea_ScrollView_ScrollbarHorizontal,
		SafeArea_ScrollView_ScrollbarHorizontal_SlidingArea_Handle,
		SafeArea_ScrollView_ScrollbarVertical,
		SafeArea_ScrollView_ScrollbarVertical_SlidingArea_Handle,
    }
}