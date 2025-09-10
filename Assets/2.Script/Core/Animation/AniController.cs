using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AniController : MonoBehaviour
{
    public Animator anim;
    
    private float _speed = 1;

    private readonly Dictionary<string, Action<string>> _onAnimationCompleteDics = new();

    public void Initialize(Animator ani)
    {
        anim = ani;
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

            for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip clip = anim.runtimeAnimatorController.animationClips[i];

                if (!clip.name.Equals(clipName))
                    continue;

                AnimationEvent animationEndEvent = new AnimationEvent();
                animationEndEvent.time = clip.length;
                animationEndEvent.functionName = "AnimationCompleteHandler";
                animationEndEvent.stringParameter = clip.name;

                if (clip.events.ToList().FindAll(e => e.stringParameter.Equals(animationEndEvent.stringParameter)).Count <= 0)
                    clip.AddEvent(animationEndEvent);
            }
        }
    }
    public void RemoveEndFunc(string clipName)
    {
        if (_onAnimationCompleteDics.ContainsKey(clipName))
            _onAnimationCompleteDics.Remove(clipName);
    }

    public void AniSpeed(float speed)
    {
        this._speed = speed;
        anim.speed = speed;
    }
    public void SetTrigger(int hashCode)
    {
        anim.SetTrigger(hashCode);
    }
    public void SetTrigger(string clipName)
    {
        anim.SetTrigger(clipName);
    }
    public void SetBool(int hashCode, bool value)
    {
        anim.SetBool(hashCode, value);
    }
    public void SetFloat(int hashCode, float value)
    {
        anim.SetFloat(hashCode, value);
    }
    public void AnimationCompleteHandler(string clipName)
    {
        if (_onAnimationCompleteDics.ContainsKey(clipName))
            _onAnimationCompleteDics[clipName]?.Invoke(clipName);
    }
    public float GetClipLength(string clipName)
    {
        for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = anim.runtimeAnimatorController.animationClips[i];
            if (clip.name.Equals(clipName))
            {
                return clip.length;
            }
        }

        return 0;
    }
    public bool GetBool(int hashCode)
    {
        return anim.GetBool(hashCode);
    }

    // #region Time
    //
    // private void OnEnable()
    // {
    //     Managers.Time.TimeAdd(this);
    // }
    //
    // private void OnDisable()
    // {
    //     Managers.Time.TimeRemove(this);
    // }
    //
    // public void TimeScale(float value)
    // {
    //     if (!anim)
    //         return;
    //
    //     AniSpeed(value);
    // }
    // public void TimeUnScale()
    // {
    //     Managers.Time.TimeRemove(this);
    //     AniSpeed(1);
    // }
    //
    // #endregion
}