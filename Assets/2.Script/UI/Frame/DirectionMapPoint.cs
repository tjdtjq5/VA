using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMapPoint : UIFrame
{
    [SerializeField] private Sprite _noneIcon;

    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    public void UISet(DungeonNode node, bool isCurrent)
    {
        GetImage(UIImageE.Icon).sprite = node.DungeonRoom == null ? _noneIcon : node.DungeonRoom.DirectionMapIcon;
        GetImage(UIImageE.Icon).SetNativeSize();

        GetImage(UIImageE.Icon).Fade(isCurrent ? 0f : 1f);
    }
    
	public enum UIImageE
    {
		Icon,
    }
}