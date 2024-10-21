using TMPro;

public class UITextPro : UIBase
{
    TextMeshProUGUI Text
    {
        get
        {
            return GetComponent<TextMeshProUGUI>();
        }
    }

    public string text
    {
        set
        {
            Text.text = value;
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        Text.raycastTarget = false;
    }
}
