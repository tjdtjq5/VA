using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAniController : MonoBehaviour
{
    private SkeletonAnimation _sa;

    private readonly Dictionary<string, Action<string>> _onAnimationCompleteDics = new();
    private readonly Dictionary<string, Dictionary<string, Action>> _onAnimationEventDics = new(); // clipName, eventName
    private readonly Dictionary<int, string> _playAniClipName = new();

    public void Initialize(SkeletonAnimation sa)
    {
        this._sa = sa;
        sa.AnimationState.Complete += EndListener;
        sa.AnimationState.Event += EventListener;
    }

    public void Play(string aniName, bool isLoop, bool isDupli = false ,int index = 0)
    {
        if (!isDupli && IsPlay(aniName, index))
            return;
        
        try
        {
            _sa.AnimationState.SetAnimation(index, aniName, isLoop);
            _playAniClipName.TryAdd_H(index, aniName, true);
        }
        catch
        {
            UnityHelper.Error_H($"SpineAniController Play Error\naniName : {aniName}");
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

    public void SetEndFunc(string clipName, Action<string> callback)
    {
        if (_onAnimationCompleteDics.ContainsKey(clipName))
        {
            _onAnimationCompleteDics[clipName] -= callback;
            _onAnimationCompleteDics[clipName] += callback;
        }
        else
        {
            _onAnimationCompleteDics.Add(clipName, callback);
        }
    }
    void EndListener(TrackEntry trackEntry)
    {
        string clipName = trackEntry.Animation.Name;
        _playAniClipName.TryAdd_H(trackEntry.TrackIndex, "", true);

        if (_onAnimationCompleteDics.ContainsKey(clipName))
            _onAnimationCompleteDics[clipName]?.Invoke(clipName);
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
        
        if (_onAnimationEventDics.ContainsKey(clipName))
            if (_onAnimationEventDics[clipName].ContainsKey(eName))
                _onAnimationEventDics[clipName][eName]?.Invoke();
    }
    public void Clear()
    {
        _sa.Initialize(true);
        _sa = null;
    }
}
