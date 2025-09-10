using System.Collections.Generic;
using Shared.BBNumber;
using Shared.CSharp;
using UnityEngine;

public class FloatingText : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		
		_animator = UnityHelper.FindChild<Animator>(this.gameObject, true);
		_csr = UnityHelper.FindChild<ContentSizeRectTransform>(this.gameObject, true);
		
		_aniController = _animator.Initialize();

        base.Initialize();
    }

    private ContentSizeRectTransform _csr;
    private Animator _animator;
    private AniController _aniController;

	private readonly int _normalHash = Animator.StringToHash("Normal");
	private readonly int _textHash = Animator.StringToHash("Text");
	private readonly int _criticalHash = Animator.StringToHash("Cri");
    private readonly int _maxValueLength = 7;
    private readonly string _criStr = "Cri_{0}";
	private readonly string _superCriStr = "SuperCri_{0}";
	private readonly string _blueStr = "Blue_{0}";
    private readonly string _iconStr = "Icon_{0}";
    
    public void Damage(Vector3 pos, BBNumber value, DamageType damageType, CriticalType criticalType)
    {
        this.transform.position = pos;
        _aniController.SetTrigger(criticalType == CriticalType.None ? _normalHash : _criticalHash);

        SetDamageType(damageType);

		switch (criticalType)
		{
			case CriticalType.None:
				SetValue(value, TextColor.Basic);
				break;
			case CriticalType.Critical:
				SetValue(value, TextColor.Critical);
				break;
			case CriticalType.SuperCritical:
				SetValue(value, TextColor.SuperCritical);
				break;
		}
        _csr.SetFitHorizontal();
    }
	public void Miss(Vector3 pos)
	{
		this.transform.position = pos;
		_aniController.SetTrigger(_textHash);

		SetDamageType(DamageType.None);
		SetValue("Miss", TextColor.Blue);
	}
	public void Heal(Vector3 pos, BBNumber value)
	{
		this.transform.position = pos;
		_aniController.SetTrigger(_textHash);

		SetDamageType(DamageType.None);
		SetValue($"+{value.Alphabet()}", TextColor.Green);
	}

    private void SetDamageType(DamageType damageType)
    {
	    GetImage(UIImageE.Main_Type_TextImg).gameObject.SetActive(damageType != DamageType.None);
	    GetImage(UIImageE.Main_Type_Icon).gameObject.SetActive(damageType != DamageType.None);

	    if (damageType != DamageType.None)
	    {
		    GetImage(UIImageE.Main_Type_TextImg).sprite = Managers.Atlas.GetAlphabetNumber(damageType.ToString());
		    GetImage(UIImageE.Main_Type_TextImg).SetNativeSize();
		    GetImage(UIImageE.Main_Type_Icon).sprite = Managers.Atlas.GetAlphabetNumber(CSharpHelper.Format_H(_iconStr,damageType));
		    GetImage(UIImageE.Main_Type_Icon).SetNativeSize();
	    }
    }

    private void SetValue(BBNumber value, TextColor textColor)
    {
	    string valueString = value.Alphabet();
	    int len = Mathf.Min(valueString.Length, _maxValueLength);
		List<string> valueList = new();

	    for (int i = 0; i < len; i++)
	    {
		    string c = valueString[i].ToString();
		    c = c.ToUpper();
		    c = c.Replace(".", "Comma");
			switch (textColor)
			{
				case TextColor.Blue:
					c = CSharpHelper.Format_H(_blueStr, c);
					break;
				case TextColor.SuperCritical:
					c = CSharpHelper.Format_H(_superCriStr, c);
					break;
				case TextColor.Critical:
					c = CSharpHelper.Format_H(_criStr, c);
					break;
				case TextColor.Basic:
					break;
				default:
					break;
			}

			valueList.Add(c);
	    }

		SetValue(valueList);
    }

	private void SetValue(string value, TextColor textColor)
	{
		List<string> valueList = new();
		int len = Mathf.Min(value.Length, _maxValueLength);

		for (int i = 0; i < len; i++)
		{
			string c = value[i].ToString();
			c = c.ToUpper();
			c = c.Replace(".", "Comma");

			switch (textColor)
			{
				case TextColor.Blue:
					c = CSharpHelper.Format_H(_blueStr, c);
					break;
				case TextColor.SuperCritical:
					c = CSharpHelper.Format_H(_superCriStr, c);
					break;
				case TextColor.Critical:
					c = CSharpHelper.Format_H(_criStr, c);
					break;
				case TextColor.Basic:
					break;
				default:
					break;
			}

			valueList.Add(c);
		}	

		SetValue(valueList);
	}

	private enum TextColor
	{
		Basic,
		Critical,
		SuperCritical,
		Blue,
		Green,
	}

	private void SetValue(List<string> valueList)
	{
		for (int i = 0; i < _maxValueLength; i++)
		    GetValueImage(i).gameObject.SetActive(false);

		for (int i = 0; i < valueList.Count; i++)
		{
			GetValueImage(i).gameObject.SetActive(true);
		    GetValueImage(i).sprite = Managers.Atlas.GetAlphabetNumber(valueList[i]);
		    GetValueImage(i).SetNativeSize();
		}
	}

    private UIImage GetValueImage(int index)
    {
	    switch (index)
	    {
		    case 0:
			    return GetImage(UIImageE.Main_Numbers_A);
		    case 1:
			    return GetImage(UIImageE.Main_Numbers_B);
		    case 2:
			    return GetImage(UIImageE.Main_Numbers_C);
		    case 3:
			    return GetImage(UIImageE.Main_Numbers_D);
		    case 4:
			    return GetImage(UIImageE.Main_Numbers_E);
		    case 5:
			    return GetImage(UIImageE.Main_Numbers_F);
		    case 6:
			    return GetImage(UIImageE.Main_Numbers_G);
		    default:
			    return null;
	    }
    }
	public enum UIImageE
    {
		Main_Type_Icon,
		Main_Type_TextImg,
		Main_Numbers_A,
		Main_Numbers_B,
		Main_Numbers_C,
		Main_Numbers_D,
		Main_Numbers_E,
		Main_Numbers_F,
		Main_Numbers_G,
    }
}