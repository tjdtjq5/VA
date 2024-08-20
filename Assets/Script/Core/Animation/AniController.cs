using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniController : MonoBehaviour
{
    public Animator anim;

    public Action<string> OnAnimationStart;
    public Action<string> OnAnimationComplete;

    public void Initialize(Animator ani)
    {
        anim = ani;

        for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            AnimationClip clip = anim.runtimeAnimatorController.animationClips[i];

            AnimationEvent animationStartEvent = new AnimationEvent();
            animationStartEvent.time = 0;
            animationStartEvent.functionName = "AnimationStartHandler";
            animationStartEvent.stringParameter = clip.name;

            AnimationEvent animationEndEvent = new AnimationEvent();
            animationEndEvent.time = clip.length;
            animationEndEvent.functionName = "AnimationCompleteHandler";
            animationEndEvent.stringParameter = clip.name;

            clip.AddEvent(animationStartEvent);
            clip.AddEvent(animationEndEvent);
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
    public void AnimationStartHandler(string name)
    {
        OnAnimationStart?.Invoke(name);
    }
    public void AnimationCompleteHandler(string name)
    {
        OnAnimationComplete?.Invoke(name);
    }
    public bool GetBool(int hashCode)
    {
        return anim.GetBool(hashCode);
    }
}