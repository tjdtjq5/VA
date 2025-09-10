using System.Collections;
using Spine.Unity;
using UnityEngine;

public class UIInGameRest : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<InGameBuffCard>(typeof(InGameBuffCardE));



		_spineAniController = skeletonGraphic.Initialize();
		_spineAniController.SetEndFunc(_playAniName, EndRest);
		
		GetButton(UIButtonE.Main_OkBtn).AddClickEvent((ped) => OnClickRest());

        base.Initialize();
    }
    
    [SerializeField] SkeletonGraphic skeletonGraphic;
    [SerializeField] private GameObject characterSpineObject;
    [SerializeField] private InGameBuffCard buffCard;
    [SerializeField] private Buff restBuff;
    
    private SpineAniController _spineAniController;

    private readonly string _noneAniName = "0";
    private readonly string _playAniName = "1";

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);
	    
	    _spineAniController.Play(_noneAniName, true);
      characterSpineObject.SetActive(false);
	    
	    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(true);
	    buffCard.UISet(restBuff);
    }

    void OnClickRest()
    {
	    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(false);
      characterSpineObject.SetActive(true);
	    _spineAniController.Play(_playAniName, false);
    }

    void EndRest()
    {
	    Managers.Observer.Player.CharacterBuff.PushBuff(Managers.Observer.Player, restBuff);
	    ClosePopupUIPlayAni();
    }

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
	public enum UIButtonE
    {
		Main_OkBtn,
    }
	public enum InGameBuffCardE
    {
		Main_InGameBuffCard,
    }
}