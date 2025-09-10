using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITabButton : UIButton
{
    public Action<int> SwitchOnHandler {  get; set; }
    public Action<int> SwitchOffHandler {  get; set; }
    public Action<bool> SwitchHandler { get; set; }

    public int Index { get; private set; }
    public bool IsSwitch { get; set; }
    private bool _isAllOff;

    private readonly int _switchHash = Animator.StringToHash("Switch");

    protected override void Initialize()
    {
        base.Initialize();

        UIOffSet();

        AddClickEvent(OnClickEvent);
    }
    public void Set(int index, bool isAllOff)
    {
        this.Index = index;
        this._isAllOff = isAllOff;
    }
    public virtual bool Switch(bool flag)
    {
        IsSwitch = flag;

        if (flag)
        {
            AniController.SetBool(_switchHash, true);

            UIOnSet();
        }
        else
        {
            AniController.SetBool(_switchHash, false);

            UIOffSet();
        }

        return true;
    }

    void OnClickEvent(PointerEventData ped)
    {
        if (!_isAllOff && IsSwitch)
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

    protected virtual void UIOnSet() { }
    protected virtual void UIOffSet() { }
}