using System;
using UnityEngine;

public class TweenFloat : Tween<float>
{
    public TweenFloat(object target, string identifier, float startValue, float endValue, float duration, Action<float> onTweenUpdate) : base(target, identifier, startValue, endValue, duration, onTweenUpdate)
    {
    }

    public override float Interpolate(float start, float end, float t) => Mathf.LerpUnclamped(start, end, t);
    
    public TweenFloat SetEaseFloat(EaseType easeType)
    {
        _easeType = easeType;
        return this;
    }

    public TweenFloat SetPingPongFloat(int loopCount = 1)
    {
        _loopCount = loopCount;
        _pingPong = true;
        return this;
    }
    
    public TweenFloat SetLoopFloat(int loopCount = 1)
    {
        _loopCount = loopCount;
        return this;
    }

    public TweenFloat SetOnCompleteFloat(Action onComplete)
    {
        _onUpdate = onComplete;
        return this;
    }

    public TweenFloat SetIgnoreTimeScaleFloat()
    {
        IgnoreTimeScale = true;
        return this;
    }

    public TweenFloat SetOnUpdateFloat(Action onUpdate)
    {
        _onUpdate = onUpdate;
        return this;
    }

    public TweenFloat SetOnPerceontCompletedFloat(float percentCompleted, Action onPercentCompleted)
    {
        float percentTime = Mathf.Clamp01(percentCompleted);
        if (!_onPercentCompleted.TryAdd(percentTime, onPercentCompleted))
            _onPercentCompleted[percentTime] += onPercentCompleted;

        return this;
    }

    public TweenFloat SetStartDelayFloat(float delayTime)
    {
        DelayTime = delayTime;
        return this;
    }
}
