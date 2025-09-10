using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIInGameFlipOverCard : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));


		_aniController = animator.Initialize();
		_aniController.SetEndFunc(_flip, (aniName) => FlipEnd());
		
		GetButton(UIButtonE.Main_Button).AddClickEvent(OnClick);

        base.Initialize();
    }

    public FlipOverCardType Type { get; protected set; }

    public Action<FlipOverCardType> OnFlipStart;
    public Action<FlipOverCardType> OnFlipEnd;
    
    [SerializeField] Animator animator;
    [SerializeField] Sprite basicSprite;
    [SerializeField] Sprite rareSprite;
    [SerializeField] Sprite legendSprite;

    private UIInGameFlipOver _uiInGameFlipOver;
    private AniController _aniController;

    private readonly string _noneBack = "NoneBack";
    private readonly string _noneFront = "NoneFront";
    private readonly string _flip = "Flip";
    private readonly string _light = "Light";

    public bool IsFlipped { get; private set; }

    public void Initialize(UIInGameFlipOver uiInGameFlipOver)
    {
	    this._uiInGameFlipOver = uiInGameFlipOver;
    }
    public void UISet(FlipOverCardType type)
    {
	    this.Type = type;
	    this.IsFlipped = false;
	    
	    _aniController.SetTrigger(_noneFront);

	    switch (type)
	    {
		    case FlipOverCardType.Basic:
			    GetImage(UIImageE.Main_Icon).sprite = basicSprite; 
			    break;
		    case FlipOverCardType.Rare:
			    GetImage(UIImageE.Main_Icon).sprite = rareSprite; 
			    break;
		    case FlipOverCardType.Legendary:
			    GetImage(UIImageE.Main_Icon).sprite = legendSprite; 
			    break;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(type), type, null);
	    }
	    
	    GetImage(UIImageE.Main_Icon).SetNativeSize();
    }

    void OnClick(PointerEventData ped)
    {
	    if (IsFlipped)
		    return;

	    if (_uiInGameFlipOver.IsFlipping)
		    return;
	    
	    if (_uiInGameFlipOver.IsEnded)
		    return;

	    _aniController.SetTrigger(_flip);

	    OnFlipStart?.Invoke(Type);
    }

    void FlipEnd()
    {
	    IsFlipped = true;
	    
	    _aniController.SetTrigger(_noneBack);
	    
	    OnFlipEnd?.Invoke(Type);
    }

    public void Light()
    {
	    _aniController.SetTrigger(_light);
    }
	public enum UIImageE
    {
		Main_Bg2,
		Main_Bg1,
		Main_Icon,
		Light,
    }
	public enum UIButtonE
    {
		Main_Button,
    }
}
public enum FlipOverCardType
{
	Basic,
	Rare,
	Legendary,
}