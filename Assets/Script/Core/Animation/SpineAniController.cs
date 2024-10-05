using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAniController : MonoBehaviour
{
    SkeletonAnimation sa;

    Dictionary<string, Action<string>> OnAnimationCompleteDics = new();
    Dictionary<string, Action> OnAnimationEventDics = new();
    Dictionary<int, string> PlayAniClipName = new();

    public void Initialize(SkeletonAnimation sa)
    {
        this.sa = sa;
        sa.AnimationState.Complete += EndListener;
        sa.AnimationState.Event += EventListener;
    }

    public void Play(string aniName, bool isLoop, bool isDupli = false ,int index = 0)
    {
        if (!isDupli && IsPlay(aniName, index))
            return;
        
        try
        {
            sa.AnimationState.SetAnimation(index, aniName, isLoop);
            PlayAniClipName.TryAdd_H(index, aniName, true);
        }
        catch
        {
            UnityHelper.LogError_H($"SpineAniController Play Error\naniName : {aniName}");
        }
    }
    public void AniSpeed(float _speed)
    {
        sa.AnimationState.TimeScale = _speed;
    }
    public bool IsPlay(string aniName, int index = 0)
    {
        return PlayAniClipName.TryGet_H(index).Equals(aniName);
    }
    public string GetClipName(int index)
    {
        return sa.AnimationState.GetCurrent(index).Animation.Name;
    }

    public void SetEndFunc(string clipName, Action<string> callback)
    {
        if (OnAnimationCompleteDics.ContainsKey(clipName))
        {
            OnAnimationCompleteDics[clipName] -= callback;
            OnAnimationCompleteDics[clipName] += callback;
        }
        else
        {
            OnAnimationCompleteDics.Add(clipName, callback);
        }
    }
    void EndListener(TrackEntry trackEntry)
    {
        string clipName = trackEntry.Animation.Name;
        PlayAniClipName.TryAdd_H(trackEntry.TrackIndex, "", true);

        if (OnAnimationCompleteDics.ContainsKey(clipName))
            OnAnimationCompleteDics[clipName]?.Invoke(clipName);
    }
    public void SetEventFunc(string clipName, Action callback)
    {
        if (OnAnimationEventDics.ContainsKey(clipName))
        {
            OnAnimationEventDics[clipName] -= callback;
            OnAnimationEventDics[clipName] += callback;
        }
        else
        {
            OnAnimationEventDics.Add(clipName, callback);
        }
    }
    void EventListener(TrackEntry trackEntry, Spine.Event e)
    {
        string eName = e.Data.Name;

        if (OnAnimationEventDics.ContainsKey(eName))
            OnAnimationEventDics[eName]?.Invoke();
    }
    public void Clear()
    {
        sa.Initialize(true);
        sa = null;
    }
}
