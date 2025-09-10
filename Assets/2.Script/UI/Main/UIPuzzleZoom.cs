using UnityEngine;

public class UIPuzzleZoom : UIFrame
{
    protected override void Initialize()
    {
        base.Initialize();
    }
    
    [SerializeField] Transform baseRoot;
    [SerializeField] Transform puzzleRoot;
    [SerializeField] Transform itemRoot;
    [SerializeField] Transform chainRoot;
    [SerializeField] Transform multiplierRoot;

    private float _sizeUpValue = 1.2f;
    private float _cellSize = 9f;
    private float _initX = 0f;
    private float _initY = -30.37f;
    private float _positionWidth = -40f;
    private float _tweenDuration = 0.1f;

    public void Set(float cellSize, float initX, float initY)
    {
        _cellSize = cellSize;
        _initX = initX;
        _initY = initY;
    }
    
    public void Zoom(Vector3 cellPosition)
    {
	    Scale(_sizeUpValue);

	    cellPosition.x -= _initX;
	    cellPosition.y -= _initY;
	    
	    float xValue = cellPosition.x == 0 ? 0 : cellPosition.x / _cellSize * _positionWidth;
	    float yValue = cellPosition.y == 0 ? 0 : cellPosition.y / _cellSize * _positionWidth;
	    
	    Vector3 movePosition = Vector3.zero;
	    movePosition.x += xValue;
	    movePosition.y += yValue;
	    
	    Position(movePosition);
    }

    public void ZoomOut()
    {
	    Scale(1);
	    Position(Vector2.zero);
    }

    void Scale(float scaleValue)
    {
	    Vector3 scale = new Vector3(scaleValue, scaleValue, 1);
	    Managers.Tween.TweenScale(baseRoot.transform, baseRoot.transform.localScale, scale, _tweenDuration);
	    Managers.Tween.TweenScale(puzzleRoot.transform, puzzleRoot.transform.localScale, scale, _tweenDuration);
	    Managers.Tween.TweenScale(itemRoot.transform, itemRoot.transform.localScale, scale, _tweenDuration);
	    Managers.Tween.TweenScale(chainRoot.transform, chainRoot.transform.localScale, scale, _tweenDuration);
	    Managers.Tween.TweenScale(multiplierRoot.transform, multiplierRoot.transform.localScale, scale, _tweenDuration);
    }

    void Position(Vector3 localPosition)
    {
	    if (baseRoot.localPosition != localPosition)
		    Managers.Tween.TweenLocalPosition(baseRoot, baseRoot.localPosition, localPosition, _tweenDuration);

	    if (puzzleRoot.localPosition != localPosition)
		    Managers.Tween.TweenLocalPosition(puzzleRoot, puzzleRoot.localPosition, localPosition, _tweenDuration);
	    
	    if (itemRoot.localPosition != localPosition)
		    Managers.Tween.TweenLocalPosition(itemRoot, itemRoot.localPosition, localPosition, _tweenDuration);
	    
	    if (chainRoot.localPosition != localPosition)
		    Managers.Tween.TweenLocalPosition(chainRoot, chainRoot.localPosition, localPosition, _tweenDuration);
	    
	    if (multiplierRoot.localPosition != localPosition)
		    Managers.Tween.TweenLocalPosition(multiplierRoot, multiplierRoot.localPosition, localPosition, _tweenDuration);
    }
}