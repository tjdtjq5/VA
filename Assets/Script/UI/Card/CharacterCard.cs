using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));
		Bind<CharacterCardSlider>(typeof(CharacterCardSliderE));
		Bind<UIButton>(typeof(UIButtonE));

        base.Initialize();
    }
    CharacterCardData _data;

    protected override void UISet()
    {
        base.UISet();

        GetButton(UIButtonE.Button).AddClickEvent(Click);
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
        GetTextPro(UITextProE.Main_Name).text = name;
    }
    void LevelSet(int level)
    {
        if (level < 1)
            GetTextPro(UITextProE.Main_LvText).text = "";
        else
            GetTextPro(UITextProE.Main_LvText).text = $"Lv.{level}";
    }
    void GradeSetting(Grade grade)
    {
        GetImage(UIImageE.Main_Grade).sprite = Managers.Atlas.GetGrade(grade);
        GetImage(UIImageE.Main_Grade).Image.SetNativeSize();

        GetImage(UIImageE.Main_BG).sprite = Managers.Atlas.GetGradeBg(grade);
    }
    void CharacterImgSet(Sprite sprite)
    {
        if (sprite == null)
        {
            GetImage(UIImageE.Main_Character).Image.color = Color.clear;
        }
        else
        {
            GetImage(UIImageE.Main_Character).Image.color = Color.white;
            GetImage(UIImageE.Main_Character).sprite = sprite;
            GetImage(UIImageE.Main_Character).Image.SetNativeSize();
        }
    }
    void JobSetting(CharacterJob job)
    {

    }
    void SliderSetting(int cardCount, int awake, bool isPlayerData)
    {
        Get<CharacterCardSlider>(CharacterCardSliderE.Main_CharacterCardSlider).UISet(cardCount, awake, isPlayerData);
    }
    void Click(PointerEventData ped)
    {
        string popupName = "MainScene/CharacterInfoPopup";
        Managers.UI.ShopPopupUI<CharacterInfoPopup>(popupName, CanvasOrderType.Top);
    }
    
	public enum UIImageE
    {
		Main_BG,
		Main_BgDeco,
		Main_Character,
		Main_Grade,
		Main_Job,
    }
	public enum UITextProE
    {
		Main_LvText,
		Main_Name,
    }
	public enum CharacterCardSliderE
    {
		Main_CharacterCardSlider,
    }
	public enum UIButtonE
    {
		Button,
    }
}
public class CharacterCardData : ICardData
{
	public CharacterPlayerData playerData;
	public CharacterTableData tableData;
    public CharacterSO soData;
}