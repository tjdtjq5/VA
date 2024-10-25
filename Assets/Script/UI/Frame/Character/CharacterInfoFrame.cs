public class CharacterInfoFrame : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIText>(typeof(UITextE));
		Bind<CharacterCardSlider>(typeof(CharacterCardSliderE));

        base.Initialize();
    }

    protected override void UISet()
    {
        base.UISet();
    }
    public void UISet(string characterCode)
    {
        UnityHelper.Log_H(characterCode);

        CharacterTableData tableData = Managers.Table.CharacterTable.Get(characterCode);
        CharacterSO characterSO = Managers.Table.CharacterTable.GetTableSO(characterCode);
        CharacterPlayerData playerData = Managers.PlayerData.Character.Get(characterCode);

        GetImage(UIImageE.Character).sprite = characterSO.Icon;
        GetImage(UIImageE.Character).SetNativeSize();

        GetTextPro(UITextProE.Text).text = characterSO.DisplayName;

        Get<CharacterCardSlider>(CharacterCardSliderE.CharacterCardSlider).UISet(playerData);

        GetImage(UIImageE.Type_Grade).color = DefineColor.GetGrade((Grade)tableData.grade);

        GetImage(UIImageE.Type_Grade_Icon).sprite = Managers.Atlas.GetGrade((Grade)tableData.grade);
        GetImage(UIImageE.Type_Grade_Icon).SetNativeSize();

        GetText(UITextE.Type_Job_Text).text = Managers.Script.Get((CharacterJob)tableData.job);
        GetText(UITextE.Type_Tribe_Text).text = Managers.Script.Get((Tribe)tableData.tribeType);
    }

    public enum UIImageE
    {
		Shadow,
		Character,
		Type_Grade,
		Type_Grade_Icon,
		Type_Job,
		Type_Tribe,
    }
	public enum UITextProE
    {
		Text,
    }
	public enum UITextE
    {
		Type_Job_Text,
		Type_Tribe_Text,
    }
	public enum CharacterCardSliderE
    {
		CharacterCardSlider,
    }
}