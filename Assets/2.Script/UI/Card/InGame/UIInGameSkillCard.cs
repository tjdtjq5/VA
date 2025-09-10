using System;
using Shared.Enums;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameSkillCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));

		_selectAniController = selectAnimator.Initialize();
		
		GetButton(UIButtonE.Main_Button).AddClickEvent((ped) => OnClickButton());
		GetButton(UIButtonE.Main_WordTipButton).AddClickEvent((ped) => OnClickWordTip());
        base.Initialize();
    }

	public UIButton GetWordTipButton() => GetButton(UIButtonE.Main_WordTipButton);
	public Transform GetRecomTr => recomTr;
    
    private UIInGameSkillCardData _data;
    
    [SerializeField] private RectTransform titleRectTransform;
    [SerializeField] private HorizontalLayoutGroup titleLayoutGroup;
    [SerializeField] private ContentSizeFitter titleContentSizeFitter;
    [SerializeField] private Animator selectAnimator;
    [SerializeField] private Sprite underBgSprite;
    [SerializeField] private Sprite underBgSpecialSprite;
	[SerializeField] private Transform recomTr;
    
    private AniController _selectAniController;
    
	private readonly string _wordTipPath = "InGame/UIWordTip";
    private readonly int _noneHash = Animator.StringToHash("None");
    private readonly int _playHash = Animator.StringToHash("Play");
	private readonly Vector2 _effectSizeDelta = new Vector2(833, 240);
    
    public override void Setting(ICardData data)
    {
	    _data = (UIInGameSkillCardData)data;
	    
	    _data.OnSelect -= OnSelect;
	    _data.OnSelect += OnSelect;

	    Setting(_data.Skill, _data.EffectParent);

		GetButton(UIButtonE.Main_WordTipButton).gameObject.SetActive(_data.Skill.WordTips.Count > 0);
    }

    public void Setting(Skill skill, Transform effectParent)
    {
	    SetGrade(skill.Grade);
	    SetPlus(skill.IsPlus);
	    
	    Set(skill);

	    SetEffect(skill.Grade, effectParent);
    }

    public void Setting(Buff buff, Transform effectParent)
    {
	    SetGrade(buff.buffGrade);
	    SetPlus(false);

	    Set(buff);

	    SetEffect(buff.buffGrade, effectParent);
    }

    void Set(IdentifiedObject skillOrBuff)
    {
	    GetText(UITextE.Main_Title_Text).text = skillOrBuff.DisplayName;
	    titleLayoutGroup.SetLayoutHorizontal();
	    titleContentSizeFitter.SetLayoutHorizontal();
	    titleRectTransform.ForceRebuildLayoutImmediate();
	    
	    GetTextPro(UITextProE.Main_Explain).text = skillOrBuff.Description;
	    
	    GetImage(UIImageE.Main_IconBg_Icon).sprite = skillOrBuff.Icon;
	    GetImage(UIImageE.Main_IconBg_Icon).SetNativeSize();
    }

    void OnClickButton()
    {
	    if (_data != null && _data.OnSelectedFunc != null)
		    _data.OnSelectedFunc(_data.Index, _data.Skill);
    }

    void OnSelect(int index, bool flag)
    {
	    if (_data.Index == index)
	    {
		    _selectAniController.SetTrigger(flag ? _playHash : _noneHash);
	    }
    }

    void SetGrade(Grade grade)
    {
	    GetImage(UIImageE.Main_IconBg).sprite = Managers.Atlas.GetSkillGradeBg(grade);

	    switch (grade)
	    {
		    case Grade.D:
			    GetImage(UIImageE.Main_UnderBg).SetColor("959595");
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSprite;
			    break;
		    case Grade.C:
			    GetImage(UIImageE.Main_UnderBg).SetColor("64C578");
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSprite;
			    break;
		    case Grade.B:
			    GetImage(UIImageE.Main_UnderBg).SetColor("62B4D6");
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSprite;
			    break;
		    case Grade.A:
			    GetImage(UIImageE.Main_UnderBg).SetColor("D487DB");
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSprite;
			    break;
		    case Grade.S:
			    GetImage(UIImageE.Main_UnderBg).SetColor("DDA363");
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSprite;
			    break;
		    case Grade.SS:
			    GetImage(UIImageE.Main_UnderBg).SetColor("D66062");
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSprite;
			    break;
		    case Grade.SSS:
			    GetImage(UIImageE.Main_UnderBg).SetColor(Color.white);
			    GetImage(UIImageE.Main_UnderBg).sprite = underBgSpecialSprite;
			    break;
	    }
    }

    void SetPlus(bool isPlus)
    {
	    GetImage(UIImageE.Main_IconBg_Plus).gameObject.SetActive(isPlus);
	    GetImage(UIImageE.Main_UnderBg_Star).gameObject.SetActive(isPlus);
    }

    void SetEffect(Grade grade, Transform effectParent)
    {
		SkillCardEffect cardEffect = Managers.Resources.Instantiate<SkillCardEffect>("Prefab/Effect/UI/SkillCardEffect", effectParent);
		cardEffect.transform.position = this.transform.position;
		cardEffect.UISet(grade, _effectSizeDelta);
    }

	void OnClickWordTip()
	{
		if (_data.Skill == null)
			return;
			
		if (_data.Skill.WordTips.Count == 0)
			return;
			
		Managers.UI.ShopPopupUI<UIWordTip>(_wordTipPath, CanvasOrderType.Middle).UISet(_data.Skill.WordTips);
	}

	public enum UIImageE
    {
		Main_UpperBg,
		Main_UnderBg,
		Main_UnderBg_Star,
		Main_IconBg,
		Main_IconBg_Icon,
		Main_IconBg_Plus,
		Main_Select,
    }
	public enum UITextE
    {
		Main_Title_Text,
    }
	public enum UITextProE
    {
		Main_Explain,
    }
	public enum UIButtonE
    {
		Main_Button,
		Main_WordTipButton,
    }
}
public class UIInGameSkillCardData : ICardData
{
	public int Index;
	public Func<int, Skill, bool> OnSelectedFunc; 
	public Skill Skill;
	public Transform EffectParent;

	public Action<int, bool> OnSelect;
	public void SetSelect(int index, bool flag)
	{	
		OnSelect?.Invoke(index, flag);
	}
}