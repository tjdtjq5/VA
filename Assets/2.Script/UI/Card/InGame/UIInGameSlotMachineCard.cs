using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameSlotMachineCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }
    public override void Setting(ICardData data)
    {
        UIInGameSlotMachineCardData cardData = (UIInGameSlotMachineCardData)data;

        GetImage(UIImageE.Icon).sprite = cardData.SkillOrBuff.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();
    }
	public enum UIImageE
    {
		Icon,
    }
}

public class UIInGameSlotMachineCardData : ICardData
{
    public IdentifiedObject SkillOrBuff;
}