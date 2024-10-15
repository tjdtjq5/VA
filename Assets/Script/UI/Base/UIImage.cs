using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIBase
{
    Image Image
    {
        get
        {
            return GetComponent<Image>();
        }
    }
    public Sprite sprite
    {
        set
        {
            Image.sprite = value;
        }
    }
    protected override void Initialize()
    {
        base.Initialize();

        Image.raycastTarget = false;
    }
}
