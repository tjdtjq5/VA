using System.Collections.Generic;
using System.Linq;
using Shared.CSharp;
using Shared.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInGameDevil : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<InGameBuffCard>(typeof(InGameBuffCardE));
		Bind<UIInGameSkillCard>(typeof(UIInGameSkillCardE));
		Bind<UIButton>(typeof(UIButtonE));

		
		GetButton(UIButtonE.Main_SelectCancle).AddClickAniEvent(OnClickCancle);
		GetButton(UIButtonE.Main_SelectOk).AddClickAniEvent(OnClickOK);

        base.Initialize();
    }

    [SerializeField] private Buff deBuff;
    [SerializeField] private List<Skill> skills;
	[SerializeField] private Transform mainTransform;

    private readonly string _getPopupName = "InGame/UIInGameGet";

    private Skill _randomSkill;
    private bool _isBtnFlag = false;

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

		SkillDeckType playerDeck = Managers.Observer.Player.CharacterSkill.GetMainSkillDeckType();
		List<Skill> deckSkills = skills.FindAll(s => s.SkillDeckTypes.Contains(playerDeck) && s.Grade >= Grade.S);

		if (deckSkills.Count <= 0)
			deckSkills = skills;

	    List<Skill> checkLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(deckSkills);

		if (checkLearnSkills.Count <= 0)
			checkLearnSkills = skills;

	    _randomSkill = checkLearnSkills[(int)UnityHelper.Random_H(0, checkLearnSkills.Count)];
	    _isBtnFlag = false;
	    
	    Get<InGameBuffCard>(InGameBuffCardE.Main_Middle_InGameBuffCard).UISet(deBuff);
		UIInGameSkillCard skillCard = Get<UIInGameSkillCard>(UIInGameSkillCardE.Main_Middle_UIInGameSkillCard);
	    skillCard.Setting(_randomSkill, mainTransform);
    }

    void OnClickCancle(PointerEventData ped)
    {
	    if (_isBtnFlag)
		    return;
	    _isBtnFlag = true;
	    
	    ClosePopupUIPlayAni();
    }
    void OnClickOK(PointerEventData ped)
    {
	    if (_isBtnFlag)
		    return;
	    _isBtnFlag = true;

	    Managers.Observer.Player.CharacterSkill.PushSkill(_randomSkill);
	    Managers.Observer.Player.CharacterBuff.PushBuff(Managers.Observer.Player, deBuff);
	    
	    UIInGameGet uiInGameGet = Managers.UI.ShopPopupUI<UIInGameGet>(_getPopupName, CanvasOrderType.Middle);
	    uiInGameGet.UISetSkillOrBuff(_randomSkill);

	    uiInGameGet.OnClose -= ClosePopupUI;
	    uiInGameGet.OnClose += ClosePopupUI;
    }
    
    
#if UNITY_EDITOR
    [Button]
    public void LoadSkills()
    {
		this.skills.Clear();
		int deckTypeLength = CSharpHelper.GetEnumLength<SkillDeckType>();
		for (int i = 0; i < deckTypeLength; i++)
		{
			SkillDeckType deckType = (SkillDeckType)i;
			this.skills.AddRange(Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + $"/{deckType.ToString()}").ToList());
		}
        
	    UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
	    UnityEditor.EditorUtility.SetDirty(pSelectObj);
	    UnityEditor.AssetDatabase.Refresh();
    }
#endif

	public enum UIImageE
    {
		BlackPannel,
		Main_Title,
    }
	public enum UITextE
    {
		Main_Title_Text,
		Main_Ex,
    }
	public enum InGameBuffCardE
    {
		Main_Middle_InGameBuffCard,
    }
	public enum UIInGameSkillCardE
    {
		Main_Middle_UIInGameSkillCard,
    }
	public enum UIButtonE
    {
		Main_SelectCancle,
		Main_SelectOk,
    }
}