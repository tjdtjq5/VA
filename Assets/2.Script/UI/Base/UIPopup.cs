using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UIPopup : UIFrame
{
    public Action OnClose;

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

        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1080, 1920);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 0f;
        }
    }

    public virtual void OpenUISet(CanvasOrderType orderType)
    {
        Managers.UI.SetPopupCanvas(gameObject, orderType);
        AniController.SetTrigger(openHash);
    }

    public virtual void ClosePopupUI()
    {
        OnClose?.Invoke();
        OnClose = null;
        Managers.UI.ClosePopupUI(this);
    }
    public void ClosePopupUIPlayAni() => AniController.SetTrigger(closeHash);
    void CloseAniEndFunc(string clipName) => ClosePopupUI();
}
