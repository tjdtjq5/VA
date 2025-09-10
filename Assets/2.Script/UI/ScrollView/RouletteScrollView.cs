using System;
using System.Collections;
using System.Collections.Generic;
using AssetKits.ParticleImage;
using Sirenix.OdinInspector;
using UnityEngine;

public class RouletteScrollView : UIScrollView
{
    public bool IsPlay { get; set; }
    public Action OnEnd;
    
    [SerializeField] ParticleImage failParticle;
    [SerializeField] Animator animator;
    
    Tween<float> _tween;
    IEnumerator _playCoroutine;
    private AniController _anicontroller;

    private readonly int _noneHash = Animator.StringToHash("None");
    private readonly int _playHash = Animator.StringToHash("Play");
    
    private readonly int _multipleCount = 5;
    private readonly float _adjustSpeed = 5f;
    private float Speed => (DataCount - _addDataCount) / _adjustSpeed;

    private int _addDataCount;
    private int _dataOriginCount;


    public override void UISet(UIScrollViewLayoutStartAxis axis, string cardName, List<ICardData> dataList, int selectIndex = 0, int columnCount = 1,
        UIScrollViewLayoutStartCorner corner = UIScrollViewLayoutStartCorner.Middle, float spacingX = 0,
        float spacingY = 0, float paddingX = 0, float paddingY = 0)
    {
        this._anicontroller = animator.Initialize();
        this._dataOriginCount = dataList.Count;
        _addDataCount = dataList.Count * _multipleCount;
        
        var tempDataList = new System.Collections.Generic.List<ICardData>();
        tempDataList.AddRange(dataList);

        for (int i = 0; i < _multipleCount; i++)
        {
            for (int j = 0; j < tempDataList.Count; j++)
                dataList.Add(tempDataList[j]);
        }
        
        base.UISet(axis, cardName, dataList, selectIndex, columnCount, corner, spacingX, spacingY, paddingX, paddingY);
    }

    [Button]
    public void Test(int selectIndex)
    {
        if (_tween != null)
            _tween.FullKill();
        
        SetContentsSize(selectIndex);
    }

    public void Roll()
    {
        float start = GetContentsSelect(this.DataCount - _addDataCount + 2);
        float end = GetContentsSelect(2);
        
        if (_tween != null)
            _tween.FullKill();
        
        _tween = Managers.Tween.TweenScrollbar(this.Scrollbar, start, end, Speed).SetEase(EaseType.Linear).SetLoop(-1);
    }

    public void Play(float delay, int selectIndex)
    {
        if (_playCoroutine != null)
            StopCoroutine(_playCoroutine);

        _playCoroutine = PlayCoroutine(delay, selectIndex);
        StartCoroutine(_playCoroutine);
    }

    IEnumerator PlayCoroutine(float delay, int selectIndex)
    {
        IsPlay = true;
        
        _anicontroller.SetTrigger(_playHash);
        
        _tween.FullKill();
        
        float start = GetContentsSelect(this.DataCount - _addDataCount + 2);
        float end = GetContentsSelect(2);
        float duration = Speed / 7.5f;
        float waitTime = 1.5f;
        
        _tween = Managers.Tween.TweenScrollbar(this.Scrollbar, start, end, duration)
            .SetEase(EaseType.Linear)
            .SetLoop(-1);
        
        yield return new WaitForSeconds(waitTime);
        
        _tween.Pause();
        
        start = GetContentsSelect(this.DataCount - _addDataCount + 2 + (_dataOriginCount * _multipleCount / 2));
        end = GetContentsSelect((selectIndex) + (_dataOriginCount * _multipleCount / 2));

        _tween = Managers.Tween.TweenScrollbar(this.Scrollbar, start, end, duration)
            .SetEase(EaseType.Linear);
        
        yield return new WaitForSeconds(duration);
        
        SetContentsSize(selectIndex + _dataOriginCount);
        
        _anicontroller.SetTrigger(_noneHash);
        
        OnEnd?.Invoke();

        yield return new WaitForSeconds(2f);
        
         Roll();
         IsPlay = false;
    }

    public void Fail()
    {
        if (_playCoroutine != null)
            StopCoroutine(_playCoroutine);
        
        _playCoroutine = FailCoroutine();
        StartCoroutine(_playCoroutine);
    }

    IEnumerator FailCoroutine()
    {
        IsPlay = true;
        
        _anicontroller.SetTrigger(_playHash);
        
        _tween.FullKill();
        
        float start = GetContentsSelect(this.DataCount - _addDataCount + 2);
        float end = GetContentsSelect(2);
        float duration = Speed / 7.5f;
        float waitTime = 1f;
        
        _tween = Managers.Tween.TweenScrollbar(this.Scrollbar, start, end, duration)
            .SetEase(EaseType.Linear)
            .SetLoop(-1);
        
        yield return new WaitForSeconds(waitTime);

        for (int i = 0; i < CardList.Count; i++)
            CardList[i].gameObject.SetActive(false);
        
        _anicontroller.SetTrigger(_noneHash);

        failParticle.Play();

        _tween.FullKill();
        
        yield return new WaitForSeconds(.8f);

        for (int i = 0; i < CardList.Count; i++)
            CardList[i].gameObject.SetActive(true);
        
        Roll();
        IsPlay = false;
    }
}
