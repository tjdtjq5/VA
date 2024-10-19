using UnityEngine;

public class CharacterCard : UICard
{
    CharacterCardData _data;

	protected override void Initialize()
	{
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));
		Bind<CharacterCardSlider>(typeof(CharacterCardSliderE));

		base.Initialize();
	}
    public override void Setting(ICardData data)
    {
        _data = (CharacterCardData)data;

        bool isPlayerDataExist = _data.playerData != null;
        int awake = isPlayerDataExist ? _data.playerData.Awake : 0;
        Grade grade = (Grade)_data.tableData.grade;
        int level = isPlayerDataExist ? _data.playerData.Level : 0;
        int cardCount = isPlayerDataExist ? 1 : -1;

        NameSetting(_data.soData.DisplayName);
        SliderSetting(cardCount, awake, isPlayerDataExist);
        GradeSetting(grade);
        CharacterImgSet(_data.soData.Icon);
        LevelSet(level);
    }
    void NameSetting(string name)
    {
        GetText(UITextE.TextName).text = name;
    }
    void LevelSet(int level)
    {
        if (level < 1)
            GetText(UITextE.TextLv).text = "";
        else
            GetText(UITextE.TextLv).text = $"Lv.{level}";
    }
    void GradeSetting(Grade grade)
    {
        GetImage(UIImageE.Grade).sprite = Managers.Atlas.GetGrade(grade);
        GetImage(UIImageE.Grade).Image.SetNativeSize();

        GetImage(UIImageE.BG).sprite = Managers.Atlas.GetGradeBg(grade);
    }
    void CharacterImgSet(Sprite sprite)
    {
        if (sprite == null)
        {
            GetImage(UIImageE.Character).Image.color = Color.clear;
        }
        else
        {
            GetImage(UIImageE.Character).Image.color = Color.white;
            GetImage(UIImageE.Character).sprite = sprite;
            GetImage(UIImageE.Character).Image.SetNativeSize();
        }
    }
    void JobSetting(CharacterJob job)
    {

    }
    void SliderSetting(int cardCount, int awake, bool isPlayerData)
    {
        Get<CharacterCardSlider>(CharacterCardSliderE.CharacterCardSlider).UISet(cardCount, awake, isPlayerData);
    }
    
	public enum UIImageE
    {
		BG,
		BgDeco,
		Character,
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