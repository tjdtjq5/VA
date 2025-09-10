using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tween<T> : ITween
{
    protected T _startValue;
    protected T _endValue;
    protected float _duration;
    protected Action<T> _onTweenUpdate;
    protected float _elapsedTime;
    
    protected float _delayElapsedTime;

    protected int _loopsCompleted;
    protected bool _reverse;
    protected bool _pingPong;
    protected int _loopCount = 1;

    protected readonly List<float> _excutesKeys = new();
    protected Action _onUpdate;
    protected Dictionary<float, Action> _onPercentCompleted = new();
    protected EaseType _easeType = EaseType.Linear;

    public Tween(object target, string identifier, T startValue, T endValue, float duration, Action<T> onTweenUpdate)
    {
        this.Target = target;
        this.Identifier = identifier;
        this._startValue = startValue;
        this._endValue = endValue;
        this._duration = duration;
        this._onTweenUpdate = onTweenUpdate;
        
        Managers.Tween.AddTween(this);
    }
    
    public void Update()
    {
        if (!IsPaused)
        {
            if (IgnoreTimeScale)
                _delayElapsedTime += Managers.Time.UnscaledTime;
            else
                _delayElapsedTime += Managers.Time.FixedDeltaTime;

            if (_delayElapsedTime >= DelayTime)
            {
                if (IsComplete)
                    return;

                if (IsTargetDestroyed())
                {
                    FullKill();
                    return;
                }

                if (IgnoreTimeScale)
                    _elapsedTime += Managers.Time.UnscaledTime;
                else
                    _elapsedTime += Managers.Time.FixedDeltaTime;
                float t = _elapsedTime / _duration;
                float easedT = Ease(_easeType, t);

                T currentValue;

                if (_reverse)
                    currentValue = Interpolate(_endValue, _startValue, easedT);
                else
                    currentValue = Interpolate(_startValue, _endValue, easedT);
        
                _onUpdate?.Invoke();
                _onTweenUpdate?.Invoke(currentValue);

                _excutesKeys.Clear();
                
                foreach (var pair in _onPercentCompleted)
                {
                    if (pair.Key >= 0f && t >= pair.Key)
                    {
                        pair.Value?.Invoke();
                        _excutesKeys.Add(pair.Key);
                    }
                }

                for (int i = 0; i < _excutesKeys.Count; i++)
                    _onPercentCompleted.Remove(_excutesKeys[i]);

                if (_elapsedTime >= _duration)
                {
                    _loopsCompleted++;
                    _elapsedTime = 0f;

                    if (_pingPong)
                        _reverse = !_reverse;

                    if (_loopCount > 0 && _loopsCompleted >= _loopCount)
                        OnCompleteKill();
                }
            }
        }
    }

    public abstract T Interpolate(T start, T end, float t);
    public void OnCompleteKill()
    {
        IsComplete = true;
        _onUpdate = null;
        _onTweenUpdate = null;
        _onPercentCompleted = null;
    }

    public void FullKill()
    {
        OnCompleteKill();
        WasKilled = true;
        OnComplete = null;
    }

    public bool IsTargetDestroyed()
    {
        if (Target is MonoBehaviour mono && !mono)
            return true;

        if (Target is GameObject go && !go)
            return true;
        
        if (Target is Delegate del && del.Target == null)
            return true;

        return false;
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Resume()
    {
        IsPaused = false;
    }

    public object Target { get; protected set; }
    public bool IsComplete { get; protected set; }
    public bool WasKilled { get; protected set; }
    public bool IsPaused { get; protected set; }
    public bool IgnoreTimeScale { get; protected set; }
    public string Identifier { get; protected set; }
    public float DelayTime { get; protected set; }
    public Action OnComplete { get; set; }

    public Tween<T> SetEase(EaseType easeType)
    {
        _easeType = easeType;
        return this;
    }

    public Tween<T> SetPingPong(int loopCount = 1)
    {
        _loopCount = loopCount;
        _pingPong = true;
        return this;
    }
    
    public Tween<T> SetLoop(int loopCount = 1)
    {
        _loopCount = loopCount;
        return this;
    }

    public Tween<T> SetOnComplete(Action onComplete)
    {
        OnComplete = onComplete;
        return this;
    }

    public Tween<T> SetIgnoreTimeScale()
    {
        IgnoreTimeScale = true;
        return this;
    }

    public Tween<T> SetOnUpdate(Action onUpdate)
    {
        _onUpdate = onUpdate;
        return this;
    }

    public Tween<T> SetOnPerceontCompleted(float percentCompleted, Action onPercentCompleted)
    {
        float percentTime = Mathf.Clamp01(percentCompleted);
        if (!_onPercentCompleted.TryAdd(percentTime, onPercentCompleted))
            _onPercentCompleted[percentTime] += onPercentCompleted;

        return this;
    }

    public Tween<T> SetStartDelay(float delayTime)
    {
        DelayTime = delayTime;
        return this;
    }

    #region Calculation

    public static float Linear(float t)
    {
        return t;
    }
    public static float ExpoEaseIn(float t)
    {
        return t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1));
    }
    public static float ExpoEaseOut(float t)
    {
        return Mathf.Approximately(t, 1) ? 1 : 1 - Mathf.Pow(2, -10 * t);
    }
    public static float ExpoEaseInOut(float t)
    {
        if (t == 0) return 0f;
        if (Mathf.Approximately(t, 1)) return 1f;
        if (t < 0.5f) return 0.5f * Mathf.Pow(2, 20 * t - 10);
        return 1 - 0.5f * Mathf.Pow(2, -20 * t + 10);
    }
    public static float ExpoEaseOutIn(float t)
    {
        if (t < 0.5f) return ExpoEaseOut(2 * t) * 0.5f;
        return ExpoEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float CircEaseIn(float t)
    {
        return 1 - Mathf.Sqrt(1 - t * t);
    }
    public static float CircEaseOut(float t)
    {
        return Mathf.Sqrt(1 - (t - 1) * (t - 1));
    }
    public static float CircEaseInOut(float t)
    {
        if (t < 0.5f) return 0.5f * (1 - Mathf.Sqrt(1 - 4 * t * t));
        return 0.5f * (Mathf.Sqrt(1 - (2 * t - 2) * (2 * t - 2)) + 1);
    }
    public static float CircEaseOutIn(float t)
    {
        if (t < 0.5f) return CircEaseOut(2 * t) * 0.5f;
        return CircEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float QuadEaseIn(float t)
    {
        return t * t;
    }
    public static float QuadEaseOut(float t)
    {
        return t * (2 - t);
    }
    public static float QuadEaseInOut(float t)
    {
        if (t < 0.5f) return 2 * t * t;
        return -1 + (4 - 2 * t) * t;
    }
    public static float QuadEaseOutIn(float t)
    {
        if (t < 0.5f) return QuadEaseOut(2 * t) * 0.5f;
        return QuadEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float SineEaseIn(float t)
    {
        return 1 - Mathf.Cos(t * Mathf.PI / 2);
    }
    public static float SineEaseOut(float t)
    {
        return Mathf.Sin(t * Mathf.PI / 2);
    }
    public static float SineEaseInOut(float t)
    {
        return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1);
    }
    public static float SineEaseOutIn(float t)
    {
        if (t < 0.5f) return SineEaseOut(2 * t) * 0.5f;
        return SineEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float CubicEaseIn(float t)
    {
        return t * t * t;
    }
    public static float CubicEaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }
    public static float CubicEaseInOut(float t)
    {
        if (t < 0.5f) return 4 * t * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 3) / 2f;
    }
    public static float CubicEaseOutIn(float t)
    {
        if (t < 0.5f) return CubicEaseOut(2 * t) * 0.5f;
        return CubicEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float QuartEaseIn(float t)
    {
        return t * t * t * t;
    }
    public static float QuartEaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }

    public static float QuartEaseInOut(float t)
    {
        if (t < 0.5f) return 8 * t * t * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 4) / 2f;
    }
    public static float QuartEaseOutIn(float t)
    {
        if (t < 0.5f) return QuartEaseOut(2 * t) * 0.5f;
        return QuartEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float QuintEaseIn(float t)
    {
        return t * t * t * t * t;
    }
    public static float QuintEaseOut(float t)
    {
        return 1 - Mathf.Pow(1 - t, 5);
    }
    public static float QuintEaseInOut(float t)
    {
        if (t < 0.5f) return 16 * t * t * t * t * t;
        return 1 - Mathf.Pow(-2 * t + 2, 5) / 2f;
    }
    public static float QuintEaseOutIn(float t)
    {
        if (t < 0.5f) return QuintEaseOut(2 * t) * 0.5f;
        return QuintEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float ElasticEaseIn(float t)
    {
        return t == 0 ? 0 : t == 1 ? 1 : Mathf.Pow(2 , 10 * t - 10) * Mathf.Sin((t *10 - 10.75f) * ((2 * Mathf.PI) / 3f)); 
    }
    public static float ElasticEaseOut(float t)
    {
        return t == 0 ? 0 : Mathf.Approximately(t, 1) ? 1 : Mathf.Pow(2 , -10 * t) * Mathf.Sin((t *10 - 0.75f) * ((2 * Mathf.PI) / 3f)) + 1; 
    }
    public static float ElasticEaseInOut(float t)
    {
        if (t == 0) return 0f;
        if (Mathf.Approximately(t, 1)) return 1f;
        if (t < 0.5f) return -(Mathf.Pow(2f, 20f * t - 10) * Mathf.Sin((20 * t - 11.125f) * ((2 * Mathf.PI) / 4.5f))) / 2f;
        return Mathf.Pow(2f, -20f * t + 10) * Mathf.Sin((20 * t - 11.125f) * ((2 * Mathf.PI) / 4.5f)) / 2f + 1;
    }

    public static float ElasticEaseOutIn(float t)
    {
        if (t < 0.5f) return ElasticEaseOut(2 * t) * 0.5f;
        return ElasticEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float BounceEaseIn(float t)
    {
        return 1 - BounceEaseOut(1 - t);
    }
    public static float BounceEaseOut(float t)
    {
        if (t < 1 / 2.75f) return 7.5625f * t * t;
        else if (t < 2 / 2.75f) return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
        else if (t < 2.5f / 2.75f) return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
        else return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
    }
    public static float BounceEaseInOut(float t)
    {
        if (t < 0.5f) return BounceEaseIn(t * 2) * 0.5f;
        return BounceEaseOut(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float BounceEaseOutIn(float t)
    {
        if (t < 0.5f) return BounceEaseOut(2 * t) * 0.5f;
        return BounceEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    public static float BackEaseIn(float t)
    {
        float s = 1.70158f;
        return t * t * ((s + 1) * t - s);
    }
    public static float BackEaseOut(float t)
    {
        float s = 1.70158f;
        return (t -= 1) * t * ((s + 1) * t + s) + 1;
    }
    public static float BackEaseInOut(float t)
    {
        float s = 1.70158f * 1.525f;
        if (t < 0.5f) return (t * 2) * (t * 2) * ((s + 1) * (t * 2) - s) * 0.5f;
        return ((t * 2 - 2) * (t * 2 - 2) * ((s + 1) * (t * 2 - 2) + s) + 2) * 0.5f;
    }
    public static float BackEaseOutIn(float t)
    {
        if (t < 0.5f) return BackEaseOut(2 * t) * 0.5f;
        return BackEaseIn(2 * t - 1) * 0.5f + 0.5f;
    }
    #endregion
    
    #region Ease enum and Helper

    public static float Ease(EaseType equation, float t )
    {
        switch (equation)
        {
            case EaseType.Linear:
                return Linear(t);
            case EaseType.ExpoEaseInOut:
                return ExpoEaseInOut(t);
            case EaseType.ExpoEaseOutIn:
                return ExpoEaseOutIn(t);
            case EaseType.ExpoEaseIn:
                return ExpoEaseIn(t);
            case EaseType.ExpoEaseOut:
                return ExpoEaseOut(t);
            case EaseType.CircEaseInOut:
                return CircEaseInOut(t);
            case EaseType.CircEaseOutIn:
                return CircEaseOutIn(t);
            case EaseType.CircEaseIn:
                return CircEaseIn(t);
            case EaseType.CircEaseOut:
                return CircEaseOut(t);
            case EaseType.QuadEaseInOut:
                return QuadEaseInOut(t);
            case EaseType.QuadEaseOutIn:
                return QuadEaseOutIn(t);
            case EaseType.QuadEaseIn:
                return QuadEaseIn(t);
            case EaseType.QuadEaseOut:
                return QuadEaseOut(t);
            case EaseType.SineEaseInOut:
                return SineEaseInOut(t);
            case EaseType.SineEaseOutIn:
                return SineEaseOutIn(t);
            case EaseType.SineEaseIn:
                return SineEaseIn(t);
            case EaseType.SineEaseOut:
                return SineEaseOut(t);
            case EaseType.CubicEaseInOut:
                return CubicEaseInOut(t);
            case EaseType.CubicEaseOutIn:
                return CubicEaseOutIn(t);
            case EaseType.CubicEaseIn:
                return CubicEaseIn(t);
            case EaseType.CubicEaseOut:
                return CubicEaseOut(t);
            case EaseType.QuartEaseInOut:
                return QuartEaseInOut(t);
            case EaseType.QuartEaseOutIn:
                return QuartEaseOutIn(t);
            case EaseType.QuartEaseIn:
                return QuartEaseIn(t);
            case EaseType.QuartEaseOut:
                return QuartEaseOut(t);
            case EaseType.QuintEaseInOut:
                return QuintEaseInOut(t);
            case EaseType.QuintEaseOutIn:
                return QuintEaseOutIn(t);
            case EaseType.QuintEaseIn:
                return QuintEaseIn(t);
            case EaseType.QuintEaseOut:
                return QuintEaseOut(t);
            case EaseType.ElasticEaseInOut:
                return ElasticEaseInOut(t);
            case EaseType.ElasticEaseOutIn:
                return ElasticEaseOutIn(t);
            case EaseType.ElasticEaseIn:
                return ElasticEaseIn(t);
            case EaseType.ElasticEaseOut:
                return ElasticEaseOut(t);
            case EaseType.BounceEaseInOut:
                return BounceEaseInOut(t);
            case EaseType.BounceEaseOutIn:
                return BounceEaseOutIn(t);
            case EaseType.BounceEaseIn:
                return BounceEaseIn(t);
            case EaseType.BounceEaseOut:
                return BounceEaseOut(t);
            case EaseType.BackEaseInOut:
                return BackEaseInOut(t);
            case EaseType.BackEaseOutIn:
                return BackEaseOutIn(t);
            case EaseType.BackEaseIn:
                return BackEaseIn(t);
            case EaseType.BackEaseOut:
                return BackEaseOut(t);
            default:
                return Linear(t);
        }
    }
    #endregion
}

public enum EaseType
{
    Linear,
    ExpoEaseInOut,
    ExpoEaseOutIn,
    ExpoEaseIn,
    ExpoEaseOut,
    CircEaseInOut,
    CircEaseOutIn,
    CircEaseIn,
    CircEaseOut,
    QuadEaseInOut,
    QuadEaseOutIn,
    QuadEaseIn,
    QuadEaseOut,
    SineEaseInOut,
    SineEaseOutIn,
    SineEaseIn,
    SineEaseOut,
    CubicEaseInOut,
    CubicEaseOutIn,
    CubicEaseIn,
    CubicEaseOut,
    QuartEaseInOut,
    QuartEaseOutIn,
    QuartEaseIn,
    QuartEaseOut,
    QuintEaseInOut,
    QuintEaseOutIn,
    QuintEaseIn,
    QuintEaseOut,
    ElasticEaseInOut,
    ElasticEaseOutIn,
    ElasticEaseIn,
    ElasticEaseOut,
    BounceEaseInOut,
    BounceEaseOutIn,
    BounceEaseIn,
    BounceEaseOut,
    BackEaseInOut,
    BackEaseOutIn,
    BackEaseIn,
    BackEaseOut,
}