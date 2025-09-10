using System;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    public Action OnEnded;
    
    [SerializeField] private UIImage blackImage;
    
    private Tween<float> _tween;
    
    private readonly float _inDuration = 0.05f;
    private readonly float _outDuration = 0.25f;
    private readonly float _alpha = 0.95f;

    public void Play(float time)
    {
        if(_tween != null)
            _tween.FullKill();

        float waitTime = time - _inDuration + _outDuration;
        waitTime = Mathf.Max(waitTime, 0f);

        FadeIn(_inDuration, waitTime, _outDuration);
    }

    void FadeIn(float inDuration, float fadeTime, float outDuration)
    {
        _tween = Managers.Tween.TweenAlpha(blackImage, 0, _alpha, inDuration).SetOnPerceontCompleted(1, () => Wait(fadeTime, outDuration));
    }

    void Wait(float fadeTime, float outDuration)
    {
        _tween = Managers.Tween.TweenInvoke(fadeTime).SetOnPerceontCompleted(1, () => FadeOut(outDuration));
    }

    void FadeOut(float outDuration)
    {
        _tween = Managers.Tween.TweenAlpha(blackImage, _alpha, 0, outDuration).SetOnPerceontCompleted(1, End);
    }

    void End()
    {
        OnEnded?.Invoke();
        Managers.Resources.Destroy(this.gameObject);
    }
}
