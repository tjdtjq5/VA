using System.Collections.Generic;

public class UIInGameLearn : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<UITabButtonParent>(typeof(UITabButtonParentE));
		Bind<UIButton>(typeof(UIButtonE));
		
		GetButton(UIButtonE.Main_OkBtn).AddClickEvent((ped) => ClosePopupUIPlayAni());

		GetTabButtonParent(UITabButtonParentE.Main_Bg_WoodTab).SwitchOnHandler += OnTabSwitchOn;
		GetTabButtonParent(UITabButtonParentE.Main_Bg_WoodTab).SwitchOffHandler += OnTabSwitchOff;
		
        base.Initialize();
    }

    private readonly string _cardPrefab = "InGame/UIInGameLearnCard";

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);
	    
	    GetTabButtonParent(UITabButtonParentE.Main_Bg_WoodTab).UISet(0);
    }

    void OnTabSwitchOn(int index)
    {
	    switch (index)
	    {
		    case 0:
			    SetSkill();
			    break;
		    case 1:
			    SetBuff();
			    break;
	    }
    }

    void OnTabSwitchOff(int index)
    {
	    
    }

    void SetSkill()
    {
	    List<Skill> skills = Managers.Observer.Player.CharacterSkill.Skills;
	    List<ICardData> cardDatas = new List<ICardData>();
	    for (int i = 0; i < skills.Count; i++)
	    {
		    cardDatas.Add(new UIInGameLearnCardData() { SkillOrBuff = skills[i] });
	    }
	    GetScrollView(UIScrollViewE.Main_Bg_InnerBg_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPrefab, cardDatas, 0, 4, UIScrollViewLayoutStartCorner.Middle, 20, 20);
    }

    void SetBuff()
    {
	    List<Buff> buffs = Managers.Observer.Player.CharacterBuff.Buffs;
	    List<ICardData> cardDatas = new List<ICardData>();
	    for (int i = 0; i < buffs.Count; i++)
	    {
		    cardDatas.Add(new UIInGameLearnCardData() { SkillOrBuff = buffs[i] });
	    }
	    
	    GetScrollView(UIScrollViewE.Main_Bg_InnerBg_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPrefab, cardDatas, 0, 4);
    }
    
	public enum UIImageE
    {
		BlackPannel,
		Main_Title,
		Main_Bg,
		Main_Bg_InnerBg,
    }
	public enum UITextE
    {
		Main_Title_Text,
    }
	public enum UIScrollViewE
    {
		Main_Bg_InnerBg_ScrollView,
    }
	public enum UITabButtonParentE
    {
		Main_Bg_WoodTab,
    }
	public enum UIButtonE
    {
		Main_OkBtn,
    }
}