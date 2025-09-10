using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodsCollectEffect : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    private Camera _camera;
    private UIGoodsController _uiController;
    private string _item;
    private RectTransform _destTr;
    private Vector3 _start, _end, _point1;
    private float _percent, _duration;
    private readonly float _minDuration = 0.5f, _maxDuration = 0.9f, r = 3f;
    private bool _isSwitch = false;
    
    public void UISet(UIGoodsController uiController, string item, RectTransform destTr)
    {
        this._camera = Managers.Observer.Camera;
        this._uiController = uiController;
        this._item = item;
        this._destTr = destTr;
        this._percent = 0;
        this._start = transform.position;
        this._duration = UnityHelper.Random_H(_minDuration, _maxDuration);
        this._point1 = GetNewPoint(_start, UnityHelper.Random_H(0, 360f), r);
        
        GetImage(UIImageE.Image).sprite = Managers.Atlas.GetItem(item, false);
        GetImage(UIImageE.Image).SetNativeSize();

        _isSwitch = true;
    }

    private void Update()
    {
        if (!_isSwitch)
            return;
        
        if (_percent >= 1f)
        {
            _isSwitch = false;
            Managers.Resources.Destroy(this.gameObject);
            _uiController.GetUIGoods(_item).ScalePlay();
            _uiController.OnGoodsEffectCollected();
            return;
        }

        UpdateEndPoint();
        _percent += Managers.Time.DeltaTime / _duration;
        this.transform.position = QuadraticCurve(_start, _point1, _end, _percent);
    }

    private void UpdateEndPoint()
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(null, _destTr.position);

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_destTr, screenPoint, _camera, out Vector3 worldPoint))
        {
            this._end = worldPoint;
        }
    }
    
    public float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }

    public Vector2 GetNewPoint(Vector3 start, float angle, float r)
    {
        angle = DegreeToRadian(angle);

        Vector2 position = Vector2.zero;
        position.x = Mathf.Cos(angle) * r + start.x;
        position.y = Mathf.Sin(angle) * r + start.y;

        return position;
    }

    public Vector2 Lerp(Vector2 a, Vector2 b, float t)
    {
        return a + (b - a) * t;
    }

    public Vector2 QuadraticCurve(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p1 = Lerp(a, b, t);
        Vector2 p2 = Lerp(b, c, t);

        return Lerp(p1, p2, t);
    }
    
	public enum UIImageE
    {
		Image,
    }
}