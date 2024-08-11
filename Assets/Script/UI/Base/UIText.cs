using UnityEngine.UI;

public class UIText : UIBase
{
    Text Text
    {
        get
        {
            return GetComponent<Text>();
        }
    }

    public string text
    {
        set
        {
            Text.text = value;
        }
    }
}
