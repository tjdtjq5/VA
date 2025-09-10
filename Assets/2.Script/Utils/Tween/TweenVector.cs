using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenVector : Tween<Vector3>
{
    public TweenVector(object target, string identifier, Vector3 startValue, Vector3 endValue, float duration, Action<Vector3> onTweenUpdate) : base(target, identifier, startValue, endValue, duration, onTweenUpdate)
    {
    }

    public override Vector3 Interpolate(Vector3 start, Vector3 end, float t) => Vector3.LerpUnclamped(start, end, t);
    
    public TweenVector SetEaseVector(EaseType easeType)
    {
        _easeType = easeType;
        return this;
    }

    public TweenVector SetPingPongVector(int loopCount = 1)
    {
        _loopCount = loopCount;
        _pingPong = true;
        return this;
    }
    
    public TweenVector SetLoopVector(int loopCount = 1)
    {
        _loopCount = loopCount;
        return this;
    }

    public TweenVector SetOnCompleteVector(Action onComplete)
    {
        _onUpdate = onComplete;
        return this;
    }

    public TweenVector SetIgnoreTimeScaleVector()
    {
        IgnoreTimeScale = true;
        return this;
    }

    public TweenVector SetOnUpdateVector(Action onUpdate)
    {
        _onUpdate = onUpdate;
        return this;
    }

    public TweenVector SetOnPerceontCompletedVector(float percentCompleted, Action onPercentCompleted)
    {
        float percentTime = Mathf.Clamp01(percentCompleted);
        if (!_onPercentCompleted.TryAdd(percentTime, onPercentCompleted))
            _onPercentCompleted[percentTime] += onPercentCompleted;

        return this;
    }

    public TweenVector SetStartDelayVector(float delayTime)
    {
        DelayTime = delayTime;
        return this;
    }
}
