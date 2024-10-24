using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UICard : UIFrame
{
    protected Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    protected AniController AniController;

    int inHash = UnityEngine.Animator.StringToHash("In");

    protected override void Initialize()
    {
        if (Animator)
            AniController = Animator.Initialize();
    }
    public virtual void Setting(ICardData data) { }
    public virtual void In(float time)
        => AniController.SetFloat(inHash, time);

    private void OnEnable() => In(1);
}
