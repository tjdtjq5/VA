using System.Collections.Generic;
using UnityEngine;

public class UIBattle : UIFrame
{
    protected override void Initialize()
    {
		Bind<UIImage>(typeof(UIImageE));
		Bind<UIText>(typeof(UITextE));

		_aniController = animator.Initialize();
		_lightAniController = lightAnimator.Initialize();

        base.Initialize();
    }
    
    [SerializeField] private Animator animator;
    [SerializeField] private Animator lightAnimator;
    [SerializeField] private Transform skillViewContents;
    
    private AniController _aniController;
    private AniController _lightAniController;
    private readonly int _openHash = Animator.StringToHash("Open");
    private readonly int _closeHash = Animator.StringToHash("Close");
    private readonly int _playHash = Animator.StringToHash("Play");
    private readonly string _skillViewCardPath = "Prefab/UI/Card/InGame/SkillViewCard";

    private List<InGameBattleSkillViewCard> _skillViewCards = new();
    private bool _isOpen = false;

    public void TurnUISet(int currentPage, int maxPage, bool isEffect)
    {
	    GetText(UITextE.SafeArea_Turn_TurnText).text = $"턴 {currentPage + 1} / {maxPage}";
	    
	    if(isEffect)
		    _lightAniController.SetTrigger(_playHash);
    }
    public void TurnOpen()
    {
	    if (_isOpen)
		    return;
	    
	    _isOpen = true;
	    _aniController.SetTrigger(_openHash);
    }
    public void TurnClose()
    {
	    if (!_isOpen)
		    return;
	    
	    _isOpen = false;
	    _aniController.SetTrigger(_closeHash);
    }

    public void SkillPlay(Skill skill)
    {
		return;
		
	    if (!skill.IsView)
		    return;
	    
	    InGameBattleSkillViewCard card = null;
	    for (int i = 0; i < _skillViewCards.Count; i++)
	    {
		    if (!_skillViewCards[i].gameObject.activeSelf)
		    {
			    card = _skillViewCards[i];
			    card.gameObject.SetActive(true);
			    break;
		    }
	    }

	    if (card == null)
	    {
		    card = Managers.Resources.Instantiate<InGameBattleSkillViewCard>(_skillViewCardPath, skillViewContents);
		    _skillViewCards.Add(card);
	    }
	    
	    card.UISet(skill);
    }

	public enum UIImageE
    {
		SafeArea_Turn_Black,
    }
	public enum UITextE
    {
		SafeArea_Turn_TurnText,
    }
}