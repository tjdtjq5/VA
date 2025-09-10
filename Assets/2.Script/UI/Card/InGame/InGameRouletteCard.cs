using System;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using UnityEngine;

public class InGameRouletteCard : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    
    public int Index { get; private set; }
    UIInGameRoulette UIInGameRoulette { get; set; }

    [SerializeField] private ParticleImage selectParticle;
    [SerializeField] private Animator iconAnimator;

    public List<Skill> rewardSkills;
    
    private AniController _iconAniController;
    private Skill _reward;
    
    private readonly int _iconPlayHash = Animator.StringToHash("Play");
    
    public void Initialize(UIInGameRoulette uiInGameRoulette, int index)
    {
	    this.Index = index;
	    this.UIInGameRoulette = uiInGameRoulette;

	    _iconAniController = iconAnimator.Initialize();

	    uiInGameRoulette.OnSelect += OnSelect;
	    uiInGameRoulette.OnResultSelect += OnResultSelect;
    }
    
    public void UISetIObject()
    {
	    List<Skill> learnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(rewardSkills);

	    _reward = learnSkills[0];
	    
	    SetSkill(_reward);
    }
    void SetSkill(Skill skill)
    {
	    if (skill)
		    GetImage(UIImageE.Bg).sprite = Managers.Atlas.GetSkillGradeBg(skill.Grade);

	    GetImage(UIImageE.Icon).sprite = skill.Icon;
	    GetImage(UIImageE.Icon).SetNativeSize();
    }

    void OnSelect(int index)
    {
	    if (Index == index)
	    {
		    _iconAniController.SetTrigger(_iconPlayHash);
	    }
	    
	    GetImage(UIImageE.Select).gameObject.SetActive(Index == index);
    }

    void OnResultSelect(int index)
    {
	    if (Index == index)
	    {
		    selectParticle.Play();
	    }
    }

    public IdentifiedObject GetResult()
    {
	    return _reward;
    }
    
	public enum UIImageE
    {
		Select,
		Bg,
		Icon,
    }
}