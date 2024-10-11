using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITabButton : UIFrame
{
    public Action<int> SwitchOnHandler {  get; set; }
    public Action<int> SwitchOffHandler {  get; set; }

    public int Index { get; private set; }
    bool isAllOff;

    Image Image
    {
        get
        {
            return GetComponent<Image>();
        }
    }
    protected Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    protected AniController AniController;

    bool isSwitch = false;
    int switchHash = UnityEngine.Animator.StringToHash("Switch");

    private void Start()
    {
        Image.raycastTarget = true;
    }
    protected override void Initialize()
    {
        base.Initialize();

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
        }
        else
        {
            AniController.SetBool(switchHash, false);

            if (SwitchOffHandler != null)
                SwitchOffHandler.Invoke(Index);
        }
    }
    void OnClickEvent(PointerEventData ped)
    {
        if (!isAllOff && isSwitch)
            return;

        Switch(!isSwitch);
    }
}