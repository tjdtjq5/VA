using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChainCombo : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

		_aniController = animator.Initialize();

        base.Initialize();
    }
    
    [SerializeField] private Animator animator;
    
    private AniController _aniController;
    private AttackGrade _currentGrade;
    
    private readonly int _inOutHash = Animator.StringToHash("InOut");
    private readonly int _changeHash = Animator.StringToHash("Change");
    private readonly int _gradeHash = Animator.StringToHash("Grade");
    private readonly string _basicColor = "FFFFFF";
    private readonly string _focusColor = "FFEA6C";
    private readonly string _fatalColor = "FF6B75";
    private readonly string _ouiColor = "E96BFF";
    private readonly Vector2 _bgSizeOne = new Vector2(240, 110);
    private readonly Vector2 _bgSizeTen = new Vector2(330, 110);
    private readonly int _maxCount = 99;

    public void UISet(PuzzleType puzzleType, int count)
    {
	    if (count == 0)
	    {
		    return;
	    }
	    
	    AttackGrade grade = GameDefine.GetAttackGrade(count);
	    
	    SetGrade(grade);
	    SetCount(count);

	    _aniController.SetBool(_inOutHash, true);
	    _aniController.SetTrigger(_currentGrade == grade ? _changeHash : _gradeHash);
	    
	    _currentGrade = grade;
    }

    void SetCount(int count)
    {
	    count = Mathf.Min(count, _maxCount);
	    
	    if (count < 10)
	    {
		    GetImage(UIImageE.Numbers_1).gameObject.SetActive(true);
		    GetImage(UIImageE.Numbers_10).gameObject.SetActive(false);
		    GetImage(UIImageE.BG).RectTransform.sizeDelta = _bgSizeOne;

		    int one = count % 10;

		    GetImage(UIImageE.Numbers_1).sprite = Managers.Atlas.GetFontCombo(one.ToString());
	    }
	    else
	    {
		    GetImage(UIImageE.Numbers_1).gameObject.SetActive(true);
		    GetImage(UIImageE.Numbers_10).gameObject.SetActive(true);
		    GetImage(UIImageE.BG).RectTransform.sizeDelta = _bgSizeTen;

		    int one = count % 10;
		    count /= 10;
		    int ten = count % 10;

		    GetImage(UIImageE.Numbers_1).sprite = Managers.Atlas.GetFontCombo(one.ToString());
		    GetImage(UIImageE.Numbers_10).sprite = Managers.Atlas.GetFontCombo(ten.ToString());
	    }
    }
    void SetGrade(AttackGrade grade)
    {
	    switch (grade)
	    {
		    case AttackGrade.Basic:
			    GetText(UITextE.Text).text = "일반 공격";
			    GetImage(UIImageE.Numbers_1).SetColor(_basicColor);
			    GetImage(UIImageE.Numbers_10).SetColor(_basicColor);
			    break;
		    case AttackGrade.Focus:
			    GetText(UITextE.Text).text = "집중 공격";
			    GetImage(UIImageE.Numbers_1).SetColor(_focusColor);
			    GetImage(UIImageE.Numbers_10).SetColor(_focusColor);
			    break;
		    case AttackGrade.Fatal:
			    GetText(UITextE.Text).text = "필살 공격";
			    GetImage(UIImageE.Numbers_1).SetColor(_fatalColor);
			    GetImage(UIImageE.Numbers_10).SetColor(_fatalColor);
			    break;
		    case AttackGrade.Oui:
			    GetText(UITextE.Text).text = "최종 오의";
			    GetImage(UIImageE.Numbers_1).SetColor(_ouiColor);
			    GetImage(UIImageE.Numbers_10).SetColor(_ouiColor);
			    break;
	    }
    }

    public void Out()
    {
	    _aniController.SetBool(_inOutHash, false);
	    _currentGrade = AttackGrade.Basic;
    }
    
	public enum UIImageE
    {
		BG,
		Numbers_10,
		Numbers_1,
    }
	public enum UITextE
    {
		Text,
    }
}