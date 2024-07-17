using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollView : UIBase
{
    UICard _cardPrepab;
    float _cardWidth;
    float _cardHeight;
    int _columnCount = 1;
    Vector2 _cardPivot = new Vector2(0, 1);

    ScrollRect _scrollRect;
    List<ICardData> _dataList = new List<ICardData>();
    List<UICard> _cardList = new List<UICard>();

    float _offsetHeight;
    int _selectIndex;
    bool _isView = false;
    bool _isCreate = false;

    public override void Initialize()
    {
        base.Initialize();

        _scrollRect = UnityHelper.GetOrAddComponent<ScrollRect>(this.gameObject);
    }
    public virtual void Setting(string cardName, int columnCount = 1)
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

        _cardWidth = _cardPrepab.RectTransform.sizeDelta.x;
        _cardHeight = _cardPrepab.RectTransform.sizeDelta.y;

        _columnCount = columnCount;
        _isView = false;
    }
    public virtual void View(List<ICardData> dataList, int selectIndex = 0)
    {
        if (_cardPrepab == null)
        {
            UnityHelper.LogError_H($"UIScrollView View Card Prepab Null Error");
            return;
        }

        _dataList = dataList;

        _selectIndex = selectIndex;
        _isView = true;

        SetContentsSize();
        Create();
    }
    public virtual void Create()
    {
        if (!_isView)
        {
            return;
        }

        _cardList.Clear();

        int cardCount = ((int)(RectTransform.rect.height / _cardHeight) + (_dataList.Count / _columnCount)) * _columnCount;

        if (!_isCreate)
        {
            for (int i = 0; i < cardCount; i++)
            {
                GameObject cardGo = Managers.Resources.Instantiate($"UI/Card/{_cardPrepab.name}", _scrollRect.content);
                UICard card = UnityHelper.GetOrAddComponent<UICard>(cardGo);

                card.RectTransform.pivot = _cardPivot;

                _cardList.Add(card);

                int hfloor = i / _columnCount;
                int wfloor = i % _columnCount;
                cardGo.transform.localPosition = new Vector3(wfloor * _cardWidth, hfloor * -_cardHeight);

                SetData(card, i);
            }
            _isCreate = true;
        }
        else
        {
            for (int i = 0; i < _scrollRect.content.childCount; i++)
            {
                UICard card = _scrollRect.content.GetChild(i).GetComponent<UICard>();
                _cardList.Add(card);

                int hfloor = i / _columnCount;
                int wfloor = i % _columnCount;
                card.transform.localPosition = new Vector3(wfloor * _cardWidth, hfloor * -_cardHeight);

                SetData(card, i);
            }
        }

        _offsetHeight = (_cardList.Count / _columnCount + 1) * _cardHeight;
    }
    void SetContentsSize()
    {
        bool isRemain = _dataList.Count % _columnCount != 0;
        int hfloor = _dataList.Count / _columnCount + (isRemain ? 1 : 0);
        _scrollRect.content.sizeDelta = new Vector2(_columnCount * _cardWidth, (hfloor * _cardHeight) + 50);
    }
    bool RocateItem(UICard _card, float _contentsY, float _scrollHeight)
    {
        float itemPos = _card.transform.localPosition.y + _contentsY;

        if (itemPos > _cardHeight * 2)
        {
            _card.transform.localPosition -= new Vector3(0, _offsetHeight, 0);
            return true;
        }
        else if (itemPos < -_scrollHeight - (_cardHeight * 2))
        {
            _card.transform.localPosition += new Vector3(0, _offsetHeight, 0);
            return true;
        }
        return false;
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
        float contentsY = _scrollRect.content.anchoredPosition.y;

        RectTransform scrollRectTr = _scrollRect.GetComponent<RectTransform>();
        float scrollHeight = scrollRectTr.rect.height;

        foreach (var card in _cardList)
        {
            bool isChanged = RocateItem(card, contentsY, scrollHeight);

            if (isChanged)
            {
                int hfloor = (int)(-card.transform.localPosition.y / _cardHeight);
                int wfloor = (int)(card.transform.localPosition.x / _cardWidth);
                int idx = hfloor * _columnCount + wfloor;
                SetData(card, idx);
            }
        }
    }
}
