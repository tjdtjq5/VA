using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public abstract class UICard : UIFrame
{
    protected Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }
    protected AniController AniController;
    protected CanvasGroup CanvasGroup;

    int inHash = UnityEngine.Animator.StringToHash("In");
    int playHash = UnityEngine.Animator.StringToHash("Play");

    protected override void Initialize()
    {
        if (Animator)
            AniController = Animator.Initialize();

        CanvasGroup = this.gameObject.GetOrAddComponent<CanvasGroup>();
    }

    public abstract void Setting(ICardData data);
    public void Fade(float alpha) => CanvasGroup.alpha = alpha;
    public virtual void In(float time)
        => AniController.SetFloat(inHash, time);

    public void Play() => AniController.SetTrigger(playHash);
    private void OnEnable() => In(1);
}
