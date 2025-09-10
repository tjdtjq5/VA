using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFindEnemy : UIFrame
{
    protected override void Initialize()
    {
        base.Initialize();

        _rightAniController = rightAnimator.Initialize();
        _leftAniController = leftAnimator.Initialize();
    }

    [SerializeField] private Animator rightAnimator;
    [SerializeField] private Animator leftAnimator;
    
    private AniController _rightAniController;
    private AniController _leftAniController;
    
    private readonly int _inHash = Animator.StringToHash("In");
    private readonly int _outHash = Animator.StringToHash("Out");
    private bool _isRightFlag = false;
    private bool _isLeftFlag = false;

    public void RightSwitch(bool flag)
    {
        if (_isRightFlag.Equals(flag))
            return;

        _isRightFlag = flag;
        _rightAniController.SetTrigger(flag ? _inHash : _outHash);
    }
    public void LeftSwitch(bool flag)
    {
        if (_isLeftFlag.Equals(flag))
            return;
        
        _isLeftFlag = flag;
        _leftAniController.SetTrigger(flag ? _inHash : _outHash);
    }
}
