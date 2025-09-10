using System;
using UnityEngine;

public class Move
{
    public bool IsMoving { get; set; } = false;
    private float MoveSpeed => _moveSpeed * _moveSpeedWeight;
    public string MovingAnimationName { get => Moving; set => Moving = value; }
    private readonly float _moveSpeed = 10f;
    private float _moveSpeedWeight = 1;
    
    public Action OnMove { get; set; }
    public Action OnStop { get; set; }
    public Action OnMoveEnd { get; set; }
    
    private Vector3 _destPosition;
    private bool _isStopMotion = false;

    private string Idle = "Idle";
    private string Moving = "Move";
    private string Stop = "Stop";

    public virtual void Initialize(Character character, Transform transform)
    {
        this._character = character; 
        this._transform = transform;    
        
        for (int i = 0; i < _character.SpineSpineAniControllers.Count; i++)
        {
            _character.SpineSpineAniControllers[i].SetEndFunc(Stop, OnEndStop);
        }

        _character.SetAnimation(Idle, true);
    }
    public void SetSpeed(float value = 1f) => _moveSpeedWeight = value;
    public void SetMove(Vector3 destPos, Action onStop = null)
    {
        SetMove(destPos, true, Moving, true, onStop);
    }
    public void SetMoveActionEnd(Vector3 destPos, Action onMoveEnd)
    {
        this.OnMoveEnd = onMoveEnd;
        SetMove(destPos, true, Moving, true, null);
    }
    public void SetMoveDontStopMotion(Vector3 destPos, Action onStop = null)
    {
        SetMove(destPos, false, Moving, true, onStop);
    }
    public void SetMoveDontStopMotion(string aniName, bool isLoop, Vector3 destPos, Action onStop = null)
    {
        SetMove(destPos, false, aniName, isLoop, onStop);
    }
    private void SetMove(Vector3 destPos, bool isStopMotion, string aniName, bool isLoop, Action onStop = null)
    {
        if (this._character.IsNotDetect)
            return;
        
        _destPosition = destPos;
        OnStop = onStop;
        _isStopMotion = isStopMotion;
        
        IsMoving = true;
        
        if(!string.IsNullOrEmpty(aniName))
            SetAnimation(aniName, isLoop);
    }
    public void SetTimeMove(string aniName, Vector3 destPos, float time, Action onStop = null)
    {
        if (this._character.IsNotDetect)
            return;

        IsMoving = false;

        if(!string.IsNullOrEmpty(aniName))
            SetAnimation(aniName, true);

        Vector3 startPos = this._transform.position;

        Managers.Tween.TweenPosition(this._transform, startPos, destPos, time).SetOnComplete(()=> onStop?.Invoke());
    }
    public void SetStop(bool isStopMotion)
    {
        IsMoving = false;
        OnMoveEnd?.Invoke();
        
        if(isStopMotion)
            SetAnimation(Stop, false);
    }

    private void OnEndStop()
    {
        SetIdle();
        OnStop?.Invoke();
    }
    public void SetIdle()
    {
        IsMoving = false;
        SetAnimation(Idle, true);
    }

    public void Update()
    {
        if (IsMoving)
        {
            if (this._transform.position.GetDistanceX(_destPosition) > 0.1f)
                this._transform.position = Vector3.MoveTowards(this._transform.position,_destPosition , MoveSpeed * Managers.Time.DeltaTime);
            else
            {
                if (_isStopMotion)
                    SetStop(true);
                else
                {
                    IsMoving = false;
                    OnStop?.Invoke();
                }
            }
        }
    }
    protected void SetAnimation(string aniName, bool isLoop)
    {
        _character.SetAnimation(aniName, isLoop);
    }

    public void Clear()
    {
        OnMove = null;
        OnStop = null;

        IsMoving = false;
    }
    
    private Character _character;
    private Transform _transform;
}