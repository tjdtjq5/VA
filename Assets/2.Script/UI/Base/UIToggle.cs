using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIToggle : UIBase
{
    [SerializeField] private bool isOn = false;
    [SerializeField] private bool interactable = true;

    private Toggle _toggle;
    public Toggle Toggle
    {
        get
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();
            return _toggle;
        }
    }

    public bool IsOn
    {
        get => Toggle.isOn;
        set => Toggle.isOn = value;
    }

    public bool Interactable
    {
        get => Toggle.interactable;
        set => Toggle.interactable = value;
    }

    public void AddListener(UnityAction<bool> action)
    {
        Toggle.onValueChanged.AddListener(action);
    }

    public void RemoveListener(UnityAction<bool> action)
    {
        Toggle.onValueChanged.RemoveListener(action);
    }

    public void RemoveAllListeners()
    {
        Toggle.onValueChanged.RemoveAllListeners();
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        if (_toggle == null)
            _toggle = GetComponent<Toggle>();
            
        if (_toggle != null)
        {
            _toggle.isOn = isOn;
            _toggle.interactable = interactable;
        }
        else
        {
            UnityHelper.Error_H("Toggle component not found!");
        }
    }
}
