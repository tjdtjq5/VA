using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;
using Random = UnityEngine.Random;
using Unity.Cinemachine;
using Shared.CSharp;

public class TweenManager
{
    private Dictionary<string, ITween> _activeTweens = new();

    public void Update()
    {
        foreach (var pair in _activeTweens.ToList())
        {
            ITween tween = pair.Value;
            tween.Update();

            if (tween.IsComplete && !tween.WasKilled)
            {
                RemoveTween(pair.Key);

                if (tween.OnComplete != null)
                {
                    tween.OnComplete.Invoke();
                    tween.OnComplete = null;
                }
            }

            if (tween.WasKilled)
                RemoveTween(pair.Key);
        }
    }
    public void AddTween<T>(Tween<T> tween)
    {
        if (_activeTweens.ContainsKey(tween.Identifier))
            _activeTweens[tween.Identifier].OnCompleteKill();

        _activeTweens[tween.Identifier] = tween;
    }
    public void RemoveTween(string identifier)
    {
        _activeTweens.Remove(identifier);
    }

    #region static helper methods

    #region  Rotation
    
    public TweenFloat TweenRotationZ(Transform transform, float start, float end, float duration)
    {
        if (Mathf.Abs(end - start) > Mathf.Abs(end - start + 360f))
            end += 360f;
        else if (Mathf.Abs(end - start) > Mathf.Abs(end - start - 360f))
            end -= 360f;
        
        string identifier = $"{transform.GetInstanceID()}_TweenRotation";
        return new TweenFloat(transform, identifier, start, end, duration, (value) =>
        {
            transform.rotation = Quaternion.AngleAxis(value, Vector3.forward);
        });
    }
    
    #endregion
    
    #region Slider

    public TweenFloat TweenSlider(UISlider slider, float start, float end, float duration)
    {
        string identifier = $"{slider.GetInstanceID()}_TweenSlider";
        return new TweenFloat(slider, identifier, start, end, duration, (value) =>
        {
            slider.value = value;
        });
    }

    #endregion
    
    #region Scrollbar

    public TweenFloat TweenScrollbar(Scrollbar scrollbar, float start, float end, float duration)
    {
        string identifier = $"{scrollbar.GetInstanceID()}_TweenScrollbar";
        return new TweenFloat(scrollbar, identifier, start, end, duration, (value) =>
        {
            scrollbar.value = value;
        });
    }

    #endregion
    
    #region FillAmount

    public TweenFloat TweenFillAmount(Image image, float start, float end, float duration)
    {
        string identifier = $"{image.GetInstanceID()}_TweenFillAmount";
        return new TweenFloat(image, identifier, start, end, duration, (value) =>
        {
            image.fillAmount = value;
        });
    }
    public TweenFloat TweenFillAmount(UIImage image, float start, float end, float duration)
    {
        string identifier = $"{image.GetInstanceID()}_TweenFillAmount";
        return new TweenFloat(image, identifier, start, end, duration, (value) =>
        {
            image.FillAmount = value;
        });
    }
    
    #endregion
    
    #region Camera

    public TweenFloat TweenCameraFieldOfView(Camera camera, float start, float end, float duration)
    {
        string identifier = $"{camera.GetInstanceID()}_TweenCameraFieldOfView";
        return new TweenFloat(camera, identifier, start, end, duration, (value) =>
        {
            camera.fieldOfView = value;
        });
    }
    public TweenFloat TweenCameraFieldOfView(CinemachineVirtualCamera camera, float start, float end, float duration)
    {
        string identifier = $"{camera.GetInstanceID()}_TweenCinemachineVirtualCameraFieldOfView";
        return new TweenFloat(camera, identifier, start, end, duration, (value) =>
        {
            camera.m_Lens.FieldOfView = value;
        });
    }
    
    #endregion
    
    #region  Color

    public TweenFloat TweenAlpha(SpriteRenderer spriteRenderer, float startAlpha, float endAlpha, float duration)
    {
        string identifier = $"{spriteRenderer.GetInstanceID()}_TweenSpriteAlpha";
        return new TweenFloat(spriteRenderer, identifier, startAlpha, endAlpha, duration, (value) =>
        {
            Color color = spriteRenderer.color;
            color.a = value;
            spriteRenderer.color = color;
        });
    }
    public TweenFloat TweenAlpha(Image image, float startAlpha, float endAlpha, float duration)
    {
        string identifier = $"{image.GetInstanceID()}_TweenImageAlpha";
        return new TweenFloat(image, identifier, startAlpha, endAlpha, duration, (value) =>
        {
            Color color = image.color;
            color.a = value;
            image.color = color;
        });
    }
    public TweenFloat TweenAlpha(UIImage image, float startAlpha, float endAlpha, float duration)
    {
        string identifier = $"{image.GetInstanceID()}_TweenUIImageAlpha";
        return new TweenFloat(image, identifier, startAlpha, endAlpha, duration, (value) =>
        {
            Color color = image.color;
            color.a = value;
            image.color = color;
        });
    }
    public TweenFloat TweenAlpha(Text text, float startAlpha, float endAlpha, float duration)
    {
        string identifier = $"{text.GetInstanceID()}_TweenTextAlpha";
        return new TweenFloat(text, identifier, startAlpha, endAlpha, duration, (value) =>
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        });
    }
    public TweenFloat TweenAlpha(UIText text, float startAlpha, float endAlpha, float duration)
    {
        string identifier = $"{text.GetInstanceID()}_TweenUITextAlpha";
        return new TweenFloat(text, identifier, startAlpha, endAlpha, duration, (value) =>
        {
            Color color = text.color;
            color.a = value;
            text.color = color;
        });
    }
    #endregion
    
    #region  Scale
    
    public TweenVector TweenScale(Transform transform, Vector3 startScale, Vector3 endScale, float duration)
    {
        string identifier = $"{transform.GetInstanceID()}_TweenScale";
        return new TweenVector(transform, identifier, startScale, endScale, duration, (value) =>
        {
            transform.localScale = value;
        });
    }

    #endregion
    
    #region  Position
    
    public TweenVector TweenPosition(Transform transform, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        string identifier = $"{transform.GetInstanceID()}_TweenPosition";
        return new TweenVector(transform, identifier, startPosition, endPosition, duration, (value) =>
        {
            transform.position = value;
        });
    }
    
    public TweenVector TweenLocalPosition(Transform transform, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        string identifier = $"{transform.GetInstanceID()}_TweenLocalPosition";
        return new TweenVector(transform, identifier, startPosition, endPosition, duration, (value) =>
        {
            transform.localPosition = value;
        });
    }
    
    public TweenVector TweenShake(Transform transform, float magnitude, float duration)
    {
        string identifier = $"{transform.GetInstanceID()}_TweenShake";
        Vector3 originalPos = transform.localPosition;
        return new TweenVector(transform, identifier, originalPos, originalPos, duration, (value) =>
        {
            Vector3 shakePos = originalPos + Random.insideUnitSphere * magnitude;
            transform.localPosition = new Vector3(shakePos.x, shakePos.y, originalPos.z);
        });
    }

    #endregion
    
    #region Text

    public TweenFloat TweenTextIncreaseValue(Text text, int start, int end, float duration, string format = null)
    {
        if (String.IsNullOrEmpty(format))
            format = "{0}";
        
        string identifier = $"{text.GetInstanceID()}_TweenTextIncreaseValue";
        return new TweenFloat(text, identifier, start, end, duration, (value) =>
        {
            if ((int)value > end)
                value = end;
            
            text.text = CSharpHelper.Format_H(format, (int)value);
        });
    }
    public TweenFloat TweenTextIncreaseValue(UIText text, int start, int end, float duration, string format = null)
    {
        if (String.IsNullOrEmpty(format))
            format = "{0}";
        
        string identifier = $"{text.GetInstanceID()}_TweenTextIncreaseValue";
        return new TweenFloat(text, identifier, start, end, duration, (value) =>
        {
            if ((int)value > end)
                value = end;
            
            text.text = CSharpHelper.Format_H(format, (int)value);
        });
    }
    
    #endregion
    
    #region  Func
    
    public TweenFloat TweenFunc(Func<float> getFloatToTween, Action<float> setFloatToTween, float endValue, float duration)
    {
        string identifier = $"{getFloatToTween.Target.GetHashCode()}_TweenFunc";
        object target = getFloatToTween.Target;
        float startValue = getFloatToTween();

        return new TweenFloat(target, identifier, startValue, endValue, duration, (value) =>
        {
            setFloatToTween.Invoke(value);
        });
    }
    public TweenVector TweenFunc(Func<Vector3> getToTween, Action<Vector3> setToTween, Vector3 endValue, float duration)
    {
        string identifier = $"{getToTween.Target.GetHashCode()}_TweenFunc";
        object target = getToTween.Target;
        Vector3 startValue = getToTween();

        return new TweenVector(target, identifier, startValue, endValue, duration, (value) =>
        {
            setToTween.Invoke(value);
        });
    }
    public TweenColor TweenFunc(Func<Color> getToTween, Action<Color> setToTween, Color endValue, float duration)
    {
        string identifier = $"{getToTween.Target.GetHashCode()}_TweenFunc";
        object target = getToTween.Target;
        Color startValue = getToTween();

        return new TweenColor(target, identifier, startValue, endValue, duration, (value) =>
        {
            setToTween.Invoke(value);
        });
    }

    public TweenFloat TweenInvoke(float duration)
    {
        float startFloat = 0;
        float endFloat = 1;
        string identifier = $"{Random.Range(0f, 100000f)}_TweenInvoke";
        
        return new TweenFloat(startFloat, identifier, startFloat, endFloat, duration, (value) =>
        {
        });
    }
    
    #endregion
  
    #endregion
}
