using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UITabButton : UIFrame
{
    public Action<int> SwitchOnHandler {  get; set; }
    public Action<int> SwitchOffHandler {  get; set; }
    public Action<bool> SwitchHandler { get; set; }

    public int Index { get; private set; }
    public bool IsSwitch { get; set; }
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

    int switchHash = Animator.StringToHash("Switch");
    int pointDownHash = Animator.StringToHash("PointDown");
    int pointUpHash = Animator.StringToHash("PointUp");
    int pressedHash = Animator.StringToHash("Pressed");

    protected override void Initialize()
    {
        base.Initialize();

        Image.raycastTarget = true;

        UIOffSet();

        if (Animator)
            AniController = Animator.Initialize();

        AddPointDownEvent(OnPointDownEvent);
        AddPointUpEvent(OnPointUpEvent);
        AddClickEvent(OnClickEvent);
    }
    public void Set(int index, bool isAllOff)
    {
        this.Index = index;
        this.isAllOff = isAllOff;
    }
    public bool Switch(bool flag)
    {
        if (IsSwitch.Equals(flag))
            return false;

        IsSwitch = flag;

        if (flag)
        {
            AniController.SetBool(switchHash, true);

            UIOnSet();
        }
        else
        {
            AniController.SetBool(switchHash, false);

            UIOffSet();
        }

        return true;
    }

    public void AddClickEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.Click);
    }
    public void AddPointDownEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.PointDown);
    }
    public void AddPointUpEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.PointUp);
    }

    void OnClickEvent(PointerEventData ped)
    {
        if (!isAllOff && IsSwitch)
            return;

        if (Switch(!IsSwitch))
        {
            if (IsSwitch)
            {
                if (SwitchOnHandler != null)
                    SwitchOnHandler.Invoke(Index);
            }
            else
            {
                if (SwitchOffHandler != null)
                    SwitchOffHandler.Invoke(Index);
            }

            if (SwitchHandler != null)
                SwitchHandler.Invoke(IsSwitch);
        }
    }
    void OnPointDownEvent(PointerEventData ped)
    {
        AniController.SetTrigger(pointDownHash);
    }
    void OnPointUpEvent(PointerEventData ped)
    {
        AniController.SetTrigger(pointUpHash);
    }

    protected override sealed void UISet() => base.UISet();
    protected virtual void UIOnSet() { }
    protected virtual void UIOffSet() { }
}