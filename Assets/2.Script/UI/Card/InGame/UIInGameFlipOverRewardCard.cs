using System;
using UnityEngine;

public class UIInGameFlipOverRewardCard : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIText>(typeof(UITextE));
		Bind<UIImage>(typeof(UIImageE));


		_aniController = animator.Initialize();

        base.Initialize();
    }


    [SerializeField] private Animator animator;
    [SerializeField] Sprite basicSprite;
    [SerializeField] Sprite rareSprite;
    [SerializeField] Sprite legendSprite;
	[SerializeField] Sprite checkSprite_On;
	[SerializeField] Sprite checkSprite_Off;
	[SerializeField] GameObject lightObj;

	private readonly string _playAniName = "Play";
	private readonly string _noneAniName = "None";

	private AniController _aniController;

    public void UISet(FlipOverCardType type)
    {
	    Sprite sprite = basicSprite;
	    
	    switch (type)
	    {
		    case FlipOverCardType.Basic:
			    sprite = basicSprite;
			    GetText(UITextE.Text).text = $"작은 보상";
			    break;
		    case FlipOverCardType.Rare:
			    sprite = rareSprite; 
			    GetText(UITextE.Text).text = $"큰 보상";
			    break;
		    case FlipOverCardType.Legendary:
			    sprite = legendSprite; 
			    GetText(UITextE.Text).text = $"매우 큰 보상";
			    break;
		    default:
			    throw new ArgumentOutOfRangeException(nameof(type), type, null);
	    }
	    
	    GetImage(UIImageE.Image).sprite = sprite;
	    
	    GetImage(UIImageE.Image).SetNativeSize();
	    
	    lightObj.gameObject.SetActive(false);

		_aniController.SetTrigger(_noneAniName);

		CheckSet(0);
    }

	public void CheckSet(int count)
	{
		GetImage(UIImageE.Check_0).sprite = count >= 1 ? checkSprite_On : checkSprite_Off;
		GetImage(UIImageE.Check_1).sprite = count >= 2 ? checkSprite_On : checkSprite_Off;
		GetImage(UIImageE.Check_2).sprite = count >= 3 ? checkSprite_On : checkSprite_Off;
	}

    public void Light()
    {
	    lightObj.gameObject.SetActive(true);
		_aniController.SetTrigger(_playAniName);
    }
	public enum UITextE
    {
		Text,
    }
	public enum UIImageE
    {
		Image,
		Check_0,
		Check_1,
		Check_2,
    }
}