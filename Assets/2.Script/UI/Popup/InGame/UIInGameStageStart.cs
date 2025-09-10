using System;
using Shared.CSharp;
using UnityEngine;

public class UIInGameStageStart : UIPopup
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

		_aniController = animator.Initialize();
		_aniController.SetEndFunc("StageStart", OnPlayEnded);

        base.Initialize();
    }

    public Action OnEnded;
    
    [SerializeField] Animator animator;
    
    private AniController _aniController;
    
    private readonly int _playHash = Animator.StringToHash("Play");
    private readonly string _stageFormat = "STAGE {0}";

    public void UISet(int stage)
    {
	    _aniController.SetTrigger(_playHash);
	    
	    GetText(UITextE.Main_Stage).text = CSharpHelper.Format_H(_stageFormat, stage);
    }

    void OnPlayEnded(string clipName)
    {
	    OnEnded?.Invoke();
	    OnEnded = null;
	    ClosePopupUI();
    }
    
	public enum UIImageE
    {
		Main_Bg,
    }
	public enum UITextE
    {
		Main_Stage,
    }
}