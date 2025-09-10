using UnityEngine;

public class UICombo : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));

		_aniController = animator.Initialize();

        base.Initialize();
    }
    
    [SerializeField] private Animator animator;
    
    private AniController _aniController;
    private Tween<float> _tween;
    
    private readonly int _inOutHash = Animator.StringToHash("InOut");
    private readonly int _changeHash = Animator.StringToHash("Change");
    private readonly int _maxCount = 999;
    
    public void UISet(int count)
    {
	    if (count == 0)
	    {
		    return;
	    }
	    
	    count = Mathf.Min(count, _maxCount);
	    
	    _aniController.SetBool(_inOutHash, true);
	    _aniController.SetTrigger(_changeHash);

		_tween?.FullKill();
	    _tween = Managers.Tween.TweenInvoke(1f)
		    .SetOnPerceontCompleted(1, () =>
		    {
			    _aniController.SetBool(_inOutHash, false);
		    });

	    if (count < 10)
	    {
		    GetImage(UIImageE.SafeArea_Numbers_1).gameObject.SetActive(true);
		    GetImage(UIImageE.SafeArea_Numbers_10).gameObject.SetActive(false);
		    GetImage(UIImageE.SafeArea_Numbers_100).gameObject.SetActive(false);
		    
		    int one = count % 10;

		    GetImage(UIImageE.SafeArea_Numbers_1).sprite = Managers.Atlas.GetFontCombo(one.ToString());
	    }
	    else if (count < 100)
	    {
		    GetImage(UIImageE.SafeArea_Numbers_1).gameObject.SetActive(true);
		    GetImage(UIImageE.SafeArea_Numbers_10).gameObject.SetActive(true);
		    GetImage(UIImageE.SafeArea_Numbers_100).gameObject.SetActive(false);
		    
		    int one = count % 10;
		    count /= 10;
		    int ten = count % 10;
		    
		    GetImage(UIImageE.SafeArea_Numbers_1).sprite = Managers.Atlas.GetFontCombo(one.ToString());
		    GetImage(UIImageE.SafeArea_Numbers_10).sprite = Managers.Atlas.GetFontCombo(ten.ToString());
	    }
	    else
	    {
		    GetImage(UIImageE.SafeArea_Numbers_1).gameObject.SetActive(true);
		    GetImage(UIImageE.SafeArea_Numbers_10).gameObject.SetActive(true);
		    GetImage(UIImageE.SafeArea_Numbers_100).gameObject.SetActive(true);
		    
		    int one = count % 10;
		    count /= 10;
		    int ten = count % 10;
		    count /= 10;
		    int hun = count % 10;
		    
		    GetImage(UIImageE.SafeArea_Numbers_1).sprite = Managers.Atlas.GetFontCombo(one.ToString());
		    GetImage(UIImageE.SafeArea_Numbers_10).sprite = Managers.Atlas.GetFontCombo(ten.ToString());
		    GetImage(UIImageE.SafeArea_Numbers_100).sprite = Managers.Atlas.GetFontCombo(hun.ToString());
	    }
    }

	public enum UIImageE
    {
		SafeArea_Combo_Combo,
		SafeArea_Numbers_100,
		SafeArea_Numbers_10,
		SafeArea_Numbers_1,
    }
}