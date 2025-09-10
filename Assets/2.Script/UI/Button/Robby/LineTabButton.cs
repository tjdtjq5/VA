using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class LineTabButton : UITabButton
{
    [SerializeField] private Sprite _rightSpriteOff;
    [SerializeField] private Sprite _rightLineOn;
    [SerializeField] private Sprite _leftSpriteOff;
    [SerializeField] private Sprite _leftLineOn;
    [SerializeField] private Sprite _middleSpriteOff;
    [SerializeField] private Sprite _middleLineOn;

    [SerializeField] private Image _bg;
    [SerializeField] private GameObject _rightLine;
    [SerializeField] private GameObject _leftLine;

    [SerializeField] private Direction _direction;

    protected override void Initialize()
    {
		Bind<UIText>(typeof(UITextE));

        base.Initialize();
    }
    [Button]
    protected override void UISet()
    {
        switch(_direction)
        {
            case Direction.Right:
                _bg.sprite = IsSwitch ? _rightLineOn : _rightSpriteOff;
                _rightLine.SetActive(false);
                _leftLine.SetActive(true);
                break;
            case Direction.Left:
                _bg.sprite = IsSwitch ? _leftLineOn : _leftSpriteOff;
                _rightLine.SetActive(true);
                _leftLine.SetActive(false);
                break;
            case Direction.Middle:
                _bg.sprite = IsSwitch ? _middleLineOn : _middleSpriteOff;
                _rightLine.SetActive(true);
                _leftLine.SetActive(true);
                break;
        }
    }
    public override bool Switch(bool flag)
    {
        base.Switch(flag);
        UISet();
        return true;
    }
	public enum UITextE
    {
		Bg_Text,
    }
}