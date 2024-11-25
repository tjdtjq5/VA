using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAniController : MonoBehaviour
{
    private SkeletonAnimation _sa;
    
    private readonly string _endEvent = "end";

    private readonly Dictionary<string, Dictionary<string, Action>> _onAnimationEventDics = new(); // clipName, eventName
    private readonly Dictionary<int, string> _playAniClipName = new();

    public void Initialize(SkeletonAnimation sa)
    {
        this._sa = sa;
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
            _sa.AnimationState.SetAnimation(index, clipName, isLoop);
            _playAniClipName.TryAdd_H(index, clipName, true);
        }
        catch
        {
            UnityHelper.Error_H($"SpineAniController Play Error\naniName : {clipName}");
        }
    }
    public void AniSpeed(float _speed)
    {
        _sa.AnimationState.TimeScale = _speed;
    }
    public bool IsPlay(string aniName, int index = 0)
    {
        return _playAniClipName.TryGet_H(index).Equals(aniName);
    }
    public string GetClipName(int index)
    {
        return _sa.AnimationState.GetCurrent(index).Animation.Name;
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
    public void Clear()
    {
        _sa.Initialize(true);
        _sa = null;
    }
}
