using UnityEngine;

public class PuzzleComboMultiplier : UIFrame
{
    protected override void Initialize()
    {
		Bind<UITextPro>(typeof(UITextProE));

        base.Initialize();
    }

    private readonly string _basicColor = "FFC634";
    private readonly string _forceColor = "FF3934";
    
    public void UISet(Character owner, int combo, int forceCount, bool isForce)
    {
	    GetTextPro(UITextProE.Text).text = $"x {GameDefine.ComboMultiplier(owner, combo, forceCount)}";
	    GetTextPro(UITextProE.Text).SetColor(isForce ? _forceColor : _basicColor);
    }

	public enum UITextProE
    {
		Text,
    }
}