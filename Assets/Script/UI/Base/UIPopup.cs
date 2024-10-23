using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UIPopup : UIFrame
{
    int openHash = UnityEngine.Animator.StringToHash("Open");
    int closeHash = UnityEngine.Animator.StringToHash("Close");
    const string closeStr = "Close";

    protected Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    protected AniController AniController;

    protected override void Initialize()
    {
        base.Initialize();

        if (Animator)
        {
            AniController = Animator.Initialize();
            AniController.SetEndFunc(closeStr, CloseAniEndFunc);
        }
    }

    public virtual void OpenUISet(CanvasOrderType orderType)
    {
        Managers.UI.SetPopupCanvas(gameObject, orderType);
        AniController.SetTrigger(openHash);
        UnityHelper.Log_H($"Open [{this.gameObject.name}]");
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
    public void ClosePopupUIPlayAni()
    {
        AniController.SetTrigger(closeHash);
    }
    void CloseAniEndFunc(string clipName)
    {
        ClosePopupUI();
    }
}
