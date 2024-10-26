public class CharacterDetailInfoCard : UICard
{
    protected override void Initialize()
    {
		Bind<UITextPro>(typeof(UITextProE));
		Bind<UIText>(typeof(UITextE));
		Bind<UIImage>(typeof(UIImageE));


        base.Initialize();
    }

    public override void Setting(ICardData data)
    {
        base.Setting(data);

        CharacterDetailInfoCardData _data = (CharacterDetailInfoCardData)data;
        Stat stat = _data.stat;

        GetTextPro(UITextProE.AbText).text = stat.DisplayName;
        GetText(UITextE.Value).text = CSharpHelper.Format_H("(+{0})", stat.Value.Alphabet());

        stat.RemoveExBonus();

        GetText(UITextE.Plus).text = CSharpHelper.Format_H("(+{0})", stat.Value.Alphabet());

        GetImage(UIImageE.Icon).sprite = stat.Icon;
        GetImage(UIImageE.Icon).SetNativeSize();
    }

	public enum UITextProE
    {
		AbText,
    }
	public enum UITextE
    {
		Value,
		Plus,
    }
	public enum UIImageE
    {
		Icon,
    }
}
public class CharacterDetailInfoCardData : ICardData
{
    public Stat stat;
}