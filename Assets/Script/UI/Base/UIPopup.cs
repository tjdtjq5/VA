public class UIPopup : UIFrame
{
    protected virtual bool IsSort => true;

    protected override void Initialize()
    {
        base.Initialize();

        Managers.UI.SetPopupCanvas(gameObject, IsSort);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
