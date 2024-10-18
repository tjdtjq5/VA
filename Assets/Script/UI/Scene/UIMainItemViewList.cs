using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainItemViewList : UIScene
{
    protected override void Initialize()
    {
		Bind<MainItemView>(typeof(MainItemViewE));

        base.Initialize();

    }

    protected override void UISet()
    {
        base.UISet();

        UISetOff();

        Get<MainItemView>(MainItemViewE.MainItemViewGesso).UISet(ItemTableCodeDefine.Gesso);
        Get<MainItemView>(MainItemViewE.MainItemViewEight).UISet(ItemTableCodeDefine.Eight);
    }

    public void UISet(ItemTableCodeDefine item)
    {
        Get<MainItemView>(MainItemViewE.MainItemViewEtc).gameObject.SetActive(true);
        Get<MainItemView>(MainItemViewE.MainItemViewEtc).UISet(item);
    }
    public void UISetOff()
    {
        Get<MainItemView>(MainItemViewE.MainItemViewEtc).gameObject.SetActive(false);
    }

	public enum MainItemViewE
    {
		MainItemViewEtc,
		MainItemViewEight,
		MainItemViewGesso,
    }
}