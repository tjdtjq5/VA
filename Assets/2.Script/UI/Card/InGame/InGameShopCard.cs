using System;
using Shared.BBNumber;
using Shared.Enums;
using UnityEngine;
using UnityEngine.UI;

public class InGameShopCard : UICard
{

	public Transform GetRecomTr => recomTr;
	private InGameShopCardData _data;
	private readonly Vector2 _effectSizeDelta = new Vector2(975, 240);
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIButton>(typeof(UIButtonE));


		GetButton(UIButtonE.Main_GoodsButton).AddClickEvent((ped) => OnClickPurchase());
		GetButton(UIButtonE.Main_WordTipButton).AddClickEvent((ped) => OnClickWordTip());
        base.Initialize();
    }

    [SerializeField] private RectTransform titleRectTransform;
    [SerializeField] private HorizontalLayoutGroup titleLayoutGroup;
    [SerializeField] private ContentSizeFitter titleContentSizeFitter;
    [SerializeField] private GoodsPrice goodsPrice;
    
    [SerializeField] private Sprite underBgSprite;
    [SerializeField] private Sprite underBgSpecialSprite;
	[SerializeField] private Transform recomTr;
    
	private readonly string _wordTipPath = "InGame/UIWordTip";
    private bool _isPurchased = false;
    
    public override void Setting(ICardData data)
    {
	    _data = (InGameShopCardData)data;
        GetText(UITextE.Main_Title_Text).text = _data.Skill.DisplayName;
        titleLayoutGroup.SetLayoutHorizontal();
        titleContentSizeFitter.SetLayoutHorizontal();
        titleRectTransform.ForceRebuildLayoutImmediate();

        GetTextPro(UITextProE.Main_Explain).text = _data.Skill.Description;
	    
        GetImage(UIImageE.Main_IconBg_Icon).sprite = _data.Skill.Icon;
        GetImage(UIImageE.Main_IconBg_Icon).SetNativeSize();
        
        SetGrade(_data.Skill.Grade);
        SetEffect(_data.Skill.Grade);
        SetPlus(_data.Skill.IsPlus);
        
        goodsPrice.UISet("Gesso");
        goodsPrice.SetCount(_data.Skill.GessoPrice, false);
        
        _isPurchased = false;

        SetButton();

		_data.UIInGameShop.OnPurchase -= SetButton;
		_data.UIInGameShop.OnPurchase += SetButton;

		GetButton(UIButtonE.Main_WordTipButton).gameObject.SetActive(_data.Skill != null && _data.Skill.WordTips.Count > 0);
    }

    void OnClickPurchase()
    {
	    if (!IsOKPurchased())
		    return;
	    
	    _isPurchased = true;

	    Managers.Observer.Gesso -= _data.Skill.GessoPrice;
	    
	    Managers.Observer.Player.CharacterSkill.PushSkill(_data.Skill);

		_data.UIInGameShop.OnPurchase?.Invoke();
    }

    bool IsOKPurchased()
    {
	    BBNumber userGesso = Managers.Observer.Gesso;
	    return !_isPurchased && userGesso >= _data.Skill.GessoPrice;
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

    void SetButton()
    {
	    GetButton(UIButtonE.Main_GoodsButton).UISet(IsOKPurchased() ? ButtonSprite.Button_Yellow : ButtonSprite.Button_Gray);
    }
    
    void SetEffect(Grade grade)
    {
	    SkillCardEffect cardEffect = Managers.Resources.Instantiate<SkillCardEffect>("Prefab/Effect/UI/SkillCardEffect", this.transform);
	    cardEffect.transform.localPosition = Vector3.zero;
	    cardEffect.UISet(grade, _effectSizeDelta);
    }

	void OnClickWordTip()
	{
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
		Main_GoodsButton,
		Main_WordTipButton,
    }
}
public class InGameShopCardData : ICardData
{
	public UIInGameShop UIInGameShop;
    public Skill Skill;
}