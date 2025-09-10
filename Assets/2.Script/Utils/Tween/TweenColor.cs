using System;
using UnityEngine;

public class TweenColor : Tween<Color>
{
    public TweenColor(object target, string identifier, Color startValue, Color endValue, float duration, Action<Color> onTweenUpdate) : base(target, identifier, startValue, endValue, duration, onTweenUpdate)
    {
    }

    public override Color Interpolate(Color start, Color end, float t) => Color.LerpUnclamped(start, end, t);
    
    public TweenColor SetEaseColor(EaseType easeType)
    {
        _easeType = easeType;
        return this;
    }

    public TweenColor SetPingPongColor(int loopCount = 1)
    {
        _loopCount = loopCount;
        _pingPong = true;
        return this;
    }
    
    public TweenColor SetLoopColor(int loopCount = 1)
    {
        _loopCount = loopCount;
        return this;
    }

    public TweenColor SetOnCompleteColor(Action onComplete)
    {
        _onUpdate = onComplete;
        return this;
    }

    public TweenColor SetIgnoreTimeScaleColor()
    {
        IgnoreTimeScale = true;
        return this;
    }

    public TweenColor SetOnUpdateColor(Action onUpdate)
    {
        _onUpdate = onUpdate;
        return this;
    }

    public TweenColor SetOnPerceontCompletedColor(float percentCompleted, Action onPercentCompleted)
    {
        float percentTime = Mathf.Clamp01(percentCompleted);
        if (!_onPercentCompleted.TryAdd(percentTime, onPercentCompleted))
            _onPercentCompleted[percentTime] += onPercentCompleted;

        return this;
    }

    public TweenColor SetStartDelayColor(float delayTime)
    {
        DelayTime = delayTime;
        return this;
    }
}
