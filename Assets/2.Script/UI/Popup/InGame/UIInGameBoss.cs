using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameBoss : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

        base.Initialize();
    }

    private readonly float openTime = 2f;
    
    IEnumerator _openCoroutine;

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    if (_openCoroutine != null)
		    StopCoroutine(_openCoroutine);
	    _openCoroutine = OpenCoroutine(); 
	    StartCoroutine(_openCoroutine);
    }

    IEnumerator OpenCoroutine()
    {
	    yield return new WaitForSeconds(openTime);
	    ClosePopupUI();
    }

    public enum UIImageE
    {
		Main_Boss,
		Main_Boss_Line1,
		Main_Boss_Line2,
		Main_Boss_Bossbg,
		Main_Boss_Icon,
		Main_TextImg,
    }
}