using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Scrollbar))]
public class UIScrollbar : UIBase
{
    [SerializeField] private float initialValue = 0f;
    [SerializeField] private bool interactable = true;

    private Scrollbar _scrollbar;
    public Scrollbar Scrollbar
    {
        get
        {
            if (_scrollbar == null)
                _scrollbar = GetComponent<Scrollbar>();
            return _scrollbar;
        }
    }

    public float value
    {
        get => Scrollbar.value;
        set => Scrollbar.value = value;
    }

    public bool Interactable
    {
        get => Scrollbar.interactable;
        set => Scrollbar.interactable = value;
    }

    public void AddListener(UnityAction<float> action)
    {
        Scrollbar.onValueChanged.AddListener(action);
    }

    public void RemoveListener(UnityAction<float> action)
    {
        Scrollbar.onValueChanged.RemoveListener(action);
    }

    public void RemoveAllListeners()
    {
        Scrollbar.onValueChanged.RemoveAllListeners();
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        if (_scrollbar == null)
            _scrollbar = GetComponent<Scrollbar>();
            
        if (_scrollbar != null)
        {
            _scrollbar.value = initialValue;
            _scrollbar.interactable = interactable;
        }
        else
        {
            UnityHelper.Error_H("Scrollbar component not found!");
        }
    }
}
