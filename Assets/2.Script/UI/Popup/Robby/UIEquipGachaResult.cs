using System.Collections;
using System.Collections.Generic;
using Shared.DTOs.Table;
using Shared.Enums;
using Spine.Unity;
using UnityEngine;

public class UIEquipGachaResult : UIPopup
{
    [SerializeField] private SkeletonGraphic _spineGraphic;
    private SpineAniController _spineAniController;
    private readonly string _resultCardPrefabPath = "Robby/GachaResultCard";
    protected override void Initialize()
    {
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIBaseScrollView>(typeof(UIBaseScrollViewE));
		Bind<UIButton>(typeof(UIButtonE));

        _spineAniController = _spineGraphic.Initialize();

        GetButton(UIButtonE.Button).AddClickEvent((ped) => OnClickCloseButton());

        base.Initialize();
    }
    public void UISet(GachaGroup gachaGroup, List<TableGachaDto> Results)
    {
         InitSet();
         ChestSet(gachaGroup);
         ScrollViewSet(Results);
    }
    private void InitSet()
    {
        GetBaseScrollView(UIBaseScrollViewE.ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _resultCardPrefabPath, new List<ICardData>());
    }
    private void ChestSet(GachaGroup gachaGroup)
    {
        switch (gachaGroup)
        {
            case GachaGroup.PicUpGacha_1:
                _spineAniController.Play("2", false, true);
                break;
            case GachaGroup.PicUpGacha_2:
                _spineAniController.Play("2", false, true);
                break;
            case GachaGroup.PicUpGacha_3:
                _spineAniController.Play("2", false, true);
                break;
            case GachaGroup.PicUpGacha_4:
                _spineAniController.Play("2", false, true);
                break;
            default:
                _spineAniController.Play("1", false, true);
                break;
        }
    }
    private void ScrollViewSet(List<TableGachaDto> Results)
    {
        List<ICardData> cardDatas = new List<ICardData>();
        for (int i = 0; i < Results.Count; i++)
        {
            cardDatas.Add(new EquipGachaResultCardData() { GachaDto = Results[i] });
        }
        GetBaseScrollView(UIBaseScrollViewE.ScrollView).UISet(UIScrollViewLayoutStartAxis.Vertical, _resultCardPrefabPath, cardDatas);
    }
    private void OnClickCloseButton()
    {
        ClosePopupUI();
    }
	public enum UITextProE
    {
		Title_Text,
    }
	public enum UIBaseScrollViewE
    {
		ScrollView,
    }
	public enum UIButtonE
    {
		Button,
    }
}