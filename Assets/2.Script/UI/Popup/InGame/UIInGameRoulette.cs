using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInGameRoulette : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIButton>(typeof(UIButtonE));
		
		for (int i = 0; i < cardParent.childCount; i++)
		{
			InGameRouletteCard card = cardParent.GetChild(i).GetComponent<InGameRouletteCard>();
			card.Initialize(this, i);
		}
		
		GetButton(UIButtonE.Main_OkBtn).AddClickEvent((ped) => OnClickRoll());

        base.Initialize();
    }
    
    public Action<int> OnSelect;
    public Action<int> OnResultSelect;
    
    [SerializeField] Transform cardParent;
    [SerializeField] Transform arrow;
    
    private readonly string _getPopupName = "InGame/UIInGameGet";
    
    private IEnumerator _rollCoroutine;
    private WaitForSeconds _tickWait = new WaitForSeconds(0.05f);
    private float _beforeAngle;

    public override void OpenUISet(CanvasOrderType orderType)
    {
	    base.OpenUISet(orderType);

	    for (int i = 0; i < cardParent.childCount; i++)
	    {
		    InGameRouletteCard rewardCard = cardParent.GetChild(i).GetComponent<InGameRouletteCard>();
		    rewardCard.UISetIObject();
	    }

	    SelectCard(-1);
	    ResultSelectCard(-1);
	    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(true);
    }

    void OnClickRoll()
    {
	    GetButton(UIButtonE.Main_OkBtn).gameObject.SetActive(false);

	    if (_rollCoroutine != null)
		    StopCoroutine(_rollCoroutine);
		int random = (int)UnityHelper.Random_H(0, cardParent.childCount);
	    _rollCoroutine = RollCoroutine(random);
	    StartCoroutine(_rollCoroutine);
    }
    void SelectCard(int index)
    {
	    OnSelect?.Invoke(index);
	    SetArrow(index);
    }
    void ResultSelectCard(int index)
    {
	    OnResultSelect?.Invoke(index);
    }

    void SetArrow(int index)
    {
	    if (index < 0 || index >= cardParent.childCount)
	    {
		    arrow.localRotation = Quaternion.Euler(0, 0, 0);
	    }
	    else
	    {
		    float angle = UnityHelper.GetAngle(arrow.position, cardParent.GetChild(index).position) - 90f;
		    Managers.Tween.TweenRotationZ(arrow, _beforeAngle, angle, 0.05f);
		    _beforeAngle = angle;
	    }
    }
    IEnumerator RollCoroutine(int randomIndex)
    {
	    yield return null;

	    int startIndex = 2;
	    int index = 0;
	    int last = cardParent.childCount * 2 + randomIndex;
	    for (int i = 0 + startIndex; i < last + startIndex; i++)
	    {
		    index = GetIndexClamp(i);
		    SelectCard(index);

		    if (i >= last - 4)
		    {
			    int pauseIndex = last - i;
			    switch (pauseIndex)
			    {
				    case 4:
					    yield return new WaitForSeconds(0.1f);
					    break;
				    case 3:
					    yield return new WaitForSeconds(0.15f);
					    break;
				    case 2:
					    yield return new WaitForSeconds(0.4f);
					    break;
				    case 1:
					    yield return new WaitForSeconds(0.6f);
					    break;
				    case 0:
					    yield return new WaitForSeconds(0.8f);
					    break;
			    }
		    }
		    else
			    yield return _tickWait;
	    }

	    ResultSelectCard(index);
	    
	    yield return new WaitForSeconds(1.5f);

	    InGameRouletteCard rewardCard = cardParent.GetChild(index).GetComponent<InGameRouletteCard>();
	    Result(rewardCard.GetResult());
    }
    int GetIndexClamp(int index) => index % cardParent.childCount;
    void Result(IdentifiedObject skillOrBuff)
    {
	    Managers.Observer.Player.StartSkillOrBuff(Managers.Observer.Player, skillOrBuff);
	    
	    UIInGameGet uiInGameGet = Managers.UI.ShopPopupUI<UIInGameGet>(_getPopupName, CanvasOrderType.Middle);
	    uiInGameGet.UISetSkillOrBuff(skillOrBuff);
	   
	    uiInGameGet.OnClose -= ClosePopupUI;
	    uiInGameGet.OnClose += ClosePopupUI;
    }

    public enum UIImageE
    {
		BlackPannel,
		Main_Title,
		Main_Board_Gear,
		Main_Board_Gear_Arrow_Icon,
    }
	public enum UITextE
    {
		Main_Title_Text,
		Main_Ex,
    }
	public enum UIButtonE
    {
		Main_OkBtn,
    }
}