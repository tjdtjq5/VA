using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UIButton : UIFrame
{
    public Image Image => GetComponent<Image>();
    protected Animator Animator => GetComponent<Animator>();
    public AniController AniController { get; protected set; }

    private readonly int _pointDownHash = UnityEngine.Animator.StringToHash("PointDown");
    private readonly int _pointUpHash = UnityEngine.Animator.StringToHash("PointUp");
    private readonly int _pressedHash = UnityEngine.Animator.StringToHash("Pressed");

    private bool _isPressed = false;
    private readonly float _pressedStartTime = 0.4f;
    private float _pressedStartTimer;
    private readonly float _pressedTime = 0.1f;
    private float _pressedTimer;
    private bool _isPointDown;
    private bool _isPointUpInside;

    protected override void Initialize()
    {
        Image.raycastTarget = true;

        if (Animator)
            AniController = Animator.Initialize();

        AddPointDownEvent(OnPointDownEvent);
        AddPointUpEvent(OnPointUpEvent);

        AniController?.SetEndFunc("PointUp", (clipName)=> 
        {
            if (_isPointUpInside)
                GetEvent(Image.gameObject, UIEvent.Trigger)?.Invoke(null);
        });
    }

    public void AddClickAniEvent(Action<PointerEventData> action)
    {
        BindEvent(Image.gameObject, action, UIEvent.Trigger);
    }
    public void AddClickEvent(Action<PointerEventData> action)
    {
        BindEvent(Image.gameObject, action, UIEvent.Click);
    }
    public void AddPointDownEvent(Action<PointerEventData> action)
    {
        BindEvent(Image.gameObject, action, UIEvent.PointDown);
    }
    public void AddPointUpEvent(Action<PointerEventData> action)
    {
        BindEvent(Image.gameObject, action, UIEvent.PointUp);
    }
    public void AddPressedEvent(Action<PointerEventData> action)
    {
        _isPressed = true;

        AddPointDownEvent(action);
    }

    public void AddOnEnterEvent(Action<PointerEventData> action)
    {
        BindEvent(Image.gameObject, action, UIEvent.Enter);
    }
    public void AddOnExitEvent(Action<PointerEventData> action)
    {
        BindEvent(Image.gameObject, action, UIEvent.Exit);
    }

    public void RemoveEvent(UIEvent uiEvent)
    {
        UnBindEvent(Image.gameObject, uiEvent);
    }

    void OnPointDownEvent(PointerEventData ped)
    {
        if (!_isPressed)
            AniController.SetTrigger(_pointDownHash);
        else
            AniController.SetBool(_pressedHash, true);
        
        _isPointDown = true;
    }
    void OnPointUpEvent(PointerEventData ped)
    {
        RectTransform rectTransform = Image.rectTransform;
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, ped.position, ped.pressEventCamera, out localPoint);
        _isPointUpInside = rectTransform.rect.Contains(localPoint);

        if (!_isPressed)
            AniController.SetTrigger(_pointUpHash);
        else
            AniController.SetBool(_pressedHash, false);
        
        _isPointDown = false;
    }
    private void FixedUpdate()
    {
        if (_isPressed)
        {
            if (_isPointDown)
            {
                float fixedDeltaTime = Managers.Time.FixedDeltaTime;
                _pressedStartTimer += fixedDeltaTime;
                _pressedTimer += fixedDeltaTime;

                if (_pressedStartTimer >= _pressedStartTime)
                {
                    if (_pressedTimer >= _pressedTime)
                    {
                        _pressedTimer = 0;
                        GetEvent(Image.gameObject, UIEvent.PointDown).Invoke(null);
                    }
                }
            }
        }
    }

    public void UISet(ButtonSprite sprite)
    {
        Image.sprite = Managers.Atlas.GetButton(sprite.ToString());
    }
}

public enum ButtonSprite
{
    Button_Gray,
    Button_Yellow,
    Button_Red,
    Button_Green,
}
