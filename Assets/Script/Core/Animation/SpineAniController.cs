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
    Dictionary<int, string> PlayAniClipName = new();

    public void Initialize(SkeletonAnimation sa)
    {
        this.sa = sa;
        sa.AnimationState.Complete += EndListener;
    }

    public void Play(string aniName, bool isLoop, int index = 0)
    {
        if (IsPlay(aniName, index))
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
    public void Clear()
    {
        sa.Initialize(true);
        sa = null;
    }
}
