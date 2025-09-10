using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Sirenix.OdinInspector;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollView : UIFrame
{
    
    public Scrollbar Scrollbar
    {
        get
        {
            switch (_axis)
            {
                case UIScrollViewLayoutStartAxis.Vertical:
                    return _scrollRect.verticalScrollbar;
                case UIScrollViewLayoutStartAxis.Horizontal:
                    return _scrollRect.horizontalScrollbar;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public int DataCount => DataList.Count;
    public int CardCount => CardList.Count;
    public UICard GetCard(int index) => CardList[index];
    public ICardData GetData(int index) => DataList[index];

    [SerializeField] private bool isPlayAni;
    
    protected UICard _cardPrepab;
    protected UIScrollViewLayoutStartAxis _axis;
    protected float _cardWidth;
    protected float _cardHeight;
    protected int _columnOrRowCount = 1;
    protected Vector2 _cardPivot = new Vector2(0, 1);
    protected float _startCornerValue = 0;
    protected float _currentValue = 0;
    protected IEnumerator _playAniCoroutine;
    protected float _paddingX;
    protected float _paddingY;

    // key : pos, value : idx
    private readonly Dictionary<Vector2, int> _idxDics = new Dictionary<Vector2, int>();

    protected ScrollRect _scrollRect;
    protected Scrollbar _bar;
    protected List<ICardData> DataList = new();
    protected readonly List<UICard> CardList = new List<UICard>();

    protected Image _scrollViewImg;

    protected Scrollbar _scrollbarHorizontal;
    protected RectTransform _scrollbarHorizontalRectTr;
    protected Image _scrollbarHorizontalImage;
    protected Vector2 _scrollbarHorizontalOriginDeltaSize;
    protected Vector2 _scrollbarHorizontalHandleRectDeltaSize;
    protected Image _scrollbarHorizontalHandleRectImage;
    protected RectTransform _scrollbarHorizontalSlidingAreaRectTr;
    protected Vector2 _scrollbarHorizontalSlidingAreaDeltaSize;

    protected Scrollbar _scrollbarVertical;
    protected RectTransform _scrollbarVerticalRectTr;
    protected Image _scrollbarVerticalImage;
    protected Vector2 _scrollbarVerticalOriginDeltaSize;
    protected Vector2 _scrollbarVerticalHandleRectDeltaSize;
    protected Image _scrollbarVerticalHandleRectImage;
    protected RectTransform _scrollbarVerticalSlidingAreaRectTr;
    protected Vector2 _scrollbarVerticalSlidingAreaDeltaSize;

    protected float _offset;

    public float GetContentsSelect(int selectIndex)
    {
        bool isRemain = DataList.Count % _columnOrRowCount != 0;

        switch (_axis)
        {
            case UIScrollViewLayoutStartAxis.Vertical:

                float contentY = _scrollRect.content.sizeDelta.y;
                float posY = ((float)selectIndex / _columnOrRowCount) * _cardHeight + (_cardHeight / 2);

                float valueY = 0;
                float scrollviewSizeY = _scrollRect.GetComponent<RectTransform>().rect.height;
                float scrollviewSizeYHarf = scrollviewSizeY / 2;
                float contentSizeYMinusHarf = contentY - scrollviewSizeYHarf;

                if (posY < scrollviewSizeYHarf)
                {
                    valueY = 0;
                }
                else if (posY > contentSizeYMinusHarf)
                {
                    valueY = 1;
                }
                else
                {
                    contentY = contentY - scrollviewSizeY;
                    posY = posY - scrollviewSizeYHarf;

                    valueY = posY / contentY;
                }

                return 1 - valueY;

                break;
            case UIScrollViewLayoutStartAxis.Horizontal:
                
                float contentX = _scrollRect.content.sizeDelta.x;
                float posX = ((float)selectIndex / _columnOrRowCount) * _cardWidth + (_cardWidth / 2);

                float valueX = 0;
                float scrollviewSizeX = _scrollRect.GetComponent<RectTransform>().rect.width;
                float scrollviewSizeXHarf = scrollviewSizeX / 2;
                float contentSizeXMinusHarf = contentX - scrollviewSizeXHarf;

                if (posX < scrollviewSizeXHarf)
                {
                    valueX = 0;
                }
                else if (posX > contentSizeXMinusHarf)
                {
                    valueX = 1;
                }
                else
                {
                    contentX = contentX - scrollviewSizeX;
                    posX = posX - scrollviewSizeXHarf;

                    valueX = posX / contentX;
                }

                return valueX;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
    public void ScrollToChild(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        var content = _scrollRect.content;
        var viewport = _scrollRect.viewport;

        float normalized = 0f;

        if (_axis == UIScrollViewLayoutStartAxis.Vertical)
        {
            // 수직
            Vector2 localPosition = content.InverseTransformPoint(target.position);

            float contentHeight = content.rect.height;
            float viewportHeight = viewport.rect.height;
            float targetYInContent = localPosition.y + contentHeight * content.pivot.y;

            float scrollRange = contentHeight - viewportHeight;
            if (scrollRange <= 0)
            {
                _scrollRect.verticalNormalizedPosition = 1;
                return;
            }

            // 중앙 정렬
            normalized = (targetYInContent - viewportHeight * 0.5f) / scrollRange;
            normalized = Mathf.Clamp01(normalized);

            _scrollRect.verticalNormalizedPosition = normalized;
        }
        else
        {
            // 수평
            Vector2 localPosition = content.InverseTransformPoint(target.position);

            float contentWidth = content.rect.width;
            float viewportWidth = viewport.rect.width;
            float targetXInContent = -localPosition.x + contentWidth * content.pivot.x;

            float scrollRange = contentWidth - viewportWidth;
            if (scrollRange <= 0)
            {
                _scrollRect.horizontalNormalizedPosition = 0;
                return;
            }

            // 중앙 정렬
            normalized = (targetXInContent - viewportWidth * 0.5f) / scrollRange;
            normalized = Mathf.Clamp01(normalized);

            _scrollRect.horizontalNormalizedPosition = normalized;
        }
    }

    protected void SetIdx(Vector2 pos, int idx)
    {
        if (_idxDics.ContainsKey(pos))
        {
            _idxDics[pos] = idx;
        }
        else
        {
            _idxDics.Add(pos, idx);
        }
    }
    protected int GetIdx(Vector2 pos)
    {
        if (_idxDics.TryGetValue(pos, out int idx))
        {
            return idx;
        }
        else
        {
            return -1;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        _scrollRect = UnityHelper.GetOrAddComponent<ScrollRect>(this.gameObject);
        _scrollRect.movementType = ScrollRect.MovementType.Clamped;

        _scrollViewImg = GetComponent<Image>();

        // Vertical
        _scrollRect.content.anchorMax = new Vector2(1, 1);
        _scrollRect.content.anchorMin = new Vector2(0, 1);

        _scrollbarHorizontal = _scrollRect.horizontalScrollbar;
        if (_scrollbarHorizontal != null)
        {
            _scrollbarHorizontalRectTr = _scrollRect.horizontalScrollbar.GetComponent<RectTransform>();
            _scrollbarHorizontalImage = _scrollRect.horizontalScrollbar.GetComponent<Image>();
            _scrollbarHorizontalOriginDeltaSize = _scrollbarHorizontalRectTr.sizeDelta;
            _scrollbarHorizontalHandleRectDeltaSize = _scrollbarHorizontal.handleRect.sizeDelta;
            _scrollbarHorizontalHandleRectImage = _scrollbarHorizontal.handleRect.GetComponent<Image>();
            _scrollbarHorizontalSlidingAreaRectTr = _scrollbarHorizontal.transform.GetChild(0).GetComponent<RectTransform>();
            _scrollbarHorizontalSlidingAreaDeltaSize = _scrollbarHorizontalSlidingAreaRectTr.sizeDelta;
        }

        _scrollbarVertical = _scrollRect.verticalScrollbar;
        if (_scrollbarVertical != null)
        {
            _scrollbarVerticalRectTr = _scrollRect.verticalScrollbar.GetComponent<RectTransform>();
            _scrollbarVerticalImage = _scrollRect.verticalScrollbar.GetComponent<Image>();
            _scrollbarVerticalOriginDeltaSize = _scrollbarVerticalRectTr.sizeDelta;
            _scrollbarVerticalHandleRectDeltaSize = _scrollbarVertical.handleRect.sizeDelta;
            _scrollbarVerticalHandleRectImage = _scrollbarVertical.handleRect.GetComponent<Image>();
            _scrollbarVerticalSlidingAreaRectTr = _scrollbarVertical.transform.GetChild(0).GetComponent<RectTransform>();
            _scrollbarVerticalSlidingAreaDeltaSize = _scrollbarVerticalSlidingAreaRectTr.sizeDelta;
        }
    }
    protected void CardSet(string cardName, int columnOrRowCount = 1, float spacingX = 0, float spacingY = 0)
    {
        if (_cardPrepab == null)
        {
            _cardPrepab = Managers.Resources.Load<UICard>($"Prefab/UI/Card/{cardName}");
        }
        else
        {
            if (!_cardPrepab.name.Equals(cardName))
            {
                _cardPrepab = Managers.Resources.Load<UICard>($"Prefab/UI/Card/{cardName}");
            }
        }

        if (_cardPrepab == null)
        {
            UnityHelper.Error_H($"UIScrollView CardPrefab Null Load\ncardName : {cardName}");
            return;
        }

        _cardWidth = _cardPrepab.RectTransform.sizeDelta.x + spacingX;
        _cardHeight = _cardPrepab.RectTransform.sizeDelta.y + spacingY;

        _columnOrRowCount = columnOrRowCount;
    }
    public virtual void UISet(UIScrollViewLayoutStartAxis axis, string cardName, List<ICardData> dataList, int selectIndex = 0, int columnCount = 1, UIScrollViewLayoutStartCorner corner = UIScrollViewLayoutStartCorner.Middle, float spacingX = 0, float spacingY = 0, float paddingX = 0, float paddingY = 0)
    {
        CardSet(cardName, columnCount, spacingX, spacingY);
        if (!_cardPrepab)
        {
            UnityHelper.Error_H($"UIScrollView View Card Prepab Null Error");
            return;
        }

        _paddingX = paddingX;
        _paddingY = paddingY;

        
        _scrollRect.horizontal = axis == UIScrollViewLayoutStartAxis.Horizontal;
        _scrollRect.vertical = axis == UIScrollViewLayoutStartAxis.Vertical;
        _bar = axis == UIScrollViewLayoutStartAxis.Vertical ? _scrollbarVertical : _scrollbarHorizontal;
        
        DataList = dataList;

        _axis = axis;
        
        SetStartCornerValue(corner, _scrollRect.content, _cardPrepab.RectTransform.sizeDelta.x, columnCount, spacingX, spacingY);
        SetAllIdx(DataList, columnCount);

        SetContentsSize(selectIndex);
        Create(cardName);
    }
    protected void Create(string cardName)
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].gameObject.SetActive(false);
        }

        int cardCount = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? ((int)(RectTransform.rect.height / _cardHeight) + 4) * _columnOrRowCount : ((int)(RectTransform.rect.width / _cardWidth) + 4) * _columnOrRowCount;
        cardCount = Math.Min(DataList.Count, cardCount);

        for (int i = 0; i < cardCount; i++)
        {
            UICard card = null;
            if (CardList.Count > i)
            {
                card = CardList[i];
                card.gameObject.SetActive(true);
            }
            else
            {
                GameObject cardGo = Managers.Resources.Instantiate($"Prefab/UI/Card/{cardName}", _scrollRect.content);
                card = UnityHelper.GetOrAddComponent<UICard>(cardGo);
                CardList.Add(card);
            }

            card.RectTransform.pivot = _cardPivot;

            int hfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i / _columnOrRowCount : i % _columnOrRowCount;
            int wfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i % _columnOrRowCount : i / _columnOrRowCount;
            Vector2 pos = new Vector2(wfloor * _cardWidth + _startCornerValue + _paddingX, hfloor * -_cardHeight - _paddingY);
            card.transform.localPosition = new Vector3(pos.x, pos.y);

            SetData(card, i);
        }

        _offset = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? (CardList.Count / _columnOrRowCount) * _cardHeight : (CardList.Count / _columnOrRowCount) * _cardWidth;

        if (isPlayAni)
        {
            if (_playAniCoroutine != null)
                StopCoroutine(_playAniCoroutine);
            _playAniCoroutine = PlayAniCoroutine();
            StartCoroutine(_playAniCoroutine);
        }
    }
    protected void SetAllIdx(List<ICardData> dataList, int columnCount = 1)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            int hfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i / _columnOrRowCount : i % _columnOrRowCount;
            int wfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i % _columnOrRowCount : i / _columnOrRowCount;
            Vector2 pos = new Vector2(wfloor * _cardWidth + _startCornerValue + _paddingX, hfloor * -_cardHeight - _paddingY);
            SetIdx(pos, i);
        }
    }
    public void SetContentsSize(int selectIndex)
    {
        bool isRemain = DataList.Count % _columnOrRowCount != 0;

        switch (_axis)
        {
            case UIScrollViewLayoutStartAxis.Vertical:
                int hfloor = DataList.Count / _columnOrRowCount + (isRemain ? 1 : 0);
                float h = (hfloor * _cardHeight) + _paddingY * 2;
                _scrollRect.content.sizeDelta = new Vector2(0, h);

                float contentY = _scrollRect.content.sizeDelta.y;
                float posY = ((float)selectIndex / _columnOrRowCount) * _cardHeight + (_cardHeight / 2);

                float valueY = 0;
                float scrollviewSizeY = _scrollRect.GetComponent<RectTransform>().rect.height;
                float scrollviewSizeYHarf = scrollviewSizeY / 2;
                float contentSizeYMinusHarf = contentY - scrollviewSizeYHarf;

                if (posY < scrollviewSizeYHarf)
                {
                    valueY = 1;
                }
                else if (posY > contentSizeYMinusHarf)
                {
                    valueY = 0;
                }
                else
                {
                    contentY = contentY - scrollviewSizeY;
                    posY = posY - scrollviewSizeYHarf;

                    valueY = 1 - posY / contentY;
                }

                _scrollRect.verticalScrollbar.value = valueY;

                break;
            case UIScrollViewLayoutStartAxis.Horizontal:
                int wfloor = DataList.Count / _columnOrRowCount + (isRemain ? 1 : 0);
                float w = (wfloor * _cardWidth) + _paddingX * 2;
                _scrollRect.content.sizeDelta = new Vector2(w, 0);

                float contentX = _scrollRect.content.sizeDelta.x;
                float posX = ((float)selectIndex / _columnOrRowCount) * _cardWidth + (_cardWidth / 2);

                float valueX = 0;
                float scrollviewSizeX = _scrollRect.GetComponent<RectTransform>().rect.width;
                float scrollviewSizeXHarf = scrollviewSizeX / 2;
                float contentSizeXMinusHarf = contentX - scrollviewSizeXHarf;

                if (posX < scrollviewSizeXHarf)
                {
                    valueX = 0;
                }
                else if (posX > contentSizeXMinusHarf)
                {
                    valueX = 1;
                }
                else
                {
                    contentX = contentX - scrollviewSizeX;
                    posX = posX - scrollviewSizeXHarf;

                    valueX = posX / contentX;
                }

                _scrollRect.horizontalScrollbar.value = valueX;
                break;
        }
    }
    protected bool RocateItem(UICard _card, float _contentsXY, float _scrollWH)
    {
        if (_card == null)
            return false;

        float itemPos = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? _card.transform.localPosition.y + _contentsXY : _card.transform.localPosition.x + _contentsXY;

        switch (_axis)
        {
            case UIScrollViewLayoutStartAxis.Vertical:
                if (itemPos > _cardHeight * 2)
                {
                    _card.transform.localPosition -= new Vector3(0, _offset, 0);
                    RocateItem(_card, _contentsXY, _scrollWH);
                    return true;
                }
                else if (itemPos < -_scrollWH - (_cardHeight * 2))
                {
                    _card.transform.localPosition += new Vector3(0, _offset, 0);
                    RocateItem(_card, _contentsXY, _scrollWH);
                    return true;
                }
                return false;
            case UIScrollViewLayoutStartAxis.Horizontal:
                if (itemPos > (_scrollWH / 2) + (_cardWidth))
                {
                    _card.transform.localPosition -= new Vector3(_offset, 0, 0);
                    RocateItem(_card, _contentsXY, _scrollWH);
                    return true;
                }
                else if (itemPos < -(_scrollWH / 2) - (_cardWidth * 2))
                {
                    _card.transform.localPosition += new Vector3(_offset, 0, 0);
                    RocateItem(_card, _contentsXY, _scrollWH);
                    return true;
                }
                return false;
            default:
                return false;
        }
    }
    protected void SetData(UICard _scrollItem, int _dataIndex)
    {
        if (_dataIndex < 0 || _dataIndex >= DataList.Count)
        {
            _scrollItem.gameObject.SetActive(false);
            return;
        }

        _scrollItem.gameObject.SetActive(true);
        _scrollItem.Setting(DataList[_dataIndex]);
    }
    private void Update()
    {
        if (!_bar || _currentValue.Equals(_bar.value))
            return;

        _currentValue = _bar.value;
        float contentsXY = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? _scrollRect.content.anchoredPosition.y : _scrollRect.content.anchoredPosition.x;

        RectTransform scrollRectTr = _scrollRect.GetComponent<RectTransform>();
        float scrollWHeight = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? scrollRectTr.rect.height : scrollRectTr.rect.width;
        float cardLenth = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? _cardHeight : _cardWidth;
        float cardRowCount = scrollWHeight / cardLenth;

        for (int i = 0; i < CardList.Count; i++)
        {
            float inValue = MathF.Abs((_axis == UIScrollViewLayoutStartAxis.Vertical) ? CardList[i].transform.localPosition.y : CardList[i].transform.position.x);
            bool isChanged = RocateItem(CardList[i], contentsXY, scrollWHeight);

            inValue = .8f - (contentsXY - inValue) / cardLenth;

            if (inValue > cardRowCount)
            {
                inValue = .8f - (inValue - cardRowCount);
                inValue = Math.Clamp(inValue, 0, 1);
                CardList[i].In(inValue);
            }
            else
            {
                inValue = Math.Clamp(inValue, 0, 1);
                CardList[i].In(inValue);
            }

            if (isChanged)
            {
                int idx = GetIdx(CardList[i].transform.localPosition);
                SetData(CardList[i], idx);
            }
        }
    }

    private IEnumerator PlayAniCoroutine()
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].Fade(0);
        }
        
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].Fade(1);
            CardList[i].Play();
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ScrollBarHorizontalActive(bool _flag)
    {
        if (_scrollbarHorizontal == null)
        {
            return;
        }
        
        _scrollbarHorizontal.interactable = _flag;
        _scrollbarHorizontalImage.enabled = _flag;
        _scrollbarHorizontalRectTr.sizeDelta = _flag ? _scrollbarHorizontalOriginDeltaSize : Vector2.zero;
        _scrollbarHorizontal.handleRect.sizeDelta = _flag ? _scrollbarHorizontalHandleRectDeltaSize : Vector2.zero;
        _scrollbarHorizontalHandleRectImage.enabled = _flag;
        _scrollbarHorizontalSlidingAreaRectTr.sizeDelta = _flag ? _scrollbarHorizontalSlidingAreaDeltaSize : Vector2.zero;
    }
    public void ScrollBarVerticalActive(bool _flag)
    {
        if (_scrollbarVertical == null)
        {
            return;
        }

        _scrollbarVertical.interactable = _flag;
        _scrollbarVerticalImage.enabled = _flag;
        _scrollbarVerticalRectTr.sizeDelta = _flag ? _scrollbarVerticalOriginDeltaSize : Vector2.zero;
        _scrollbarVertical.handleRect.sizeDelta = _flag ? _scrollbarVerticalHandleRectDeltaSize : Vector2.zero;
        _scrollbarVerticalHandleRectImage.enabled = _flag;
        _scrollbarVerticalSlidingAreaRectTr.sizeDelta = _flag ? _scrollbarVerticalSlidingAreaDeltaSize : Vector2.zero;
    }

    void SetStartCornerValue(UIScrollViewLayoutStartCorner corner, RectTransform contentRectTr, float cardWidth, int columnCount, float spacingX = 0, float spacingY = 0)
    {
        Vector2 oPivot = _scrollRect.content.pivot;
        float halfCardWidth = cardWidth / 2;
        float contentWidth = contentRectTr.localScale.x;
        float spacing = _axis == UIScrollViewLayoutStartAxis.Horizontal ? spacingY : spacingX;

        switch (corner)
        {
            case UIScrollViewLayoutStartCorner.Left:
                _scrollRect.content.pivot = new Vector2(0, oPivot.y);
                _startCornerValue = -contentWidth / 2;
                break;
            case UIScrollViewLayoutStartCorner.Middle:
                spacing = (columnCount - 1) / 2f * spacing;
                _scrollRect.content.pivot = new Vector2(0.5f, oPivot.y);
                _startCornerValue = -columnCount * halfCardWidth - spacing;
                break;
            case UIScrollViewLayoutStartCorner.Right:
                spacing = (columnCount - 1) * spacing;
                float lastCardPosX = cardWidth * columnCount;
                _scrollRect.content.pivot = new Vector2(1, oPivot.y);
                _startCornerValue = (contentWidth / 2) - lastCardPosX - spacing;
                break;
            default:
                _startCornerValue = 0;
                break;
        }
    }

    [Button]
    public void InspectorSetting()
    {
        Initialize();
        ScrollBarHorizontalActive(false);
        ScrollBarVerticalActive(false);
        _scrollViewImg.color = Color.clear;
        
        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(this.gameObject, true);
        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];
            string n = child.gameObject.name;
            child.gameObject.name = n.Replace(" ", "").Replace("_","").Replace("(Legacy)", "");
        }
        
#if UNITY_EDITOR
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}
