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
		GetButton(UIButtonE.BtnList_GetCharacterTable).AddClickEvent(OnClickCharacterTable);

		GetButton(UIButtonE.BtnList_GetItems).AddClickEvent(OnClickItemGets);

		GetButton(UIButtonE.BtnList_ItemPush).AddClickEvent(OnClickItemPush);
		GetButton(UIButtonE.BtnList_CharacterLevelUp).AddClickEvent(OnClickCharacterLevelup);
    }
    void OnClickItemTable(PointerEventData ped)
    {
        Managers.Table.ItemTable.DbGets((res) => 
		{
            UnityHelper.LogSerialize(res);
        });
    }
    void OnClickCharacterTable(PointerEventData ped)
    {
        Managers.Table.CharacterTable.DbGets((res) =>
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
		int itemCount = int.Parse(GetInputField(UIInputFieldE.BtnList_ItemCountInputField).text);

        PlayerItemData item = new PlayerItemData()
		{
			ItemCode = itemCode,
		};
		item.Set(itemCount);

        PlayerItemPushRequest req = new PlayerItemPushRequest();
		req.Push(item);

        Managers.Web.SendPostRequest<PlayerItemPushResponse>("playerItem/push", req, (res) =>
        {
            UnityHelper.LogSerialize(res);
        });
    }
    void OnClickCharacterLevelup(PointerEventData ped)
	{
        string code = GetInputField(UIInputFieldE.BtnList_CharacterInputField).text;
        int pluslevel = int.Parse(GetInputField(UIInputFieldE.BtnList_CharacterPlusLevelInputField).text);

		CharacterLevelUp(code, pluslevel);
    }
    void CharacterLevelUp(string code, int plusLevel)
	{
		CPDLevelUpRequest req = new CPDLevelUpRequest();
        req.Code = code;
		req.PlusLevel = plusLevel;

        Managers.Web.SendPostRequest<CPDLevelUpResponse>("characterPlayerData/levelup", req, (res) =>
        {
            UnityHelper.LogSerialize(res);
        });
    }

	public enum UIImageE
    {
		BackGround,
		BtnList_GetCharacterTable,
		BtnList_GetItemTable,
		BtnList_GetItems,
		BtnList_ItemInputField,
		BtnList_ItemCountInputField,
		BtnList_ItemPush,
		BtnList_CharacterInputField,
		BtnList_CharacterPlusLevelInputField,
		BtnList_CharacterLevelUp,
    }
	public enum UIButtonE
    {
		BtnList_GetCharacterTable,
		BtnList_GetItemTable,
		BtnList_GetItems,
		BtnList_ItemPush,
		BtnList_CharacterLevelUp,
    }
	public enum UIInputFieldE
    {
		BtnList_ItemInputField,
		BtnList_ItemCountInputField,
		BtnList_CharacterInputField,
		BtnList_CharacterPlusLevelInputField,
    }
}
