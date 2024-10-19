using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public TextMeshProUGUI Text { get { return text; } }
    Animator animator;

    private readonly static int kfloatingHash = Animator.StringToHash("floating");

    void Awake()
    {
        animator = UnityHelper.FindChild<Animator>(this.gameObject, true);
    }
    public void Play(Vector3 pos)
    {
        this.transform.position = pos;
        animator.SetTrigger(kfloatingHash);
    }
}
