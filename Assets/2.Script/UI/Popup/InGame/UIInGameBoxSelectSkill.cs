using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIInGameBoxSelectSkill : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIScrollView>(typeof(UIScrollViewE));
		Bind<UIButton>(typeof(UIButtonE));

		InitializePage();
		GetButton(UIButtonE.Main_Bg_OkBtn).AddClickEvent((ped) => OnClickOK());
		GetButton(UIButtonE.Main_Bg_LearnSkillBtn).AddClickEvent((ped) => OnClickLearn());

        base.Initialize();
    }
    
    public Action<int, bool> OnSelect;
    
    [SerializeField] Transform pageContainer;
    [SerializeField] private List<Skill> productSkills = new List<Skill>();

    private int _currentIndex = 0;
    private List<PageCard> _pageCards = new();
    private List<Skill> _selectedSkills = new();

    private readonly int _selectCount = 1;
    private readonly int _pageCount = 1;
    private readonly string _pageCardPrefab = "Prefab/UI/Card/InGame/PageCard";
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
    
    void InitializePage()
    {
	    for (int i = 0; i < _pageCount; i++)
	    {
		    PageCard pageCard = Managers.Resources.Instantiate<PageCard>(_pageCardPrefab);
		    pageCard.Initialize(i);
		    _pageCards.Add(pageCard);
	    }
    }

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    SetPage(0);
    }

    void SetPage(int index)
    {
	    this._currentIndex = index;
	    PageClear();
	    
	    for (int i = 0; i < _pageCards.Count; i++)
	    {
		    _pageCards[i].UISet(index == _pageCards[i].Index);
	    }

	    SetCard();
    }

    void SetCard()
    {
	    List<ICardData> datas = new();
	    List<Skill> products = GetProducts();
	    for (int i = 0; i < products.Count; i++)
	    {
		    Skill skill = products[i];
		    int index = i;
		    UIInGameSkillCardData cardData = new UIInGameSkillCardData()
			    { Index = index, OnSelectedFunc = this.OnSelectedSkill, Skill = skill };
		    this.OnSelect += cardData.SetSelect;
		    datas.Add(cardData);
	    }
	    
	    GetScrollView(UIScrollViewE.Main_Bg_InnerBg_ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _cardPath, datas, 0, 1, UIScrollViewLayoutStartCorner.Middle, 0, 35f);
    }

    void SetCount()
    {
	    GetText(UITextE.Main_Bg_Count_Text).text = $"{_selectedSkills.Count} / {_selectCount}";
    }

    void PageClear()
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
		
	    // push skill
		PushSelectedSkill();
	    
	    _currentIndex++;
	    if (_currentIndex >= _pageCount)
	    {
		    // close
		    ClosePopupUIPlayAni();
	    }
	    else
	    {
		    // next page
		    SetPage(_currentIndex);
	    }
    }
    
    void OnClickLearn()
    {
	    Managers.UI.ShopPopupUI<UIInGameLearn>("InGame/UIInGameLearn", CanvasOrderType.Middle);
    }
    
    List<Skill> GetProducts()
    {
	    List<Skill> checkLearnSkills = Managers.Observer.Player.CharacterSkill.GetCheckLearnSkills(productSkills);
	    return Managers.Random.RandomDraw(checkLearnSkills, _gradePercents, _cardMax, false);
    }
    
    #if UNITY_EDITOR
    [Button]
    public void LoadSkills()
    {
	    this.productSkills = Resources.LoadAll<Skill>(DefinePath.SkillSOResourcesPath()).ToList();
        
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
		Main_Bg_Count,
    }
	public enum UITextE
    {
		Main_Title_Text,
		Main_Bg_Count_Text,
    }
	public enum UIScrollViewE
    {
		Main_Bg_InnerBg_ScrollView,
    }
	public enum UIButtonE
    {
		Main_Bg_OkBtn,
		Main_Bg_LearnSkillBtn,
    }
}