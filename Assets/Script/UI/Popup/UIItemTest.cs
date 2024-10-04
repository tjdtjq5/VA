using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIItemTest : UIPopup
{
	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIButton>(typeof(UIButtonE));
		Bind<UIInputField>(typeof(UIInputFieldE));
	}

    protected override void UISet()
    {
        base.UISet();

		GetButton(UIButtonE.BtnList_GetItemTable).AddClickEvent(OnClickItemTable);
		GetButton(UIButtonE.BtnList_GetItems).AddClickEvent(OnClickItemGets);
		GetButton(UIButtonE.BtnList_ItemPush).AddClickEvent(OnClickItemPush);
    }
    void OnClickItemTable(PointerEventData ped)
    {
        Managers.Table.ItemTable.DbGets((res) => 
		{
            UnityHelper.LogSerialize(res);
        });
    }
    void OnClickItemGets(PointerEventData ped)
	{
		Managers.Web.SendGetRequest<PlayerItemGetResponse>("playerItem/gets", (res) => 
		{
			UnityHelper.LogSerialize(res);
		});
	}
    void OnClickItemPush(PointerEventData ped)
    {
		string itemCode = GetInputField(UIInputFieldE.BtnList_ItemInputField).text;

		PlayerItemData item = new PlayerItemData()
		{
			ItemCode = itemCode,
		};
		item.Set(300);

        PlayerItemPushRequest req = new PlayerItemPushRequest();
		req.Push(item);

        Managers.Web.SendPostRequest<PlayerItemPushResponse>("playerItem/push", req, (res) =>
        {
            UnityHelper.LogSerialize(res);
        });
    }

	public enum UIImageE
    {
		BackGround,
		BtnList_GetItemTable,
		BtnList_GetItems,
		BtnList_ItemInputField,
		BtnList_ItemPush,
    }
	public enum UIButtonE
    {
		BtnList_GetItemTable,
		BtnList_GetItems,
		BtnList_ItemPush,
    }
	public enum UIInputFieldE
    {
		BtnList_ItemInputField,
    }
}
