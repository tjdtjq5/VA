using System;
using System.Collections.Generic;
using System.Linq;
using Shared.CSharp;
using Shared.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIInGameSkill : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<UIButton>(typeof(UIButtonE));
		
		GetButton(UIButtonE.OkBtn).AddClickEvent((ped) => OnClickOK());
		GetButton(UIButtonE.LearnSkillBtn).AddClickEvent((ped) => OnClickLearn());

        base.Initialize();
    }
    
    public Action<int, bool> OnSelect;
    
    [SerializeField] private List<Skill> productSkills = new List<Skill>();
    
    private List<Skill> _selectedSkills = new();
    private List<UIInGameSkillCardData> _cardData = new();
	private List<Poolable> _recomPoolables = new();

	string recomPrefabPath = "Prefab/UI/Etc/InGame/SkillRecom";
    
    private readonly int _selectCount = 1;
    private readonly string _cardPath = "InGame/UIInGameSkillCard";
    private readonly int _cardMax = 3;
    private readonly Dictionary<Grade, float> _gradePercents = new Dictionary<Grade, float>()
    {
	    { Grade.D, 100f },
	    { Grade.C, 20f },
	    { Grade.B, 15f },
	    { Grade.A, 10f },
	    { Grade.S, 7f },
	    { Grade.SS, 5f },
	    { Grade.SSS, 3f },
    };
    private readonly Dictionary<Grade, float> _specialGradePercents = new Dictionary<Grade, float>()
    {
	    { Grade.D, 20f },
	    { Grade.C, 2f },
	    { Grade.B, 2f },
	    { Grade.A, 2f },
	    { Grade.S, 70f },
	    { Grade.SS, 2f },
	    { Grade.SSS, 2f },
    };

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);
    }

	public void UISet(UIInGameSkillType type)
	{
		Clear();
		SetCard(type);
	}

    void SetCard(UIInGameSkillType type)
    {
	    List<ICardData> datas = new();
	    List<Skill> products = GetProducts(type);

	    _cardData.Clear();
		int worldTipIndex = -1;
	    for (int i = 0; i < products.Count; i++)
	    {
		    Skill skill = products[i];
		    int index = i;
		    UIInGameSkillCardData cardData = new UIInGameSkillCardData()
			    { Index = index, OnSelectedFunc = this.OnSelectedSkill, Skill = skill, EffectParent = this.transform };
		    this.OnSelect += cardData.SetSelect;
		    datas.Add(cardData);
		    _cardData.Add(cardData);

			if(skill.WordTips.Count > 0)
				worldTipIndex = index;
	    }
	    
	    GetScrollView(UIScrollViewE.ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPath, datas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 35f);

		// #region Tutorial
		// if (worldTipIndex != -1 &&!PlayerPrefsHelper.GetBool_H(PlayerPrefsKey.tutorial_wordTip))
        // {
		// 	string tutorialPrefabPath = "Prefab/UI/Etc/Tutorial/SkillHand";
        // 	TutorialHand _wordTipTutorialHand = Managers.Resources.Instantiate<TutorialHand>(tutorialPrefabPath, this.transform);
       	//     _wordTipTutorialHand.Click();

        //     _wordTipTutorialHand.transform.position = ((UIInGameSkillCard)GetScrollView(UIScrollViewE.ScrollView).GetCard(worldTipIndex)).GetWordTipButton().transform.position;

		// 	((UIInGameSkillCard)GetScrollView(UIScrollViewE.ScrollView).GetCard(worldTipIndex)).GetWordTipButton().AddClickEvent((ped) => 
		// 	{
		// 		PlayerPrefsHelper.Set_H(PlayerPrefsKey.tutorial_wordTip, true);
		// 		Managers.Resources.Destroy(_wordTipTutorialHand.gameObject);
		// 	});
        // }
		// #endregion

		#region Recom
		SkillDeckType mainDeck = Managers.Observer.Player.CharacterSkill.GetMainSkillDeckType();

		if(mainDeck != SkillDeckType.None)
		{
			int recomIndex = products.Where(x => x.SkillDeckTypes.Count > 0 && x.SkillDeckTypes[0] == mainDeck)
						.OrderByDescending(x => x.Grade)
						.Select(x => products.IndexOf(x))
						.DefaultIfEmpty(-1)
    					.First();

			if(recomIndex == -1)
			{
				recomIndex = products.Where(x => x.SkillDeckTypes.Contains(mainDeck))
									.OrderByDescending(x => x.Grade) 
									.Select(x => products.IndexOf(x))
									.DefaultIfEmpty(-1)
									.First();
			}

			if(recomIndex != -1)
			{
				Poolable recomObj = Managers.Resources.Instantiate<Poolable>(recomPrefabPath, this.transform);
				_recomPoolables.Add(recomObj);
				recomObj.transform.position = ((UIInGameSkillCard)GetScrollView(UIScrollViewE.ScrollView).GetCard(recomIndex)).GetRecomTr.position;
			}
		}
		#endregion
    }

    void SetCount()
    {
	    GetText(UITextE.Count_Text).text = $"{_selectedSkills.Count} / {_selectCount}";
    }

    void Clear()
    {
	    _selectedSkills.Clear();
	    OnSelect = null;
	    SetCount();
    }

    void PushSelectedSkill()
    {
	    for (int i = 0; i < _selectedSkills.Count; i++)
	    {
		    Managers.Observer.Player.CharacterSkill.PushSkill(_selectedSkills[i]);
	    }
    }

    public bool OnSelectedSkill(int index, Skill skill)
    {
	    if (_selectedSkills.Contains(skill))
	    {
		    _selectedSkills.Remove(skill);
		    SetCount();
		    OnSelect?.Invoke(index, false);
		    return false;
	    }
	    
	    if (_selectedSkills.Count == _selectCount)
	    {
		    int lastIndex = _cardData.Find(c => c.Skill.CodeName.Equals(_selectedSkills[_selectCount - 1].CodeName)).Index;
		    OnSelect?.Invoke(lastIndex, false);
		    _selectedSkills.RemoveAt(_selectCount - 1);
		    SetCount();
	    }
	    
	    if (_selectedSkills.Count < _selectCount)
	    {
		    _selectedSkills.Add(skill);
		    SetCount();
		    OnSelect?.Invoke(index, true);
		    return true;
	    }
	    
	    return false;
    }

    void OnClickOK()
    {
	    if(_selectedSkills.Count < _selectCount)
	    {
		    return;
	    }

		for(int i = 0; i < _recomPoolables.Count; i++)
		{
			_recomPoolables[i].Destroy();
		}
		_recomPoolables.Clear();
		
	    // push skill
		PushSelectedSkill();
	    
		// close
		ClosePopupUIPlayAni();
    }
    
    void OnClickLearn()
    {
	    Managers.UI.ShopPopupUI<UIInGameLearn>("InGame/UIInGameLearn", CanvasOrderType.Middle);
    }
    
    List<Skill> GetProducts(UIInGameSkillType type)
    {
		SkillDeckType playerDeck = Managers.Observer.Player.CharacterSkill.GetMainSkillDeckType();
		List<Skill> tempProductSkills = new List<Skill>();
		tempProductSkills.AddRange(productSkills);

		if (playerDeck == SkillDeckType.None)
		{
			tempProductSkills.RemoveAll(skill => skill.SkillDeckTypes.Contains(SkillDeckType.None) || skill.SkillDeckTypes.Count <= 0);
		}

	    List<Skill> checkLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(tempProductSkills);

		if(type == UIInGameSkillType.Special)
		{
			return Managers.Random.RandomDraw(checkLearnSkills, _specialGradePercents, _cardMax, false);
		}
		else
		{
			return Managers.Random.RandomDraw(checkLearnSkills, _gradePercents, _cardMax, false);
		}
    }


    
    #if UNITY_EDITOR
    [Button]
    public void LoadSkills()
    {
		List<string> removeSkillCodes = new List<string>();
		removeSkillCodes.Add("Cure");
		removeSkillCodes.Add("Evasion");
		removeSkillCodes.Add("Evasion+");
		removeSkillCodes.Add("IncreasedAttackPower");
		removeSkillCodes.Add("IncreasedCriticalDamage");
		removeSkillCodes.Add("IncreasedCriticalDamage+");
		removeSkillCodes.Add("IncreasedCriticalHitChance");
		removeSkillCodes.Add("IncreasedCriticalHitChance+");
		removeSkillCodes.Add("IncreasedDefense");
		removeSkillCodes.Add("IncreasedMaximumHealth");
		removeSkillCodes.Add("Protect");

		removeSkillCodes.Add("IncreasedConcentratedAttackPower");
		removeSkillCodes.Add("IncreasedConcentrationAndMaximumStamina");
		removeSkillCodes.Add("IncreasedFocusDefense");
		

		this.productSkills.Clear();
		int deckTypeLength = CSharpHelper.GetEnumLength<SkillDeckType>();
		for (int i = 0; i < deckTypeLength; i++)
		{
			SkillDeckType deckType = (SkillDeckType)i;
			this.productSkills.AddRange(Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath() + $"/{deckType.ToString()}").ToList());
		}

		productSkills.RemoveAll(skill => removeSkillCodes.Contains(skill.CodeName));
        
	    UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
	    UnityEditor.EditorUtility.SetDirty(pSelectObj);
	    UnityEditor.AssetDatabase.Refresh();
    }
    #endif

	
	public enum UIImageE
    {
		BlackPannel,
		Title,
    }
	public enum UITextE
    {
		Title_Text,
		Count_Text,
    }
	public enum UIScrollViewE
    {
		ScrollView,
    }
	public enum UIButtonE
    {
		LearnSkillBtn,
		OkBtn,
    }
}

public enum UIInGameSkillType
{
	Basic,
	Special,
}