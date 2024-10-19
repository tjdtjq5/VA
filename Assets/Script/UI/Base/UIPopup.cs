using System;

public class UIPopup : UIFrame
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public virtual void OpenUISet(CanvasOrderType orderType)
    {
        Managers.UI.SetPopupCanvas(gameObject, orderType);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
