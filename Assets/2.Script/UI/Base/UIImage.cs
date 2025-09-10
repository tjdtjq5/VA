using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIBase
{
    [SerializeField] private bool isRaycast = false;

    private Image _image;
    public Image Image
    {
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();
            return _image;
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

    public Color color
    {
        set
        {
            Image.color = value;
        }
        get
        {
            return Image.color;
        }
    }

    public Material Material
    {
        set
        {
            Image.material = value;
        }
        get
        {
            return Image.material;
        }
    }

    public void SetColor(Color color)
    {
        this.color = color;
    }

    public void SetColor(string hexColor)
    {
        if (string.IsNullOrEmpty(hexColor))
            return;

        if (hexColor[0] != '#')
            hexColor = hexColor.Insert(0, "#");

        if (hexColor.Length == 7)
            hexColor = hexColor.Insert(hexColor.Length - 1, "FF");
        
        if (hexColor.Length != 9)
            return;

        if (ColorUtility.TryParseHtmlString(hexColor, out var result))
            this.color = result;
        else
            Debug.LogError($"Invalid hex color: {hexColor}");
    }

    public float FillAmount 
    { 
        get => Image.fillAmount; 
        set => Image.fillAmount = value; 
    }

    public void Fade(float alpha)
    {
        Color c = Image.color;
        c.a = alpha;
        Image.color = c;
    }

    public void SetNativeSize()
    {
        if (sprite == null)
            return;

        Image.SetNativeSize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        if (_image == null)
            _image = GetComponent<Image>();
            
        if (_image != null)
        {
            _image.raycastTarget = isRaycast;
        }
        else
        {
            UnityHelper.Error_H("Image component not found!\n" + this.name);
        }
    }
}
