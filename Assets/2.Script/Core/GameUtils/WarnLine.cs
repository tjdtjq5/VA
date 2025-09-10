using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WarnLine : MonoBehaviour
{
    // posX, width
    public Action<float, float> OnEnd;

    [SerializeField] private Transform redLineMask;
    [SerializeField] private Transform redLineAdditive;
    [SerializeField] private Transform leftRed;
    [SerializeField] private Transform rightRed;
    [SerializeField] private Transform underLine;
    [SerializeField] private Transform leftUnder;
    [SerializeField] private Transform rightUnder;

    private Tween<float> _tween;
    private float _currentWidth;
    private float CurrentWidth() => _currentWidth;
    private readonly float _edgeWidth = 0.05f;
    private readonly float _redLineAddtiveMinWidth = 6f;
    private float _posX;
    private float _width;
    
    public Tween<float> Play(float posX, float width, float time)
    {
        this._posX = posX;
        this._width = width;
        this.transform.position = new Vector3(posX,0,0);

        this._currentWidth = 0;
        
        SetUnderLine(width);
        SetRedLine(_currentWidth);
        
        _tween = Managers.Tween.TweenFunc(CurrentWidth, SetFunc, width, time)
            .SetOnPerceontCompleted(1, SetDestory);
        
        return _tween;
    }
    void SetFunc(float value)
    {
        _currentWidth = value;
        SetRedLine(_currentWidth);
    }

    void SetDestory()
    {
        OnEnd?.Invoke(_posX, _width);
        OnEnd = null;
        
        Managers.Resources.Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        _tween?.FullKill();
    }

    void SetUnderLine(float width)
    {
        underLine.localScale = new Vector3(width / 2f, underLine.localScale.y, underLine.localScale.z);
        leftUnder.localPosition = new Vector3(width / 2f - _edgeWidth, leftUnder.localPosition.y, leftUnder.localPosition.z);
        rightUnder.localPosition = new Vector3(-(width / 2f - _edgeWidth), rightUnder.localPosition.y, rightUnder.localPosition.z);
    }

    void SetRedLine(float width)
    {
        redLineMask.localScale = new Vector3(width, redLineMask.localScale.y, redLineMask.localScale.z);
        redLineAdditive.localScale = new Vector3(Mathf.Max(width, _redLineAddtiveMinWidth), redLineAdditive.localScale.y, redLineAdditive.localScale.z);
        leftRed.localPosition = new Vector3(width / 2f - _edgeWidth, leftRed.localPosition.y, leftRed.localPosition.z);
        rightRed.localPosition = new Vector3(-(width / 2f - _edgeWidth), rightRed.localPosition.y, rightRed.localPosition.z);
    }
    
    #if UNITY_EDITOR
    [Button]
    public void DrawUnderLine(float width)
    {
        SetUnderLine(width);
    }
    [Button]
    public void DrawRedLine(float width)
    {
        SetRedLine(width);
    }
    #endif
}
