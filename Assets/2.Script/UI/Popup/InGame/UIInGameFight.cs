using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Shared.CSharp;
using Shared.Enums;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class UIInGameFight : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));

	    _spineAniController = skeletonGraphic.Initialize();
	       
	    GetButton(UIButtonE.Main_SelectLeft).AddClickEvent((ped) => OnClickWizard());
	    GetButton(UIButtonE.Main_SelectRight).AddClickEvent((ped) => OnClickWarrior());
	    GetButton(UIButtonE.Main_OkBtn).AddClickEvent((ped) => OnClickOK());
	    
	    base.Initialize();
    }
    private readonly string _idleAniName = "1";
    private readonly string _fightAniName = "2";
    private readonly string _wizardAniName = "4";
    private readonly string _warriorAniName = "3";
    private readonly string _getPopupName = "InGame/UIInGameGet";
    
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] List<Skill> successSkills = new();

    private bool _isSelectFlag = false;
    private IEnumerator _resultCoroutine;
    private SpineAniController _spineAniController;
    private Skill _rewardSkill;
    private FightType _selectType;
    private FightType _resultType;

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    _isSelectFlag = true;
	    _spineAniController.Play(_idleAniName, true);

		SkillDeckType playerDeck = Managers.Observer.Player.CharacterSkill.GetMainSkillDeckType();
		List<Skill> deckSkills = successSkills.FindAll(s => s.SkillDeckTypes.Contains(playerDeck) && s.Grade >= Grade.S);

		if (deckSkills.Count <= 0)
			deckSkills = successSkills;

	    List<Skill> rSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(deckSkills);

		if (rSkills.Count <= 0)
			rSkills = successSkills;

		_rewardSkill = rSkills[(int)UnityHelper.Random_H(0, rSkills.Count)];
	    
	    SetReward(_rewardSkill);

	    SetButtonActive(ButtonActiveType.Select);
    }

    void SetReward(Skill rewardSkill)
    {
		GetImage(UIImageE.Main_Skill).sprite = rewardSkill == null ? Managers.Atlas.GetSkillGradeBg(Grade.D) : Managers.Atlas.GetSkillGradeBg(rewardSkill.Grade);
	    
	    GetImage(UIImageE.Main_Skill_Icon).sprite = rewardSkill == null ? null : rewardSkill.Icon;
	    GetImage(UIImageE.Main_Skill_Icon).SetNativeSize();
	    GetTextPro(UITextProE.Main_Skill_Descript).text = rewardSkill.Description;
    }

    void SetButtonActive(ButtonActiveType type)
    {
	    switch (type)
	    {
		    case ButtonActiveType.Select:
			    GetButton(UIButtonE.Main_SelectLeft).gameObject.SetActive(true);
			    GetButton(UIButtonE.Main_SelectRight).gameObject.SetActive(true);
			    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(false);
			    break;
		    case ButtonActiveType.Fight:
			    GetButton(UIButtonE.Main_SelectLeft).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_SelectRight).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(false);
			    break;
		    case ButtonActiveType.End:
			    GetButton(UIButtonE.Main_SelectLeft).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_SelectRight).gameObject.SetActive(false);
			    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(true);
			    break;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(type), type, null);
	    }
    }

    enum ButtonActiveType
    {
	    Select,
	    Fight,
	    End,
    }

    FightType GetResultType()
    {
	    int r = (int)UnityHelper.Random_H(0, 1000);
	    if (r % 2 == 0)
	    {
		    return FightType.Warrior;
	    }
	    else
	    {
		    return FightType.Wizard;
	    }
    }

    void OnClickWizard()
    {

	    if (!_isSelectFlag)
		    return;

	    _isSelectFlag = false;
	    _selectType = FightType.Wizard;
	    
	    if (_resultCoroutine != null)
		    StopCoroutine(_resultCoroutine);
	    _resultCoroutine = ResultCoroutine(GetResultType());
	    StartCoroutine(_resultCoroutine);
    }
    void OnClickWarrior()
    {

	    if (!_isSelectFlag)
		    return;

	    _isSelectFlag = false;
	    _selectType = FightType.Warrior;
	    
	    if (_resultCoroutine != null)
		    StopCoroutine(_resultCoroutine);
	    _resultCoroutine = ResultCoroutine(GetResultType());
	    StartCoroutine(_resultCoroutine);
    }

    IEnumerator ResultCoroutine(FightType resultType)
    {
	    SetButtonActive(ButtonActiveType.Fight);
	    _spineAniController.Play(_fightAniName, true);

	    yield return new WaitForSeconds(1.5f);

	    _resultType	= resultType;
	    if (resultType == FightType.Wizard)
		    _spineAniController.Play(_wizardAniName, true);
	    else
		    _spineAniController.Play(_warriorAniName, true);
	    
	    SetButtonActive(ButtonActiveType.End);
    }

    void OnClickOK()
    {
	    bool isSuccess = _resultType == _selectType;
	    if (isSuccess && _rewardSkill != null)
	    {
		    Managers.Observer.Player.StartSkillOrBuff(Managers.Observer.Player, _rewardSkill);

		    UIInGameGet uiInGameGet = Managers.UI.ShopPopupUI<UIInGameGet>(_getPopupName, CanvasOrderType.Middle);
		    uiInGameGet.UISetSkillOrBuff(_rewardSkill);

		    uiInGameGet.OnClose -= ClosePopupUIPlayAni;
		    uiInGameGet.OnClose += ClosePopupUIPlayAni;
	    }
	    else
	    {
		    ClosePopupUIPlayAni();
	    }
    }
    
#if UNITY_EDITOR
    [Button]
    public void LoadSkills()
    {
		this.successSkills.Clear();
		int deckTypeLength = CSharpHelper.GetEnumLength<SkillDeckType>();
		for (int i = 0; i < deckTypeLength; i++)
		{
			SkillDeckType deckType = (SkillDeckType)i;
			this.successSkills.AddRange(Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + $"/{deckType.ToString()}").ToList());
		}
        
	    UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
	    UnityEditor.EditorUtility.SetDirty(pSelectObj);
	    UnityEditor.AssetDatabase.Refresh();
    }
#endif

    enum FightType
    {
	    Wizard,
	    Warrior
    }
	public enum UIImageE
    {
		BlackPannel,
		Main_Title,
		Main_Skill,
		Main_Skill_Icon,
    }
	public enum UITextE
    {
		Main_Title_Text,
		Main_Ex,
    }
	public enum UITextProE
    {
		Main_Skill_Descript,
    }
	public enum UIButtonE
    {
		Main_SelectLeft,
		Main_SelectRight,
		Main_OkBtn,
    }
}