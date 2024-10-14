using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UITabButton : UIFrame
{
    public Action<int> SwitchOnHandler {  get; set; }
    public Action<int> SwitchOffHandler {  get; set; }

    public int Index { get; private set; }
    bool isAllOff;

    Image Image => GetComponent<Image>();
    protected Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    protected AniController AniController;

    bool isSwitch = false;
    int switchHash = Animator.StringToHash("Switch");

    protected override void Initialize()
    {
        base.Initialize();

        Image.raycastTarget = true;

        if (Animator)
            AniController = Animator.Initialize();

        BindEvent(Image.gameObject, OnClickEvent, UIEvent.Click);
    }
    public void Set(int index, bool isAllOff)
    {
        this.Index = index;
        this.isAllOff = isAllOff;
    }
    public void Switch(bool flag)
    {
        if (isSwitch.Equals(flag))
            return;

        isSwitch = flag;

        if (flag)
        {
            AniController.SetBool(switchHash, true);

            if (SwitchOnHandler != null)
                SwitchOnHandler.Invoke(Index);

            UIOnSet();
        }
        else
        {
            AniController.SetBool(switchHash, false);

            if (SwitchOffHandler != null)
                SwitchOffHandler.Invoke(Index);

            UIOffSet();
        }
    }
    void OnClickEvent(PointerEventData ped)
    {
        if (!isAllOff && isSwitch)
            return;

        Switch(!isSwitch);
    }

    protected override sealed void UISet() => base.UISet();
    protected virtual void UIOnSet() { }
    protected virtual void UIOffSet() { }
}