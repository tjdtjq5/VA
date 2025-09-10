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
    }
    void OnClickItemTable(PointerEventData ped)
    {
        // Managers.Table.ItemTable.DbGets((res) => 
		// {
        //     UnityHelper.SerializeL(res);
        // });
    }
    void OnClickCharacterTable(PointerEventData ped)
    {
      
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
