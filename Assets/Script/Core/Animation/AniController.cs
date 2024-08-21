using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniController : MonoBehaviour
{
    public Animator anim;

    Dictionary<string, Action<string>> OnAnimationCompleteDics = new();

    public void Initialize(Animator ani)
    {
        anim = ani;
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

            for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
            {
                AnimationClip clip = anim.runtimeAnimatorController.animationClips[i];

                if (!clip.name.Equals(clipName))
                    continue;

                AnimationEvent animationEndEvent = new AnimationEvent();
                animationEndEvent.time = clip.length;
                animationEndEvent.functionName = "AnimationCompleteHandler";
                animationEndEvent.stringParameter = clip.name;

                clip.AddEvent(animationEndEvent);
            }
        }
    }

    public void AniSpeed(float _speed)
    {
        anim.speed = _speed;
    }
    public void SetTrigger(int hashCode)
    {
        anim.SetTrigger(hashCode);
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
        OnAnimationCompleteDics[clipName]?.Invoke(clipName);
    }
    public bool GetBool(int hashCode)
    {
        return anim.GetBool(hashCode);
    }
}