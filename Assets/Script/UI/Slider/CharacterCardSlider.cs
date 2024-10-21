using System.Collections.Generic;

public class CharacterCardSlider : UISlider
{
    Dictionary<FormulaKeyword, float> formulaKeyDics = new Dictionary<FormulaKeyword, float>();

    protected override void Initialize()
	{
		Bind<UIImage>(typeof(UIImageE));
		Bind<UITextPro>(typeof(UITextProE));



		base.Initialize();

        formulaKeyDics.Add(FormulaKeyword.C_AWAKE, 0);
    }

	public void UISet(int cardCount, int awake, bool isPlayerData)
	{
		if (isPlayerData)
		{
            formulaKeyDics[FormulaKeyword.C_AWAKE] = awake;
            int needCardCount = Managers.Table.FormulaTable.GetValue(FormulaTableCodeDefine.Need_C_Awake_Card, formulaKeyDics).ToInt();

            GetTextPro(UITextProE.Count).text = $"{cardCount} / {needCardCount}";
            GetTextPro(UITextProE.Star_Text).text = $"{awake}";

            float sliderValue = cardCount / (float)needCardCount;
            this.value = sliderValue;

            GetImage(UIImageE.Arrow).gameObject.SetActive(sliderValue >= 1);
        }
		else
		{
            GetTextPro(UITextProE.Count).text = $"{0} / {0}";
            GetTextPro(UITextProE.Star_Text).text = $"{0}";
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
	public enum UITextProE
    {
		Count,
		Star_Text,
    }
}