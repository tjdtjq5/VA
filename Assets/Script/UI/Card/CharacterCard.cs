public class CharacterCard : UICard
{
	CharacterCardData _data;

	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<UISlider>(typeof(UISliderE));
	}
    public override void Setting(ICardData data)
    {
        _data = (CharacterCardData)data;

		NameSetting(_data.soData.DisplayName);
		SliderSetting(1, _data.playerData.Awake);
    }
	void NameSetting(string name)
	{
		GetText(UITextE.Text_Name).text = name;
	}
	void SliderSetting(int cardCount, int awake)
	{
		((CharacterCardSlider)GetSlider(UISliderE.CharacterCardSlider)).UISet(cardCount, awake);
	}
	public enum UIImageE
    {
		Bg_Deco_Bg_Deco_1,
		Bg_Deco_Bg_Deco_2,
		Bg_Deco_Bg_Deco_3,
		Bg_Deco_Bg_Deco_4,
		Grade,
		Job,
    }
	public enum UITextE
    {
		Text_Lv,
		Text_Name,
    }
	public enum UISliderE
    {
		CharacterCardSlider,
    }
}
public class CharacterCardData : ICardData
{
	public CharacterPlayerData playerData;
	public CharacterTableData tableData;
    public CharacterSO soData;
}