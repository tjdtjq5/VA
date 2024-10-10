using UnityEngine.EventSystems;

public class UITabButton : UIButton
{
    bool isSwitch = false;
    int switchHash = UnityEngine.Animator.StringToHash("Switch");

    protected override void Initialize()
    {
        base.Initialize();
    }
    public void SwitchTab(bool flag)
    {
        if (flag)
        {
            AniController.SetBool(switchHash, true);

            UISet();
        }
        else
        {
            AniController.SetBool(switchHash, false);
        }
    }
}