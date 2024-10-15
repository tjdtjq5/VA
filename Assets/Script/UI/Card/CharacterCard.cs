public class CharacterCard : UICard
{
    CharacterCardData _data;

	protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<CharacterCardSlider>(typeof(CharacterCardSliderE));
	}

    public override void Setting(ICardData data)
    {
        _data = (CharacterCardData)data;

        UnityHelper.SerializeL(_data);

       // NameSetting(_data.soData.DisplayName);
       // SliderSetting(1, _data.playerData.Awake);
    }
    void NameSetting(string name)
    {
        GetText(UITextE.TextName).text = name;
    }
    void SliderSetting(int cardCount, int awake)
    {
        Get<CharacterCardSlider>(CharacterCardSliderE.CharacterCardSlider).UISet(cardCount, awake);
    }
    
	public enum UIImageE
    {
		BgDeco_BgDeco1,
		BgDeco_BgDeco2,
		BgDeco_BgDeco3,
		BgDeco_BgDeco4,
		Grade,
		Job,
    }
	public enum UITextE
    {
		TextLv,
		TextName,
    }
	public enum CharacterCardSliderE
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
