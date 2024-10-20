using System.Collections.Generic;

public class CharacterCardSlider : UISlider
{
    Dictionary<FormulaKeyword, float> formulaKeyDics = new Dictionary<FormulaKeyword, float>();

    protected override void Initialize()
	{
		base.Initialize();
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

        formulaKeyDics.Add(FormulaKeyword.C_AWAKE, 0);
    }

	public void UISet(int cardCount, int awake, bool isPlayerData)
	{
		if (isPlayerData)
		{
            formulaKeyDics[FormulaKeyword.C_AWAKE] = awake;
            int needCardCount = Managers.Table.FormulaTable.GetValue(FormulaTableCodeDefine.Need_C_Awake_Card, formulaKeyDics).ToInt();

            GetText(UITextE.Count).text = $"{cardCount} / {needCardCount}";
            GetText(UITextE.Star_Text).text = $"{awake}";

            float sliderValue = cardCount / (float)needCardCount;
            this.value = sliderValue;

            GetImage(UIImageE.Arrow).gameObject.SetActive(sliderValue >= 1);
        }
		else
		{
            GetText(UITextE.Count).text = $"{0} / {0}";
            GetText(UITextE.Star_Text).text = $"{0}";
            this.value = 0;
            GetImage(UIImageE.Arrow).gameObject.SetActive(false);
        }
    }

    public enum UIImageE
    {
		Background,
		FillArea_Fill,
		Arrow,
		Star,
    }
	public enum UITextE
    {
		Count,
		Star_Text,
    }
}
