public class UIPopup : UIFrame
{
    protected virtual bool IsSort => true;
    public CanvasOrderType OrderType { get; set; }

    protected override void Initialize()
    {
        base.Initialize();

        Managers.UI.SetPopupCanvas(gameObject, OrderType);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
