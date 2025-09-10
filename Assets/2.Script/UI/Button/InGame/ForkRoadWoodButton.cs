using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ForkRoadWoodButton : UIButton
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    private readonly Vector3 _leftPosition = new Vector3(-156f, 161f, 0);
    private readonly Vector3 _middlePosition = new Vector3(16f, 161f, 0);
    private readonly Vector3 _rightPosition = new Vector3(217f, 161f, 0);

	private Direction _direction;
    
    public void UISet(Sprite icon, Direction direction)
    {
	    GetImage(UIImageE.Icon).sprite = icon;
	    GetImage(UIImageE.Icon).SetNativeSize();
	    _direction = direction;

		Invoke(nameof(SetPosition), 0.1f);
    }
    public void SetPosition()
    {
	    switch (_direction)
	    {
		    case Direction.Right:
			    this.transform.localPosition = _rightPosition;
			    break;
		    case Direction.Middle:
			    this.transform.localPosition = _middlePosition;
			    break;
		    case Direction.Left:
			    this.transform.localPosition = _leftPosition;
			    break;
	    }
    }

	public enum UIImageE
    {
		Icon,
    }
}