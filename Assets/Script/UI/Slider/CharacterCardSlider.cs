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

	public void UISet(CharacterPlayerData playerData)
	{
		if (playerData != null)
		{
            formulaKeyDics[FormulaKeyword.C_AWAKE] = playerData.Awake;
            int needCardCount = Managers.Table.FormulaTable.GetValue(FormulaTableCodeDefine.Need_C_Awake_Card, formulaKeyDics).ToInt();

            GetTextPro(UITextProE.Count).text = $"{playerData.Count} / {needCardCount}";
            GetTextPro(UITextProE.Star_Text).text = $"{playerData.Awake}";

            float sliderValue = playerData.Count / (float)needCardCount;
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