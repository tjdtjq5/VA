using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Sirenix.OdinInspector;

[RequireComponent(typeof(ScrollRect))]
public class UIScrollViewWithAdd : UIFrame
{
    #region Publics
    public Scrollbar Scrollbar
    {
        get
        {
            switch (_axis)
            {
                case UIScrollViewLayoutStartAxis.Vertical:   return _scrollRect.verticalScrollbar;
                case UIScrollViewLayoutStartAxis.Horizontal: return _scrollRect.horizontalScrollbar;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }

    public int DataCount => DataList.Count;
    public int CardCount => CardList.Count;
    public UICard GetCard(int index) => CardList[index];
    public ICardData GetData(int index) => DataList[index];
    private bool IsVertical => _axis == UIScrollViewLayoutStartAxis.Vertical;

    private (float viewStart, float viewEnd, float primarySize) GetViewWindow()
    {
        var rect = _scrollRect.GetComponent<RectTransform>().rect;
        if (IsVertical)
        {
            float start = _scrollRect.content.anchoredPosition.y; // distance scrolled down from top
            float size  = rect.height;
            return (start, start + size, size);
        }
        else
        {
            float start = -_scrollRect.content.anchoredPosition.x; // positive to the right
            float size  = rect.width;
            return (start, start + size, size);
        }
    }
    #endregion

    #region Serialized
    [SerializeField] private bool isPlayAni;
    #endregion

    #region Internal Models
    private class Section
    {
        public bool isSeparator;
        public string prefabName;
        public List<ICardData> dataList = new();
        public int columnCount = 1;   // Vertical: columns, Horizontal: rows
        public float spacingX = 0;
        public float spacingY = 0;
        public float separatorHeight = 0;
        public Vector2 cardSize;

        // runtime
        public GameObject separatorGO;

        // virtualization cache
        public float cw, ch;          // cell width/height (including spacing)
        public float startX, startY;  // cross-axis start offset (X or Y)
        public float yStart;          // Vertical primary start (from top)
        public float xStart;          // Horizontal primary start (from left)
        public float height;          // Vertical section height
        public float width;           // Horizontal section width
        public int totalRows;         // Vertical: total rows
        public int totalCols;         // Horizontal: total columns
    }
    #endregion

    #region Fields (original/adjusted)
    protected UICard _cardPrepab;
    protected UIScrollViewLayoutStartAxis _axis;
    protected float _cardWidth;
    protected float _cardHeight;
    protected int _columnOrRowCount = 1;
    protected Vector2 _cardPivot = new Vector2(0.5f, 0.5f);
    protected float _startCornerValue = 0;
    protected IEnumerator _playAniCoroutine;
    protected float _paddingX;
    protected float _paddingY;

    // --- Virtualization containers (optimized to avoid string allocs) ---
    private static long MakeKey(int sectionIndex, int itemIndex) => ((long)sectionIndex << 32) | (uint)itemIndex;
    private readonly Dictionary<long, UICard> _activeByKey = new();
    private readonly List<long> _toRemove = new();
    private readonly HashSet<long> _aliveKeys = new();

    private float _lastAxisPos = float.MinValue;
    private const int VBUF = 0; // buffer rows/cols around viewport

    protected ScrollRect _scrollRect;
    protected List<ICardData> DataList = new();           // single-section (infinite) mode only
    protected readonly List<UICard> CardList = new();

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

    // cache card sizes to avoid repeated Resources.Loads
    private readonly Dictionary<string, Vector2> _cardSizeCache = new();
    #endregion

    #region Section/Separator State
    private readonly List<Section> _sections = new();
    private readonly Dictionary<string, List<UICard>> _poolByPrefab = new(); // per-prefab pool
    private bool _useSections = false;   // multi-section layout flag
    private UIScrollViewLayoutStartCorner _corner;
    private string _masterCardPrefab;    // first section card prefab
    private int _masterColumnCount;
    private float _masterSpacingX, _masterSpacingY;
    #endregion

    #region Initialize / Lifecycle
    protected override void Initialize()
    {
        base.Initialize();

        _scrollRect = UnityHelper.GetOrAddComponent<ScrollRect>(this.gameObject);
        _scrollRect.movementType = ScrollRect.MovementType.Clamped;

        _scrollViewImg = GetComponent<Image>();

        // lock content anchors (top stretch)
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
            _scrollbarVerticalRectTr = _scrollbarVertical.GetComponent<RectTransform>();
            _scrollbarVerticalImage = _scrollbarVertical.GetComponent<Image>();
            _scrollbarVerticalOriginDeltaSize = _scrollbarVerticalRectTr.sizeDelta;
            _scrollbarVerticalHandleRectDeltaSize = _scrollbarVertical.handleRect.sizeDelta;
            _scrollbarVerticalHandleRectImage = _scrollbarVertical.handleRect.GetComponent<Image>();
            _scrollbarVerticalSlidingAreaRectTr = _scrollbarVertical.transform.GetChild(0).GetComponent<RectTransform>();
            _scrollbarVerticalSlidingAreaDeltaSize = _scrollbarVerticalSlidingAreaRectTr.sizeDelta;
        }
    }

    private void OnEnable()
    {
        if (_scrollRect == null) Initialize();
        // Use event to refresh multi-section viewport instead of per-frame polling.
        _scrollRect.onValueChanged.AddListener(_ =>
        {
            if (_useSections) RefreshVisibleSections();
        });
    }

    private void OnDisable()
    {
        if (_scrollRect != null)
            _scrollRect.onValueChanged.RemoveListener(_ =>
            {
                if (_useSections) RefreshVisibleSections();
            });
    }
    #endregion

    #region Public API (keep original call style)
    // First section setup
    public virtual void UISet(
        UIScrollViewLayoutStartAxis axis,
        string cardName,
        List<ICardData> dataList,
        int selectIndex = 0,
        int columnCount = 1,
        UIScrollViewLayoutStartCorner corner = UIScrollViewLayoutStartCorner.Middle,
        float spacingX = 0, float spacingY = 0,
        float paddingX = 0, float paddingY = 0)
    {
        ClearAll();

        _axis = axis;
        _corner = corner;
        _paddingX = paddingX;
        _paddingY = paddingY;
        _masterCardPrefab = cardName;
        _masterColumnCount = columnCount;
        _masterSpacingX = spacingX;
        _masterSpacingY = spacingY;

        // add first section
        var first = new Section
        {
            isSeparator = false,
            prefabName = cardName,
            dataList = dataList ?? new List<ICardData>(),
            columnCount = columnCount,
            spacingX = spacingX,
            spacingY = spacingY
        };
        first.cardSize = GetCardSize(cardName);
        _sections.Add(first);

        _useSections = true;

        RebuildSections(); // layout multi-sections

        if (isPlayAni)
        {
            if (_playAniCoroutine != null) StopCoroutine(_playAniCoroutine);
            _playAniCoroutine = PlayAniCoroutine();
            StartCoroutine(_playAniCoroutine);
        }
    }

    // Add a visual separator section (prefab + thickness)
    public void AddSeparator(string separatorPrefab, float height)
    {
        if (!_useSections) return;

        _sections.Add(new Section
        {
            isSeparator = true,
            prefabName = separatorPrefab,
            separatorHeight = height
        });

        RebuildSections();
    }

    // Append another data section
    public void AddData(string prefabName, List<ICardData> data, int columnCount, float spacingX, float spacingY)
    {
        if (!_useSections) return;

        var s = new Section
        {
            isSeparator = false,
            prefabName = prefabName,
            dataList = data ?? new List<ICardData>(),
            columnCount = Mathf.Max(1, columnCount),
            spacingX = spacingX,
            spacingY = spacingY,
            cardSize = GetCardSize(prefabName)
        };
        _sections.Add(s);

        RebuildSections();
    }
    #endregion

    #region Pool helpers
    private UICard GetCardFromPool(string prefabName)
    {
        if (!_poolByPrefab.TryGetValue(prefabName, out var list))
        {
            list = new List<UICard>();
            _poolByPrefab[prefabName] = list;
        }

        foreach (var c in list)
        {
            if (!c.gameObject.activeSelf)
            {
                EnsureTopAnchors(c.RectTransform);
                return c;
            }
        }

        // create new
        var go = Managers.Resources.Instantiate($"Prefab/UI/Card/{prefabName}", _scrollRect.content);
        var card = UnityHelper.GetOrAddComponent<UICard>(go);
        EnsureTopAnchors(card.RectTransform);
        list.Add(card);
        CardList.Add(card);
        return card;
    }

    private void DeactivateAllPooled()
    {
        foreach (var kv in _poolByPrefab)
            foreach (var c in kv.Value)
                c.gameObject.SetActive(false);
    }

    private Vector2 GetCardSize(string prefabName)
    {
        if (_cardSizeCache.TryGetValue(prefabName, out var cached) && cached.sqrMagnitude > 0f)
            return cached;

        var p = Managers.Resources.Load<UICard>($"Prefab/UI/Card/{prefabName}");
        if (p != null)
        {
            var sz = p.RectTransform.sizeDelta;
            _cardSizeCache[prefabName] = sz;
            return sz;
        }

        var go = Managers.Resources.Load<GameObject>($"Prefab/UI/Card/{prefabName}");
        if (go != null)
        {
            var rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                _cardSizeCache[prefabName] = rt.sizeDelta;
                return rt.sizeDelta;
            }
        }

        // last resort: spawn 1 pooled card and measure
        var temp = GetCardFromPool(prefabName);
        var size = temp.RectTransform.sizeDelta;
        temp.gameObject.SetActive(false);
        _cardSizeCache[prefabName] = size.sqrMagnitude > 0f ? size : new Vector2(100, 100);
        return _cardSizeCache[prefabName];
    }
    #endregion

    #region Section Layout (multi-section only)
    private void RebuildSections()
    {
        _scrollRect.horizontal = !IsVertical;
        _scrollRect.vertical   =  IsVertical;

        EnsureContentPivotTop();
        Canvas.ForceUpdateCanvases(); // ensure rect sizes are valid

        foreach (var kv in _activeByKey) kv.Value.gameObject.SetActive(false);
        _activeByKey.Clear();
        _aliveKeys.Clear();
        DeactivateAllPooled();

        float contentWidth  = _scrollRect.content.rect.width;
        float contentHeight = _scrollRect.content.rect.height;

        float primaryCursor = IsVertical ? _paddingY : _paddingX;

        for (int si = 0; si < _sections.Count; si++)
        {
            var s = _sections[si];

            // normalize
            s.columnCount = Mathf.Max(1, s.columnCount);
            s.cardSize    = EnsureCardSize(s.prefabName, s.cardSize);

            if (s.isSeparator)
            {
                if (s.separatorGO == null)
                {
                    s.separatorGO = Managers.Resources.Instantiate($"Prefab/UI/Card/{s.prefabName}", _scrollRect.content);
                    EnsureTopAnchors((RectTransform)s.separatorGO.transform);

                }
                var rt = (RectTransform)s.separatorGO.transform;

                if (IsVertical)
                {
                    float h = s.separatorHeight > 0 ? s.separatorHeight : rt.rect.height;
                    s.yStart = primaryCursor;
                    s.height = Mathf.Max(1f, h);
                    s.separatorGO.SetActive(false);
                    primaryCursor += s.height;
                }
                else
                {
                    float w = s.separatorHeight > 0 ? s.separatorHeight : rt.rect.width;
                    s.xStart = primaryCursor;
                    s.width  = Mathf.Max(1f, w);
                    s.startY = 0f;
                    s.separatorGO.SetActive(false);
                    primaryCursor += s.width;
                }
                continue;
            }

            // card section
            s.cw = s.cardSize.x + s.spacingX;
            s.ch = s.cardSize.y + s.spacingY;

            if (IsVertical)
            {
                s.yStart    = primaryCursor;
                s.totalRows = Mathf.CeilToInt((float)s.dataList.Count / s.columnCount);
                s.startX    = ComputeStartCornerX(_corner, contentWidth, s.columnCount, s.cardSize.x, s.spacingX) + _paddingX;
                s.height    = Mathf.Max(1f, s.totalRows * s.ch);
                primaryCursor += s.height;
            }
            else
            {
                int rows    = s.columnCount; // horizontal: columnCount == row count
                s.totalCols = Mathf.CeilToInt((float)s.dataList.Count / rows);
                s.xStart    = primaryCursor;
                s.startY    = ComputeStartCornerY(_corner, contentHeight, rows, s.cardSize.y, s.spacingY) + _paddingY;
                s.width     = Mathf.Max(1f, s.totalCols * s.cw);
                primaryCursor += s.width;
            }
        }

        if (IsVertical) _scrollRect.content.sizeDelta = new Vector2(0, primaryCursor + _paddingY);
        else            _scrollRect.content.sizeDelta = new Vector2(primaryCursor + _paddingX, 0);

        RefreshVisibleSections(force: true);
    }

    private void RefreshVisibleSections(bool force = false)
    {
        // current viewport window
        var (viewStart, viewEnd, _) = GetViewWindow();
        if (!force && Mathf.Approximately(_lastAxisPos, viewStart)) return;
        _lastAxisPos = viewStart;

        float contentWidth  = _scrollRect.content.rect.width;
        float contentHeight = _scrollRect.content.rect.height;

        // keys that should be alive this frame
        _aliveKeys.Clear();

        for (int si = 0; si < _sections.Count; si++)
        {
            var s = _sections[si];

            // section visibility (with buffer)
            float secStart = IsVertical ? s.yStart : s.xStart;
            float secSize  = IsVertical ? s.height : s.width;
            float cellLen  = IsVertical ? (s.ch > 0 ? s.ch : 1f) : (s.cw > 0 ? s.cw : 1f);
            float padPx    = cellLen * VBUF;

            if (secSize < 1f) secSize = 1f; // handle empty sections safely

            if (secStart + secSize < viewStart - padPx || secStart > viewEnd + padPx)
            {
                // outside viewport: hide separator
                if (s.isSeparator && s.separatorGO != null) s.separatorGO.SetActive(false);
                continue;
            }

            // separator only
            if (s.isSeparator)
            {
                if (s.separatorGO == null) continue;
                var rt = (RectTransform)s.separatorGO.transform;

                if (IsVertical)
                    rt.anchoredPosition = new Vector2(0, -s.yStart);
                else
                {
                    float leftEdge = -contentWidth / 2f;
                    rt.anchoredPosition = new Vector2(leftEdge + s.xStart, -s.startY);
                }

                s.separatorGO.SetActive(true);
                continue;
            }


            // card section
            if (IsVertical)
            {
                if (s.totalRows <= 0) continue;

                // convert viewport to section-local
                float localTop    = Mathf.Max(0f, viewStart - secStart);
                float localBottom = Mathf.Min(s.height, viewEnd - secStart);

                int firstRow = Mathf.FloorToInt(localTop / s.ch) - VBUF;
                int lastRow  = Mathf.CeilToInt(localBottom / s.ch) + VBUF - 1;
                firstRow = Mathf.Clamp(firstRow, 0, Mathf.Max(0, s.totalRows - 1));
                lastRow  = Mathf.Clamp(lastRow,  0, Mathf.Max(0, s.totalRows - 1));

                int firstIndex = firstRow * s.columnCount;
                int lastIndex  = Mathf.Min(s.dataList.Count - 1, (lastRow + 1) * s.columnCount - 1);

                for (int i = firstIndex; i <= lastIndex; i++)
                {
                    long key = MakeKey(si, i);
                    _aliveKeys.Add(key);

                    if (!_activeByKey.TryGetValue(key, out var card))
                    {
                        card = GetCardFromPool(s.prefabName);
                        _activeByKey[key] = card;
                    }

                    var rt = card.RectTransform;
                    card.gameObject.SetActive(true);

                    int row = i / s.columnCount;
                    int col = i % s.columnCount;

                    float x = s.startX + col * s.cw;
                    float y = -(s.yStart + row * s.ch);

                    rt.anchoredPosition = new Vector2(x, y);
                    card.Setting(s.dataList[i]);

                    float itemStartY = s.yStart + row * s.ch;                 // 카드의 "윗변" 위치(컨텐츠 상단 기준)
                    float inValue    = Visibility01(viewStart, viewEnd, itemStartY, s.cardSize.y);
                    card.In(inValue);
                }
            }
            else
            {
                int rows = Mathf.Max(1, s.columnCount);
                if (s.totalCols <= 0) continue;

                float localLeft  = Mathf.Max(0f, viewStart - secStart);
                float localRight = Mathf.Min(s.width,  viewEnd - secStart);

                int firstCol = Mathf.FloorToInt(localLeft / s.cw) - VBUF;
                int lastCol  = Mathf.CeilToInt(localRight / s.cw) + VBUF - 1;
                firstCol = Mathf.Clamp(firstCol, 0, Mathf.Max(0, s.totalCols - 1));
                lastCol  = Mathf.Clamp(lastCol,  0, Mathf.Max(0, s.totalCols - 1));

                int firstIndex = firstCol * rows;
                int lastIndex  = Mathf.Min(s.dataList.Count - 1, (lastCol + 1) * rows - 1);

                float leftEdge = -contentWidth / 2f; // pivot(0.5) left edge

                for (int i = firstIndex; i <= lastIndex; i++)
                {
                    long key = MakeKey(si, i);
                    _aliveKeys.Add(key);

                    if (!_activeByKey.TryGetValue(key, out var card))
                    {
                        card = GetCardFromPool(s.prefabName);
                        _activeByKey[key] = card;
                    }

                    var rt = card.RectTransform;
                    card.gameObject.SetActive(true);

                    int row = i % rows;       // fill rows first when horizontal
                    int col = i / rows;

                    float x = leftEdge + s.xStart + _paddingX + (s.cardSize.x * 0.5f) + col * s.cw;
                    float y = -(s.startY + row * s.ch);

                    rt.anchoredPosition = new Vector2(x, y);
                    card.Setting(s.dataList[i]);

                    float itemStartX = s.xStart + col * s.cw;                 // 카드의 "왼쪽변" 위치(컨텐츠 좌측 기준)
                    float inValue    = Visibility01(viewStart, viewEnd, itemStartX, s.cardSize.x);
                    card.In(inValue);
                }
            }
        }

        // deactivate cards that are no longer visible
        _toRemove.Clear();
        foreach (var kv in _activeByKey)
            if (!_aliveKeys.Contains(kv.Key))
                _toRemove.Add(kv.Key);

        foreach (var k in _toRemove)
        {
            var c = _activeByKey[k];
            if (c != null)
            {
                // ★ 추가: 완전히 뷰 밖이면 0 전달
                c.In(0f);
                c.gameObject.SetActive(false);
            }
            _activeByKey.Remove(k);
        }
    }

    // ensure non-zero size and return a size
    private Vector2 EnsureCardSize(string prefabName, Vector2 cached)
    {
        if (cached.x > 0f && cached.y > 0f) return cached;
        var size = GetCardSize(prefabName);
        return size.sqrMagnitude > 0f ? size : new Vector2(100, 100);
    }
    #endregion

    #region Update & Utilities
    private IEnumerator PlayAniCoroutine()
    {
        for (int i = 0; i < CardList.Count; i++)
            CardList[i].Fade(0);

        for (int i = 0; i < CardList.Count; i++)
        {
            CardList[i].Fade(1);
            CardList[i].Play();
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ScrollBarHorizontalActive(bool flag)
    {
        if (_scrollbarHorizontal == null) return;

        _scrollbarHorizontal.interactable = flag;
        _scrollbarHorizontalImage.enabled = flag;
        _scrollbarHorizontalRectTr.sizeDelta = flag ? _scrollbarHorizontalOriginDeltaSize : Vector2.zero;
        _scrollbarHorizontal.handleRect.sizeDelta = flag ? _scrollbarHorizontalHandleRectDeltaSize : Vector2.zero;
        _scrollbarHorizontalHandleRectImage.enabled = flag;
        _scrollbarHorizontalSlidingAreaRectTr.sizeDelta = flag ? _scrollbarHorizontalSlidingAreaDeltaSize : Vector2.zero;
    }

    public void ScrollBarVerticalActive(bool flag)
    {
        if (_scrollbarVertical == null) return;

        _scrollbarVertical.interactable = flag;
        _scrollbarVerticalImage.enabled = flag;
        _scrollbarVerticalRectTr.sizeDelta = flag ? _scrollbarVerticalOriginDeltaSize : Vector2.zero;
        _scrollbarVertical.handleRect.sizeDelta = flag ? _scrollbarVerticalHandleRectDeltaSize : Vector2.zero;
        _scrollbarVerticalHandleRectImage.enabled = flag;
        _scrollbarVerticalSlidingAreaRectTr.sizeDelta = flag ? _scrollbarVerticalSlidingAreaDeltaSize : Vector2.zero;
    }

    private static void EnsureTopAnchors(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot     = new Vector2(0.5f, 1f);
    }

    private void EnsureContentPivotTop()
    {
        var ct = _scrollRect.content;
        ct.pivot = new Vector2(0.5f, 1f);
    }

    // Vertical X offset by corner (Left/Middle/Right align)
    private static float ComputeStartCornerX(
        UIScrollViewLayoutStartCorner corner,
        float contentWidth, int columnCount, float cardWidth, float spacingX)
    {
        float halfCard = cardWidth / 2f;
        switch (corner)
        {
            case UIScrollViewLayoutStartCorner.Left:
                return -contentWidth / 2f + halfCard;
            case UIScrollViewLayoutStartCorner.Middle:
                float totalW = columnCount * cardWidth + (columnCount - 1) * spacingX;
                return -totalW / 2f + halfCard;
            case UIScrollViewLayoutStartCorner.Right:
                float usedW = columnCount * cardWidth + (columnCount - 1) * spacingX;
                return  contentWidth / 2f - usedW + halfCard;
            default:
                return 0;
        }
    }

    // Horizontal Y offset by corner (Left=Top, Middle=Center, Right=Bottom)
    private static float ComputeStartCornerY(
        UIScrollViewLayoutStartCorner corner,
        float contentHeight, int rowCount, float cardHeight, float spacingY)
    {
        switch (corner)
        {
            case UIScrollViewLayoutStartCorner.Left: // Top
                return 0f;
            case UIScrollViewLayoutStartCorner.Middle: // Center
            {
                float usedH = rowCount * cardHeight + Mathf.Max(0, rowCount - 1) * spacingY;
                return Mathf.Max(0f, (contentHeight - usedH) / 2f);
            }
            case UIScrollViewLayoutStartCorner.Right: // Bottom
            {
                float usedH = rowCount * cardHeight + Mathf.Max(0, rowCount - 1) * spacingY;
                return Mathf.Max(0f, contentHeight - usedH);
            }
            default:
                return 0f;
        }
    }
    // viewport [viewStart, viewEnd]와 아이템 [itemStart, itemStart+itemSize]의 겹침 비율(0~1)
    private static float Visibility01(float viewStart, float viewEnd, float itemStart, float itemSize)
    {
        float itemEnd  = itemStart + itemSize;
        float overlap  = Mathf.Max(0f, Mathf.Min(viewEnd, itemEnd) - Mathf.Max(viewStart, itemStart));
        float denom    = Mathf.Max(0.0001f, itemSize);
        return Mathf.Clamp01(overlap / denom);
    }


    private void ClearAll()
    {
        foreach (var kv in _poolByPrefab)
            foreach (var c in kv.Value)
                c.gameObject.SetActive(false);

        foreach (var s in _sections)
            if (s.separatorGO != null)
                s.separatorGO.SetActive(false);

        _sections.Clear();
        DataList.Clear();
        CardList.Clear();
        _activeByKey.Clear();
        _aliveKeys.Clear();
        _toRemove.Clear();
        _cardSizeCache.Clear();
        _useSections = false;
    }

    [Button]
    public void InspectorSetting()
    {
        Initialize();
        ScrollBarHorizontalActive(false);
        ScrollBarVerticalActive(false);
        if (_scrollViewImg != null) _scrollViewImg.color = Color.clear;

        List<RectTransform> childs = UnityHelper.FindChilds<RectTransform>(this.gameObject, true);
        for (int i = 0; i < childs.Count; i++)
        {
            RectTransform child = childs[i];
            string n = child.gameObject.name;
            child.gameObject.name = n.Replace(" ", "").Replace("_", "").Replace("(Legacy)", "");
        }

#if UNITY_EDITOR
        UnityEngine.Object pSelectObj = UnityEditor.Selection.activeObject;
        UnityEditor.EditorUtility.SetDirty(pSelectObj);
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    #endregion
}
