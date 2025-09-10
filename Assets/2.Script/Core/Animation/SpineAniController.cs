using Shared.CSharp;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAniController : MonoBehaviour, ITime
{
    public SkeletonAnimation SkeletonAnimation => _saAni;
    public SkeletonGraphic SkeletonGraphic => _saGraphic;
    public MeshRenderer MeshRenderer => _meshRenderer;
    
    private SkeletonAnimation _saAni;
    private SkeletonGraphic _saGraphic;
    private MeshRenderer _meshRenderer;
    private float _speed = 1;
    
    private readonly string _endEvent = "end";
    
    private readonly Dictionary<string, Dictionary<string, Action>> _onAnimationEventDics = new(); // clipName, eventName
    private readonly Dictionary<int, string> _playAniClipName = new();

    public void Initialize(SkeletonAnimation sa)
    {
        this._saAni = sa;
        this._meshRenderer = sa.GetComponent<MeshRenderer>();
        sa.AnimationState.Event += EventListener;
    }
    public void Initialize(SkeletonGraphic sa)
    {
        this._saGraphic = sa;
        this._meshRenderer = sa.GetComponent<MeshRenderer>();
        sa.AnimationState.Event += EventListener;
    }
    public void Play(string clipName, bool isLoop, bool isDupli = false ,int index = 0)
    {
        if (!isDupli && IsPlay(clipName, index))
        {
            EventAction(clipName, _endEvent);
            return;
        }

        try
        {
            if (_saAni)
                _saAni.AnimationState.SetAnimation(index, clipName, isLoop);
            if (_saGraphic)
                _saGraphic.AnimationState.SetAnimation(index, clipName, isLoop);

            _playAniClipName.TryAdd_H(index, clipName, true);
        }
        catch
        {
            UnityHelper.Error_H($"SpineAniController Play Error\naniName : {clipName}");
        }
    }
    public void AniSpeed(float speed)
    {
        if (this._speed.Equals(speed))
            return;
        
        this._speed = speed;
        
        if (_saAni)
            _saAni.AnimationState.TimeScale = speed;
        if (_saGraphic)
            _saGraphic.AnimationState.TimeScale = speed;
    }
    public bool IsPlay(string aniName, int index = 0)
    {
        return _playAniClipName.TryGet_H(index).Equals(aniName);
    }
    public string GetClipName(int index)
    {
        if (_saAni)
            return _saAni.AnimationState.GetCurrent(index).Animation.Name;
        if (_saGraphic)
            return _saGraphic.AnimationState.GetCurrent(index).Animation.Name;

        throw new NullReferenceException();
    }
    public void SetEndFunc(string clipName, Action callback)
    {
        SetEventFunc(clipName, _endEvent, callback);
    }
    public void SetEventFunc(string clipName, string eventName, Action callback)
    {
        if (!_onAnimationEventDics.ContainsKey(clipName))
            _onAnimationEventDics.Add(clipName, new Dictionary<string, Action>());
        
        Dictionary<string, Action> eventDic = _onAnimationEventDics[clipName];

        if (eventDic.ContainsKey(eventName))
        {
            _onAnimationEventDics[clipName][eventName] -= callback;
            _onAnimationEventDics[clipName][eventName] += callback;
        }
        else
        {
            eventDic.Add(eventName, callback);
            _onAnimationEventDics[clipName] = eventDic;
        }
    }
    void EventListener(TrackEntry trackEntry, Spine.Event e)
    {
        string clipName = trackEntry.Animation.Name;
        string eName = e.Data.Name;

        EventAction(clipName, eName);
    }
    void EventAction(string clipName, string eventName)
    {
        if (_onAnimationEventDics.ContainsKey(clipName))
            if (_onAnimationEventDics[clipName].ContainsKey(eventName))
                _onAnimationEventDics[clipName][eventName]?.Invoke();
    }
    public float GetClipLength(string clipName)
    {
        if (_saAni)
        {
            var ani = _saAni.Skeleton.Data.FindAnimation(clipName);
            return ani.Duration;
        }

        if (_saGraphic)
        {
            var ani = _saGraphic.Skeleton.Data.FindAnimation(clipName);
            return ani.Duration;
        }
        
        throw new NullReferenceException();
    }
    public void ChangeSkeletonData(SkeletonDataAsset dataAsset)
    {
        if (_saAni)
        {
            _saAni.skeletonDataAsset = dataAsset;
            _saAni.ClearState();
            _saAni.Initialize(true);
        }
        if (_saGraphic) 
        {
            _saGraphic.skeletonDataAsset = dataAsset;
            _saGraphic.Clear();
            _saGraphic.Initialize(true);
        }
    }
    public void ClearState()
    {
        if (_saAni)
        {
            _saAni.ClearState();
            _saAni.Initialize(true);
            _saAni.AnimationState.Event += EventListener;
            TimeScale(Managers.Time.Magnification);

        }
        if (_saGraphic)
        {
            _saGraphic.Clear();
            _saGraphic.Initialize(true);
            _saGraphic.AnimationState.Event += EventListener;
            TimeScale(Managers.Time.Magnification);
        }
    }
    public void Clear()
    {
        if (_saAni)
        {
            _saAni.Initialize(true);
            _saAni = null;
        }

        if (_saGraphic)
        {
            _saGraphic.Initialize(true);
            _saGraphic = null;
        }
        
        _onAnimationEventDics.Clear();
        _playAniClipName.Clear();
    }
    
    #region Time

    private void OnEnable()
    {
        Managers.Time.TimeAdd(this);
    }

    private void OnDisable()
    {
        Managers.Time.TimeRemove(this);
    }

    public void TimeScale(float value)
    {
        if (_saAni)
            _saAni.AnimationState.TimeScale = _speed * value;
        if (_saGraphic)
            _saGraphic.AnimationState.TimeScale = _speed * value;
    }
    
    public void TimeUnScale()
    {
        Managers.Time.TimeRemove(this);
        AniSpeed(1);
    }
    #endregion
}
