public class UIPopup : UIFrame
{
    protected virtual bool IsSort => true;

    protected override void Initialize()
    {
        base.Initialize();
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
