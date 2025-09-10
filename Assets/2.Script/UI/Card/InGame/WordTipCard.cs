using UnityEngine;

public class WordTipCard : UICard
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));



        base.Initialize();
    }

    private WordTipCardData _data;

    public override void Setting(ICardData data)
    {
        _data = (WordTipCardData)data;

        GetTextPro(UITextProE.Main_Title).text = _data.Title;
        GetTextPro(UITextProE.Main_Title).color = _data.TitleColor;
        GetTextPro(UITextProE.Main_Explain).text = _data.Explain;
    }
    
	public enum UIImageE
    {
		Main,
    }
	public enum UITextProE
    {
		Main_Title,
		Main_Explain,
    }
}
public class WordTipCardData : ICardData
{
	public string Title;
    public string Explain;
    public Color TitleColor;
}