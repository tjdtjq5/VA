using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGame777RouletteCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    
    [SerializeField] Sprite basicSprite;
    [SerializeField] Sprite rareSprite;
    [SerializeField] Sprite legendarySprite;
    
    public override void Setting(ICardData data)
    {
        UIInGame777RouletteCardData cardData = (UIInGame777RouletteCardData)data;
        switch (cardData.Type)
        {
            case UIInGame777RouletteCardType.Basic:
                GetImage(UIImageE.Icon).sprite = basicSprite;
                break;
            case UIInGame777RouletteCardType.Rare:
                GetImage(UIImageE.Icon).sprite = rareSprite;
                break;
            case UIInGame777RouletteCardType.Legendary:
                GetImage(UIImageE.Icon).sprite = legendarySprite;
                break;
        }
        GetImage(UIImageE.Icon).SetNativeSize();
    }

	public enum UIImageE
    {
		Icon,
    }
}

public class UIInGame777RouletteCardData : ICardData
{
    public UIInGame777RouletteCardType Type;
}

public enum UIInGame777RouletteCardType
{
    Basic,
    Rare,
    Legendary,
}