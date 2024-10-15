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

        bool isPlayerDataExist = _data.playerData != null;
        int awake = isPlayerDataExist ? _data.playerData.Awake : 0;
        Grade grade = (Grade)_data.tableData.grade;

        NameSetting(_data.soData.DisplayName);
        SliderSetting(1, awake);
        GradeSetting(grade);
    }
    void NameSetting(string name)
    {
        GetText(UITextE.TextName).text = name;
    }
    void GradeSetting(Grade grade)
    {
        GetImage(UIImageE.Grade).sprite = Managers.Atlas.GetGrade(grade);
        GetImage(UIImageE.Grade).Image.SetNativeSize();

        GetImage(UIImageE.BG).sprite = Managers.Atlas.GetGradeBg(grade);
    }
    void JobSetting(CharacterJob job)
    {

    }
    void SliderSetting(int cardCount, int awake)
    {
        Get<CharacterCardSlider>(CharacterCardSliderE.CharacterCardSlider).UISet(cardCount, awake);
    }
    
	public enum UIImageE
    {
        BG,
        BgDeco,
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
