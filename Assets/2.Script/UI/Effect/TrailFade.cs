using UnityEngine;

public class TrailFade : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    private float time;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnEnable()
    {
        time = 0f;
    }

    void Update()
    {
        time += Managers.Time.UnscaledTime;
        
        if (canvasGroup != null)
            canvasGroup.alpha = 1f - (time / duration);

        transform.localScale = Vector3.one * (1f - (time / duration));

        if (time >= duration)
            Managers.Resources.Destroy(this.gameObject);
    }
}
