public class UIPopup : UIBase
{
    public override void Initialize()
    {
        base.Initialize();

        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
