using System.Collections.Generic;
using AssetKits.ParticleImage;
using UnityEngine;

public class UIInGameSlotMachine : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<GoodsPrice>(typeof(GoodsPriceE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<RouletteScrollView>(typeof(RouletteScrollViewE));

        base.Initialize();
    }

    [SerializeField] List<Skill> leftSkillSlots = new List<Skill>();
    [SerializeField] List<Buff> leftBuffSlots = new List<Buff>();
    [SerializeField] List<Skill> middleSkillSlots = new List<Skill>();
    [SerializeField] List<Buff> middleBuffSlots = new List<Buff>();
    [SerializeField] List<Skill> rightSkillSlots = new List<Skill>();
    [SerializeField] List<Buff> rightBuffSlots = new List<Buff>();
    [SerializeField] GoodsPrice leftSlotPrice;
    [SerializeField] GoodsPrice middleSlotPrice;
    [SerializeField] GoodsPrice rightSlotPrice;
    [SerializeField] ParticleImage leftSlotParticle;
    [SerializeField] ParticleImage middleSlotParticle;
    [SerializeField] ParticleImage rightSlotParticle;
 
    
    [Header("Sprites")]
    [SerializeField] Sprite goodsSprite;
    
    private readonly string _rouletteCardPath = "InGame/UIInGameSlotMachineCard";
    private readonly int _totalGreenStar = 500;
    private readonly int _leftSlotGreenStar = 1;
    private readonly int _middleSlotGreenStar = 2;
    private readonly int _rightSlotGreenStar = 3;
    private readonly float _leftSlotPercent = 100;
    private readonly float _middleSlotPercent = 70;
    private readonly float _rightSlotPercent = 40;
    private readonly List<IdentifiedObject> _leftSlots = new List<IdentifiedObject>();
    private readonly List<IdentifiedObject> _middleSlots = new List<IdentifiedObject>();
    private readonly List<IdentifiedObject> _rightSlots = new List<IdentifiedObject>();
    private readonly List<IdentifiedObject> _resultSlots = new List<IdentifiedObject>();

    private int _currentGreenStar;
    
    protected override void UISet()
    {
	    base.UISet();

	    this._currentGreenStar = _totalGreenStar;
	    
	    SetSlotGreenStar();
	    GetButton(UIButtonE.Main_OkBtn).AddPointDownEvent((ped) => OnClickOK());
	    
	    leftSlotPrice.UISet(goodsSprite);
	    middleSlotPrice.UISet(goodsSprite);
	    rightSlotPrice.UISet(goodsSprite);
	    SetMainGreenStar();
	    
	    GetText(UITextE.Main_SlotMachine_Left_Percent_Text).text = $"{_leftSlotPercent}%"; 
	    GetText(UITextE.Main_SlotMachine_Middle_Percent_Text).text = $"{_middleSlotPercent}%"; 
	    GetText(UITextE.Main_SlotMachine_Right_Percent_Text).text = $"{_rightSlotPercent}%"; 
	    
	    GetButton(UIButtonE.Main_SlotMachine_Left_PriceBtn2).AddPointDownEvent((ped) => OnClickMachineLeft());
	    GetButton(UIButtonE.Main_SlotMachine_Middle_PriceBtn2).AddPointDownEvent((ped) => OnClickMachineMiddle());
	    GetButton(UIButtonE.Main_SlotMachine_Right_PriceBtn2).AddPointDownEvent((ped) => OnClickMachineRight());
	    GetButton(UIButtonE.Main_LearnSkillBtn).AddPointDownEvent((ped) => OnClickLearn());
	    
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).OnEnd -= OnEndMachineLeft;
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).OnEnd -= OnEndMachineMiddle;
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).OnEnd -= OnEndMachineRight;
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).OnEnd += OnEndMachineLeft;
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).OnEnd += OnEndMachineMiddle;
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).OnEnd += OnEndMachineRight;
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    _resultSlots.Clear();
	    
	    SetRoulette();
	    SetMainGreenStar();
    }

    void SetRoulette()
    {
	    _leftSlots.Clear();
	    _middleSlots.Clear();
	    _rightSlots.Clear();
	    
	    List<Skill> leftLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(leftSkillSlots);
	    List<Skill> middleLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(middleSkillSlots);
	    List<Skill> rightLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(rightSkillSlots);
	    
	    List<ICardData> leftRouletteDatas = new List<ICardData>();
	    for (int j = 0; j < leftLearnSkills.Count; j++)
	    {
		    leftRouletteDatas.Add(new UIInGameSlotMachineCardData() { SkillOrBuff = leftLearnSkills[j]});
		    _leftSlots.Add(leftLearnSkills[j]);
	    }

	    for (int j = 0; j < leftBuffSlots.Count; j++)
	    {
		    leftRouletteDatas.Add(new UIInGameSlotMachineCardData() { SkillOrBuff = leftBuffSlots[j]});
		    _leftSlots.Add(leftBuffSlots[j]);
	    }
	    
	    List<ICardData> middleRouletteDatas = new List<ICardData>();
	    for (int j = 0; j < middleLearnSkills.Count; j++)
	    {
		    middleRouletteDatas.Add(new UIInGameSlotMachineCardData() { SkillOrBuff = middleLearnSkills[j]});
		    _middleSlots.Add(middleLearnSkills[j]);
	    }

	    for (int j = 0; j < middleBuffSlots.Count; j++)
	    {
		    middleRouletteDatas.Add(new UIInGameSlotMachineCardData() { SkillOrBuff = middleBuffSlots[j]});
		    _middleSlots.Add(middleBuffSlots[j]);
	    }
	    
	    List<ICardData> rightRouletteDatas = new List<ICardData>();
	    for (int j = 0; j < rightLearnSkills.Count; j++)
	    {
		    rightRouletteDatas.Add(new UIInGameSlotMachineCardData() { SkillOrBuff = rightLearnSkills[j]});
		    _rightSlots.Add(rightLearnSkills[j]);
	    }

	    for (int j = 0; j < rightBuffSlots.Count; j++)
	    {
		    rightRouletteDatas.Add(new UIInGameSlotMachineCardData() { SkillOrBuff = rightBuffSlots[j]});
		    _rightSlots.Add(rightBuffSlots[j]);
	    }
	    
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _rouletteCardPath, leftRouletteDatas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 25f);
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _rouletteCardPath, middleRouletteDatas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 25f);
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _rouletteCardPath, rightRouletteDatas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 25f);
	    
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).Roll();
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).Roll();
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).Roll();
    }
    void SetMainGreenStar()
    {
	    Get<GoodsPrice>(GoodsPriceE.Main_GoodsPrice).UISet(goodsSprite);
	    Get<GoodsPrice>(GoodsPriceE.Main_GoodsPrice).SetCount(_currentGreenStar, false);
    }
    void SetSlotGreenStar()
    {
	    leftSlotPrice.SetCount(_leftSlotGreenStar, false);
	    middleSlotPrice.SetCount(_middleSlotGreenStar, false);
	    rightSlotPrice.SetCount(_rightSlotGreenStar, false);
    }
    void Push(IdentifiedObject skillOrBuff)
    {
	    _resultSlots.Add(skillOrBuff);
    }

    bool UseGreenStar(int value)
    {
	    if (_currentGreenStar < value)
	    {
		    return false;
	    }
	    
	    _currentGreenStar -= value;
	    SetMainGreenStar();
	    
	    return true;
    }
    bool CheckPercent(float percent)
    {
	    return UnityHelper.IsApplyPercent(percent);
    }
    void OnClickMachineLeft()
    {
	    if (!UseGreenStar(_leftSlotGreenStar))
		    return;
	    
	    if (!CheckPercent(_leftSlotPercent))
	    {
		    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).Fail();
		    return;
	    }
	    
	    int select = (int)UnityHelper.Random_H(0, _leftSlots.Count);
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).Play(0.3f, select);
	    
		IdentifiedObject selectSkillOrBuff = _leftSlots[select];
		Push(selectSkillOrBuff);
		
    }
    void OnClickMachineMiddle()
    {
	    if (!UseGreenStar(_middleSlotGreenStar))
		    return;
	    
	    if (!CheckPercent(_middleSlotPercent))
	    {
		    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).Fail();
		    return;
	    }
	    
	    int select = (int)UnityHelper.Random_H(0, _middleSlots.Count);
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).Play(0.3f, select);
	    
	    IdentifiedObject selectSkillOrBuff = _middleSlots[select];
	    Push(selectSkillOrBuff);
    }
    void OnClickMachineRight()
    {
	    if (!UseGreenStar(_rightSlotGreenStar))
		    return;
	    
	    if (!CheckPercent(_rightSlotPercent))
	    {
		    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).Fail();
		    return;
	    }
	    
	    int select = (int)UnityHelper.Random_H(0, _rightSlots.Count);
	    Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).Play(0.3f, select);
	    
	    IdentifiedObject selectSkillOrBuff = _rightSlots[select];
	    Push(selectSkillOrBuff);
    }

    void OnEndMachineLeft()
    {
	    leftSlotParticle.Play();
    }
    void OnEndMachineMiddle()
    {
	    middleSlotParticle.Play();
    }
    void OnEndMachineRight()
    {
	    rightSlotParticle.Play();
    }
    void OnClickOK()
    {
	    if (Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Left_Machine_Main_ScrollView).IsPlay)
		    return;
	    if (Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Middle_Machine_Main_ScrollView).IsPlay)
		    return;
	    if (Get<RouletteScrollView>(RouletteScrollViewE.Main_SlotMachine_Right_Machine_Main_ScrollView).IsPlay)
		    return;

	    for (int i = 0; i < _resultSlots.Count; i++)
	    {
		    Managers.Observer.Player.StartSkillOrBuff(Managers.Observer.Player, _resultSlots[i]);
	    }
		
	    ClosePopupUIPlayAni();
    }
    void OnClickLearn()
    {
	    Managers.UI.ShopPopupUI<UIInGameLearn>("InGame/UIInGameLearn", CanvasOrderType.Middle);
    }
	public enum UIImageE
    {
		BlackPannel,
		Main_Title,
		Main_SlotMachine_Left_Percent,
		Main_SlotMachine_Left_Machine_Main_Bg,
		Main_SlotMachine_Left_Machine_Main_Frame,
		Main_SlotMachine_Middle_Percent,
		Main_SlotMachine_Middle_Machine_Main_Bg,
		Main_SlotMachine_Middle_Machine_Main_Frame,
		Main_SlotMachine_Right_Percent,
		Main_SlotMachine_Right_Machine_Main_Bg,
		Main_SlotMachine_Right_Machine_Main_Frame,
    }
	public enum UITextE
    {
		Main_Title_Text,
		Main_SlotMachine_Left_Percent_Text,
		Main_SlotMachine_Middle_Percent_Text,
		Main_SlotMachine_Right_Percent_Text,
    }
	public enum GoodsPriceE
    {
		Main_GoodsPrice,
    }
	public enum UIButtonE
    {
		Main_LearnSkillBtn,
		Main_SlotMachine_Left_PriceBtn2,
		Main_SlotMachine_Middle_PriceBtn2,
		Main_SlotMachine_Right_PriceBtn2,
		Main_OkBtn,
    }
	public enum RouletteScrollViewE
    {
		Main_SlotMachine_Left_Machine_Main_ScrollView,
		Main_SlotMachine_Middle_Machine_Main_ScrollView,
		Main_SlotMachine_Right_Machine_Main_ScrollView,
    }
}