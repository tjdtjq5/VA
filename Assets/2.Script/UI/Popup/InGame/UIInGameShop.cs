using System;
using System.Collections.Generic;
using System.Linq;
using Shared.CSharp;
using Shared.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIInGameShop : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<UIButton>(typeof(UIButtonE));

        base.Initialize();
    }
    
	public Action OnPurchase;
	
    [SerializeField] GoodsPrice resetGoodsPrice;
    
    [SerializeField] private List<Skill> productSkills = new List<Skill>();

    private readonly int _resetGessoCount = 50;
	private List<Poolable> _recomPoolables = new();

	string recomPrefabPath = "Prefab/UI/Etc/InGame/SkillRecom";
    private readonly string _cardPath = "InGame/InGameShopCard";
    private readonly int _cardMax = 4;
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

    protected override void UISet()
    {
	    base.UISet();
	    
	    GetButton(UIButtonE.Main_Bg_ResetBtn).AddClickEvent((ped) => OnClickReset());
	    GetButton(UIButtonE.Main_Bg_LearnSkillBtn).AddClickEvent((ped) => OnClickLearn());
		    
	    GetButton(UIButtonE.Main_Bg_OkBtn).AddClickEvent((ped) =>
	    {
		    ClosePopupUIPlayAni();
	    });
	    
	    resetGoodsPrice.UISet("Gesso");
	    resetGoodsPrice.SetCount(_resetGessoCount, false);
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);
	    
	    SetCard();
	    SetResetButton();

        Managers.Observer.UIGoodsController.CanvasOrderSwitch(true);
    }

	public override void ClosePopupUI()
	{


		base.ClosePopupUI();
		Managers.Observer.UIGoodsController.CanvasOrderSwitch(false);
	}

    void SetCard()
    {
	    List<ICardData> datas = new();
	    List<Skill> products = GetProducts();
	    for (int i = 0; i < products.Count; i++)
	    {
		    Skill skill = products[i];
		    datas.Add(new InGameShopCardData() { Skill = skill, UIInGameShop = this });
	    }
	    
	    GetScrollView(UIScrollViewE.Main_Bg_InnerBg_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPath, datas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 35f);

		#region Recom
		for(int i = 0; i < _recomPoolables.Count; i++)
			_recomPoolables[i].Destroy();
		_recomPoolables.Clear();

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
				recomObj.transform.position = ((InGameShopCard)GetScrollView(UIScrollViewE.Main_Bg_InnerBg_ScrollView).GetCard(recomIndex)).GetRecomTr.position;
			}
		}
		#endregion
    }
    
    void SetResetButton()
    {
	    bool isOK = IsResetOk();
	    GetButton(UIButtonE.Main_Bg_ResetBtn).UISet(isOK ? ButtonSprite.Button_Green : ButtonSprite.Button_Gray);
    }
    void OnClickReset()
    {
	    if(!IsResetOk())
	    {
		    return;
	    }

	    Managers.Observer.Gesso -= _resetGessoCount;
	    SetCard();
	    
	    SetResetButton();
    }

    void OnClickLearn()
    {
	    Managers.UI.ShopPopupUI<UIInGameLearn>("InGame/UIInGameLearn", CanvasOrderType.Middle);
    }
    
    List<Skill> GetProducts()
    {
		SkillDeckType playerDeck = Managers.Observer.Player.CharacterSkill.GetMainSkillDeckType();
		List<Skill> tempProductSkills = new List<Skill>();
		tempProductSkills.AddRange(productSkills);

		if (playerDeck == SkillDeckType.None)
		{
			tempProductSkills.RemoveAll(skill => skill.SkillDeckTypes.Contains(SkillDeckType.None) || skill.SkillDeckTypes.Count <= 0);
		}

	    List<Skill> checkLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(tempProductSkills);
	    return Managers.Random.RandomDraw(checkLearnSkills, _gradePercents, _cardMax, false);
    }
    bool IsResetOk() => Managers.Observer.Gesso >= _resetGessoCount;
    
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
	public enum UIButtonE
    {
		Main_Bg_OkBtn,
		Main_Bg_ResetBtn,
		Main_Bg_LearnSkillBtn,
    }
}