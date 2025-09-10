using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ForkRoadButton : UIButton
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    
    [SerializeField] Sprite rightSprite;
    [SerializeField] Sprite middleSprite;
    [SerializeField] Sprite leftSprite;

    private readonly float _iconMoveX = 40f;

    [Button]
    public void Test(Direction direction)
    {
	    Bind<UIImage>(typeof(UIImageE));
			
	    GetImage(UIImageE.Bg).sprite = GetSprite(direction);
	    GetImage(UIImageE.Bg).SetNativeSize();
	    GetImage(UIImageE.Bg_Icon).SetNativeSize();

	    Vector3 iconPos = GetImage(UIImageE.Bg_Icon).transform.localPosition;
	    iconPos.x = GetIconMoveX(direction);
	    GetImage(UIImageE.Bg_Icon).transform.localPosition = iconPos;
    }

    public void UISet(Sprite icon, Direction direction)
    {
	    GetImage(UIImageE.Bg).sprite = GetSprite(direction);
	    GetImage(UIImageE.Bg).SetNativeSize();
	    GetImage(UIImageE.Bg_Icon).sprite = icon;
	    GetImage(UIImageE.Bg_Icon).SetNativeSize();

	    Vector3 iconPos = GetImage(UIImageE.Bg_Icon).transform.localPosition;
	    iconPos.x = GetIconMoveX(direction);
	    GetImage(UIImageE.Bg_Icon).transform.localPosition = iconPos;
    }

    private Sprite GetSprite(Direction direction)
    {
	    switch (direction)
	    {
		    case Direction.Right:
			    return rightSprite;
		    case Direction.Middle:
			    return middleSprite;
		    case Direction.Left:
			    return leftSprite;
		    default:
			    return null;
	    }
    }

    private float GetIconMoveX(Direction direction)
    {
	    switch (direction)
	    {
		    case Direction.Right:
			    return -_iconMoveX;
		    case Direction.Middle:
			    return -_iconMoveX;
		    case Direction.Left:
			    return _iconMoveX;
		    default:
			    return 0;
	    }
    }
    
	public enum UIImageE
    {
		Bg,
		Bg_Icon,
    }
}