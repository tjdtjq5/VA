using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIBase
{
    public Image Image
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
            if (value != null)
                Image.sprite = value;
            else
            {
                Image.sprite = null;
                Image.color = Color.clear;
            }
        }
        get
        {
            return Image.sprite;
        }
    }
    public void SetNativeSize()
    {
        if (sprite == null)
            return;

        this.Image.SetNativeSize();
    }
    protected override void Initialize()
    {
        base.Initialize();

        Image.raycastTarget = false;
    }
}
