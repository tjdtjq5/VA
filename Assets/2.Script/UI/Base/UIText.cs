using UnityEngine;
using UnityEngine.UI;

public class UIText : UIBase
{
    [SerializeField] private bool isChangeLanguage = true;

    private Text _text;
    protected Text Text
    {
        get
        {
            if (_text == null)
                _text = GetComponent<Text>();
            return _text;
        }
    }

    public string text
    {
        get => Text.text;
        set => Text.text = value;
    }

    public Color color
    {
        set
        {
            Text.color = value;
        }
        get
        {
            return Text.color;
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
            UnityHelper.Error_H($"Invalid hex color: {hexColor}");
    }

    public void Fade(float alpha)
    {
        Color c = Text.color;
        c.a = alpha;
        Text.color = c;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        if (_text == null)
            _text = GetComponent<Text>();
            
        if (_text != null)
        {
            _text.raycastTarget = false;
            if (isChangeLanguage)
            {
                Managers.Language.OnChangeLanguage += ChangeLanguage;
                ChangeLanguage(); // 초기 폰트 설정
            }
        }
        else
        {
            UnityHelper.Error_H("Text component not found!");
        }
    }

    protected virtual void ChangeLanguage()
    {
        if (_text != null && Managers.Language.Font != null)
            _text.font = Managers.Language.Font;
    }

    private void OnEnable()
    {
        if (isChangeLanguage && _text != null)
            Managers.Language.OnChangeLanguage += ChangeLanguage;
    }
    
    private void OnDisable()
    {
        if (isChangeLanguage)
            Managers.Language.OnChangeLanguage -= ChangeLanguage;
    }

    private void OnDestroy()
    {
        if (isChangeLanguage)
            Managers.Language.OnChangeLanguage -= ChangeLanguage;
    }
}
