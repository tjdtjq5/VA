using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

[RequireComponent(typeof(ScrollRect))]
public class UIBaseScrollView : UIFrame
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
    public ScrollRect ScrollRect => _scrollRect;
    public int DataCount => DataList.Count;
    public int CardCount => CardList.Count;
    public UICard GetCard(int index) => CardList[index];
    public ICardData GetData(int index) => DataList[index];

    [SerializeField] private bool isPlayAni;

    protected float GetSpacingX() => _gridLayoutGroup.spacing.x;
    protected float GetSpacingY() => _gridLayoutGroup.spacing.y;
    protected int GetColumnOrRowCount() => _gridLayoutGroup.constraintCount;

    protected ScrollRect _scrollRect;
    protected Scrollbar _bar;
    protected GridLayoutGroup _gridLayoutGroup;
    protected Image _scrollViewImg;

    protected List<ICardData> DataList = new();
    protected readonly List<UICard> CardList = new List<UICard>();
    protected UIScrollViewLayoutStartAxis _axis;
    protected float _cardWidth;
    protected float _cardHeight;

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
    protected IEnumerator _playAniCoroutine;

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

        _gridLayoutGroup = _scrollRect.content.GetOrAddComponent<GridLayoutGroup>();
    }

    public void UISet(UIScrollViewLayoutStartAxis axis, string cardName, List<ICardData> dataList)
    {
        _axis = axis;

        ScrollRect.horizontal = axis == UIScrollViewLayoutStartAxis.Horizontal;
        ScrollRect.vertical = axis == UIScrollViewLayoutStartAxis.Vertical;
        DataList = dataList;

        CardSet(cardName, dataList);
        SetContentsSize(0);

        if (isPlayAni)
        {
            PlayAniCr();
        }
    }
    private void CardSet(string cardName, List<ICardData> dataList)
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < dataList.Count; i++)
        {
            UICard card = null;
            if (CardList.Count > i)
            {
                card = CardList[i];
                card.gameObject.SetActive(true);
            }
            else
            {
                UICard cardGo = Managers.Resources.Instantiate<UICard>($"Prefab/UI/Card/{cardName}", _scrollRect.content);
                card = cardGo;
                CardList.Add(cardGo);
            }

            card.Setting(dataList[i]);
        }

        if (CardList.Count != 0)
        {
            _cardWidth = CardList[0].RectTransform.sizeDelta.x + GetSpacingX();
            _cardHeight = CardList[0].RectTransform.sizeDelta.y + GetSpacingY();
            _gridLayoutGroup.cellSize = new Vector2(CardList[0].RectTransform.sizeDelta.x, CardList[0].RectTransform.sizeDelta.y);
        }
    }
    public void SetContentsSize(int selectIndex)
    {
        bool isRemain = DataList.Count % GetColumnOrRowCount() != 0;

        switch (_axis)
        {
            case UIScrollViewLayoutStartAxis.Vertical:
                int hfloor = DataList.Count / GetColumnOrRowCount() + (isRemain ? 1 : 0);
                float h = (hfloor * _cardHeight) + GetSpacingY() * 2;
                _scrollRect.content.sizeDelta = new Vector2(0, h);

                float contentY = _scrollRect.content.sizeDelta.y;
                float posY = ((float)selectIndex / GetColumnOrRowCount()) * _cardHeight + (_cardHeight / 2);

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
                int wfloor = DataList.Count / GetColumnOrRowCount() + (isRemain ? 1 : 0);
                float w = (wfloor * _cardWidth) + GetSpacingX() * 2;
                _scrollRect.content.sizeDelta = new Vector2(w, 0);

                float contentX = _scrollRect.content.sizeDelta.x;
                float posX = ((float)selectIndex / GetColumnOrRowCount()) * _cardWidth + (_cardWidth / 2);

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
    public void PlayAniCr()
    {
        if (_playAniCoroutine != null)
            StopCoroutine(_playAniCoroutine);
        _playAniCoroutine = PlayAniCoroutine();
        StartCoroutine(_playAniCoroutine);
    }
    private IEnumerator PlayAniCoroutine()
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].gameObject.SetActive(true);
            CardList[i].Play();
            yield return new WaitForSeconds(0.02f);
        }
    }
    public void PlayAniAll()
    {
        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].Play();
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
