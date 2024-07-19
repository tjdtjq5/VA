using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollView : UIBase
{
    UICard _cardPrepab;
    UIScrollViewLayoutStartAxis _axis;
    float _cardWidth;
    float _cardHeight;
    int _columnOrRowCount = 1;
    Vector2 _cardPivot = new Vector2(0, 1);
    float _startCornerValue = 0;

    // key : pos, value : idx
    Dictionary<Vector2, int> idxDics = new Dictionary<Vector2, int>();

    ScrollRect _scrollRect;
    List<ICardData> _dataList = new List<ICardData>();
    List<UICard> _cardList = new List<UICard>();

    Scrollbar _scrollbarHorizontal;
    RectTransform _scrollbarHorizontalRectTr;
    Image _scrollbarHorizontalImage;
    Vector2 _scrollbarHorizontalOriginDeltaSize;
    Vector2 _scrollbarHorizontalHandleRectDeltaSize;
    Image _scrollbarHorizontalHandleRectImage;
    RectTransform _scrollbarHorizontalSlidingAreaRectTr;
    Vector2 _scrollbarHorizontalSlidingAreaDeltaSize;

    Scrollbar _scrollbarVertical;
    RectTransform _scrollbarVerticalRectTr;
    Image _scrollbarVerticalImage;
    Vector2 _scrollbarVerticalOriginDeltaSize;
    Vector2 _scrollbarVerticalHandleRectDeltaSize;
    Image _scrollbarVerticalHandleRectImage;
    RectTransform _scrollbarVerticalSlidingAreaRectTr;
    Vector2 _scrollbarVerticalSlidingAreaDeltaSize;

    float _offset;

    void SetIdx(Vector2 pos, int idx)
    {
        if (idxDics.ContainsKey(pos))
        {
            idxDics[pos] = idx;
        }
        else
        {
            idxDics.Add(pos, idx);
        }
    }
    int GetIdx(Vector2 pos)
    {
        if (idxDics.TryGetValue(pos, out int idx))
        {
            return idx;
        }
        else
        {
            return -1;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        _scrollRect = UnityHelper.GetOrAddComponent<ScrollRect>(this.gameObject);

        _scrollRect.content.anchorMax = new Vector2(0.5f, 1);
        _scrollRect.content.anchorMin = new Vector2(0.5f, 1);

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
    void CardSetting(string cardName, int columnOrRowCount = 1, float spacingX = 0, float spacingY = 0)
    {
        if (_cardPrepab == null)
        {
            _cardPrepab = Managers.Resources.Load<UICard>($"UI/Card/{cardName}");
        }
        else
        {
            if (!_cardPrepab.name.Equals(cardName))
            {
                _cardPrepab = Managers.Resources.Load<UICard>($"UI/Card/{cardName}");
            }
        }

        if (_cardPrepab == null)
        {
            UnityHelper.LogError_H($"UIScrollView _cardPrepab Null Load\ncardName : {cardName}");
            return;
        }

        _cardWidth = _cardPrepab.RectTransform.sizeDelta.x + spacingX;
        _cardHeight = _cardPrepab.RectTransform.sizeDelta.y + spacingY;

        _columnOrRowCount = columnOrRowCount;
    }
    public void View(UIScrollViewLayoutStartAxis axis, string cardName, List<ICardData> dataList, int selectIndex = 0, int columnCount = 1, UIScrollViewLayoutStartCorner corner = UIScrollViewLayoutStartCorner.Middle, float spacingX = 0, float spacingY = 0)
    {
        CardSetting(cardName, columnCount, spacingX, spacingY);

        if (_cardPrepab == null)
        {
            UnityHelper.LogError_H($"UIScrollView View Card Prepab Null Error");
            return;
        }

        _dataList = dataList;

        _axis = axis;

        SetStartCornerValue(corner, _scrollRect.content, _cardPrepab.RectTransform.sizeDelta.x, columnCount);
        SetAllIdx(_dataList, columnCount);

        SetContentsSize(selectIndex);
        Create();
    }
    public void Create()
    {
        for (int i = 0; i < _cardList.Count; i++)
        {
            Managers.Resources.Destroy(_cardList[i].gameObject);
        }
        _cardList.Clear();

        int cardCount = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? ((int)(RectTransform.rect.height / _cardHeight) + 2) * _columnOrRowCount : ((int)(RectTransform.rect.width / _cardWidth) + 2) * _columnOrRowCount;
        cardCount = Math.Min(_dataList.Count, cardCount);

        for (int i = 0; i < cardCount; i++)
        {
            GameObject cardGo = Managers.Resources.Instantiate($"UI/Card/{_cardPrepab.name}", _scrollRect.content);
            UICard card = UnityHelper.GetOrAddComponent<UICard>(cardGo);

            card.RectTransform.pivot = _cardPivot;

            _cardList.Add(card);

            int hfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i / _columnOrRowCount : i % _columnOrRowCount;
            int wfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i % _columnOrRowCount : i / _columnOrRowCount;
            Vector2 pos = new Vector2(wfloor * _cardWidth + _startCornerValue, hfloor * -_cardHeight);
            card.transform.localPosition = new Vector3(pos.x, pos.y);

            SetData(card, i);
        }

        _offset = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? (_cardList.Count / _columnOrRowCount) * _cardHeight : (_cardList.Count / _columnOrRowCount) * _cardWidth;
    }
    void SetAllIdx(List<ICardData> dataList, int columnCount = 1)
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            int hfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i / _columnOrRowCount : i % _columnOrRowCount;
            int wfloor = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? i % _columnOrRowCount : i / _columnOrRowCount;
            Vector2 pos = new Vector2(wfloor * _cardWidth + _startCornerValue, hfloor * -_cardHeight);
            SetIdx(pos, i);
        }
    }
    void SetContentsSize(int selectIndex)
    {
        bool isRemain = _dataList.Count % _columnOrRowCount != 0;

        switch (_axis)
        {
            case UIScrollViewLayoutStartAxis.Vertical:
                int hfloor = _dataList.Count / _columnOrRowCount + (isRemain ? 1 : 0);
                float h = (hfloor * _cardHeight);
                _scrollRect.content.sizeDelta = new Vector2(_cardWidth * _columnOrRowCount, h);

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
                int wfloor = _dataList.Count / _columnOrRowCount + (isRemain ? 1 : 0);
                float w = wfloor * _cardWidth;
                _scrollRect.content.sizeDelta = new Vector2(w, _cardHeight * _columnOrRowCount);

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
    bool RocateItem(UICard _card, float _contentsXY, float _scrollWH)
    {
        if (_card == null)
            return false;

        float itemPos = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? _card.transform.localPosition.y + _contentsXY : _card.transform.localPosition.x + _contentsXY;

        switch (_axis)
        {
            case UIScrollViewLayoutStartAxis.Vertical:
                if (itemPos > _cardHeight)
                {
                    _card.transform.localPosition -= new Vector3(0, _offset, 0);
                    RocateItem(_card, _contentsXY, _scrollWH);
                    return true;
                }
                else if (itemPos < -_scrollWH - (_cardHeight))
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
    void SetData(UICard _scrollItem, int _dataIndex)
    {
        if (_dataIndex < 0 || _dataIndex >= _dataList.Count)
        {
            _scrollItem.gameObject.SetActive(false);
            return;
        }

        _scrollItem.gameObject.SetActive(true);
        _scrollItem.Setting(_dataList[_dataIndex]);
    }
    private void Update()
    {
        float contentsXY = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? _scrollRect.content.anchoredPosition.y : _scrollRect.content.anchoredPosition.x;

        RectTransform scrollRectTr = _scrollRect.GetComponent<RectTransform>();
        float scrollWHeight = (_axis == UIScrollViewLayoutStartAxis.Vertical) ? scrollRectTr.rect.height : scrollRectTr.rect.width;

        for (int i = 0; i < _cardList.Count; i++)
        {
            bool isChanged = RocateItem(_cardList[i], contentsXY, scrollWHeight);

            if (isChanged)
            {
                int idx = GetIdx(_cardList[i].transform.localPosition);
                SetData(_cardList[i], idx);
            }
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

    void SetStartCornerValue(UIScrollViewLayoutStartCorner corner, RectTransform contentRectTr, float cardWidth, int columnCount)
    {
        Vector2 oPivot = _scrollRect.content.pivot;
        float halfCardWidth = cardWidth / 2;
        float contentWidth = contentRectTr.localScale.x;

        switch (corner)
        {
            case UIScrollViewLayoutStartCorner.Left:
                _scrollRect.content.pivot = new Vector2(0, oPivot.y);
                _startCornerValue = -contentWidth / 2;
                break;
            case UIScrollViewLayoutStartCorner.Middle:
                _scrollRect.content.pivot = new Vector2(0.5f, oPivot.y);
                _startCornerValue = -columnCount * halfCardWidth;
                break;
            case UIScrollViewLayoutStartCorner.Right:
                float lastCardPosX = cardWidth * columnCount;
                _scrollRect.content.pivot = new Vector2(1, oPivot.y);
                _startCornerValue = (contentWidth / 2) - lastCardPosX;
                break;
            default:
                _startCornerValue = 0;
                break;
        }
    }
}
