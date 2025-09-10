using UnityEngine.EventSystems;

public class UIOption : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIButton>(typeof(UIButtonE));
		
		GetButton(UIButtonE.SafeArea_Option).AddClickEvent(OnClickLearn);

        base.Initialize();
    }
    
    void OnClickLearn(PointerEventData ped)
    {
	    Managers.UI.ShopPopupUI<UIInGameLearn>("InGame/UIInGameLearn", CanvasOrderType.Middle);
    }

	public enum UIButtonE
    {
		SafeArea_Option,
    }
}