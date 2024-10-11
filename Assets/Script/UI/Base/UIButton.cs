using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UIButton : UIFrame
{
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

    int pointDownHash = UnityEngine.Animator.StringToHash("PointDown");
    int pointUpHash = UnityEngine.Animator.StringToHash("PointUp");
    int pressedHash = UnityEngine.Animator.StringToHash("Pressed");

    bool isPressed = false;
    float pressedStartTime = 0.4f; float pressedStartTimer;
    float pressedTime = 0.1f; float pressedTimer;
    bool isPointDown;

    protected override void Initialize()
    {
        Image.raycastTarget = true;

        if (Animator)
            AniController = Animator.Initialize();

        AddPointDownEvent(OnPointDownEvent);
        AddPointUpEvent(OnPointUpEvent);
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
    public void AddDragEvent(Action<PointerEventData> _action)
    {
        BindEvent(Image.gameObject, _action, UIEvent.Drag);
    }
    public void AddPressedEvent(Action<PointerEventData> _action)
    {
        isPressed = true;

        AddPointDownEvent(_action);
    }

    void OnPointDownEvent(PointerEventData ped)
    {
        if (!isPressed)
            AniController.SetTrigger(pointDownHash);
        else
            AniController.SetBool(pressedHash, true);

        isPointDown = true;
    }
    void OnPointUpEvent(PointerEventData ped)
    {
        if (!isPressed)
            AniController.SetTrigger(pointUpHash);
        else
            AniController.SetBool(pressedHash, false);

        isPointDown = false;
    }
    private void FixedUpdate()
    {
        if (isPressed)
        {
            if (isPointDown)
            {
                float fixedDeltaTime = Managers.Time.FixedDeltaTime;
                pressedStartTimer += fixedDeltaTime;
                pressedTimer += fixedDeltaTime;

                if (pressedStartTimer >= pressedStartTime)
                {
                    if (pressedTimer >= pressedTime)
                    {
                        pressedTimer = 0;
                        GetEvent(Image.gameObject, UIEvent.PointDown).Invoke(null);
                    }
                }
            }
        }
    }
}
